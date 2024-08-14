using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public static ProjectileManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void InstantiateProjectile(ProjectileController _projectile, Transform _transform)
    {
        Instantiate(_projectile, _transform);
    }
}
