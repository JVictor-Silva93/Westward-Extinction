using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] PlayerController controller;

    [SerializeField] int maxHp = 10;
    [SerializeField] int hp;
    [SerializeField] float speed = 1f;
    [SerializeField] int damage = 10;
    [SerializeField] float attackCooldown = 1f;
    [SerializeField] private bool canAttack = true;

    public EnemyCounter enemyCounter;

    private void Awake()
    {
        enemyCounter = FindObjectOfType<EnemyCounter>();
    }

    private void Start()
    {
        canAttack = true;
    }
    
    public void SpawnEnemy(EnemySO _enemy, PlayerController _controler)
    {
        this.maxHp = _enemy.maxHp;
        this.hp = maxHp;
        this.speed = _enemy.speed;
        this.damage = _enemy.damage;
        this.attackCooldown = _enemy.cooldown;

        this.controller = _controler;
        this.canAttack = true;

        Vector2 rand;
        switch (Random.Range(0, 4))
        {
            case 0: rand = new(7, 0); break;
            case 1: rand = new(-7, 0); break;
            case 2: rand = new(0, -7); break;
            case 3: rand = new(0, 7); break;
            default: rand = new(7, 7); break;
        }
        transform.position = new(_controler.transform.position.x + rand.x,
                _controler.transform.position.y + rand.y, 0f);
    }

    private void OnTriggerEnter(Collider _collision)
    {
        StartCoroutine(Attack(_collision));
    }

    private void OnTriggerStay(Collider _collision)
    {
        StartCoroutine(Attack(_collision));
    }

    public void ModifyHp(int _value)
    {
        if (hp + _value >= maxHp)
            hp = maxHp;
        else if (hp + _value <= 0)
        {
            Debug.Log("Enemy is Dead");
            this.gameObject.SetActive(false);
            // Spawn two enemies for every 1 that dies
            enemyCounter.IncrementEnemyCount();
        }
        else
            hp += _value;
    }


    private void Update()
    {
        // choose which player to moveTowards
        transform.position = Vector3.MoveTowards(
            transform.position,
            controller.transform.position,
            speed * Time.deltaTime);
    }


    private IEnumerator Attack(Collider _collision)
    {
        Debug.Log("CanAttack: " + canAttack);
        if (canAttack)
        {
            canAttack = false;
            if (_collision != null && _collision.CompareTag("Player"))
            {
                PlayerController player = _collision.GetComponent<PlayerController>();
                player.ModifyHp(damage * -1);
            }
            yield return new WaitForSeconds(attackCooldown);
            canAttack = true;
        }
    }
}