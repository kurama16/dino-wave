using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Passive/All In")]
public class AllInSkillSO : PassiveSkillSO
{
    public float leechValue = 1f;

    public override void Apply(GameObject caster)
    {
        if (caster.TryGetComponent<PlayerStats>(out var playerHealth))
            playerHealth.RestoreHealth(leechValue);
    }
}