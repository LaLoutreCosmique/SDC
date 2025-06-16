using NaughtyAttributes;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum DoorSpecificType
{
	LevierCombo,
	LevierOrder,
	Delayed,
	Cooldown,
	DoorCondition,
	Reset
}

[System.Serializable]
public struct DoorStateCondition
{
	public int doorID;
	public bool shouldBeOpen;
}

[System.Serializable]
public struct DoorCondition
{
	public DoorSpecificType type;
	public float delay;
	public float duration;
	public List<DoorStateCondition> otherDoorsCondition;
	public List<int> order;
}

public class TTDoor : TriggerTile
{
	[SerializeField] private bool isOpen = false;
	private Animator animator;
	[SerializeField] private List<DoorCondition> conditions;

	private List<Tile> roomTiles;
	private List<TTLever> roomLevers;
	private List<TTDoor> roomDoors;

	private void Start()
	{
		animator = model.GetComponent<Animator>();
		animator.SetBool("IsOpen", isOpen);
		animator.Play(isOpen ? "Opened" : "Closed");
		roomTiles = parentRoom.GetAllTiles();
		roomLevers = roomTiles.Where(tile => tile.triggerType == "TTLever").Select(tile => tile.GetComponent<TTLever>()).ToList();
		roomDoors = roomTiles.Where(tile => tile.triggerType == "TTDoor").Select(tile => tile.GetComponent<TTDoor>()).ToList();
	}

	public override void OnTileTrigger()
	{
		if (!isOpen && CheckAllConditions())
		{
			OpenDoor();
		}
		else
		{
			CloseDoor();
		}
	}

	public override void TTReset()
	{
		CloseDoor();
	}

	private void OpenDoor()
	{
		animator.SetBool("IsOpen", true);
		isOpen = true;
	}

	private void CloseDoor()
	{
		animator.SetBool("IsOpen", false);
		isOpen = false;
	}

	public bool CheckAllConditions()
	{
		foreach (var cond in conditions)
		{
			if (!ConditionMet(cond))
				return false;
		}
		return true;
	}

	private bool ConditionMet(DoorCondition cond)
	{
		switch (cond.type)
		{
			case DoorSpecificType.LevierCombo:
				return triggerIds.All(id => roomLevers.Any(lever => lever.triggerIds.Contains(id) && lever.IsActivated()));

			case DoorSpecificType.LevierOrder:
				foreach (int id in cond.order)
				{
					var lever = roomLevers.FirstOrDefault(l => l.triggerIds.Contains(id));
					if (lever == null || !lever.IsActivated())
					{
						foreach (var orderLever in roomLevers)
							orderLever.TTReset();
						return false;
					}
				}
				return true;

			case DoorSpecificType.Delayed:
				StartCoroutine(DelayedOpen(cond.delay));
				return false;

			case DoorSpecificType.Cooldown:
				if (isOpen)
				{
					StartCoroutine(CooldownClose(cond.duration));
					return false;
				}
				else
				{
					StartCoroutine(CooldownClose(cond.duration));
					return true;
				}

			case DoorSpecificType.DoorCondition:
				return cond.otherDoorsCondition.All(doorCondition =>
				{
					var door = roomDoors.FirstOrDefault(d => d.triggerIds.Contains(doorCondition.doorID));
					return door != null && door.isOpen == doorCondition.shouldBeOpen;
				});

			case DoorSpecificType.Reset:
				foreach (var door in roomDoors)
				{
					door.TTReset();
				}
				return true;

			default:
				Debug.LogWarning($"Error condition type: {cond.type} for door name : {name}");
				return false;
		}
	}

	private IEnumerator DelayedOpen(float delay)
	{
		yield return new WaitForSeconds(delay);
		OpenDoor();
	}

	private IEnumerator CooldownClose(float delay)
	{
		yield return new WaitForSeconds(delay);
		CloseDoor();
	}
}
