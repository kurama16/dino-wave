using System;
using UnityEngine;

public interface IDamageable { void TakeDamage(float amount); }

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [Tooltip("Player max health")]
    [SerializeField] private float maxHealth = 100f;
    [Tooltip("How many lives the player has")]
    [SerializeField] private int startingLives = 3;
    [SerializeField] private Transform playerSpawnPoint;

    private float _currentHealth;

    public event Action<float, float> OnPlayerHealthChanged;
    public event Action<int> OnLivesChanged;
    public event Action OnPlayerDie;

    private int _currentLives { get; set; }

    private void Awake()
    {
        _currentLives = startingLives;
        _currentHealth = maxHealth;
    }

    private void Start()
    {
        OnLivesChanged?.Invoke(_currentLives);
    }

    public float GetCurrentHealth() => _currentHealth;
    public float GetMaxHealth() => maxHealth;
    public int GetCurrentLives() => _currentLives;

    public void TakeDamage(float amount)
    {
        if (amount <= 0)
            return;

        AudioManager.Instance.PlayPlayerHit();

        _currentHealth = Mathf.Max(0, _currentHealth - amount);
        OnPlayerHealthChanged?.Invoke(_currentHealth, maxHealth);

        if(_currentHealth == 0)
            HandleHealthDepleted();
    }

    private void HandleHealthDepleted()
    {
        _currentLives--;
        OnLivesChanged?.Invoke(_currentLives);

        if(_currentLives >= 0)
        {
            _currentHealth = maxHealth;
            gameObject.transform.position = playerSpawnPoint.transform.position;
            OnPlayerHealthChanged?.Invoke(_currentHealth, maxHealth);
        }

        if(_currentLives == 0)
        {
            OnPlayerDie?.Invoke();
            Die();
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }
}
