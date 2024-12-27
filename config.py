class Config:
    SECRET_KEY = 'your-secret-key-here'
    DEBUG = True
    PORT = 5000
    # Отключаем перезагрузку при изменении файлов
    USE_RELOADER = False
    # Привязываемся только к localhost
    SERVER_NAME = None
    PREFERRED_URL_SCHEME = 'http' 