using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefab;
    public GameObject[] powerupPrefab;

    public LayerMask m_LayerMask;
    public Camera cam;

    private RoundCount roundCountScript;

    private int enemyCount;
    private int waveNumber = 2;

    private float enemySpawnRangeX = 20.0f;
    private float enemySpawnRangeZ = 7.5f;
    private float prefabSpawnRangeX = 12f;
    private float prefabSpawnRangeZ = 4f;

    private Vector3 testvector;
    private int testint;

    // Start is called before the first frame update
    void Start()
    {
        roundCountScript = GameObject.Find("RoundCounter").GetComponent<RoundCount>();
        cam = Camera.main;
        SpawnEnemyWave(waveNumber, false);
    }

    // Update is called once per frame
    void Update()
    {
        CheckEnemyCount();
    }

    // Checks if a new wave should spawn.
    void CheckEnemyCount()
    {
        enemyCount = FindObjectsOfType<EnemyController>().Length;
        if (enemyCount == 1)
        {
            waveNumber++;
            if (waveNumber % 10 == 0)
            {
                SpawnEnemyWave(waveNumber, true);
            }
            else
            {
                SpawnEnemyWave(waveNumber, false);
            }
        }
    }

    // Spawns the wave.
    void SpawnEnemyWave(int enemiesToSpawn, bool bossWave)
    {
        roundCountScript.UpdateRoundCounter(enemiesToSpawn - 1);

        Vector3 placeToSpawn = new Vector3();

        // Spawns random new enemies with random positions.
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            placeToSpawn = GenerateEnemySpawnPosition();

            int randNumber = UnityEngine.Random.Range(0, 3);

            // Guarantees one boss spawning if it's a bosswave.
            if (bossWave)
            {
                randNumber = 3;
                bossWave = false;
            }

            // Creates a box to check for collisions when trying to spawn.
            Collider[] collisionWithEnemy = Physics.OverlapBox(new Vector3(placeToSpawn.x, (placeToSpawn.y + 1.2f), placeToSpawn.z), enemyPrefab[randNumber].transform.localScale * 3, Quaternion.identity, m_LayerMask);
            
            if (collisionWithEnemy.Length > 1)
            {
                // If there are collisions other than the ground, spawn a new enemy with a new spawnpoint.
                enemiesToSpawn++;
            }
            else
            {
                // If there are no other collisions, spawn at the chosen spawnpoint.
                Instantiate(enemyPrefab[randNumber], placeToSpawn, enemyPrefab[randNumber].transform.rotation);
            }
        }

        // Calls the method to spawn a new powerup at the start of a new wave.
        if (GameObject.FindObjectsOfType<PowerUp>().Length == 0)
        {
            InstantiatePowerUp(10);
        }
    }

    // Tries spawning the powerup until a clear location is found or the max amount of iterations is reached.
    void InstantiatePowerUp(int maxAttempts)
    {
        Vector3 placeToSpawn = GeneratePowerupSpawnPosition();

        int randNumber = UnityEngine.Random.Range(0, 5);

        GameObject powerup = Instantiate(powerupPrefab[randNumber], placeToSpawn, Quaternion.identity);

        float radius = powerup.GetComponent<SphereCollider>().radius * 1.5f;

        Collider[] collisions = Physics.OverlapSphere(placeToSpawn, radius);

        if (collisions.Length > 0 && maxAttempts > 0)
        {
            Destroy(powerup);
            InstantiatePowerUp(maxAttempts - 1);
        }
    }

    // Spawns the permanent health increase when a troll boss is killed.
    public void SpawnHealthIncrease(Vector3 spawnPos)
    {
        Instantiate(powerupPrefab[5], spawnPos, Quaternion.identity);
    }


    // Generates a spawnpoint for the powerup.
    private Vector3 GeneratePowerupSpawnPosition()
    {
        Vector3 playerPos = GameObject.Find("Player").transform.position;
        Vector3 spawnPos = new Vector3(UnityEngine.Random.Range(-prefabSpawnRangeX, prefabSpawnRangeX) + playerPos.x, 1.2f, UnityEngine.Random.Range(-prefabSpawnRangeZ, prefabSpawnRangeZ) + playerPos.z);

        return spawnPos;
    }

    // Generates a random spawn position outside the camera's view for each enemy.
    private Vector3 GenerateEnemySpawnPosition()
    {
        Vector3 playerPos = GameObject.Find("Player").transform.position;

        Vector3 finalSpawn = new Vector3();
        Vector3 randomPos = new Vector3();
        Vector3 pos = new Vector3();

        do
        {
            float spawnPosX = UnityEngine.Random.Range(-enemySpawnRangeX, enemySpawnRangeX) + playerPos.x;
            float spawnPosZ = UnityEngine.Random.Range(-enemySpawnRangeZ, enemySpawnRangeZ) + playerPos.z;

            randomPos = new Vector3(spawnPosX, 0.5f, spawnPosZ);

            pos = cam.WorldToViewportPoint(new Vector3(randomPos.x, cam.nearClipPlane, randomPos.z));
        } while ((pos.x < 1 && pos.x > 0) || (pos.y < 1 && pos.y > 0));

        finalSpawn = randomPos;

        return finalSpawn;
    }
}
