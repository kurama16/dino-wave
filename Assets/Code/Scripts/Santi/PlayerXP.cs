using System;
using UnityEngine;

public class PlayerXP : MonoBehaviour
{
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int enemiesToLevelUp = 4;
    [SerializeField] private int currentXP = 0;

    private int enemiesDefeated = 0;
    private int turretBuiltCount = 0;
    private int[] turretLevelRequirements = new int[4] { 2, 4, 6, 8 };

    public int CurrentLevel => currentLevel;
    public int CurrentXP => currentXP;
    public int XPToNextLevel => enemiesToLevelUp;

    public event Action<float, float> OnXPChanged;
    public event Action<int> OnLevelChanged;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("XP"))
        {
            CollectXP(1);
            Destroy(other.gameObject);
        }
    }

    public void CollectXP(int amount = 1)
    {
        currentXP += amount;
        enemiesDefeated += amount;
        OnXPChanged?.Invoke(currentXP, enemiesToLevelUp);

        if (enemiesDefeated >= enemiesToLevelUp)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        currentLevel++;
        enemiesDefeated = 0;
        currentXP = 0;
        OnLevelChanged?.Invoke(currentLevel);
        OnXPChanged?.Invoke(currentXP, enemiesToLevelUp);
    }

    public bool CanBuildTurret()
    {
        if (turretBuiltCount >= turretLevelRequirements.Length) return false;
        return currentLevel >= turretLevelRequirements[turretBuiltCount];
    }

    public void RegisterTurretBuild()
    {
        if (turretBuiltCount >= turretLevelRequirements.Length) return;

        turretBuiltCount++;
        if (turretBuiltCount >= 4)
        {
            LevelUp();
        }
    }

    public void ResetTurrets()
    {
        turretBuiltCount = 0;
    }
}