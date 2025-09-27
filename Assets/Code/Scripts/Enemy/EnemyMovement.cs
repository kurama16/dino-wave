using UnityEngine;
using UnityEngine.AI;
public class EnemyMovement : MonoBehaviour
{

    private NavMeshAgent navAgent;
    private Transform playerTransform;
    [Header("Layers")]
    [SerializeField] private LayerMask terrainLayer;
    [SerializeField] private LayerMask playerLayerMask;

    private void Awake()
    {
        GameObject playerObject = GameObject.Find("Player");
        if (playerObject != null)
            playerTransform = playerObject.transform;

        navAgent = GetComponent<NavMeshAgent>();
    }


    private void Update()
    {

        PerformChase();
    }


    private void PerformChase()
    {
        if (playerTransform != null)
        {
            Vector3 dir = playerTransform.position - transform.position;
            dir.y = 0f;
            navAgent.SetDestination(playerTransform.position);
            transform.LookAt(dir);
        }
    }

}
