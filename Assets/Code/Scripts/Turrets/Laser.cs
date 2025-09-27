using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private int damage = 10;

    private Transform target;

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += speed * Time.deltaTime * dir;

        if (Vector3.Distance(transform.position, target.position) < 0.2f)
        {
            target.GetComponent<IDamageable>()?.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}