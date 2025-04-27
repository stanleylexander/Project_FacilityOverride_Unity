using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySeeking : MonoBehaviour
{
    public float moveSpeed = 3f;
    Rigidbody rb;

    public Pathfinding pathfinding;
    private List<Node> path;
    private int targetIndex = 0;
    public float nodeReachThreshold = 0.2f;

    public LayerMask obstacleMask; // <<< Tambahan, drag layer Obstacle ke sini
    public float obstacleCheckDistance = 1.0f; // Jarak Raycast untuk cek obstacle

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        if (pathfinding.gridRef.finalPath != null)
        {
            path = pathfinding.gridRef.finalPath;
        }
    }

    void FixedUpdate()
    {
        if (path == null || path.Count == 0)
            return;

        if (targetIndex >= path.Count)
        {
            rb.velocity = Vector3.zero;
            return;
        }

        Vector3 targetPosition = new Vector3(
            path[targetIndex].worldPosition.x,
            transform.position.y,
            path[targetIndex].worldPosition.z
        );

        Vector3 direction = (targetPosition - transform.position).normalized;

        // Cek apakah ada obstacle di depan
        if (!Physics.Raycast(transform.position, direction, obstacleCheckDistance, obstacleMask))
        {
            // Kalau tidak ada halangan, move ke target
            Vector3 move = direction * moveSpeed;
            rb.MovePosition(transform.position + move * Time.fixedDeltaTime);

            // Cek sudah dekat dengan node belum
            if (Vector3.Distance(transform.position, targetPosition) < nodeReachThreshold)
            {
                targetIndex++;
            }
        }
        else
        {
            // Kalau ada obstacle, berhenti dulu atau nanti bisa ditambahkan logic cari jalan lain
            rb.velocity = Vector3.zero;
        }
    }
}
