using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] EnemyPrefabs; // Array to store different enemy prefabs
    public GameObject EnemyBoss;

    float maxSpawnRateInSeconds = 2f;
    int currentWave = 0;

    public void ResetEnemies()
    {
        currentWave = 0;
        maxSpawnRateInSeconds = 2f;
    }

    void Start()
    {
    }

    void Update()
    {
    }

    void SpawnEnemy()
    {
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        // Select a random enemy prefab from the array
        GameObject selectedEnemy = EnemyPrefabs[Random.Range(0, EnemyPrefabs.Length)];

        GameObject anEnemy = (GameObject)Instantiate(selectedEnemy);
        anEnemy.transform.position = new Vector2(Random.Range(min.x, max.x), max.y);


        currentWave++;
        if (currentWave >= 10)
        {
            UnscheduleEnemySpawner();
            currentWave = 0;

            Invoke("SpawnBoss",3f);


            return;
        }

        ScheduleNextEnemySpawn();

       
    }

    void SpawnBoss()
    {
        if (FindObjectOfType<GameManager>().GMState == GameManager.GameManagerState.GameOver) return;

        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        GameObject selectedEnemy = EnemyBoss;
        GameObject anEnemy = (GameObject)Instantiate(selectedEnemy);
        anEnemy.transform.position = new Vector2(max.x / 2f, max.y / 2f);
    }

    void ScheduleNextEnemySpawn()
    {
        float spawnInNSeconds;

        if (maxSpawnRateInSeconds > 1f)
        {
            spawnInNSeconds = Random.Range(1f, maxSpawnRateInSeconds);
        }
        else
            spawnInNSeconds = 1f;

        Invoke("SpawnEnemy", spawnInNSeconds);
    }

    void IncreaseSpawnRate()
    {
        if (maxSpawnRateInSeconds > 1f)
            maxSpawnRateInSeconds--;

        if (maxSpawnRateInSeconds == 1f)
            CancelInvoke("IncreaseSpawnRate");
    }

    public void ScheduleEnemySpawner()
    {
        if (FindObjectOfType<GameManager>().GMState == GameManager.GameManagerState.GameOver) return;

        Invoke("SpawnEnemy", maxSpawnRateInSeconds);

        InvokeRepeating("IncreaseSpawnRate", 0f, 30f);
    }

    public void UnscheduleEnemySpawner()
    {
        CancelInvoke("SpawnEnemy");
        CancelInvoke("IncreaseSpawnRate");
    }
}