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
- [ ] К��талог игр
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

## 🔑 Настройка Google Drive

1. Создайте проект в Google Cloud Console
2. Включите Google Drive API
3. Создайте Service Account
4. Скачайте credentials.json
5. Скопируйте файл в корень проекта
6. Предоставьте доступ к папке на Google Drive для service account email

Пример структуры credentials.json можно найти в Examples/credentials.example.json
