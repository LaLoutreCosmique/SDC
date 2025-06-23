using UnityEngine;


public enum PropType
{
	None,
	Grass,
	Remains,
	Rock,
	Weapons,
	RandomObject,
	Gold,
	Mushrooms
}

public class Prop : MonoBehaviour
{
	public PropType propType = PropType.None;
}
