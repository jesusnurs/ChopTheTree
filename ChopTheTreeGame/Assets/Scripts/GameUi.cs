using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUi : MonoBehaviour
{
    [SerializeField] private StateMachine stateMachine;
    [SerializeField] private TimerController timerController;
    
    [Space]
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button giveUpButton;
    [SerializeField] private TextMeshProUGUI currentChoppedLogsText;
    [SerializeField] private TextMeshProUGUI bestChoppedLogsText;
    [SerializeField] private Slider timerBar;

    [Space] 
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameUiPanel;
    
    [Space]
    [SerializeField] private RectTransform settingsPanel;
    [SerializeField] private Vector2 settingsFromToPos;
    [SerializeField] private float animationDuration = 0.5f;
    
    private int bestChoppedLogs;
    private int currentChoppedLogs;
    private bool isSettingsVisible;
    
    private const string BestChoppedLogsKey = "BestChoppedLogs";

    private void Awake()
    {
        stateMachine.OnGameEnded += GameOver;
        stateMachine.OnChopLog += IncreaseChoppedLogsCount;
        stateMachine.OnMainMenuOpened += MainMenu;

        timerController.OnTimerUpdated += UpdateTimer;
        
        settingsButton.onClick.AddListener(ToggleMenu);
        playButton.onClick.AddListener(StartGame);
        continueButton.onClick.AddListener(ContinueGame);
        giveUpButton.onClick.AddListener(stateMachine.OpenMainMenu);
        
        bestChoppedLogs = PlayerPrefs.GetInt(BestChoppedLogsKey, 0);
        bestChoppedLogsText.text = bestChoppedLogs.ToString();
        currentChoppedLogsText.text = 0.ToString();
    }
    
    private void ToggleMenu()
    {
        RotateGear();
        
        float endValue = isSettingsVisible ? settingsFromToPos.x : settingsFromToPos.y;
        isSettingsVisible = !isSettingsVisible;

        if (isSettingsVisible)
        {
        }
        else
        {
        }
        
        settingsPanel.DOAnchorPosX(endValue, animationDuration).SetEase(Ease.InOutQuad);
    }
    
    private void RotateGear()
    {
        settingsButton.transform.DORotate(new Vector3(0, 0, isSettingsVisible ? -360 : 360), animationDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.InOutQuad);
    }

    private void StartGame()
    {
        stateMachine.StartGame();
        
        gameUiPanel.SetActive(true);
        playButton.gameObject.SetActive(false);
        settingsButton.gameObject.SetActive(false);
        timerBar.gameObject.SetActive(true);
        isSettingsVisible = false;
        
        currentChoppedLogs = 0;
        currentChoppedLogsText.text = currentChoppedLogs.ToString();
    }

    private void MainMenu()
    {
        gameUiPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        settingsButton.gameObject.SetActive(true);
        playButton.gameObject.SetActive(true);
        timerBar.gameObject.SetActive(false);
    }

    private void ContinueGame()
    {
        stateMachine.ContinueGame();
        
        gameUiPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        playButton.gameObject.SetActive(false);
    }

    private void GameOver()
    {
        gameOverPanel.SetActive(true);
    }
    
    private void IncreaseChoppedLogsCount()
    {
        currentChoppedLogs++;
        currentChoppedLogsText.text = currentChoppedLogs.ToString();

        if (bestChoppedLogs < currentChoppedLogs)
        {
            PlayerPrefs.SetInt(BestChoppedLogsKey, currentChoppedLogs);
            bestChoppedLogs = currentChoppedLogs;
            bestChoppedLogsText.text = bestChoppedLogs.ToString();
        }
    }

    private void UpdateTimer()
    {
        timerBar.value = timerController.GetCurrentTimeRatio();
    }
}
