using System;
using UnityEngine;

public interface IDamageable { void TakeDamage(float amount); }

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Salud")]
    [Tooltip("Player max health")]
    [SerializeField] private float MaxHealth = 100f;

    private float _currentHealth;

    public event Action<float, float> OnPlayerHealthChanged;
    
    private void Awake()
    {
        _currentHealth = MaxHealth;
        Debug.LogWarning("Eliminar GetKeyDown M en PlayerHealth script antes de hacer el push");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            this.TakeDamage(10);
    }

    public float GetCurrentHealth() => _currentHealth;
    public float GetMaxHealth() => MaxHealth;

    public void TakeDamage(float amount)
    {
        if (amount <= 0)
            return;

        _currentHealth = Mathf.Max(0, _currentHealth - amount);
        OnPlayerHealthChanged?.Invoke(_currentHealth, MaxHealth);
    }
}
