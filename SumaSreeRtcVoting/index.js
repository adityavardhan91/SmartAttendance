// app.js
const express = require('express');
const http = require('http');
const socketIO = require('socket.io');
const { v4: uuidv4 } = require('uuid');

const app = express();
const server = http.createServer(app);
const io = socketIO(server);

app.get('/', (req, res) => {
  res.sendFile(__dirname + '/index.html');
});

io.on('connection', (socket) => {
  const roomId = generateRoomId();
  const roomInfo = {
    roomId,
    host: socket.id,
    questions: [],
  };
  socket.join(roomId);

  socket.emit('roomCreated', roomInfo);

  console.log(`Socket connected to room: ${roomId}`);
  socket.on('joinRoom', (requestedRoomId) => {
    if (isValidRoomId(requestedRoomId)) {
      // Leave the current room
      socket.leave(roomId);

      // Join the requested room
      socket.join(requestedRoomId);

      // Send updated room information to the client
      io.to(requestedRoomId).emit('roomUpdated', getRoomInfo(requestedRoomId));

      // Send a message or perform any necessary actions
      console.log(`Socket joined room: ${requestedRoomId}`);
    } else {
      console.log(`Invalid room ID: ${requestedRoomId}`);
    }
  });

  // Listen for the 'postQuestion' event from the host
  socket.on('postQuestion', (question) => {
    if (socket.id === roomInfo.host) {
      // Only the host can post questions
      roomInfo.questions.push(question);

      // Broadcast the updated room information to all participants
      io.to(roomId).emit('roomUpdated', getRoomInfo(roomId));
    } else {
      console.log('Only the host can post questions.');
    }
  });

  socket.on('answerQuestion', (answer) => {
    // Add your logic for handling participant answers
    console.log(`Participant ${socket.id} answered: ${answer}`);
  });

  function getRoomInfo(roomId) {
    return {
      roomId,
      host: roomInfo.host,
      questions: roomInfo.questions,
    };
  }
});

function generateRoomId() {
  return `room_${uuidv4()}`;
}

function isValidRoomId(roomId) {
  return typeof roomId === 'string' && roomId.trim() !== '';
}

const PORT = process.env.PORT || 3000;
server.listen(PORT, () => {
  console.log(`Server running on http://localhost:${PORT}`);
});
