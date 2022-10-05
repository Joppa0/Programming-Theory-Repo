using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefab;

    public int enemyCount;
    public int waveNumber = 1;

    public float spawnRange = 9.0f;

    public LayerMask m_LayerMask;
 
    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemyWave(waveNumber);
    }

    // Update is called once per frame
    void Update()
    {
        //Spawns a new wave and increments the wave number if there are no enemies left
        enemyCount = FindObjectsOfType<EnemyController>().Length;
        if (enemyCount == 0)
        {
            waveNumber++;
            SpawnEnemyWave(waveNumber);
        }
    }

    void SpawnEnemyWave(int enemiesToSpawn)
    {
        //Spawns random new enemies with random positions
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            //Stores the place to spawn the enemy
            Vector3 placeToSpawn = GenerateSpawnPosition();

            int randNumber = Random.Range(0, 3);

            //Creates a box to check for collisions when trying to spawn
            Collider[] collisionWithEnemy = Physics.OverlapBox(placeToSpawn, enemyPrefab[randNumber].transform.localScale / 2, Quaternion.identity, m_LayerMask);

            //If there are collisions other than the ground, spawn a new enemy with a new spawnpoint
            if (collisionWithEnemy.Length > 1)
            {
                enemiesToSpawn++;
            }
            //If there are no other collisions, spawn at the chosen spawnpoint
            else
            {
                Instantiate(enemyPrefab[randNumber], placeToSpawn, enemyPrefab[randNumber].transform.rotation);
            }
        }
    }

    private Vector3 GenerateSpawnPosition()
    {
        //Generates a random position inside a certain distance of the player position
        Vector3 playerPos = GameObject.Find("Player").transform.position;

        float spawnPosX = Random.Range(-spawnRange, spawnRange) + playerPos.x;
        float spawnPosZ = Random.Range(-spawnRange, spawnRange) + playerPos.z;

        Vector3 randomPos = new Vector3(spawnPosX, 0.5f, spawnPosZ);
        return randomPos;
    }
}
