using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="EnemySO", menuName = "Enemies/Create new Enemy")]
public class EnemySO : ObjectIDSO
{
    //public Sprite sprite;
    //public Animator animator;

    public string enemyName;
    public int maxHp;
    public float speed;
    public int damage;
    public float cooldown;
    public bool canCarryDrBarnum;
    public bool hasKnockbackAbility;
    public bool hasSidePushAbility;
    public bool debuffsPlayerDamage;
    public bool isStealthy;
    public GameObject enemyPrefab; // Reference to the enemy's prefab
}
