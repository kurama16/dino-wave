using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityController : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private PlayerArchetypeSO archetype;
    [SerializeField] private LevelProgressionSO progression;

    [Header("Runtime")]
    [SerializeField] private List<ActiveSkillSO> activeSkills = new();
    [SerializeField] private List<PassiveSkillSO> passiveSkills = new();

    private CooldownService _cooldownService;
    private PlayerStats _playerStats;
    private int _levelIndexProgression = 0;
    private float _globalCDMultiplier = 1f;

    private void Awake()
    {
        _playerStats = GetComponent<PlayerStats>();
        _cooldownService = GetComponent<CooldownService>();

        _playerStats.SetMaxHealth(archetype.baseMaxHealth);
        _playerStats.IncreaseDamage(archetype.baseDamage);
        activeSkills.AddRange(archetype.startingActiveSkills);
        passiveSkills.AddRange(archetype.startingPassiveSkills);

        foreach (var passiveSkill in passiveSkills)
            passiveSkill.Apply(gameObject);
    }

    public bool TryUse(ActiveSkillSO skill)
    {
        if (!activeSkills.Contains(skill))
            return false;
        if (_cooldownService.IsOnCooldown(skill.skillId)) 
            return false;

        skill.Execute(gameObject);
        _cooldownService.StartKillCooldwn(skill, _globalCDMultiplier);

        return true;
    }

    public void LevelUp()
    {
        _levelIndexProgression++;
        var index = _levelIndexProgression - 1;
        if (index > progression.levels.Length)
        {
            Debug.Log("Max level reached!!!");
            return;
        }    

        var entry = progression.levels[index];

        //life
        if(entry.addMaxHealth != 0f)
        {
            _playerStats.IncreaseMaxHealth(entry.addMaxHealth);
            Debug.Log("addMaxHealth: " + _playerStats.GetMaxHealth());
        }

        //global cooldown
        if(entry.globalCooldownMultiplierReduction != 0f)
        {
            _globalCDMultiplier -= entry.globalCooldownMultiplierReduction;
            Debug.Log("globalCooldownMultiplierReduction: " + _globalCDMultiplier);
        }

        //fire rate
        if (entry.reduceFireRate != 0f)
        {
            _playerStats.ReduceFireRate(entry.reduceFireRate);
            Debug.Log("reduceFireRate: " + _playerStats.GetCurrentFireRate());
        }
        
        //damage
        if(entry.addMaxDamage != 0f)
        {
            _playerStats.IncreaseDamage(entry.addMaxDamage);
            Debug.Log("addMaxDamage: " + _playerStats.GetCurrentDamage());
        }

        if (entry.addTurretLimit != 0) 
        {
            _playerStats.IncreaseTurretBuildLimit(entry.addTurretLimit);
            Debug.Log("addTurrentLimit: " + _playerStats.GetTurretBuiltLimit());
        }

        //skills
        //if (entry.grantPassives != null)
        //{
        //    foreach(var passive in entry.grantPassives)
        //    {
        //        if (!passiveSkills.Contains(passive))
        //        {
        //            if (!passiveSkills.Contains(passive))
        //            {
        //                passiveSkills.Add(passive);
        //                passive.Apply(gameObject);
        //            }
        //        }
        //    }
        //}
        //if(entry.grantActives != null)
        //{
        //    foreach (var active in entry.grantActives)
        //    {
        //        if(!activeSkills.Contains(active))
        //            activeSkills.Add(active);
        //    }
        //}
    }
}
