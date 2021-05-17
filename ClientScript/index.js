const WebSocket = require('ws');
const prompt  = require('prompt');

const ws = new WebSocket('ws://localhost:1234/animations')

function do_prompt() {
  prompt.get(['command'], (err, result) => {
    msg = result.command;
    console.log("Sending:\n%o", msg);
    ws.send(msg)
    do_prompt()
  });
}

ws.on('message', (msg) => {
  console.log("Received:\n%o", msg);
});

ws.on('open', () => {
  do_prompt()
})

ws.on("error", (err) => {
  console.log("Error:\n%o", err);
});

prompt.start();
