using NaughtyAttributes;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.Tilemaps.Tilemap;

[Serializable]
public enum Orientation : int
{
	Ground = 0,
	Left = -3,
	Right = 3,
	Bottom = -1,
	Top = 1
}

public class TTSpike : TriggerTile
{
	[SerializeField] [OnValueChanged("ChangeOrientation")] private Orientation orientation;

	private void ChangeOrientation()
	{
		Vector3 nextPos = Vector3.zero;
		Quaternion nextQuat = Quaternion.identity;
		if (orientation == Orientation.Ground)
		{
			nextPos = Vector3.up;
		}
		else
		{
			int orient = (int)(orientation);
			nextPos.x = Math.Abs(orient) == 3 ? Math.Sign(orient) * 0.5f : 0;
			nextPos.y = 1f;
			nextPos.z = Math.Abs(orient) == 1 ? orient * 0.5f : 0;

			nextQuat.eulerAngles = new Vector3(
				Math.Abs(orient) == 1 ? -orient * 90f : 0,
				0f,
				Math.Abs(orient) == 3 ? Math.Sign(orient) * 90f : 0);
		}


		model.transform.localPosition = nextPos;
		model.transform.localRotation = nextQuat;
	}

	public override void OnPlayerTrigger(Player player)
	{
		GameManager.Instance.OnPlayerDieFCT();
	}
}
