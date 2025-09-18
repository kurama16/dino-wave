using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemiesToSpawn;
    [SerializeField] private List<GameObject> enemySpawnPoint;
    [SerializeField] private float timeBetweenWaves = 30f;
    [SerializeField] private int enemyAmount = 8;
    [SerializeField] private int maxWaves = 8;
    [SerializeField] private int currentWave = 0;
    [SerializeField] private TMP_Text nextWaveText; 

    private float timer;


    void Awake()
    {
        timer = timeBetweenWaves;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        UpdateWaveText();


        if (timer <= 0 && currentWave < maxWaves)
        {
            for (int i = 0; i < enemyAmount + currentWave; i++)
            {
                GameObject enemyToSpawn = enemiesToSpawn[Random.Range(0, enemiesToSpawn.Count)];
                GameObject spawnPoint = enemySpawnPoint[Random.Range(0, enemySpawnPoint.Count)];

                Instantiate(enemyToSpawn, spawnPoint.transform.position, spawnPoint.transform.rotation);
            }
            timer = timeBetweenWaves;
            currentWave++;
        }

    }

    private void UpdateWaveText() {

        if (nextWaveText == null)
        {
            return;
        }
        if (currentWave < maxWaves)
        {
            nextWaveText.text = "Next wave in " + Mathf.CeilToInt(timer);
        }
        else
        {
            nextWaveText.text = "Last wave";
        }

    }

}
