using UnityEngine;

public class PlayerProyectile : MonoBehaviour
{
    private float _currentDamage;

    public void SetDamage(float damage) => _currentDamage = damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent<IDamageable>(out var target))
            {
                target.TakeDamage(_currentDamage);
            }

            Destroy(gameObject);
        }
    }
}
