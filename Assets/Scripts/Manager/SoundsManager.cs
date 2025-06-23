using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{

    public static SoundsManager Instance;

    public bool grounded;

    public AudioSource audioSourceRoll;
    public AudioSource audioSourceWind;

    public AudioClip[] impacts;


    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    //private bool isRolling;

    //public void UpdateRollSound(float magnitude)
    //{
    //    StartCoroutine(IUpdateRollSound(magnitude));
    //}

    //public IEnumerator IUpdateRollSound(float magnitude)
    //{
    //    if (grounded && magnitude > 0.05f)
    //    {
    //        float vol = Mathf.Clamp(magnitude, 0.05f, 5f);
    //        vol /= 15;


    //        while (audioSourceRoll.volume != vol)
    //        {
    //            audioSourceWind.volume = Mathf.Lerp(audioSourceRoll.volume, vol, /*0f * Time.deltaTime*/0.05f);
    //            yield return 0f;
    //        }

    //        audioSourceWind.volume = vol * 2;

    //        if (!isRolling)
    //        {
    //            audioSourceRoll.Play();
    //            audioSourceWind.Play();

    //            isRolling = true;
    //        }
    //    }
    //    else {
    //        while (audioSourceWind.volume != 0.20f)
    //        {
    //            audioSourceWind.volume = Mathf.Lerp(audioSourceWind.volume, 0.20f, 1f * Time.deltaTime);
    //            yield return 0f;
    //        }

    //        if (isRolling)
    //        {
    //            audioSourceRoll.Stop();

    //            isRolling = false;
    //        }
    //    }
    //}
}
