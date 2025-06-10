using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelGenerator : MonoBehaviour
{
    /// <summary>
    /// 0 for random.
    /// </summary>
    [SerializeField] int seed = 0;
    [SerializeField] Transform cameraTest;

    List<Room> loadedRooms = new();
    List<List<Room>> staticBags = new();
    List<List<Room>> currentBags = new();
    List<Room> spawnedRooms = new();
    System.Random bagsRnd;
    int currentRoomId = -1;
    private Vector3 nextPos;

    const int bagsAmount = 2;

    void Start()
    {
        loadedRooms = Resources.LoadAll<Room>("Rooms/PlayableRooms").ToList();
        nextPos = cameraTest.position;

        InitBags();
        SpawnNextRoom();
    }

    private void Update()
    {
        if (Keyboard.current.kKey.wasPressedThisFrame)
            SpawnNextRoom();

        
        Vector3 offset = new Vector3(0, 14, -13);
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
        Room newRoom = Instantiate(nextRoom, newRoomPos, Quaternion.identity).Setup(this);
        spawnedRooms.Add(newRoom);

        // remove previous rooms
        while (spawnedRooms.Count() > 4)
        {
            Destroy(spawnedRooms[0].gameObject);
            spawnedRooms.RemoveAt(0);
        }
        
        if (spawnedRooms.Count != 1)
            nextPos = cameraTest.position + (Vector3.forward * spawnedRooms[^1].roomLength);
    }
}
