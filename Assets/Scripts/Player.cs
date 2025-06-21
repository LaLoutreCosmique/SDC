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

	[SerializeField] private Transform VFX;


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

		fallForce = Vector3.down * m_FallForce;
		Ray ray = new Ray(transform.position, Vector3.down);
		if (Physics.Raycast(ray, out RaycastHit hit, BallRadius + 0.3f, ~m_IgnoreLayersRay))
		{
			fallForce = Vector3.zero;
		}

		m_PlayerRb.linearVelocity = Vector3.Lerp(m_PlayerRb.linearVelocity, (movement + fallForce) * m_Speed, m_Acceleration * Time.fixedDeltaTime);
		if (m_PlayerRb.linearVelocity.magnitude <= 0.01f)
		{
			m_PlayerRb.linearVelocity = Vector3.zero;
			m_PlayerRb.angularVelocity = Vector3.zero;
		}
	}

	void SetVFXTransform(Vector3 movement)
    {
        VFX.localRotation = Quaternion.Euler(m_PlayerRb.linearVelocity);
        //      if (movement == Vector3.zero)
        //{
        //}
        //else if (movement == new Vector3(0, 0, 1.5f))
        //{
        //	VFX.localScale = Vector3.one;
        //          VFX.localRotation = Quaternion.Euler(0, 0, 0);
        //      }
        //else if (movement == new Vector3(0, 0, -1.5f))
        //      {
        //          VFX.localScale = Vector3.one;
        //          VFX.localRotation = Quaternion.Euler(0, 180, 0);
        //      }
        //      else if (movement == new Vector3(1, 0, 0))
        //      {
        //          VFX.localScale = new Vector3(.5f, .5f, .5f);
        //          VFX.localRotation = Quaternion.Euler(0, 90, 0);
        //      }
        //      else if (movement == new Vector3(-1, 0, 0))
        //      {
        //          VFX.localScale = new Vector3(.5f, .5f, .5f);
        //          VFX.localRotation = Quaternion.Euler(0, 270, 0);
        //      }
        //      else if (movement == new Vector3(.71f, 0, 1.06f))
        //      {
        //          VFX.localRotation = Quaternion.Euler(0, 0, 45);
        //          VFX.localScale = new Vector3(.5f, .5f, .5f);
        //      }
        //      else if (movement == new Vector3(-.71f, 0, 1.06f))
        //      {
        //          VFX.localRotation = Quaternion.Euler(0, 0, 315);
        //          VFX.localScale = new Vector3(.5f, .5f, .5f);
        //      }
        //      else if (movement == new Vector3(.71f, 0, -1.06f))
        //      {
        //          VFX.localRotation = Quaternion.Euler(0, 0, 135);
        //          VFX.localScale = new Vector3(.5f, .5f, .5f);
        //      }
        //      else if (movement == new Vector3(-.71f, 0, -1.06f))
        //      {
        //          VFX.localRotation = Quaternion.Euler(0, 0, 225f);
        //          VFX.localScale = new Vector3(.5f, .5f, .5f);
        //      }
    }
}
