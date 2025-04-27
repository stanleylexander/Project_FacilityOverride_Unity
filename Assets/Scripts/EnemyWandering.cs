using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWandering : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float wanderCooldown = 1f;

    private Vector3 moveDirection = Vector3.zero;
    private Rigidbody rb;
    private float cooldownTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cooldownTimer = wanderCooldown;
        PickRandomDirection();
    }

    void FixedUpdate()
    {
        cooldownTimer -= Time.fixedDeltaTime;

        if (cooldownTimer <= 0f)
        {
            PickRandomDirection();
            cooldownTimer = wanderCooldown;
        }

        // Apply movement
        rb.velocity = moveDirection * moveSpeed;
    }

    void PickRandomDirection()
    {
        // Random unit sphere gives random 3D direction
        Vector3 randomDir = Random.onUnitSphere;
        randomDir.y = 0f; // Optional: remove vertical movement if you want enemy only move on ground
        moveDirection = randomDir.normalized;
    }
}
