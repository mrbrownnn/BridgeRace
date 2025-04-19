namespace _GAME.Scripts
{
   using UnityEngine;
   using UnityEngine.UI;
   using _GAME.Scripts.Enum;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gameplayPanel;
    [SerializeField] private GameObject levelCompletePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button playButton;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private Text levelText;
    [SerializeField] private Joystick joystick;

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
        
        // Set up button listeners
        if (playButton != null) playButton.onClick.AddListener(OnPlayClicked);
        if (nextLevelButton != null) nextLevelButton.onClick.AddListener(OnNextLevelClicked);
        if (retryButton != null) retryButton.onClick.AddListener(OnRetryClicked);
        if (resumeButton != null) resumeButton.onClick.AddListener(OnResumeClicked);
        if (menuButton != null) menuButton.onClick.AddListener(OnMenuClicked);
        
        // Default UI state
        ShowUI(GameState.MainMenu);
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
        }
        
        // Remove button listeners
        if (playButton != null) playButton.onClick.RemoveListener(OnPlayClicked);
        if (nextLevelButton != null) nextLevelButton.onClick.RemoveListener(OnNextLevelClicked);
        if (retryButton != null) retryButton.onClick.RemoveListener(OnRetryClicked);
        if (resumeButton != null) resumeButton.onClick.RemoveListener(OnResumeClicked);
        if (menuButton != null) menuButton.onClick.RemoveListener(OnMenuClicked);
    }

    private void HandleGameStateChanged(GameState newState)
    {
        ShowUI(newState);
        UpdateLevelText();
    }

    public void ShowUI(GameState state)
    {
        // Hide all panels first
        mainMenuPanel.SetActive(false);
        gameplayPanel.SetActive(false);
        levelCompletePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        pausePanel.SetActive(false);
        
        // Show appropriate panel
        switch (state)
        {
            case GameState.MainMenu:
                mainMenuPanel.SetActive(true);
                break;
            case GameState.Playing:
                gameplayPanel.SetActive(true);
                break;
            case GameState.Win:
                levelCompletePanel.SetActive(true);
                break;
            case GameState.Lose:
                gameOverPanel.SetActive(true);
                break;
            case GameState.Paused:
                pausePanel.SetActive(true);
                break;
        }
    }

    private void UpdateLevelText()
    {
        if (levelText != null)
        {
            levelText.text = "Level " + (GameManager.Instance.CurrentLevel + 1);
        }
    }

    public Joystick GetJoystick()
    {
        return joystick;
    }

    // Button handlers
    private void OnPlayClicked()
    {
        GameManager.Instance.StartGame();
    }

    private void OnNextLevelClicked()
    {
        GameManager.Instance.NextLevel();
    }

    private void OnRetryClicked()
    {
        GameManager.Instance.RestartLevel();
    }

    private void OnResumeClicked()
    {
        GameManager.Instance.ResumeGame();
    }

    private void OnMenuClicked()
    {
        GameManager.Instance.ReturnToMenu();
    }
}
}