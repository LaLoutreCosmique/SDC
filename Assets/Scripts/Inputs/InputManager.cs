using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Gyroscope = UnityEngine.Gyroscope;

public class InputManager : MonoBehaviour
{
	[SerializeField] Player m_Player;
	[SerializeField] PlayerInfos playerInfos;

	PlayerInputs m_PlayerInputs;
	PlayerInputs.PlayerActions m_PlayerActions;
	
	Quaternion m_CalibrationRotation;

	//private void OnEnable()
	//{
	//}

	private void OnDisable()
	{
		if (SystemInfo.supportsGyroscope)
		{
			GameManager.Instance.OnPlayerFall -= CalibrateGyro;
			GameManager.Instance.OnPlayerDie -= CalibrateGyro;
		}
	}

    void Start()
    {
        m_PlayerInputs = new PlayerInputs();
        m_PlayerActions = m_PlayerInputs.Player;
        m_PlayerActions.Enable();
        m_PlayerActions.AltMove.performed += ctx => HandleMovement(ctx.ReadValue<Vector2>());
        m_PlayerActions.AltMove.canceled += ctx => HandleMovement(ctx.ReadValue<Vector2>());
        
        if (SystemInfo.supportsGyroscope && AttitudeSensor.current != null)
        {
            InputSystem.EnableDevice(AttitudeSensor.current);

			GameManager.Instance.OnPlayerFall += CalibrateGyro;
			GameManager.Instance.OnPlayerDie += CalibrateGyro;


			if (playerInfos.hasDiedOnce)
			{
				m_CalibrationRotation = new Quaternion(PlayerPrefs.GetFloat("GyroX"), PlayerPrefs.GetFloat("GyroY"), PlayerPrefs.GetFloat("GyroZ"), PlayerPrefs.GetFloat("GyroW"));
			}
			else
			{
				CalibrateGyro();
			}
		}
	}

	private void Update()
	{
		if (SystemInfo.supportsGyroscope && AttitudeSensor.current != null && AttitudeSensor.current.enabled)
		{
			Quaternion rota = AttitudeSensor.current.attitude.ReadValue(); 
			rota = ConvertRightHandedToLeftHandedQuaternion(rota);
			rota *= Quaternion.Inverse(m_CalibrationRotation);
			RotaToDirection(rota); 
		}
	}
	
	private Quaternion ConvertRightHandedToLeftHandedQuaternion(Quaternion rightHandedQuaternion)
	{
		return new Quaternion (-rightHandedQuaternion.x,
			-rightHandedQuaternion.z,
			-rightHandedQuaternion.y,
			rightHandedQuaternion.w);
	}
	
	void RotaToDirection(Quaternion rotation)
	{
		Vector3 eulerAngles = rotation.eulerAngles;
		
		float xAngle = NormalizeAngle(eulerAngles.x);
		float zAngle = NormalizeAngle(eulerAngles.z);
		
		float xInclination = Mathf.Sin(xAngle * Mathf.Deg2Rad);
        float zInclination = Mathf.Sin(zAngle * Mathf.Deg2Rad);

		Vector2 direction = new Vector2(-zInclination, xInclination);
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

	public void CalibrateGyro()
	{
		if (!SystemInfo.supportsGyroscope) return;
		
		m_CalibrationRotation = ConvertRightHandedToLeftHandedQuaternion(AttitudeSensor.current.attitude.ReadValue());
		
		PlayerPrefs.SetFloat("GyroX", m_CalibrationRotation.x);
		PlayerPrefs.SetFloat("GyroY", m_CalibrationRotation.y);
		PlayerPrefs.SetFloat("GyroZ", m_CalibrationRotation.z);
		PlayerPrefs.SetFloat("GyroW", m_CalibrationRotation.w);
	}
}
