using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] Button m_PlayButton;
    [SerializeField] Button m_ResetButton;
    [SerializeField] LoadingScreen m_LoadingScreen;

    private void Start()
    {
        m_PlayButton.onClick.AddListener(Play);
        m_ResetButton.onClick.AddListener(ResetScore);
    }

    void Play()
    {
        m_LoadingScreen.LoadScene("Scenes/Game");
    }

    void ResetScore()
    {
        PlayerPrefs.SetInt("BEST_SCORE", 0);
    }
}
