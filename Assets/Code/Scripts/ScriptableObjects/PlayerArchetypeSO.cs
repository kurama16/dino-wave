using UnityEngine;

[CreateAssetMenu(menuName = "RPG/Archetype")]
public class PlayerArchetypeSO : ScriptableObject
{
    public string archetype;
    public string displayName;
    [TextArea] public string description;
    public float baseMaxHealth = 100f;
    public float baseDamage = 20f;
    public ActiveSkillSO[] startingActiveSkills;
    public PassiveSkillSO[] startingPassiveSkills;
}
