using NaughtyAttributes;
using UnityEngine;
using static Tile;

[RequireComponent(typeof(Tile))]
public class TriggerTile : MonoBehaviour
{
    [Header("Global Settings:")]
    [SerializeField] [MaxValue(4)] int triggerId;
    [SerializeField] bool isTriggered;
    [SerializeField] [OnValueChanged("InitModel")] GameObject modelPrefab;

    protected Tile tile;
    protected Room parentRoom => tile.parentRoom;
    public GameObject model;

    public void InitModel()
    {
        if (model)
            DestroyImmediate(model);

        if (modelPrefab)
            model = Instantiate(modelPrefab, transform);
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
