using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] InputManager m_InputManager;
    [SerializeField] LoadingScreen m_LoadingScreen;
    
    [Header("PAUSE")]
    [SerializeField] private GameObject m_PauseMenu;
    [SerializeField] private Button m_PauseButton;
    [SerializeField] private Button m_ResumeButton;
    [SerializeField] private Button m_PauseRestartButton;
    [SerializeField] private Button m_SfxButton;
    [SerializeField] private Button m_MusicButton;
    [SerializeField] private Button m_CalibrateButton;
    [SerializeField] private Button m_DayModeButton;
    [SerializeField] private Button m_QuitButton;

    [Header("SCORE")]
    [SerializeField] private TextMeshProUGUI m_RoomCount;
    
    [Header("END POPUP")]
    [SerializeField] private GameObject m_EndPopup;
    [SerializeField] private Button m_RestartButton;
    [SerializeField] private GameObject m_BestScoreRibbon;
    [SerializeField] private TextMeshProUGUI m_CurrentScoreText;
    [SerializeField] private TextMeshProUGUI m_BestScoreText;
    [SerializeField] private GameObject m_NewBestScore;
    
    private bool m_IsPaused;
    private bool m_IsSfxEnabled;
    private bool m_IsMusicEnabled;
    private int m_CurrentScore;
    private bool m_IsDay;
    
    const string m_BestScoreKey = "BEST_SCORE";
    const string m_DayModeKey = "DAY_MODE";

    private void Start()
    {
        m_PauseButton.onClick.AddListener(TogglePause);
        m_ResumeButton.onClick.AddListener(ResumeGame);
        m_PauseRestartButton.onClick.AddListener(() =>
        {
            GameManager.Instance.OnPlayerFallFCT();
            ResumeGame();
        });
        m_SfxButton.onClick.AddListener(ToggleSfx);
        m_MusicButton.onClick.AddListener(ToggleMusic);
        m_CalibrateButton.onClick.AddListener(m_InputManager.CalibrateGyro);
        m_DayModeButton.onClick.AddListener(ToggleDayMode);
        m_QuitButton.onClick.AddListener(GoToMainMenu);
        m_RestartButton.onClick.AddListener(RestartGame);
        
        if (PlayerPrefs.GetInt(m_DayModeKey) == 1)
            ToggleDayMode();
        
        LevelGenerator.Instance.OnLevelCompleted.AddListener(UpdateScore);
        
        GameManager.Instance.OnPlayerFall += DisplayEndPopup;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnPlayerFall -= DisplayEndPopup;
    }

    void TogglePause()
    {
        if (m_IsPaused)
            ResumeGame();
        else
            PauseGame();
    }

    void PauseGame()
    {
        m_IsPaused = true;
        GameManager.Instance.PauseGame();
        m_PauseMenu.SetActive(true);
    }

    void ResumeGame()
    {
        m_IsPaused = false;
        GameManager.Instance.ResumeGame();
        m_PauseMenu.SetActive(false);
    }

    void ToggleSfx()
    {
        Debug.Log("Y A PAS DE SON AAAAAH");
    }

    void ToggleMusic()
    {
        Debug.Log("Y A PAS DE MUSIQUE AAAAAAH");
    }

    void GoToMainMenu()
    {
        GameManager.Instance.ResumeGame();
        SceneManager.LoadScene("Scenes/MainMenu");
    }

    void UpdateScore()
    {
        m_CurrentScore++;
        m_RoomCount.text = m_CurrentScore.ToString();
    }

    void DisplayEndPopup()
    {
        m_EndPopup.SetActive(true);
        
        int best = PlayerPrefs.GetInt(m_BestScoreKey);
        if (m_CurrentScore > best)
        {
            m_BestScoreRibbon.SetActive(true);
            m_NewBestScore.SetActive(true);
            best = m_CurrentScore;
            PlayerPrefs.SetInt(m_BestScoreKey, best);
        }
        
        m_CurrentScoreText.text = m_CurrentScore.ToString();
        m_BestScoreText.text = best.ToString();
    }

    void RestartGame()
    {
        m_LoadingScreen.LoadScene(SceneManager.GetActiveScene().name);
    }

    void ToggleDayMode()
    {
        if (m_IsDay)
        {
            GameManager.Instance.SetNight();
            m_DayModeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Enable Day Mode";
            PlayerPrefs.SetInt(m_DayModeKey, 0);
        }
        else
        {
            GameManager.Instance.SetDay();
            m_DayModeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Disable Day Mode";
            PlayerPrefs.SetInt(m_DayModeKey, 1);
        }
        
        m_IsDay = !m_IsDay;
    }
}
