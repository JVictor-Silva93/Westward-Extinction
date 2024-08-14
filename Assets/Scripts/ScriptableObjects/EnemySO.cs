using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="EnemySO", menuName = "Enemies/Create new Enemy")]
public class EnemySO : ObjectIDSO
{
    //public Sprite sprite;
    //public Animator animator;

    public float maxHp;
    public float speed;
    public float damage;
    public float cooldown;

}
