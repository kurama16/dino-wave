using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Passive/All In")]
public class HealOnKillSO : PassiveSkillSO
{
    public float leechValue = 2f;

    public override void Apply(GameObject caster)
    {
        var bindings = caster.GetComponent<PassiveBindings>();

        CombatEvents.OnEnemyKilled += OnEnemyKilled;

        bindings.Bind(this, () => CombatEvents.OnEnemyKilled -= OnEnemyKilled);
    }

    public override void Unapply(GameObject caster)
    {
        var bindings = caster.GetComponent<PassiveBindings>();
        bindings?.Unbind(this);
    }

    void OnEnemyKilled(GameObject killer, GameObject victim)
    {
        if (killer.CompareTag("Player"))
        {
            if (killer.TryGetComponent<PlayerStats>(out var playerHealth))
            {
                playerHealth.RestoreHealth(leechValue);
            }
        }
    }
}