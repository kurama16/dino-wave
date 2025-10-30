using System;
using UnityEngine;

public class PlayerXP : MonoBehaviour
{
    [Header("Player Experience Stats")]
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int currentXP = 0;
    [SerializeField] private int xpToNextLevel = 4;

    public Action<float, float> OnXPChanged;
    public Action<int> OnLevelChanged;

    private PlayerAbilityController _playerAbilityController;

    public int GetCurrentLevel() => currentLevel;
    public int GetCurrentXP() => currentXP;
    public int GetXPToNextLevel() => xpToNextLevel;

    private void Awake()
    {
        _playerAbilityController = GetComponent<PlayerAbilityController>();
    }

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
        _playerAbilityController.LevelUp();
    }

    //Mover esto a otro script donde se puedan trackear las misiones
    public void RegisterTurretBuild()
    {
    //    if (turretBuiltCount >= turretLevelRequirements.Length) return;

    //    turretBuiltCount++;
    //    if (turretBuiltCount >= 4)
    //    {
    //        LevelUp();
    //    }
    }
}