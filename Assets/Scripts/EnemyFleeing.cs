using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFleeing : MonoBehaviour
{
    public float moveSpeed = 3f;
    Vector3 moveVec = Vector3.zero;
    Rigidbody rb;
    public GameObject target;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation; // Hanya boleh gerak XZ
    }

    void KinematicFlee()
    {
        // Calculate flee direction (menjauh dari target)
        Vector3 direction = transform.position - target.transform.position;

        // Normalize untuk dapet unit vector
        moveVec = direction.normalized;
    }

    void FixedUpdate()
    {
        if (target == null) return;

        KinematicFlee();

        Vector3 move = moveVec * moveSpeed;
        rb.MovePosition(transform.position + move * Time.fixedDeltaTime);
    }
}
