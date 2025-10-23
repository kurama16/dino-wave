using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Active/EagleEye")]
public class EagleEyeSO : ActiveSkillSO
{
    public override void Execute(GameObject caster)
    {
        var playerAbilityController = caster.GetComponent<PlayerAbilityController>();
    }
}
