using System;
using UnityEngine;

public class ChangeLevelTrigger : MonoBehaviour
{
    [SerializeField] Room room;
    bool triggered = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            room.ChangeLevel();
            triggered = true;
        }
    }
}
