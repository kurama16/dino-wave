using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Passive/Vitality")]
public class VitalitySkillSO : PassiveSkillSO
{
    public float bonusHealth = 20f;

    public override void Apply(GameObject caster)
    {
        if(caster.TryGetComponent<PlayerStats>(out var playerHealth))
            playerHealth.IncreaseMaxHealth(bonusHealth);
    }
}