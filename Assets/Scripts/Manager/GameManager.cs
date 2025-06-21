using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public event Action OnPlayerFall;

    [SerializeField] PlayerInfos playerInfos;
    
    [Header("DAY MODE")]
    [SerializeField] private Material m_NightSkybox;
    [SerializeField] private Material m_DaySkybox;
    [SerializeField] private GameObject m_DayLight;

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

    public void SetDay()
    {
        RenderSettings.skybox = m_DaySkybox;
        m_DayLight.SetActive(true);
    }

    public void SetNight()
    {
        RenderSettings.skybox = m_NightSkybox;
        m_DayLight.SetActive(false);
    }
}
