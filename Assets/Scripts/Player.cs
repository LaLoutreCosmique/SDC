using System;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
	Rigidbody m_PlayerRb;

	[SerializeField] float m_Speed;
	[SerializeField] float m_Acceleration;
	[SerializeField] float m_FallForce;

	[HideInInspector] public Vector3 moveDirection;
	[HideInInspector] public Vector3 fallForce;

	[HideInInspector] public float BallRadius;
	[SerializeField] private LayerMask m_IgnoreLayersRay;


    [SerializeField] Slider slider;

    private void Start()
	{
		m_PlayerRb = GetComponent<Rigidbody>();
		BallRadius = GetComponent<SphereCollider>().radius;
	}

	private void FixedUpdate()
	{
		Move();

		if (transform.position.y < -2)
		{
			GameManager.Instance.OnPlayerFallFCT();
		}
	}

	void Move()
	{
		Vector3 movement = new Vector3(moveDirection.x, 0, moveDirection.y * 1.5f/*(1 + slider.value)*/);

		m_IgnoreLayersRay = ~m_IgnoreLayersRay;
		Ray ray = new Ray(transform.position, Vector3.down);
		if (Physics.Raycast(ray, out RaycastHit hit, 10f, m_IgnoreLayersRay))
		{
			if (hit.distance > BallRadius)
			{
				fallForce = Vector3.down * m_FallForce;
			}
			else
			{
				fallForce = Vector3.zero;
			}
		}
		else
		{
			fallForce = Vector3.down * m_FallForce;
		}

		m_PlayerRb.linearVelocity = Vector3.Lerp(m_PlayerRb.linearVelocity, (movement + fallForce) * m_Speed, m_Acceleration * Time.fixedDeltaTime);
		if (m_PlayerRb.linearVelocity.magnitude <= 0.01f)
		{
			m_PlayerRb.linearVelocity = Vector3.zero;
			m_PlayerRb.angularVelocity = Vector3.zero;
		}
	}
}
