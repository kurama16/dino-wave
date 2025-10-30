using UnityEngine;

public abstract class PassiveSkillSO : SkillSO
{
    public abstract void Apply(GameObject caster);

    public abstract void Unapply(GameObject caster);
}
