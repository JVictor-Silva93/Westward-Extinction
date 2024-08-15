using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public static EnemySpawnManager Instance;

    [SerializeField] private EnemyController tempEnemy;
    [SerializeField] private List<EnemySO> enemies;

    private float currentSpawnTime;
    [SerializeField] private int spawnTime;

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnEnemy()
    {
        tempEnemy = EnemyPool.SharedInstance.GetPooledEnemies();
        tempEnemy.gameObject.SetActive(true);

        // random spawning of enemies, and they will target one player at random
        tempEnemy.SpawnEnemy(enemies[Random.Range(0, enemies.Count)],
            PlayerController.players[Random.Range(0, PlayerController.players.Count)]);

    }

    private void Update()
    {
        currentSpawnTime += Time.deltaTime;

        if (currentSpawnTime > spawnTime) 
        {
            currentSpawnTime -= spawnTime;

            if (PlayerController.players.Count > 0)
            {
                SpawnEnemy();
            }
        }
    }

}
