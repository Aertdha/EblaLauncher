const { ipcRenderer } = require('electron')

ipcRenderer.send('get-games');
ipcRenderer.on('games-list', (event, games) => {
    displayGames(games);
}); 