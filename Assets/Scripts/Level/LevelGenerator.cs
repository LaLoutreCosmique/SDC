using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

[System.Serializable]
public class SpawnChances
{
	[Range(0, 100)] public float minChance;
	[Range(0, 100)] public float maxChance1;
	[Range(0, 100)] public float maxChance2;
	[Range(0, 100)] public float maxChance3;
}

public class LevelGenerator : MonoBehaviour
{
	public static LevelGenerator Instance;

	public UnityEvent OnLevelCompleted;
	
	/// <summary>
	/// 0 for random.
	/// </summary>
	[SerializeField] int seed = 0;
	[SerializeField] Transform cameraTest;

	List<Room> loadedRooms = new();
	List<List<Room>> staticBags = new();
	List<List<Room>> currentBags = new();
	public List<Room> spawnedRooms = new();
	System.Random bagsRnd;
	int currentRoomId = -1;
	private Vector3 nextPos;
	[SerializeField] Vector3 cameraOffset;

	private List<GameObject> props = new();
	[SerializeField] private SpawnChances propsSpawnChances;
	private float c1, c2, c3;

	[SerializeField] private GameObject ParticuleRoom;
	[SerializeField] private float particuleheight = -1.5f;

	const int bagsAmount = 2;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
	}

	void Start()
	{
		loadedRooms = Resources.LoadAll<Room>("Rooms/PlayableRooms").ToList();
		nextPos = cameraTest.position;

		props = Resources.LoadAll<GameObject>("Props").ToList();

		InitBags();
		SpawnNextRoom();
	}

	private void Update()
	{
		if (Keyboard.current.kKey.wasPressedThisFrame)
			SpawnNextRoom();

		
		cameraTest.transform.position = Vector3.Lerp(cameraTest.transform.position, nextPos, Time.deltaTime * 10);
	}

	void InitBags()
	{
		// create/get seed
		if (seed == 0) 
			seed = Random.Range(0, int.MaxValue);
		System.Random rnd = new System.Random(seed);
		bagsRnd = new System.Random(seed);

		// generate bags
		staticBags.Clear();
		List<Room> shuffledRooms = loadedRooms.OrderBy(x => rnd.Next()).ToList();

		int sliceSize = loadedRooms.Count() / bagsAmount;
		for (int bagIndex = 0; bagIndex < bagsAmount; bagIndex++)
		{
			// get shuffled slice
			int sliceStart = sliceSize * bagIndex;
			staticBags.Add(loadedRooms.GetRange(sliceStart, sliceSize).OrderBy(x => rnd.Next()).ToList());
		}
	}

	public void SpawnNextRoom()
	{
		currentRoomId++;

		// refill bags
		if (currentBags.Count == 0)
		{
			currentBags = new(staticBags);
			for (int bagIndex = 0; bagIndex < currentBags.Count(); bagIndex++)
				currentBags[bagIndex] = currentBags[bagIndex].OrderBy(x => bagsRnd.Next()).ToList();
		}

		// get next room
		Room nextRoom = currentBags[0][0];
		currentBags[0].RemoveAt(0);
		if (currentBags[0].Count == 0)
			currentBags.RemoveAt(0);

		Vector3 newRoomPos = spawnedRooms.Count > 0
			? Vector3.forward * (spawnedRooms[^1].transform.position.z + spawnedRooms[^1].roomLength)
			: Vector3.forward * 5; //TODO c'est super moche mais tant pis... 5 = taille de la firstRoom 
		Room newRoom = Instantiate(nextRoom, newRoomPos, Quaternion.identity);
		spawnedRooms.Add(newRoom);

		// remove previous rooms
		while (spawnedRooms.Count() > 4)
		{
			Destroy(spawnedRooms[0].gameObject);
			spawnedRooms.RemoveAt(0);
		}

		// cam pos
		if (spawnedRooms.Count != 1) // Disable cam movement for the first level
		{
			int roomWidth = spawnedRooms[^2].GetRoomWidth() * 2 + 1;
			int lvlOffset = spawnedRooms[^2].roomLength > roomWidth
				? spawnedRooms[^2].roomLength
				: roomWidth;

			nextPos = new Vector3(cameraOffset.x, cameraOffset.y + lvlOffset * 1.2f, cameraOffset.z - lvlOffset / 2f) + spawnedRooms[^2].transform.position;
		}

		// spawn Particule
		if (ParticuleRoom != null)
		{
			GameObject particule = Instantiate(ParticuleRoom, newRoom.transform.position, Quaternion.identity);
			particule.transform.SetParent(newRoom.transform);
			particule.transform.localPosition = new Vector3(0, particuleheight, 0);
			particule.GetComponent<ParticleSystem>().Play();
		}

		// spawn props
		foreach (Tile tile in newRoom.GetAllTiles())
		{
			if (tile.model == Tile.Model.Floor && tile.triggerType == "None")
			{
				c3 = Random.Range(propsSpawnChances.minChance, propsSpawnChances.maxChance3);
				c2 = Random.Range(c3, propsSpawnChances.maxChance2);
				c1 = Random.Range(c2, propsSpawnChances.maxChance1);
				float randomPick = Random.Range(0, 100);
				int count = (randomPick < c3) ? 3 :
							(randomPick < c2) ? 2 :
							(randomPick < c1) ? 1 : 0;

				PropType randomProptype = PropType.None;
				float chancePropType = Random.Range(0, 100);
				randomProptype = chancePropType < 50 ? PropType.Grass :
					chancePropType < 65 ? PropType.Remains :
					chancePropType < 75 ? PropType.Rock :
					chancePropType < 85 ? PropType.Weapons :
					chancePropType < 90 ? PropType.RandomObject :
					chancePropType < 95 ? PropType.Gold :
					PropType.Mushrooms;

				List<GameObject> filtered = props.Where(p =>
				{
					Prop prop = p.GetComponent<Prop>();
					return prop != null && prop.propType == randomProptype;
				}).ToList();

				for (int i = 0; i < count; i++)
				{
					GameObject prop = Instantiate(filtered[Random.Range(0, filtered.Count)]);
					prop.transform.SetParent(tile.transform);

					float randomRange = 0.4f;
					prop.transform.localPosition = new Vector3(Random.Range(-randomRange, randomRange), 0.5f, Random.Range(-randomRange, randomRange));
					Vector3 rot = prop.transform.localEulerAngles;
					rot.y = Random.Range(0, 360);
					prop.transform.localEulerAngles = rot;
				}
			}
		}
	}
}
