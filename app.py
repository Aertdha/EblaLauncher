from flask import Flask, render_template
from flask_cors import CORS

app = Flask(__name__)
CORS(app)  # Включаем CORS для разработки

@app.route('/')
def index():
    return render_template('index.html', title="Game Launcher")

@app.route('/api/games')
def get_games():
    # Здесь будет логика получения списка игр
    return {
        'games': [
            {'id': 1, 'name': 'Test Game 1', 'status': 'installed'},
            {'id': 2, 'name': 'Test Game 2', 'status': 'not_installed'}
        ]
    }

@app.route('/login')
def login():
    return {"status": "ok"}

@app.route('/download')
def download():
    return {"status": "downloading"}

if __name__ == '__main__':
    app.run(debug=True) 