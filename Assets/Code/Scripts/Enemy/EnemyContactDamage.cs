using UnityEngine;

public class EnemyContactDamage : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private float amount = 10f;
    [SerializeField] private float tickInterval = 1f;

    private float _timer;
    private bool _playerInside;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent<IDamageable>(out var target))
        {
            _playerInside = true;
            _timer = 0f;
            target.TakeDamage(amount);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_playerInside && other.CompareTag("Player") && other.TryGetComponent<IDamageable>(out var target))
        {
            _timer += Time.deltaTime;
            if (_timer >= tickInterval)
            {
                target.TakeDamage(amount);
                _timer = 0f;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInside = false;
        }
    }
}
