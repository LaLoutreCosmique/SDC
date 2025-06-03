using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("Tile:")]
    [SerializeField] Model model;
    [SerializeField] bool canDecay;
    [SerializeField] bool isFlipped;

    [Header("Triggers:")]
    [SerializeField] int triggerId;
    [SerializeField] TriggerType triggerType;
    [SerializeField] bool isTriggered;

    [Header("References:")]
    [SerializeField] Transform modelsParent;
    [SerializeField] Transform triggersOffParent;
    [SerializeField] Transform triggersOnParent;

    public enum Model
    {
        Floor = 0,
        Wall = 1,
        Pillar = 2,
    }

    public enum TriggerType
    {
        None = 0,
        Lever = 1,
        Door = 2,
        JumpPad = 3,
    }

    private void OnValidate()
    {
        // rotation
        transform.eulerAngles = Vector3.up * (isFlipped ? 90 : 0);

        // on/off
        triggersOffParent.gameObject.SetActive(!isTriggered);
        triggersOnParent.gameObject.SetActive(isTriggered);

        // visuals toggle
        EnableChild((int)triggerType, triggersOnParent);
        EnableChild((int)triggerType, triggersOffParent);
        EnableChild((int)model, modelsParent);
    }

    void EnableChild(int index, Transform parent)
    {
        // diable all child
        foreach (Transform child in parent)
            child.gameObject.SetActive(false);

        // enable one child if found
        if (index >= parent.childCount) return;
        parent.GetChild(index).gameObject.SetActive(true);
    }
}
