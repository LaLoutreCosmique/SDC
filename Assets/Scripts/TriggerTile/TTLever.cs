using System.Linq;
using UnityEngine;

public class TTLever : TriggerTile
{
	[SerializeField] private bool activated = false;
	private Animator animator;

	[SerializeField] private AudioClip clipLeverOn, clipLeverOff;
	[SerializeField] private AudioClip clipDoorOn, clipDoorOff;

    private void Awake()
    {

        clipLeverOn = (AudioClip)Resources.Load("Sounds/Interactable_Lever_On");
        clipLeverOff = (AudioClip)Resources.Load("Sounds/Interactable_Lever_Off");
    }

    private void Start()
	{
		animator = model.GetComponent<Animator>() ?? model.AddComponent<Animator>();
		animator.SetBool("Activated", activated);
	}

	public override void OnPlayerTrigger(Player player)
	{
		foreach (var tile in parentRoom.GetAllTiles())
		{
			if (tile.triggerType == "TTDoor")
			{
				var door = tile.GetComponent<TTDoor>();
				if (door != null && triggerIds.Any(id => door.triggerIds.Contains(id)))
				{
					door.OnTileTrigger();
				}
			}
		}
		activated = !activated;
		animator.SetBool("Activated", activated);

        if (activated)
        {
            GameObject audioObj = new GameObject("Lever Audio");
            audioObj.transform.position = transform.position;
            AudioSource audioSource = audioObj.AddComponent<AudioSource>();

            audioSource.outputAudioMixerGroup = SoundsManager.Instance.sfxAudioMixer;
            audioSource.volume = .17f;
            audioSource.PlayOneShot(clipLeverOn);
            //Handheld.Vibrate();
        }
        else
        {
            GameObject audioObj = new GameObject("Lever Audio");
            audioObj.transform.position = transform.position;
            AudioSource audioSource = audioObj.AddComponent<AudioSource>();

            audioSource.outputAudioMixerGroup = SoundsManager.Instance.sfxAudioMixer;
            audioSource.volume = .17f;
            audioSource.PlayOneShot(clipLeverOff);
            //Handheld.Vibrate();
        }
	}

	public override void TTReset()
	{
		activated = false;
		animator.SetBool("Activated", activated);
	}

	public bool IsActivated() => activated;
}
