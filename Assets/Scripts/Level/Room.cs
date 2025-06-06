using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] Difficulty difficulty;

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
}
