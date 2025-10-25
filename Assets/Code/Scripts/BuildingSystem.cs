using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private List<GameObject> buildingPoints;
    [SerializeField] private float distanceTreshold = 2.0f;
    [SerializeField] private float gizmoOffsetY = 2.0f;

    [Header("UI & Prefabs")]
    [SerializeField] private BuildingSystemUIManager buildingUIManager;
    [SerializeField] private GameObject buildPointGizmoPrefab;

    private GameObject buildPointGizmo;
    private GameObject nearestPoint;
    private PlayerXP playerXP;

    private void Awake()
    {
        buildPointGizmo = Instantiate(buildPointGizmoPrefab, Vector3.zero, buildPointGizmoPrefab.transform.rotation);
        buildPointGizmo.SetActive(false);
    }

    private void Update()
    {
        if (playerXP == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag);
            if (playerObj != null) playerXP = playerObj.GetComponent<PlayerXP>();
            if (playerXP == null) return;
        }

        nearestPoint = GetActiveNearestBuildingPoint();

        if (nearestPoint == null)
        {
            buildingUIManager.CloseBuildMenu();
            return;
        }

        if (Input.GetKeyDown(KeyCode.E)) 
        {
            buildingUIManager.OpenBuildMenu();
        }
    }

    private GameObject GetActiveNearestBuildingPoint()
    {
        Vector3 playerPos = playerXP.transform.position;
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
        if (nearestPoint == null || playerXP == null) return;

        if (!playerXP.CanBuildTurret())
        {
            Debug.Log("Ya construiste el máximo de torretas permitidas o no tienes nivel suficiente.");
            return;
        }

        playerXP.RegisterTurretBuild();

        Vector3 nearestPos = nearestPoint.transform.position;
        Vector3 spawnPos = new Vector3(nearestPos.x, turret.transform.position.y, nearestPos.z);

        nearestPoint.SetActive(false);
        Instantiate(turret, spawnPos, Quaternion.identity);
        buildingUIManager.CloseBuildMenu();
    }
}