using System;
using UnityEngine;

public class PlayerXP : MonoBehaviour
{
    [Header("Player Experience Stats")]
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int currentXP = 0;
    [SerializeField] private int xpToNextLevel = 4;
    [SerializeField] private int[] turretLevelRequirements = new int[4] { 2, 4, 6, 8 };

    public Action<float, float> OnXPChanged;
    public Action<int> OnLevelChanged;

    private int _turretBuiltCount = 0;

    public int GetCurrentLevel() => currentLevel;
    public int GetCurrentXP() => currentXP;
    public int GetXPToNextLevel() => xpToNextLevel;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("XP")) 
            return;

        if (other.TryGetComponent<Collider>(out var collider)) 
            collider.enabled = false;

        CollectXP();

        Destroy(other.gameObject);
    }

    private void CollectXP()
    {
        currentXP++;
        OnXPChanged.Invoke(currentXP, xpToNextLevel);
        
        if (currentXP >= xpToNextLevel)
        {
            currentXP = 0;
            LevelUp();
        }
    }

    private void LevelUp()
    {
        currentLevel++;
        OnLevelChanged.Invoke(currentLevel);
    }

    public int NextTurretLevelRequirement()
    {
        if (_turretBuiltCount >= turretLevelRequirements.Length) return int.MaxValue;
        return turretLevelRequirements[_turretBuiltCount];
    }

    public void RegisterTurretBuild()
    {
        _turretBuiltCount++;
    }

    public void ResetTurretBuilds()
    {
        _turretBuiltCount = 0;
    }
}