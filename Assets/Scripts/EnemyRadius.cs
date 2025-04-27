using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRadius : MonoBehaviour
{
    public GameObject target; // Player
    public float detectionRadius = 5f; // Radius untuk switch ke Seeking
    public float fleeTime = 30f; // Waktu sebelum enemy mulai fleeing

    private float timer = 0f;
    private bool isFleeing = false;

    private EnemyWandering wandering;
    private EnemySeeking seeking;
    private EnemyFleeing fleeing;

    void Start()
    {
        // Ambil komponen yang sudah ada di GameObject Enemy
        wandering = GetComponent<EnemyWandering>();
        seeking = GetComponent<EnemySeeking>();
        fleeing = GetComponent<EnemyFleeing>();

        // Pastikan semuanya awalnya aktif sesuai kondisi
        EnableWandering();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= fleeTime)
        {
            if (!isFleeing)
            {
                StartFleeing();
            }
        }
        else
        {
            HandleBehavior();
        }
    }

    void HandleBehavior()
    {
        if (target == null) return;

        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        if (distanceToTarget <= detectionRadius)
        {
            EnableSeeking();
        }
        else
        {
            EnableWandering();
        }
    }

    void StartFleeing()
    {
        isFleeing = true;
        wandering.enabled = false;
        seeking.enabled = false;
        fleeing.enabled = true;
    }

    void EnableWandering()
    {
        if (isFleeing) return;

        wandering.enabled = true;
        seeking.enabled = false;
        fleeing.enabled = false;
    }

    void EnableSeeking()
    {
        if (isFleeing) return;

        wandering.enabled = false;
        seeking.enabled = true;
        fleeing.enabled = false;
    }
}
