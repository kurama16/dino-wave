using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Active/MultiShoot")]
public class MultiShootSO : ActiveSkillSO
{
    public override void Execute(GameObject caster)
    {
        Debug.Log("Execute MultiShoot");
        var playerAbilityController = caster.GetComponent<PlayerAbilityController>();
    }
}
