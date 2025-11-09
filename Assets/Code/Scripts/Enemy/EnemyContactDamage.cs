using UnityEngine;

public class EnemyContactDamage : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private float amount = 10f;
    [SerializeField] private float tickInterval = 1f;

    private float _playerContactDamageTimer;
    private bool _playerInContact;
    private float _breachContactDamageTimer;
    private bool _breachInContact;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent<IDamageable>(out var playerTarget))
        {
            _playerInContact = true;
            _playerContactDamageTimer = 0f;
            playerTarget.TakeDamage(amount, other.gameObject);
        }

        if(other.CompareTag("TimeGap") && other.TryGetComponent<IDamageable>(out var breachTarget))
        {
            _breachInContact = true;
            _breachContactDamageTimer = 0f;
            breachTarget.TakeDamage(amount, other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_playerInContact && other.CompareTag("Player") && other.TryGetComponent<IDamageable>(out var playerTarget))
        {
            _playerContactDamageTimer += Time.deltaTime;
            if (_playerContactDamageTimer >= tickInterval)
            {
                playerTarget.TakeDamage(amount, other.gameObject);
                _playerContactDamageTimer = 0f;
            }
        }

        if (_breachInContact && other.CompareTag("TimeGap") && other.TryGetComponent<IDamageable>(out var breachTarget))
        {
            _breachContactDamageTimer += Time.deltaTime;
            if (_breachContactDamageTimer >= tickInterval)
            {
                breachTarget.TakeDamage(amount, other.gameObject);
                _breachContactDamageTimer = 0f;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInContact = false;
        }

        if (other.CompareTag("TimeGap"))
        {
            _breachInContact = false;
        }
    }
}
