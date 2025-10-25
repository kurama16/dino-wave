using UnityEngine;

public abstract class ActiveSkillSO : SkillSO
{
    public float cooldown = 2f;
    public float duration = 2f;
    public Sprite icon;

    public abstract void Execute(GameObject caster);
}