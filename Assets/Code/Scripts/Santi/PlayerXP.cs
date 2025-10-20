using UnityEngine;

public class PlayerXP : MonoBehaviour
{
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int currentXP = 0;
    [SerializeField] private int xpToNextLevel = 4;
    [SerializeField] private float xpPickupRange = 2f;

    private int turretBuiltCount = 0;
    [SerializeField] private int[] turretLevelRequirements = new int[4] { 2, 4, 6, 8 };

    public int CurrentLevel => currentLevel;

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
        Destroy(xpObject);

        if (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        currentLevel++;
        currentXP = 0;
        Debug.Log("Player subió de nivel a: " + currentLevel);
    }

    public int NextTurretLevelRequirement()
    {
        if (turretBuiltCount >= turretLevelRequirements.Length) return int.MaxValue;
        return turretLevelRequirements[turretBuiltCount];
    }

    public void RegisterTurretBuild()
    {
        turretBuiltCount++;
    }

    public void ResetTurretBuilds()
    {
        turretBuiltCount = 0;
    }
}