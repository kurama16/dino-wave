using NUnit.Framework;
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

    void Awake()
    {
        if (player == null) {
            throw new InvalidOperationException("Player cannot be null");
        }

        if (buildingPoints == null || buildingPoints.Count == 0) {
            throw new InvalidOperationException("Building points are null or empty");
        }

        if (buildPointGizmoPrefab == null)
        {
            throw new InvalidOperationException("No build point gizmo provided");
        }

        buildPointGizmo = Instantiate(buildPointGizmoPrefab, Vector3.zero, buildPointGizmoPrefab.transform.rotation);
        buildPointGizmo.SetActive(false);
    }

    void Update()
    {
        nearestPoint = GetActiveNearestBuildingPoint();

        if (nearestPoint == null) {
            buildingUIManager.CloseBuildMenu();
            return;
        }

        if (Input.GetKeyDown(openMenuKey)) {
            buildingUIManager.OpenBuildMenu();
        }
    }

    private GameObject GetActiveNearestBuildingPoint()
    {
        // No me interesa el eje Y para calcular la distancia
        Vector2 playerPosition = new Vector2(player.transform.position.x, player.transform.position.z);

        foreach (GameObject buildPoint in buildingPoints)
        {
            if (!buildPoint.activeSelf)
            {
                continue;
            }

            Vector2 buildPointPosition = new Vector2(buildPoint.transform.position.x, buildPoint.transform.position.z);

            if (Vector2.Distance(playerPosition, buildPointPosition) < distanceTreshold) {
                ShowGizmo(buildPoint.transform.position);
                return buildPoint;
            }
        }

        HideGizmo();
        return null;
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
        {
            return;
        }

        Vector3 nearestPos = nearestPoint.transform.position;
        Vector3 spawnPos = new Vector3(nearestPos.x, turret.transform.position.y, nearestPos.z);

        nearestPoint.SetActive(false);
        Instantiate(turret, spawnPos, Quaternion.identity);
        buildingUIManager.CloseBuildMenu();
    }
}
