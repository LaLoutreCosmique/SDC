using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using static Tile;

[RequireComponent(typeof(Tile))]
public class TriggerTile : MonoBehaviour
{
    [Header("Global Settings:")]
	[SerializeField] public List<int> triggerIds = new();
	[SerializeField] bool isTriggered;
    [SerializeField] [OnValueChanged("InitModel")] GameObject modelPrefab;

    protected Tile tile;
    protected Room parentRoom;
    public GameObject model;

    public void InitModel()
    {
        if (model)
            DestroyImmediate(model);

        if (modelPrefab)
        {
            model = Instantiate(modelPrefab, transform);
        }
	}

    public void SetTile(Tile tile)
    {
        this.tile = tile;
		this.parentRoom = tile.parentRoom;
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            OnPlayerTrigger(other.GetComponent<Player>());
    }

    public virtual void OnPlayerTrigger(Player player)
    {

    }

    public virtual void OnTileTrigger()
    {
        
    }
}
