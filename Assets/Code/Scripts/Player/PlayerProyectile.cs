using UnityEngine;

public class PlayerProyectile : MonoBehaviour
{
    [Tooltip("Da�o que hace el proyectil")]
    [SerializeField] float damage = 10f;
    [Tooltip("Tiempo de vida del proyectil")]
    [SerializeField] float lifeTime = 5f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.GetComponent<Collider>().TryGetComponent<IDamageable>(out var target))
            {
                target.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
