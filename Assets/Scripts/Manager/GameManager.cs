using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public event Action OnPlayerFall;
    public event Action OnPlayerDie;

	[SerializeField] PlayerInfos playerInfos;
    
    [Header("DAY MODE")]
    [SerializeField] private Material m_NightSkybox;
    [SerializeField] private Material m_DaySkybox;
    [SerializeField] private GameObject m_DayLight;

    [SerializeField] AudioClip jingleStart;
    [SerializeField] AudioClip jingleEnd;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			this.enabled = false;
		}
	}

	private void Start()
    {
        GameObject audioObj = new GameObject("2D Audio");
        AudioSource audioSource = audioObj.AddComponent<AudioSource>();


        audioSource.outputAudioMixerGroup = SoundsManager.Instance.sfxAudioMixer;
        audioSource.volume = .35f;
        audioSource.PlayOneShot(jingleStart);

        Destroy(audioObj, jingleStart.length);
    }

    bool hasend;

    public void OnPlayerFallFCT()
    {
        playerInfos.hasDiedOnce = true;
        OnPlayerFall?.Invoke();


        if (!hasend)
        {
            hasend = true;


            GameObject audioObj = new GameObject("2D Audio");
            AudioSource audioSource = audioObj.AddComponent<AudioSource>();

            audioSource.outputAudioMixerGroup = SoundsManager.Instance.sfxAudioMixer;
            audioSource.volume = .35f;
            audioSource.PlayOneShot(jingleEnd);

            Destroy(audioObj, jingleEnd.length);
        }
        
    }

    public void OnPlayerDieFCT()
    {
        playerInfos.hasDiedOnce = true;
        OnPlayerDie?.Invoke();
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
        if (m_DayLight == null)
            m_DayLight = GameObject.FindWithTag("DayLight");
		m_DayLight.SetActive(true);
    }

    public void SetNight()
    {
        RenderSettings.skybox = m_NightSkybox;
		if (m_DayLight == null)
			m_DayLight = GameObject.FindWithTag("DayLight");
		m_DayLight.SetActive(false);
    }
}
