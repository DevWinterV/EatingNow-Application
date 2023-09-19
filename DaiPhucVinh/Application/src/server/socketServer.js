const http = require('http');
const express = require('express');
const socketIO = require('socket.io');

const app = express();
const server = http.createServer(app);
const io = socketIO(server);

io.on('connection', (socket) => {
  console.log('A user connected');

  socket.on('disconnect', () => {
    console.log('User disconnected');
  });

  // Lắng nghe sự kiện gửi thông báo từ máy chủ và gửi nó tới tất cả các trình duyệt đang kết nối
  socket.on('sendNotification', (message) => {
    io.emit('receiveNotification', message);
  });
});

const PORT = process.env.PORT || 4000;
server.listen(PORT, () => {
  console.log(`Server is running on port ${PORT}`);
});
