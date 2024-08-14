using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool SharedInstance;
    public List<ProjectileController> pooledProjectiles;
    public int amountToPool;

    [SerializeField] private ProjectileController P_Projectile;

    private void Awake()
    {
        SharedInstance = this;
        pooledProjectiles = new();
        ProjectileController tmp;
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(P_Projectile, transform);
            tmp.gameObject.SetActive(false);
            pooledProjectiles.Add(tmp);
        }
    }

    public ProjectileController AddMoreProjectiles()
    {
        ProjectileController tmp;
        tmp = Instantiate(P_Projectile, transform);
        tmp.gameObject.SetActive(false);
        pooledProjectiles.Add(tmp);

        return tmp;
    }

    public ProjectileController GetPooledProjectiles()
    {
        for (int i = 0;i < amountToPool; i++)
        {
            if (!pooledProjectiles[i].gameObject.activeInHierarchy)
            {
                return pooledProjectiles[i];
            }
        }

        return AddMoreProjectiles();
    }
}
