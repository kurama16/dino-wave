using System;
using UnityEngine;

public interface IDamageable { void TakeDamage(float amount, GameObject doneBy); }

public class PlayerStats : MonoBehaviour, IDamageable
{
    [Header("Health Stats")]
    [Tooltip("How many lives the player has")]
    [SerializeField] private int startingLives = 3;
    [SerializeField] private Transform playerSpawnPoint;
    [Header("Damage Stats")]
    [SerializeField] private float projectileSpeed = 20f;
    [SerializeField] private float fireRate = 0.5f;

    private float _currentHealth = 0;
    private float _currentDamage = 0;
    private float _currentProjectileSpeed;
    private float _currentFireRate;
    private float _maxHealth;
    private int _currentTurretLimit = 0;
    private int _currentTurretBuiltCount = 0;

    public event Action<float, float> OnPlayerHealthChanged;
    public event Action<int> OnLivesChanged;
    public event Action OnPlayerDie;

    private int _currentLives { get; set; }

    private void Awake()
    {
        _currentLives = startingLives;
    }

    private void Start()
    {
        _currentProjectileSpeed = projectileSpeed;
        _currentFireRate = fireRate;
        OnLivesChanged?.Invoke(_currentLives);
        Debug.Log("_currentHealth: " + _currentHealth);
        Debug.Log("_currentDamage: " + _currentDamage);
        Debug.Log("_currentProjectileSpeed: " + _currentProjectileSpeed);
        Debug.Log("_currentFireRate: " + _currentFireRate);
        Debug.Log("_currentTurretLimit: " + _currentTurretLimit);
    }

    public float GetCurrentHealth() => _currentHealth;

    public void SetCurrentHealth(float health) => _currentHealth = health;

    public float GetMaxHealth() => _maxHealth;

    public void SetMaxHealth(float maxHealth) => _maxHealth = maxHealth;

    public int GetCurrentLives() => _currentLives;

    public void IncreaseMaxHealth(float health) 
    {
        _maxHealth += health; 
        Mathf.Min(_currentHealth + health, _maxHealth); 
    }

    public void IncreaseDamage(float damage) => _currentDamage += damage;

    public float GetCurrentDamage() => _currentDamage;

    public float GetCurrentFireRate() => _currentFireRate;

    public float GetCurrentProjectileSpeed() => _currentProjectileSpeed;

    public void ReduceFireRate(float fireRate) => _currentFireRate -= fireRate;

    public int GetTurretBuiltLimit() => _currentTurretLimit;

    public void IncreaseTurretBuildLimit(int limit) => _currentTurretLimit += limit;

    public int GetTurretBuildCount() => _currentTurretBuiltCount;

    public int IncreaseTurretBuildCount() => _currentTurretBuiltCount += 1;

    public void RestoreHealth(float amount)
    {
        if (amount <= 0)
            return;

        _currentHealth = Mathf.Min(_maxHealth, _currentHealth + amount);
        OnPlayerHealthChanged?.Invoke(_currentHealth, _maxHealth);
    }

    public void TakeDamage(float amount, GameObject doneBy)
    {
        if (amount <= 0)
            return;

        AudioManager.Instance.PlayPlayerHit();

        _currentHealth = Mathf.Max(0, _currentHealth - amount);
        OnPlayerHealthChanged?.Invoke(_currentHealth, _maxHealth);
        Debug.Log(_currentHealth);

        if(_currentHealth == 0)
            HandleHealthDepleted();
    }

    private void HandleHealthDepleted()
    {
        _currentLives--;
        OnLivesChanged?.Invoke(_currentLives);

        if(_currentLives >= 0)
        {
            _currentHealth = _maxHealth;
            gameObject.transform.position = playerSpawnPoint.transform.position;
            OnPlayerHealthChanged?.Invoke(_currentHealth, _maxHealth);
        }

        if(_currentLives == 0)
        {
            OnPlayerDie?.Invoke();
            Die();
        }
    }

    private void Die()
    {
        if (TryGetComponent<PlayerAbilityController>(out var abilityController))
            abilityController.UnapplyAllPassives();

        gameObject.SetActive(false);
    }
}
