using UnityEngine;

public class FixRotation : MonoBehaviour
{

    [SerializeField] Transform target;
    void Update()
    {
        transform.position = target.position;
    }
}
