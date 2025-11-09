using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class PlayerProyectile : MonoBehaviour
{
    private float _currentDamage;
    private GameObject _owner;

    public void SetDamage(float damage) => _currentDamage = damage;
    
    public void Initialize(GameObject owner, float damage)
    {
        _owner = owner;
        _currentDamage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent<IDamageable>(out var target))
            {
                target.TakeDamage(_currentDamage, _owner);
            }

            Destroy(gameObject);
        }
    }
}
