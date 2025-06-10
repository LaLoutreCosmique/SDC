using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

public class TileLine : MonoBehaviour
{
#if UNITY_EDITOR
    public Tile TilePrefab;
    [OnValueChanged("SetLineLength")]
    [Range(1, 10)] public int LineLength;
    
    public Tile[] currentTiles { get; private set; }

    public TileLine Setup(Tile prefab, int length)
    {
        TilePrefab = prefab;
        Tile newTile = (Tile)PrefabUtility.InstantiatePrefab(TilePrefab, transform);
        newTile.transform.position = Vector3.zero;
        AddTiles(new List<Tile> {newTile}, length);

        return this;
    }
    
    void SetLineLength()
    {
        List<Tile> tiles = GetComponentsInChildren<Tile>().ToList();
        int diff = LineLength - (tiles.Count+1)/2;
        
        if (LineLength == 1) SetToOneTile(tiles);
        else if (diff > 0) AddTiles(tiles, diff*2);
        else if (diff < 0) RemoveTiles(tiles, -diff*2);
        
        currentTiles = GetComponentsInChildren<Tile>();
    }

    void SetToOneTile(List<Tile> tiles)
    {
        for (int i = tiles.Count; i != 1; i--)
        {
            DestroyImmediate(tiles[i-1].gameObject);
            tiles.RemoveAt(i-1);
        }
        
        tiles[0].transform.position = Vector3.zero;
    }

    void AddTiles(List<Tile> tiles, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (i % 2 == 0)
            {
                foreach (Tile tile in tiles)
                {
                    tile.transform.position = new Vector3(tile.transform.position.x - 1, 0, 0);
                }
            } 
            
            Tile newTile = (Tile)PrefabUtility.InstantiatePrefab(TilePrefab, transform);
            newTile.transform.position = new Vector3(tiles[^1].transform.position.x + 1, 0, 0);
            tiles.Add(newTile);
            newTile.name = "Tile " + tiles.IndexOf(newTile);
        }
    }

    void RemoveTiles(List<Tile> tiles, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (i % 2 == 0)
            {
                foreach (Tile tile in tiles)
                {
                    tile.transform.position = new Vector3(tile.transform.position.x + 1, 0, 0);
                }
            } 
            
            DestroyImmediate(tiles[^1].gameObject);
            tiles.RemoveAt(tiles.Count-1);
        }
    }
#endif
}
