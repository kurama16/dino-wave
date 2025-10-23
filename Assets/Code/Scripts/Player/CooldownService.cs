using System.Collections.Generic;
using UnityEngine;

public class CooldownService : MonoBehaviour
{
    private readonly Dictionary<string, float> skillCDUntil = new();
    
    public bool IsOnCooldown(string id)
    {
        return skillCDUntil.TryGetValue(id, out var cooldown) && cooldown > Time.time;
    }

    public void StartKillCooldwn(ActiveSkillSO skill, float cooldownMultiplayer = 1f)
    {
        var end = Time.time + (skill.cooldown * cooldownMultiplayer);
        skillCDUntil[skill.skillId] = end;
    }
}
