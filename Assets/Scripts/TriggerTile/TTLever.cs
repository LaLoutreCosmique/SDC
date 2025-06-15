using System.Linq;
using UnityEngine;

public class TTLever : TriggerTile
{
	public override void OnPlayerTrigger(Player player)
	{
		parentRoom.GetAllTiles().ForEach(tile =>
		{
			if (tile.triggerType == "TTDoor")
			{
				TTDoor door = tile.GetComponent<TTDoor>();
				if (door != null)
				{
					if (triggerIds.Any(id => door.triggerIds.Contains(id)))
						door.OnTileTrigger();
				}
			}
		});
	}
}
