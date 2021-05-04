const WebSocket = require('ws');
const prompt  = require('prompt');

const ws = new WebSocket('ws://localhost:32919/add-cube')

ws.on('message', () => {
  console.log(err.data);
});

ws.on("error", (err) => {
  console.log(err);
  console.log(err.stack);
});

prompt.start();

prompt.get(['name', 'x', 'y', 'z'], (err, result) => {
  msg = result.name + ' ' + result.x + ' ' + result.y + ' ' + result.z;
  ws.send(msg)
});
