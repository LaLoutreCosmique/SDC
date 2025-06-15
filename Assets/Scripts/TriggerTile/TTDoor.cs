using System.Collections;
using UnityEngine;

public class TTDoor : TriggerTile
{
	[SerializeField] private bool isOpen = false;
	private Animator animator;

	private void Awake()
	{
		animator = model.GetComponent<Animator>();
		animator.SetBool("IsOpen", isOpen);
	}

	public override void OnTileTrigger()
	{
		if (!isOpen)
		{
			animator.SetBool("IsOpen", true);
			isOpen = true;
		}
		else
		{
			animator.SetBool("IsOpen", false);
			isOpen = false;
		}
	}
}
