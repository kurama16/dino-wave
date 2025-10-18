using System;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [Tooltip("Enemy max health")]
    [SerializeField] private float MaxHealth = 100f;

    private float _currentHealth;

    public event Action<float, float> OnEnemyHealthChanged;
    public UnityEvent OnEnemyDie;

    private void Awake()
    {
        _currentHealth = MaxHealth;
    }

    public float GetCurrentHealth() => _currentHealth;
    public float GetMaxHealth() => MaxHealth;

    public void TakeDamage(float amount)
    {
        if (amount <= 0) return;

        _currentHealth = Mathf.Max(0, _currentHealth - amount);
        OnEnemyHealthChanged?.Invoke(_currentHealth, MaxHealth);

        if (_currentHealth <= 0)
            Die();
    }

    private void Die()
{
    EnemyXPDrop xpDrop = GetComponent<EnemyXPDrop>();
    if (xpDrop != null) xpDrop.Die();

    OnEnemyDie?.Invoke();
    Destroy(gameObject);
}
}
