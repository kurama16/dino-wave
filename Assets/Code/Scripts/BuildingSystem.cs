using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    [Header("Object Refs")]
    [SerializeField] private GameObject player;
    [SerializeField] private BuildingSystemUIManager buildingUIManager;

    [Header("Configuration")]
    [SerializeField] private KeyCode openMenuKey;
    [SerializeField] private List<GameObject> buildingPoints;
    [SerializeField] private float distanceTreshold = 2.0f;
    [SerializeField] private float gizmoOffsetY = 2.0f;

    [Header("Assets")]
    [SerializeField] private GameObject buildPointGizmoPrefab;

    private GameObject buildPointGizmo;
    private GameObject nearestPoint;
    private PlayerStats playerStats;

    void Awake()
    {
        if (player == null) throw new InvalidOperationException("Player no asignado");
        if (buildingPoints == null || buildingPoints.Count == 0) throw new InvalidOperationException("Building points vacíos");
        if (buildPointGizmoPrefab == null) throw new InvalidOperationException("No se asignó buildPointGizmoPrefab");

        playerStats = player.GetComponent<PlayerStats>();
        if (playerStats == null) throw new InvalidOperationException("El Player no tiene PlayerStats asignado");

        buildPointGizmo = Instantiate(buildPointGizmoPrefab, Vector3.zero, buildPointGizmoPrefab.transform.rotation);
        buildPointGizmo.SetActive(false);
    }

    void Update()
    {
        nearestPoint = GetActiveNearestBuildingPoint();

        if (nearestPoint == null)
        {
            buildingUIManager.CloseBuildMenu();
            return;
        }

        if (Input.GetKeyDown(openMenuKey))
        {
            buildingUIManager.OpenBuildMenu();
        }
    }

    private GameObject GetActiveNearestBuildingPoint()
    {
        Vector3 playerPos = player.transform.position;
        GameObject closest = null;
        float shortestDist = float.MaxValue;

        foreach (var point in buildingPoints)
        {
            if (!point.activeSelf) continue;

            Vector2 pointPos2D = new Vector2(point.transform.position.x, point.transform.position.z);
            Vector2 playerPos2D = new Vector2(playerPos.x, playerPos.z);
            float dist = Vector2.Distance(playerPos2D, pointPos2D);

            if (dist < distanceTreshold && dist < shortestDist)
            {
                shortestDist = dist;
                closest = point;
            }
        }

        if (closest != null)
            ShowGizmo(closest.transform.position);
        else
            HideGizmo();

        return closest;
    }

    private void ShowGizmo(Vector3 position)
    {
        buildPointGizmo.transform.position = new Vector3(position.x, position.y + gizmoOffsetY, position.z);
        buildPointGizmo.SetActive(true);
    }

    private void HideGizmo()
    {
        buildPointGizmo.SetActive(false);
    }

    public void BuildTurret(Turret turret)
    {
        if (nearestPoint == null)
            return;

        //TODO: Mover la validacion del requerimiento al player y prevenir directamente que muestre el canvas.
        if (playerStats.GetTurretBuildCount() < playerStats.GetTurretBuiltLimit())
        {
            playerStats.IncreaseTurretBuildCount();
            //TODO: Una vez que se incrementa disparar el quest tracker q todavia no esta creado
            if (player.TryGetComponent(out PlayerXP playerXP))
            {
                playerXP.RegisterTurretBuild();
            }

            AudioManager.Instance.PlayBuild();
            Vector3 nearestPos = nearestPoint.transform.position;
            Vector3 spawnPos = new Vector3(nearestPos.x, turret.transform.position.y, nearestPos.z);

            nearestPoint.SetActive(false);
            Instantiate(turret, spawnPos, Quaternion.identity);
            buildingUIManager.CloseBuildMenu();
        }
    }
}