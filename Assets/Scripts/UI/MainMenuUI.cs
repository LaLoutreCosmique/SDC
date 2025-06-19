using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] Button m_PlayButton;

    private void Start()
    {
        m_PlayButton.onClick.AddListener(Play);
    }

    void Play()
    {
        SceneManager.LoadScene("Scenes/Game");
    }
}
