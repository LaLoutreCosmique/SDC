using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private GameObject m_LoadingPanel;
    [SerializeField] private Slider m_LoadingSlider;

    private void Start()
    {
        m_LoadingPanel.transform.localScale = new Vector3(0, 1, 1);
    }

    public void LoadScene(string sceneName)
    {
        m_LoadingPanel.SetActive(true);
        m_LoadingPanel.transform.DOScaleX(1, .5f).SetEase(Ease.OutBounce).OnComplete(() => StartCoroutine(LoadSceneRoutine(sceneName)));
    }

    IEnumerator LoadSceneRoutine(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (operation != null && !operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            m_LoadingSlider.value = progress;
            yield return null;
        }
    }
}
