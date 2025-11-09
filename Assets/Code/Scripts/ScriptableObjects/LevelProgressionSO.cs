using UnityEngine;

[CreateAssetMenu(menuName = "RPG/Level Progression")]
public class LevelProgressionSO : ScriptableObject
{
    [System.Serializable]
    public class LevelUpEntry
    {
        public float addMaxHealth;
        public float addMaxDamage;
        public float reduceFireRate;
        public int addTurretLimit;
        public float globalCooldownMultiplierReduction;
        //public PassiveSkillSO[] grantPassives;
        //public ActiveSkillSO[] grantActives;
    }

    public LevelUpEntry[] levels;
}