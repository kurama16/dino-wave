using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [Header("Salud")]
    [Tooltip("Enemy max health")]
    [SerializeField] private float MaxHealth = 100f;
    
    private float _currentHealth;

    public event Action<float, float> OnEnemyHealthChanged;

    private void Awake()
    {
        _currentHealth = MaxHealth;
    }

    public float GetCurrentHealth() => _currentHealth;
    public float GetMaxHealth() => MaxHealth;

    public void TakeDamage(float amount)
    {
        if (amount <= 0)
            return;

        _currentHealth = Mathf.Max(0, _currentHealth - amount);
        OnEnemyHealthChanged?.Invoke(_currentHealth, MaxHealth);

        if (_currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
