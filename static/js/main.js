document.addEventListener('DOMContentLoaded', () => {
    fetchGames();
});

async function fetchGames() {
    try {
        const response = await fetch('http://localhost:5000/api/games');
        const data = await response.json();
        displayGames(data.games);
    } catch (error) {
        console.error('Error fetching games:', error);
    }
}

function displayGames(games) {
    const gamesList = document.getElementById('games-list');
    gamesList.innerHTML = games.map(game => `
        <div class="game-item">
            <h3>${game.name}</h3>
            <p>Status: ${game.status}</p>
        </div>
    `).join('');
} 