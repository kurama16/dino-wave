using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject ProjectilePrefab;
    public Transform FirePoint;
    public float ProjectileSpeed = 20f;
    public float FireRate = 0.5f;
    public float ProjectileLifetime = 5f;
    public Collider PlayerCollider;

    private float _nextFireTime;

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= _nextFireTime)
        {
            Shoot();
            _nextFireTime = Time.time + FireRate;
        }
    }

    private void Shoot()
    {
        GameObject projectile = Instantiate(ProjectilePrefab, FirePoint.position, FirePoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        Collider projectileCollider = projectile.GetComponent<Collider>();

        if (rb != null)
        {
            rb.linearVelocity = FirePoint.forward * ProjectileSpeed;
        }

        if (projectileCollider != null && PlayerCollider != null)
        {
            Physics.IgnoreCollision(projectileCollider, PlayerCollider);
        }

        Destroy(projectile, ProjectileLifetime);
    }
}