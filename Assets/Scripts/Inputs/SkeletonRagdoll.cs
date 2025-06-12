using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class SkeletonRagdoll : MonoBehaviour
{
    List<Rigidbody> rigidbodies = new();

    [SerializeField] private Player player;
	private Rigidbody playerRb => player.GetComponent<Rigidbody>();
	[SerializeField] private float ballOffset = 0.3f;
	[SerializeField] private float forceMultiplier = 10f;
	[SerializeField] private float maxLinearSpeed = 10f;
	[SerializeField] private float maxRotationSpeed = 10f;

	private int isSkeletonMoving = 1; // 0 not moving, 1 moving with gravity, 2 rotating with ball

	void Start()
    {
        rigidbodies = new List<Rigidbody>(GetComponentsInChildren<Rigidbody>());

		foreach (Rigidbody rb in rigidbodies)
		{
			rb.isKinematic = false;
			rb.useGravity = true;
			rb.maxLinearVelocity = maxLinearSpeed;
			rb.maxAngularVelocity = maxRotationSpeed;
		}
	}

    void FixedUpdate()
    {
		KeepSkeletonInsideBall();
	}

	void KeepSkeletonInsideBall()
	{
		bool playerInputMovement = player.moveDirection.magnitude >= 0.01f;
		bool playerIsMoving = playerRb.linearVelocity.magnitude >= 0.01f;

		if (!playerInputMovement && !playerIsMoving) // Stop movement (add timer maybe)
		{
			if (isSkeletonMoving == 0)
				return;

			foreach (Rigidbody rb in rigidbodies)
			{
				rb.isKinematic = true;
				rb.useGravity = false;
			}
			isSkeletonMoving = 0;
			return;
		}

		if (playerInputMovement) // Rotate with ball
		{
			if (isSkeletonMoving != 2)
			{
				foreach (Rigidbody rb in rigidbodies)
				{
					rb.isKinematic = false;
					rb.useGravity = true;
				}
				isSkeletonMoving = 2;
			}

			transform.position = player.transform.position;
			transform.rotation = player.transform.rotation;
		}
		else // Move with gravity
		{
			if (isSkeletonMoving != 1)
			{
				foreach (Rigidbody rb in rigidbodies)
				{
					rb.isKinematic = false;
					rb.useGravity = true;
				}  
				isSkeletonMoving = 1;
			}
		}

		// Keep the rigidbodies inside the ball
		foreach (Rigidbody rb in rigidbodies)
		{
			float distanceToCenter = Vector3.Distance(rb.position, player.transform.position);
			float ballRadius = player.BallRadius - ballOffset;
			if (distanceToCenter > ballRadius)
			{
				Vector3 directionToCenter = (player.transform.position - rb.position).normalized;
				float distanceOut = distanceToCenter - ballRadius;
				rb.AddForce(directionToCenter * distanceOut * forceMultiplier, ForceMode.VelocityChange);
			}
			else
			{
				rb.linearVelocity = Vector3.zero;
			}

		}
	}
}
