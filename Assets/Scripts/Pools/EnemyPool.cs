using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool SharedInstance;
    public List<EnemyController> pooledEnemies;
    public int amountToPool;

    [SerializeField] private EnemyController P_Enemy;

    private void Awake()
    {
        SharedInstance = this; 
        pooledEnemies = new();
        EnemyController tmp;
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(P_Enemy, transform);
            tmp.gameObject.SetActive(false);
            pooledEnemies.Add(tmp);
            Debug.Log(tmp + " num: " + i);
        }
    }

    public EnemyController AddMoreEnemies()
    {
        EnemyController tmp;
        tmp = Instantiate(P_Enemy, transform);
        tmp.gameObject.SetActive(false);
        pooledEnemies.Add(tmp);

        return tmp;
    }

    public EnemyController GetPooledEnemies()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            Debug.Log("num: " + i);
            if (!pooledEnemies[i].gameObject.activeInHierarchy)
            {
                return pooledEnemies[i];
            }
        }

        return AddMoreEnemies();
    }
}
