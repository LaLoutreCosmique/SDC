using System.Linq;
using UnityEngine;

public class TTLever : TriggerTile
{
	[SerializeField] private bool activated = false;
	private Animator animator;

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
	}

	public override void TTReset()
	{
		activated = false;
		animator.SetBool("Activated", activated);
	}

	public bool IsActivated() => activated;
}
