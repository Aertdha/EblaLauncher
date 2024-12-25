const { app, BrowserWindow } = require('electron')
const path = require('path')

function createWindow() {
    console.log('Creating window from main.js...')
    const win = new BrowserWindow({
        width: 1200,
        height: 800,
        frame: false,
        webPreferences: {
            nodeIntegration: true,
            contextIsolation: false
        }
    })

    // Загружаем локальный URL
    win.loadURL('http://localhost:5000')
    
    // Для отладки
    win.webContents.openDevTools()
    
    console.log('Window created!')
}

app.whenReady().then(() => {
    console.log('Electron app is ready')
    createWindow()

    app.on('activate', () => {
        if (BrowserWindow.getAllWindows().length === 0) {
            createWindow()
        }
    })
})

app.on('window-all-closed', () => {
    if (process.platform !== 'darwin') {
        app.quit()
    }
}) 