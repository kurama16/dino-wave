using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject ProjectilePrefab;
    [SerializeField] private Transform FirePoint;
    [SerializeField] private float ProjectileLifetime = 5f;
    [SerializeField] private Collider PlayerCollider;

    private float _nextFireTime;
    private PlayerStats _playerHealth;

    private void Awake()
    {
        _playerHealth = GetComponentInParent<PlayerStats>();
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= _nextFireTime)
        {
            Shoot();
            _nextFireTime = Time.time + _playerHealth.GetCurrentFireRate();
        }
    }

    private void Shoot()
    {
        GameObject projectile = Instantiate(ProjectilePrefab, FirePoint.position, FirePoint.rotation);
        AudioManager.Instance.PlayShoot();

        var playerProjectile = projectile.GetComponent<PlayerProyectile>();
        playerProjectile.SetDamage(_playerHealth.GetCurrentDamage());
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        Collider projectileCollider = projectile.GetComponent<Collider>();

        if (rb != null)
        {
            rb.linearVelocity = FirePoint.forward * _playerHealth.GetCurrentProjectileSpeed();
        }

        if (projectileCollider != null && PlayerCollider != null)
        {
            Physics.IgnoreCollision(projectileCollider, PlayerCollider);
        }

        Destroy(projectile, ProjectileLifetime);
    }
}