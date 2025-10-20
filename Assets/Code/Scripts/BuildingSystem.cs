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
    private PlayerXP playerXP;

    void Awake()
    {
        if (player == null) throw new InvalidOperationException("Player no asignado");
        if (buildingPoints == null || buildingPoints.Count == 0) throw new InvalidOperationException("Building points vacíos");
        if (buildPointGizmoPrefab == null) throw new InvalidOperationException("No se asignó buildPointGizmoPrefab");

        playerXP = player.GetComponent<PlayerXP>();
        if (playerXP == null) throw new InvalidOperationException("El Player no tiene PlayerXP asignado");

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
        Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.z);

        foreach (GameObject buildPoint in buildingPoints)
        {
            if (!buildPoint.activeSelf) continue;

            Vector2 buildPointPos = new Vector2(buildPoint.transform.position.x, buildPoint.transform.position.z);

            if (Vector2.Distance(playerPos, buildPointPos) < distanceTreshold)
            {
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
        if (nearestPoint == null) return;

        int playerLevel = playerXP.CurrentLevel;
        int nextLevelRequirement = playerXP.NextTurretLevelRequirement();

        if (playerLevel >= nextLevelRequirement)
        {
            playerXP.RegisterTurretBuild();

            AudioManager.Instance.PlayBuild();
            Vector3 nearestPos = nearestPoint.transform.position;
            Vector3 spawnPos = new Vector3(nearestPos.x, turret.transform.position.y, nearestPos.z);

            nearestPoint.SetActive(false);
            Instantiate(turret, spawnPos, Quaternion.identity);
            buildingUIManager.CloseBuildMenu();
        }
        else
        {
            Debug.Log($"No puedes construir esta torreta todavía. Nivel requerido: {nextLevelRequirement}");
        }
    }
}