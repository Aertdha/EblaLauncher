const { createApp } = Vue

createApp({
    data() {
        return {
            title: 'EbLauncher',
            message: 'Загрузка приложения...'
        }
    },
    mounted() {
        // Здесь будет инициализация приложения
        console.log('Vue приложение запущено');
    },
    methods: {
        // Методы приложения
    }
}).mount('#app') 