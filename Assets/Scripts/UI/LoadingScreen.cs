using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private GameObject m_LoadingPanel;
    [SerializeField] private Slider m_LoadingSlider;
    
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    IEnumerator LoadSceneRoutine(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        m_LoadingPanel.SetActive(true);

        while (operation != null && !operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            m_LoadingSlider.value = progress;
            yield return null;
        }
    }
}
