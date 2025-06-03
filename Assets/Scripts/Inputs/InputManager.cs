using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] Player m_Player;
    
    PlayerInputs m_PlayerInputs;
    PlayerInputs.PlayerActions m_PlayerActions;
    Gyroscope m_Gyro;
    
    void Start()
    {
        m_PlayerInputs = new PlayerInputs();
        m_PlayerActions = m_PlayerInputs.Player;
        m_PlayerActions.Enable();
        m_PlayerActions.AltMove.performed += ctx => HandleMovement(ctx.ReadValue<Vector2>());
        m_PlayerActions.AltMove.canceled += ctx => HandleMovement(ctx.ReadValue<Vector2>());
        
        if (SystemInfo.supportsGyroscope)
        {
            //Input.gyro.enabled = true;
            m_Gyro = Input.gyro;
        }
    }

    private void Update()
    {
        if (Input.gyro.enabled)
        {
           //GyroToDirection(m_Gyro.attitude);
        }
    }
    
    void GyroToDirection(Quaternion rotation)
    {
        Vector3 eulerAngles = rotation.eulerAngles;
        
        float xAngle = NormalizeAngle(eulerAngles.x);
        float yAngle = NormalizeAngle(eulerAngles.y);

        float xInclination = Mathf.Sin(xAngle * Mathf.Deg2Rad);
        float yInclination = Mathf.Tan(yAngle * Mathf.Deg2Rad);

        Vector2 direction = new Vector2(xAngle, yAngle);
        direction.Normalize();
        Debug.Log(direction);
        HandleMovement(direction);
    }

    float NormalizeAngle(float angle)
    {
        if (angle > 180f)
        {
            angle -= 360f;
        }
        return angle;
    }

    void HandleMovement(Vector2 direction)
    {
        if (m_Player == null) return;
        
        m_Player.moveDirection = direction;
    }
}
