using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public static EnemySpawnManager Instance { get; private set; }

    [SerializeField] private List<EnemySO> enemies;
    [SerializeField] private List<GameObject> spawnPoints;
    private EnemyController tempEnemy;

    [SerializeField] private int spawnTime;
    private float currentSpawnTime;

    private void Awake()
    {
        Instance = this;
    }

    public void InstantiateEnemy()
    {
        tempEnemy = EnemyPool.SharedInstance.GetPooledEnemies();
        tempEnemy.gameObject.SetActive(true);

        // random spawning of enemies, and they will target one player at random
        tempEnemy.InstantiateEnemy(enemies[Random.Range(0, enemies.Count)],
            PlayerController.players[Random.Range(0, PlayerController.players.Count)]);

        // newly spawned enemy will be spawned at a randomly selected point from a predefined list
        tempEnemy.transform.position = spawnPoints[Random.Range(0, spawnPoints.Count)].transform.position;

    }

    private void Update()
    {   // adjusted code so that the spawn timer will increase towards the threshold as long as it is not prepared to spawn an enemy.
        if (currentSpawnTime > spawnTime) 
        {
            if (PlayerController.players != null && PlayerController.players.Count > 0)
            // In the instance an enemy cannot be spawned, the update will run in a null loop where the game will not spawn an enemy until there is a player to fight it.
            {
                // Then when the enemy is spawned it will restart the spawn timer
                currentSpawnTime -= spawnTime;
                InstantiateEnemy();
            }
        } else
        {
            currentSpawnTime += Time.deltaTime;
        }
    }

}
