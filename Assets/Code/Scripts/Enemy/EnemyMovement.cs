using UnityEngine;
using UnityEngine.AI;
public class EnemyMovement : MonoBehaviour
{

    private NavMeshAgent navAgent;
    private EnemySoundManager enemySoundManager;

    private Transform playerTransform;
    [Header("Layers")]
    [SerializeField] private LayerMask terrainLayer;
    [SerializeField] private LayerMask playerLayerMask;

    [Header("Sound config")]
    [SerializeField] private float timeBetweenSteps = 2;
    private float timeUntilNextStep = 0;

    private void Awake()
    {
        GameObject playerObject = GameObject.Find("Player");
        if (playerObject != null)
            playerTransform = playerObject.transform;

        navAgent = GetComponent<NavMeshAgent>();
        enemySoundManager = GetComponent<EnemySoundManager>();
    }
    private void Start()
    {
        if(enemySoundManager == null) return;
        enemySoundManager.PlayAwake();

    }


    private void Update()
    {
        timeUntilNextStep -= Time.deltaTime;
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
            MakeMoveSound();
        }
    }
    private void MakeMoveSound()
    {
        if (timeUntilNextStep > 0 || enemySoundManager == null) return;
        
        timeUntilNextStep = timeBetweenSteps;
        enemySoundManager.PlayMove();

    }

}
