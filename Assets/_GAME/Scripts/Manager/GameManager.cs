namespace _GAME.Scripts
{
    using UnityEngine;
    using System;
    using _GAME.Scripts.Enum;

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
    
        public event Action<GameState> OnGameStateChanged;
    
        [SerializeField] private LevelManager levelManager;
    
        private GameState _gameState;
        private int _currentLevel = 0;
    
        public GameState CurrentGameState
        {
            get { return _gameState; }
        }
    
        public int CurrentLevel
        {
            get { return _currentLevel; }
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        
            SetGameState(GameState.MainMenu);
        }

        public void SetGameState(GameState newState)
        {
            if (_gameState == newState) return;
        
            _gameState = newState;
        
            switch (newState)
            {
                case GameState.MainMenu:
                    Time.timeScale = 1f;
                    break;
                case GameState.Playing:
                    Time.timeScale = 1f;
                    break;
                case GameState.Paused:
                    Time.timeScale = 0f;
                    break;
                case GameState.Win:
                    Time.timeScale = 1f;
                    break;
                case GameState.Lose:
                    Time.timeScale = 1f;
                    break;
            }
        
            OnGameStateChanged?.Invoke(newState);
        }

        public void StartGame()
        {
            _currentLevel = 0;
            levelManager.LoadLevel(_currentLevel);
            SetGameState(GameState.Playing);
        }

        public void NextLevel()
        {
            _currentLevel++;
            _currentLevel %= 3; // Cycle through available levels
            levelManager.LoadLevel(_currentLevel);
            SetGameState(GameState.Playing);
        }

        public void RestartLevel()
        {
            levelManager.LoadLevel(_currentLevel);
            SetGameState(GameState.Playing);
        }

        public void ResumeGame()
        {
            SetGameState(GameState.Playing);
        }

        public void PauseGame()
        {
            SetGameState(GameState.Paused);
        }

        public void ReturnToMenu()
        {
            SetGameState(GameState.MainMenu);
        }
    }
}