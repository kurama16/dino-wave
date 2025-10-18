using UnityEngine;

public class PlayerXP : MonoBehaviour
{
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int enemiesToLevelUp = 4;
    [SerializeField] private int currentXP = 0;
    [SerializeField] private float xpPickupRange = 2f;

    private int enemiesDefeated = 0;

    private int turretBuiltCount = 0;
    private readonly int[] turretLevelRequirements = new int[4] { 2, 4, 6, 8 };

    public int CurrentLevel => currentLevel;

    public int NextTurretLevelRequirement()
    {
        if (turretBuiltCount >= turretLevelRequirements.Length) return int.MaxValue;
        return turretLevelRequirements[turretBuiltCount];
    }

    public bool CanBuildTurret()
    {
        return currentLevel >= NextTurretLevelRequirement();
    }

    public void RegisterTurretBuild()
    {
        turretBuiltCount++;
    }

    public void ResetXPAndTurrets()
    {
        currentLevel = 1;
        currentXP = 0;
        enemiesDefeated = 0;
        turretBuiltCount = 0;
    }

    void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, xpPickupRange);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("XP"))
            {
                CollectXP(hit.gameObject);
            }
        }
    }

    private void CollectXP(GameObject xpObject)
    {
        currentXP++;
        enemiesDefeated++;
        Destroy(xpObject);

        if (enemiesDefeated >= enemiesToLevelUp)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        currentLevel++;
        enemiesDefeated = 0;
        Debug.Log("Player subió de nivel a: " + currentLevel);
    }
}