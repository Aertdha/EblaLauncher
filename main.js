const { app, BrowserWindow } = require('electron')
const path = require('path')

/**
 * Создает и настраивает основное окно приложения
 * с отключенным системным оформлением и поддержкой Node.js
 */
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

    win.loadURL('http://localhost:5000')
    
    // Включаем инструменты разработчика в режиме разработки
    win.webContents.openDevTools()
    
    console.log('Window created!')
}

app.whenReady().then(() => {
    console.log('Electron app is ready')
    createWindow()

    // Пересоздаем окно на macOS при активации приложения
    app.on('activate', () => {
        if (BrowserWindow.getAllWindows().length === 0) {
            createWindow()
        }
    })
})

// Закрываем приложение при закрытии всех окон (кроме macOS)
app.on('window-all-closed', () => {
    if (process.platform !== 'darwin') {
        app.quit()
    }
})