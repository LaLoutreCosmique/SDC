using UnityEngine;

public abstract class TriggerInput : TriggerTile
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            OnTrigger(other.GetComponent<Player>());
    }

    public virtual void OnTrigger(Player player)
    {
        isTriggered = !isTriggered;
        parentRoom.OnTriggerUpdated();
    }
}
