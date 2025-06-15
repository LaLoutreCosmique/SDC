using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using NaughtyAttributes;

public class Tile : MonoBehaviour
{
    [Header("Tile:")]
    [SerializeField] Model model;
    [Dropdown("TriggerTypes")] [OnValueChanged("OnTriggerTypeChanged")] public string triggerType;
    [SerializeField] bool canDecay;
    [SerializeField] bool isFlipped;

    [Header("References:")]
    [SerializeField] Transform modelsParent;

    [HideInInspector] public Room parentRoom;

    private List<string> TriggerTypes { get { return TypeToString(); }}
    private List<Type> CachedTriggerTypes = new();

    public enum Model
    {
        Floor = 0,
        Wall = 1,
        Pillar = 2,
        Hole = 3,
    }


    private void OnValidate()
    {
        // rotation
        transform.eulerAngles = Vector3.up * (isFlipped ? 90 : 0);
        EnableChild((int)model, modelsParent);
    }

    private void OnTriggerTypeChanged()
    {
        if (TryGetComponent<TriggerTile>(out TriggerTile triggerTile))
        {
            if (triggerTile.name != triggerType)
            {
                DestroyImmediate(triggerTile.model);
                DestroyImmediate(triggerTile);
                if (triggerType != "None")
                {
                    TriggerTile newTT = gameObject.AddComponent(StringToType(triggerType)) as TriggerTile;
                    newTT.InitModel();
                }

            }
        }
        else
        {
            if (triggerType != "None")
            {
                TriggerTile newTT = gameObject.AddComponent(StringToType(triggerType)) as TriggerTile;
                newTT.InitModel();
            }
        }
    }

    Type StringToType(string typeName)
    {
        return GetAllDerivedClasses().Find(t => t.ToString() == typeName);
    }

    List<string> TypeToString()
    {
        var newList = GetAllDerivedClasses().Select(t => t.ToString()).ToList();
        newList.Insert(0, "None");
        return newList;
    }

    List<Type> GetAllDerivedClasses()
    {
        if (CachedTriggerTypes.Count == 0)
            CachedTriggerTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsSubclassOf(typeof(TriggerTile))).ToList();
        return CachedTriggerTypes;
    }

    public void EnableChild(int index, Transform parent)
    {
        // diable all child
        foreach (Transform child in parent)
            child.gameObject.SetActive(false);

        // enable one child if found
        if (index >= parent.childCount) return;
        parent.GetChild(index).gameObject.SetActive(true);
    }
}
