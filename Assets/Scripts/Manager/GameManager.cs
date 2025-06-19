using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public event Action OnPlayerFall;

    [SerializeField] PlayerInfos playerInfos;

    void Awake()
    {
        Instance = this;
    }

    public void OnPlayerFallFCT()
    {
        playerInfos.hasDiedOnce = true;
        OnPlayerFall?.Invoke();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }
}
