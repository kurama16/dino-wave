using System;
using UnityEngine;

public class TimeGapController : MonoBehaviour, IDamageable
{
    [Header("TimeGap stats")]
    [SerializeField] private float maxHealth = 150f;

    [Header("TimeGap levitation movement")]
    [SerializeField] private float floatAmplitude = 0.5f;
    [SerializeField] private float floatFrequency = 2f;
    
    private Vector3 _startPos;
    private float _currentHealth;
    private bool _isDead = false;

    public event Action<float, float> OnTimeGapHealthChanged;
    public event Action OnTimeGapDestroy;

    private void Awake()
    {
        _currentHealth = maxHealth;
    }

    private void Start()
    {
        _startPos = transform.position;
    }

    private void Update()
    {
        float newY = _startPos.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    public float GetCurrentHealth() => _currentHealth;

    public float GetMaxHealth() => maxHealth;

    public void TakeDamage(float amount)
    {
        if (_isDead || amount <= 0) 
            return;

        _currentHealth -= amount;

        OnTimeGapHealthChanged?.Invoke(_currentHealth, maxHealth);

        if (_currentHealth <= 0)
        {
            Die();
            OnTimeGapDestroy.Invoke();
        }
            
    }

    private void Die()
    {
        if (_isDead) 
            return;
        
        _isDead = true;
        Destroy(gameObject);
    }
}