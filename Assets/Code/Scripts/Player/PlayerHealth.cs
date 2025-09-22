using System;
using UnityEngine;

public interface IDamageable { void TakeDamage(float amount); }

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [Tooltip("Player max health")]
    [SerializeField] private float MaxHealth = 100f;

    private float _currentHealth;

    public event Action<float, float> OnPlayerHealthChanged;
    
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
        OnPlayerHealthChanged?.Invoke(_currentHealth, MaxHealth);

        if(_currentHealth == 0)
            Die();
    }

    private void Die()
    {
        gameObject.SetActive(false);
        Debug.Log("Player has die");
    }
}
