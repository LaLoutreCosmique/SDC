using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] Button m_PlayButton;
    [SerializeField] LoadingScreen m_LoadingScreen;

    private void Start()
    {
        m_PlayButton.onClick.AddListener(Play);
    }

    void Play()
    {
        m_LoadingScreen.LoadScene("Scenes/Game");
    }
}
