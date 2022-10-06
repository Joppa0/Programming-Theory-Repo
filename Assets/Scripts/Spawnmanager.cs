using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefab;
    public GameObject[] powerupPrefab;

    private int enemyCount;
    private int waveNumber = 2;

    [SerializeField] private float spawnRangeX = 20.0f;
    [SerializeField] private float spawnRangeZ = 7.5f;

    public LayerMask m_LayerMask;
    public Camera cam;
 
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        SpawnEnemyWave(waveNumber);
    }

    // Update is called once per frame
    void Update()
    {
        //Spawns a new wave and increments the wave number if there are no enemies left
        enemyCount = FindObjectsOfType<EnemyController>().Length;
        if (enemyCount == 1)
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
            Collider[] collisionWithEnemy = Physics.OverlapBox(placeToSpawn, enemyPrefab[randNumber].transform.localScale, Quaternion.identity, m_LayerMask);

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
        //Generates a random spawn position outside the camera's view
        bool goodSpawn = false;

        Vector3 finalSpawn = new Vector3();

        while (goodSpawn == false)
        {
            Vector3 playerPos = GameObject.Find("Player").transform.position;

            float spawnPosX = Random.Range(-spawnRangeX, spawnRangeX) + playerPos.x;
            float spawnPosZ = Random.Range(-spawnRangeZ, spawnRangeZ) + playerPos.z;

            Vector3 randomPos = new Vector3(spawnPosX, 0.5f, spawnPosZ);

            Vector3 pos = cam.WorldToViewportPoint(new Vector3(randomPos.x, cam.nearClipPlane, randomPos.z));

            if (!(pos.x < 1 && pos.x > 0) || !(pos.y < 1 && pos.y > 0))
            {
                goodSpawn = true;
                finalSpawn = randomPos;
            }
        }
        return finalSpawn;
    }
}
