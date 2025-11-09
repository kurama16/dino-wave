using UnityEngine;

public static class CombatEvents
{
    public static event System.Action<GameObject, GameObject> OnEnemyKilled;

    public static void RaiseEnemyKilled(GameObject killer, GameObject victim) => OnEnemyKilled?.Invoke(victim, killer);
}
