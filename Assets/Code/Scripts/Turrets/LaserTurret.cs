using UnityEngine;

public class LaserTurret : Turret
{
    [SerializeField] private GameObject laserPrefab;
    private float cooldown = 0f;

    void Update()
    {
        cooldown -= Time.deltaTime;


        if (cooldown < 0f)
        {
            Transform enemy = GetNearestEnemy();

            if (enemy != null)
            {
                Shoot(enemy);
                cooldown = 1f / attackSpeed;
            }
        }
    }

    void Shoot(Transform enemy)
    {
        GameObject laserGO = Instantiate(laserPrefab, shootingPoint.position, Quaternion.identity);
        Laser laser = laserGO.GetComponent<Laser>();
        laser.SetTarget(enemy);
    }

    Transform GetNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDist = Mathf.Infinity;
        GameObject nearest = null;

        foreach (GameObject e in enemies)
        {
            float dist = Vector3.Distance(transform.position, e.transform.position);
            if (dist < shortestDist && dist <= range)
            {
                shortestDist = dist;
                nearest = e;
            }
        }

        return nearest != null ? nearest.transform : null;
    }
}