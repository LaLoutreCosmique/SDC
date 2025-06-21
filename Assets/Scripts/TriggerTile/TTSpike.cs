using UnityEngine;

public class TTSpike : TriggerTile
{
	public override void OnPlayerTrigger(Player player)
	{
		GameManager.Instance.OnPlayerDieFCT();
	}
}
