using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

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
            Shoot(1);
            _nextFireTime = Time.time + _playerHealth.GetCurrentFireRate();
        }
    }

    public void Shoot(int projectilesCount)
    {
        float spreadDegrees = 12f;
        float lateralSpacing = 0.05f;

        float half = (projectilesCount > 1) ? (projectilesCount - 1) * 0.5f : 0f;

        for (int i = 0; i < projectilesCount; i++)
        {
            float t = (projectilesCount == 1) ? 0f : (i - half) / half;
            float angle = t * (spreadDegrees * 0.5f) * 2f;

            Quaternion rotation = Quaternion.AngleAxis(angle, FirePoint.up) * FirePoint.rotation;

            Vector3 position = FirePoint.position + (FirePoint.right * (i - half) * lateralSpacing);

            GameObject projectile = Instantiate(ProjectilePrefab, position, rotation);
            AudioManager.Instance.PlayShoot();

            var playerProjectile = projectile.GetComponent<PlayerProyectile>();
            playerProjectile.Initialize(gameObject.transform.parent.gameObject, _playerHealth.GetCurrentDamage());
            if (projectile.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.linearVelocity = rotation * Vector3.forward * _playerHealth.GetCurrentProjectileSpeed();
            }
            if (projectile.TryGetComponent<Collider>(out var projectileCollider) && PlayerCollider != null)
            {
                Physics.IgnoreCollision(projectileCollider, PlayerCollider);
            }

            Destroy(projectile, ProjectileLifetime);
        }
    }

}