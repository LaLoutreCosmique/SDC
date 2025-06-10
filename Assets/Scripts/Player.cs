using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody m_PlayerRb;
    
    [SerializeField] float m_Speed;
    [SerializeField] float m_Acceleration;
    [SerializeField] float m_FallForce;
    
    [HideInInspector] public Vector3 moveDirection;
    [HideInInspector] public Vector3 fallForce;

    private void Start()
    {
        m_PlayerRb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move();
        
        Ray ray = new Ray(transform.position, Vector3.down);
        if (!Physics.Raycast(ray))
        {
            fallForce = Vector3.down * m_FallForce;
        }
        else
        {
            fallForce = Vector3.zero;
        }

        if (transform.position.y < -2)
        {
            LevelGenerator.Instance.Restart();
        }
    }

    void Move()
    {
        Vector3 movement = new Vector3(moveDirection.x, 0, moveDirection.y);

        # if UNITY_ANDROID
            movement = -movement;
        #endif

        m_PlayerRb.linearVelocity = Vector3.Lerp(m_PlayerRb.linearVelocity, (movement + fallForce) * m_Speed, m_Acceleration * Time.fixedDeltaTime);
    }
}
