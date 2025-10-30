using System.Collections.Generic;
using UnityEngine;

public class CooldownService : MonoBehaviour
{
    private readonly Dictionary<string, float> _skillCDUntil = new();

    public bool IsOnCooldown(string id)
    {
        return _skillCDUntil.TryGetValue(id, out var cooldown) && cooldown > Time.time;
    }

    public float GetRemaining(string id)
    {
        return _skillCDUntil.TryGetValue(id, out var cooldown) ? Mathf.Max(0, cooldown - Time.time) : 0f;
    }

    public float StartKillCooldwn(ActiveSkillSO skill, float cooldownMultiplayer = 1f)
    {
        var end = Time.time + (skill.cooldown * cooldownMultiplayer);
        _skillCDUntil[skill.skillId] = end;
        return end;
    }
}
