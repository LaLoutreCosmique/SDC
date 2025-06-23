using System;
using UnityEngine;

public class ChangeLevelTrigger : MonoBehaviour
{
    [SerializeField] Room room;
    [SerializeField] Collider NoReturnCollider;


    [SerializeField] private AudioClip levelCompleteSound;

    bool triggered = false;

    private void Awake()
    {
        levelCompleteSound = (AudioClip)Resources.Load("Sounds/Jingle_RoomComplete");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            room.ChangeLevel();
            triggered = true;
            NoReturnCollider.enabled = true;

            GameObject audioObj = new GameObject("2D Audio");
            AudioSource audioSource = audioObj.AddComponent<AudioSource>();

            audioSource.outputAudioMixerGroup = SoundsManager.Instance.sfxAudioMixer;
            audioSource.volume = .35f;
            audioSource.PlayOneShot(levelCompleteSound);

            Destroy(audioObj, levelCompleteSound.length);

            //AudioSource.PlayClipAtPoint(levelCompleteSound, transform.position, 0.25f);
        }
    }
}
