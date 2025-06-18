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
}
