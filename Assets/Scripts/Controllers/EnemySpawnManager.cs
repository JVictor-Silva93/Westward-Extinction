using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public static EnemySpawnManager Instance;

    [SerializeField] private EnemyController tempEnemy;
    [SerializeField] private List<PlayerController> players;
    [SerializeField] private List<EnemySO> enemies;

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
            players[Random.Range(0, players.Count)]);

    }

    private void Start()
    {
        SpawnEnemy();
    }

}
