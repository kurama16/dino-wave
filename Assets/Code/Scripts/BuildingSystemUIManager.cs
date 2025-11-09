using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingSystemUIManager : MonoBehaviour
{
    [SerializeField] private BuildingSystem buildingSystem;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private Transform contentParent;
    [SerializeField] private GameObject optionPrefab;

    [SerializeField] private List<Turret> turretOptions;

    private void Awake()
    {
        menuPanel.SetActive(false);

        if (turretOptions == null || turretOptions.Count == 0)
        {
            throw new InvalidOperationException("No turret options provided");
        }

        foreach (Turret turret in turretOptions)
        {
            GameObject option = Instantiate(optionPrefab, contentParent);
            option.GetComponentInChildren<TextMeshProUGUI>().text = turret.TurretName;

            Button btn = option.GetComponent<Button>();
            btn.onClick.AddListener(() => OnBuildingOptionClick(turret));
        }

    }

    public void OpenBuildMenu()
    {
        menuPanel.SetActive(true);
        Debug.Log("OpenBuildMenu: " + menuPanel.activeSelf);
    }

    public void CloseBuildMenu()
    {
        menuPanel.SetActive(false);
    }

    public void OnBuildingOptionClick(Turret turret)
    {
        Debug.Log("Clicked " + turret.TurretName);
        buildingSystem.BuildTurret(turret);
    }
}