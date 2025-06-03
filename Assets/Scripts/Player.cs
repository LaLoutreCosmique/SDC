using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody m_PlayerRb;
    
    [HideInInspector] public Vector3 moveDirection;

    private void Start()
    {
        m_PlayerRb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Vector3 movement = new Vector3(moveDirection.x, 0, moveDirection.y);
        m_PlayerRb.AddForce(movement * 3f);
    }
}
