using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] InputManager m_InputManager;
    
    [Header("PAUSE")]
    [SerializeField] private GameObject m_PauseMenu;
    [SerializeField] private Button m_PauseButton;
    [SerializeField] private Button m_ResumeButton;
    [SerializeField] private Button m_SfxButton;
    [SerializeField] private Button m_MusicButton;
    [SerializeField] private Button m_CalibrateButton;
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
    
    const string m_BestScoreKey = "BEST_SCORE";

    private void Start()
    {
        m_PauseButton.onClick.AddListener(TogglePause);
        m_ResumeButton.onClick.AddListener(ResumeGame);
        m_SfxButton.onClick.AddListener(ToggleSfx);
        m_MusicButton.onClick.AddListener(ToggleMusic);
        m_CalibrateButton.onClick.AddListener(m_InputManager.CalibrateGyro);
        m_QuitButton.onClick.AddListener(GoToMainMenu);
        
        LevelGenerator.Instance.OnLevelCompleted.AddListener(UpdateScore);
        
        GameManager.Instance.OnPlayerFall += DisplayEndPopup;
        m_RestartButton.onClick.AddListener(RestartGame);
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
