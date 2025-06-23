using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public event Action OnPlayerFall;

    [SerializeField] PlayerInfos playerInfos;

    [SerializeField] AudioClip jingleStart;
    [SerializeField] AudioClip jingleEnd;

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameObject audioObj = new GameObject("2D Audio");
        AudioSource audioSource = audioObj.AddComponent<AudioSource>();

        audioSource.clip = jingleStart;
        audioSource.Play();

        Destroy(audioObj, jingleStart.length);
    }

    public void OnPlayerFallFCT()
    {
        playerInfos.hasDiedOnce = true;
        OnPlayerFall?.Invoke();

        GameObject audioObj = new GameObject("2D Audio");
        AudioSource audioSource = audioObj.AddComponent<AudioSource>();

        audioSource.clip = jingleEnd;
        audioSource.Play();

        Destroy(audioObj, jingleEnd.length);
        
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
