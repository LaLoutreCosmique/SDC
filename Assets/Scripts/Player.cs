using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody m_PlayerRb;
    
    [SerializeField] float m_Speed;
    [SerializeField] float m_Acceleration;
    
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

        # if UNITY_ANDROID
            movement = -movement;
        #endif

        m_PlayerRb.linearVelocity = Vector3.Lerp(m_PlayerRb.linearVelocity, movement * m_Speed, m_Acceleration * Time.fixedDeltaTime);
    }
}
