using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent navAgent;
    private EnemySoundManager enemySoundManager;
    private Transform playerTransform;

    [Header("Sound config")]
    [SerializeField] private float timeBetweenSteps = 2f;
    private float timeUntilNextStep = 0f;

    private void Awake()
    {
        // Buscar el jugador por nombre
        GameObject playerObject = GameObject.Find("Player");
        if (playerObject != null)
            playerTransform = playerObject.transform;

        navAgent = GetComponent<NavMeshAgent>();
        enemySoundManager = GetComponent<EnemySoundManager>();
    }

    private void Start()
    {
        if(enemySoundManager != null)
            enemySoundManager.PlayAwake();
    }

    private void Update()
    {
        timeUntilNextStep -= Time.deltaTime;
        PerformChase();
    }

    private void PerformChase()
    {
        if (playerTransform == null) return;

        // Calcular dirección hacia el jugador, solo en plano XZ
        Vector3 dir = playerTransform.position - transform.position;
        dir.y = 0f;

        // Decirle al NavMeshAgent a dónde ir
        navAgent.SetDestination(playerTransform.position);

        // Hacer que el enemigo mire suavemente al jugador
        if(dir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }

        MakeMoveSound();
    }

    private void MakeMoveSound()
    {
        if (timeUntilNextStep > 0f || enemySoundManager == null) return;

        timeUntilNextStep = timeBetweenSteps;
        enemySoundManager.PlayMove();
    }
}