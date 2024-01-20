const express = require('express');
const http = require('http');
const socketIO = require('socket.io');

const app = express();
const server = http.createServer(app);
const io = socketIO(server);

const roomLimits = new Map();
const roomWords = new Map();

io.on('connection', (socket) => {
  console.log('A user connected');

  // Listen for joining a room
  socket.on('joinRoom', (roomName) => {
    const currentRoomUsers = roomLimits.get(roomName) || 0;

    if (currentRoomUsers < 5) {
      // Allow user to join the room
      socket.join(roomName);
      roomLimits.set(roomName, currentRoomUsers + 1);
      io.to(roomName).emit('userCount', currentRoomUsers + 1);

      // Generate a random word for the room
      const word = generateRandomWord();
      roomWords.set(roomName, word);
      io.to(roomName).emit('startGame', word);

      // Listen for drawing events in the room
      socket.on('draw', (data) => {
        // Broadcast the drawing data to all clients in the room
        io.to(roomName).emit('draw', data);
      });

      // Listen for chat messages
      socket.on('chatMessage', (message) => {
        const userAnswer = message.trim().toLowerCase();
        const correctAnswer = word.trim().toLowerCase();

        if (userAnswer === correctAnswer) {
          // Announce winners in the chat
          io.to(roomName).emit('announceWinner', socket.username);
        }
      });

      // Handle disconnect event
      socket.on('disconnect', () => {
        socket.leave(roomName);
        roomLimits.set(roomName, Math.max(currentRoomUsers - 1, 0));
        io.to(roomName).emit('userCount', Math.max(currentRoomUsers - 1, 0));
        console.log('User disconnected');
      });
    } else {
      // Notify user that the room is full
      socket.emit('roomFull');
    }
  });

  // Listen for setting username
  socket.on('setUsername', (username) => {
    socket.username = username;
  });
});

function generateRandomWord() {
  const words = ['apple', 'banana', 'chocolate', 'elephant', 'guitar', 'umbrella', 'ocean', 'penguin'];
  const randomIndex = Math.floor(Math.random() * words.length);
  return words[randomIndex];
}

const PORT = process.env.PORT || 3000;
server.listen(PORT, () => {
  console.log(`Server is running on http://localhost:${PORT}`);
});
