# EblaLauncher

Кастомный лаунчер для игр с возможностью загрузки и управления играми.

## 🚀 Технологии

- Backend: C# (.NET 7+)
- Frontend: HTML, CSS, JavaScript
- Desktop: Electron.NET
- Database: SQLite (planned)

## 📋 Требования

- .NET SDK 7.0+
- Node.js 16+
- npm 8+
- ElectronNET.CLI (глобально)

## 🛠 Установка

1. Клонируйте репозиторий:
   git clone https://github.com/Aertdha/EblaLauncher.git
   cd EblaLauncher

2. Установите глобальные зависимости:
   dotnet tool install ElectronNET.CLI -g

3. Восстановите зависимости проекта:
   dotnet restore
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
