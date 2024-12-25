# EblaLauncher

Кастомный лаунчер для игр с возможностью загрузки и управления играми.

## 🚀 Технологии

- Backend: Python/Flask
- Frontend: HTML, CSS, JavaScript
- Desktop: Electron
- Database: SQLite (planned)

## 📋 Требования

- Python 3.8+
- Node.js 16+
- npm 8+

## 🛠 Установка

1. Клонируйте репозиторий:
   git clone https://github.com/Aertdha/EblaLauncher.git
   cd EblaLauncher

2. Настройте Python окружение:
   python -m venv venv

   # Windows
   .\venv\Scripts\activate

   # Linux/MacOS
   source venv/bin/activate

   pip install -r requirements.txt

3. Установите Node.js зависимости:
   npm install

## 🚦 Запуск

1. Запустите Flask сервер:
   python app.py

2. В новом терминале запустите Electron приложение:
   npm run dev

## 🌟 Текущий функционал

- [ ] Авторизация пользователей
- [ ] Каталог игр
- [ ] Загрузка игр
- [ ] Управление установленными играми
- [ ] Автообновление

## 🤝 Контрибуция

Если вы хотите внести свой вклад в проект, пожалуйста, ознакомьтесь с руководством по контрибуции в файле CONTRIBUTING.md.

## 📝 Лицензия

MIT License
