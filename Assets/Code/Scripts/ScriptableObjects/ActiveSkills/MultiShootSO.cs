using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Active/MultiShoot")]
public class MultiShootSO : ActiveSkillSO
{
    public override void Execute(GameObject caster)
    {
        var weapon = caster.GetComponentInChildren<Weapon>();
        if (weapon != null)
        {
            weapon.Shoot(4);
        }
    }
}
