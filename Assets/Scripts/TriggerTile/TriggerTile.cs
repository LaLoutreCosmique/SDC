using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(Tile))]
public class TriggerTile : MonoBehaviour
{
    [Header("Global Settings:")]
    [SerializeField] [MaxValue(4)] int triggerId;
    [SerializeField] protected bool isTriggered;
    [SerializeField] [OnValueChanged("InitModel")] GameObject modelPrefab;
    [SerializeField] GameObject animationTransform;

    protected Tile tile;
    protected Room parentRoom => tile.parentRoom;
    public GameObject model;

    protected virtual void Awake()
    {
        tile = GetComponent<Tile>();
    }

    public void InitModel()
    {
        if (model)
            DestroyImmediate(model);

        if (modelPrefab)
            model = Instantiate(modelPrefab, transform);
    }
}
