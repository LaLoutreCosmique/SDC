using System;
using UnityEngine;
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
    
    private bool m_IsPaused;
    private bool m_IsSfxEnabled;
    private bool m_IsMusicEnabled;

    private void Start()
    {
        m_PauseButton.onClick.AddListener(TogglePause);
        m_ResumeButton.onClick.AddListener(ResumeGame);
        m_SfxButton.onClick.AddListener(ToggleSfx);
        m_MusicButton.onClick.AddListener(ToggleMusic);
        m_CalibrateButton.onClick.AddListener(m_InputManager.CalibrateGyro);
        
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

    public void ToggleSfx()
    {
        
    }

    public void ToggleMusic()
    {
        
    }
}
