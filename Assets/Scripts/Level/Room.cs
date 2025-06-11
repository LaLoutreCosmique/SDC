using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] Difficulty difficulty;
    [OnValueChanged("SetRoomLength")]
    [Range(1, 20)] public int roomLength;
    LevelGenerator levelGenerator;

    /// <summary>
    /// The difficulty to complete the room.
    /// 
    /// Easy:
    /// One step, fast to complete.
    /// 
    /// Medium:
    /// Multiple steps, easy obstacles (walls/pillars).
    /// 
    /// Hard:
    /// Multiple steps, hard obstacles (holes/jumps), and distubances (enemies).
    /// 
    /// </summary>
    public enum Difficulty
    {
        Unspecified = -1,
        Easy = 0,
        Medium = 1,
        Hard = 2,
    }

    private void Awake()
    {
        foreach (var childTile in GetComponentsInChildren<Tile>())
            childTile.parentRoom = this;
    }

    public Room Setup(LevelGenerator lvlGenerator)
    {
        levelGenerator = lvlGenerator;
        return this;
    }

    private void SetRoomLength()
    {
        List<TileLine> tileLines = GetComponentsInChildren<TileLine>().ToList();
        int difference = roomLength - tileLines.Count;
        
        if (difference > 0) AddLineToRoom(tileLines, difference);
        if (difference < 0) RemoveLineToRoom(tileLines, -difference);
    }

    private void AddLineToRoom(List<TileLine> tileLines, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject emptyGO = new GameObject();
            TileLine newLineGO = emptyGO.AddComponent<TileLine>();
            TileLine newLine = Instantiate(newLineGO, transform).Setup(tileLines[^1].TilePrefab, tileLines[^1].LineLength);
            DestroyImmediate(newLineGO.gameObject);
            newLine.transform.position = new Vector3(0, 0, tileLines[^1].transform.position.z+1);
            tileLines.Add(newLine);
            newLine.name = "Line " + tileLines.IndexOf(tileLines[^1]);

            for (int j = 0; j < newLine.currentTiles.Length; j++)
            {
                newLine.currentTiles[j] = tileLines[^1].currentTiles[j];
            }
        }
    }
    
    private void RemoveLineToRoom(List<TileLine> tileLines, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            DestroyImmediate(tileLines[^1].gameObject);
            tileLines.RemoveAt(tileLines.Count - 1);
        }
    }

    public void ChangeLevel()
    {
        levelGenerator.SpawnNextRoom();
    }

    public int GetRoomWidth()
    {
        TileLine[] tileLines = GetComponentsInChildren<TileLine>();
        int max = 0;
        foreach (TileLine line in tileLines)
        {
            if (line.LineLength > max) max = line.LineLength;
        }
        
        return max;
    }
}
