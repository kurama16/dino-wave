using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemiesToSpawn;
    [SerializeField] private List<GameObject> enemySpawnPoint;
    [SerializeField] private float timeBetweenWaves = 30f;
    [SerializeField] private int enemyAmount = 8;
    [SerializeField] private int maxWaves = 8;
    [SerializeField] private int currentWave = 0;
    [SerializeField] private TMP_Text nextWaveText;
    [SerializeField] private TMP_Text waveCount;

    public UnityEvent onGameWon;

    private float timer = 0;
    private int _currentEnemies = 0;

    private bool _spawningComplete = false;

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

                var enemy = Instantiate(enemyToSpawn, spawnPoint.transform.position, spawnPoint.transform.rotation);
                enemy.GetComponent<EnemyHealth>().OnEnemyDie.AddListener(HandleEnemyDie);

                _currentEnemies++;
            }
            timer = timeBetweenWaves;
            currentWave++;
            NotifySpawningComplete();
        }
    }

    private void HandleEnemyDie()
    {
        _currentEnemies = Mathf.Max(0, _currentEnemies - 1);
        if (_spawningComplete) 
            StartCoroutine(TryCompleteWaveEndOfFrame());
    }

    private void NotifySpawningComplete()
    {
        _spawningComplete = true;
        if (_currentEnemies == 0) 
            StartCoroutine(TryCompleteWaveEndOfFrame());
    }

    private System.Collections.IEnumerator TryCompleteWaveEndOfFrame()
    {
        yield return new WaitForEndOfFrame();

        if (!_spawningComplete || _currentEnemies > 0) 
            yield break;

        bool isLastWave = currentWave >= maxWaves;
        if (isLastWave)
        {
            Debug.Log("onGameWon");
            onGameWon?.Invoke();
        }
        else
        {
            _spawningComplete = false;
        }
    }

    private void UpdateWaveText() {

        if (nextWaveText == null || waveCount == null)
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
        waveCount.text = "Wave " + currentWave + "/" + maxWaves;


    }

}
