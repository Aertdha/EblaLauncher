from PyQt6.QtWidgets import QApplication, QMainWindow
from PyQt6.QtWebEngineWidgets import QWebEngineView
from PyQt6.QtCore import QUrl, QTimer
import sys
import time

class MainWindow(QMainWindow):
    """
    Основное окно приложения, реализующее встроенный браузер на базе QtWebEngine.
    Обеспечивает отображение веб-интерфейса, запущенного на локальном Flask-сервере.
    """
    def __init__(self):
        super().__init__()
        self.setWindowTitle("EbLauncher")
        # Устанавливаем стандартное положение и размер окна
        self.setGeometry(100, 100, 1200, 800)

        # Инициализируем компонент браузера и настраиваем его на локальный адрес
        self.browser = QWebEngineView()
        self.browser.setUrl(QUrl("http://127.0.0.1:5000"))
        self.setCentralWidget(self.browser)

        # Инициализируем отложенную перезагрузку страницы для гарантированной
        # загрузки контента после полного запуска сервера
        self.reload_timer = QTimer()
        self.reload_timer.singleShot(200, self.reload_page)

    def reload_page(self):
        """Принудительное обновление страницы браузера"""
        self.browser.reload()

def run_desktop_app():
    """
    Точка входа для десктопного приложения.
    Инициализирует Qt-приложение и запускает главное окно.
    """
    # Задержка необходима для гарантированного запуска Flask-сервера
    time.sleep(1)
    
    app = QApplication(sys.argv)
    window = MainWindow()
    window.show()
    sys.exit(app.exec())