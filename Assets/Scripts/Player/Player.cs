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

	[HideInInspector] public float BallRadius;
	[SerializeField] private LayerMask m_IgnoreLayersRay;

	[SerializeField] private LayerMask groundLayer;
	[SerializeField] private float groundRaycastSize;

	[SerializeField] private Transform VFX;


	private bool grounded;

	[Header("Reglages du son")]
	public float speedThreshold = 0.1f;
	public float maxVolume = 1.0f;
	public float baseVolumeMultiplier = 0.7f;
	public float volumeSmoothing = 5f;
	public float groundCheckDistance = 0.6f;

	[Header("Vent")]
	public float windMinVolume = 0.15f;
	public float windBaseVolume = 0.2f;
	public float windMaxVolume = 0.4f;

	private AudioSource rollingAudio;
	private AudioSource windAudio;

	[Header("Impact")]
	[SerializeField] private AudioClip[] wallImpactClips;
	[SerializeField] private GameObject impactVFXPrefab;
	[SerializeField] private float impactVolume = 0.8f;
	[SerializeField] private string wallTag = "Wall";
	[SerializeField] private string pillarTag = "Pillar";
	[SerializeField] private float impactSpeedThreshold = 2.5f;
	[SerializeField] private float impactCooldown = 0.3f;
	private float lastImpactTime = -Mathf.Infinity;

	private void Start()
	{
		m_PlayerRb = GetComponent<Rigidbody>();
		BallRadius = GetComponent<SphereCollider>().radius;

		rollingAudio = SoundsManager.Instance.audioSourceRoll;
		windAudio = SoundsManager.Instance.audioSourceWind;

		rollingAudio.volume = 0f;
		windAudio.volume = 0f;

		GameManager.Instance.OnPlayerDie += Die;
	}

	private void OnDestroy()
	{
		GameManager.Instance.OnPlayerDie -= Die;
	}

	private void FixedUpdate()
	{
		Move();

		if (transform.position.y < -2)
		{
			GameManager.Instance.OnPlayerFallFCT();
		}
	}

	private void Update()
	{
		CheckGrounded();

		float speed = m_PlayerRb.GetPointVelocity(transform.position).magnitude;

		if (grounded && speed > speedThreshold)
		{
			if (!rollingAudio.isPlaying)
				rollingAudio.Play();

			float targetRollVolume = Mathf.Clamp01(speed / 10f) * maxVolume * baseVolumeMultiplier;
			rollingAudio.volume = Mathf.Lerp(rollingAudio.volume, targetRollVolume, Time.deltaTime * volumeSmoothing);
		}
		else
		{
			rollingAudio.volume = Mathf.Lerp(rollingAudio.volume, 0f, Time.deltaTime * volumeSmoothing);
			if (rollingAudio.volume < 0.01f && rollingAudio.isPlaying)
				rollingAudio.Stop();
		}

		float windTargetVolume;

		if (grounded)
		{
			windTargetVolume = Mathf.Clamp(
				windMinVolume + (speed / 10f) * (windMaxVolume - windMinVolume),
				windMinVolume,
				windMaxVolume
			);
		}
		else
		{
			windTargetVolume = windMinVolume;
		}

		if (!windAudio.isPlaying)
			windAudio.Play();

		windAudio.volume = Mathf.Lerp(windAudio.volume, windTargetVolume, Time.deltaTime * volumeSmoothing);

	}
	private void OnCollisionEnter(Collision collision)
	{
		if (!collision.collider.CompareTag(wallTag) && !collision.collider.CompareTag(pillarTag))
			return;

		float currentTime = Time.time;
		float impactSpeed = m_PlayerRb.GetPointVelocity(transform.position).magnitude;

		if (impactSpeed < impactSpeedThreshold)
			return;

		if (currentTime - lastImpactTime < impactCooldown)
			return;

		lastImpactTime = currentTime;

		//if ()
		//{

		//	Handheld.Vibrate();
		//}

		// VFX
		if (impactVFXPrefab != null && wallImpactClips.Length > 0)
		{
			ContactPoint contact = collision.contacts[0];
			Quaternion rotation = Quaternion.LookRotation(contact.normal);
			GameObject go = Instantiate(impactVFXPrefab, contact.point, rotation);


            AudioClip _clip = wallImpactClips[UnityEngine.Random.Range(0, wallImpactClips.Length)];
			AudioSource _source = go.GetComponent<AudioSource>();
            _source.clip = _clip;
            _source.Play();

			Destroy(go, _clip.length);

        }
	}

	public void Die()
	{
		m_PlayerRb.isKinematic = true;
	}

	void Move()
	{
		Vector3 movement = new Vector3(moveDirection.x, 0, moveDirection.y * 1.5f);

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

	void CheckGrounded()
	{
		grounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
	}
}
