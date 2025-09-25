using UnityEngine;
using UnityEngine.AI;

public class EnemyTimeGapChase : MonoBehaviour
{
    [Header("Chase")]
    [SerializeField] private float detectionRadius = 30f;
    [SerializeField] private Transform playerTransform;

    [Header("Damage")]
    [SerializeField] private float damageAmount = 10f;
    [SerializeField] private float tickInterval = 1f;

    private NavMeshAgent agent;
    private float timer;
    private bool touchingTimeGap;
    private Transform timeGapTransform;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.Find("Player");
            if (playerObj != null) playerTransform = playerObj.transform;
        }

        GameObject tg = GameObject.FindGameObjectWithTag("TimeGap");
        if (tg != null)
            timeGapTransform = tg.transform;
    }

    private void Update()
    {
        if (timeGapTransform == null || timeGapTransform.gameObject == null)
            timeGapTransform = null;

        Transform target = GetClosestTarget();
        if (target != null)
            agent.SetDestination(target.position);

        if (touchingTimeGap && timeGapTransform != null && timeGapTransform.TryGetComponent<IDamageable>(out var dmg))
        {
            timer += Time.deltaTime;
            if (timer >= tickInterval)
            {
                dmg.TakeDamage(damageAmount);
                timer = 0f;
            }
        }
    }

    private Transform GetClosestTarget()
    {
        float closestDist = Mathf.Infinity;
        Transform closest = null;

        if (playerTransform != null)
        {
            float dist = Vector3.Distance(transform.position, playerTransform.position);
            if (dist < closestDist && dist <= detectionRadius)
            {
                closestDist = dist;
                closest = playerTransform;
            }
        }

        if (timeGapTransform != null)
        {
            float dist = Vector3.Distance(transform.position, timeGapTransform.position);
            if (dist < closestDist && dist <= detectionRadius)
            {
                closestDist = dist;
                closest = timeGapTransform;
            }
        }

        return closest;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TimeGap"))
        {
            touchingTimeGap = true;
            timeGapTransform = other.transform;
            timer = 0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TimeGap"))
        {
            touchingTimeGap = false;
            timer = 0f;
        }
    }
}