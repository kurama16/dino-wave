using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Active/EagleEye")]
public class EagleEyeSO : ActiveSkillSO
{
    public float boostAmount = 1.2f;
    public override void Execute(GameObject caster)
    {
        if (caster.TryGetComponent<PlayerStats>(out var playerStats))
        {
            playerStats.SetBoostDamage(boostAmount, duration);
        }
    }
}
