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
   ```bash
   git clone https://github.com/Aertdha/EblaLauncher.git
   cd EblaLauncher
   ```

2. Установите глобальные зависимости:
   ```bash
   dotnet tool install ElectronNET.CLI -g
   ```

3. Восстановите зависимости проекта:
   ```bash
   dotnet restore
   ```

## 🚦 Запуск

1. Запустите в режиме разработки:
   ```bash
   electronize start
   ```

2. Для сборки релиза:
   ```bash
   electronize build /target win
   ```

## 🌟 Текущий функционал

- [ ] Авторизация пользователей
- [ ] Каталог игр
- [ ] Загрузка игр
- [ ] Управление установленными играми
- [ ] Автообновление

## 🤝 Разработка

- Используется IPC для связи фронтенда с беком
- Прямой доступ к Windows API через C#
- Hot Reload в режиме разработки
- Автоматическая сборка для Windows

## 🤝 Контрибуция

Если хочешь внести свой вклад в проект, ознакомься с руководством по контрибуции в файле CONTRIBUTING.md.

## 📝 Лицензия

MIT License
