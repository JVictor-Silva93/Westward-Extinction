using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] PlayerController controller;

    [SerializeField] float maxHp = 10f;
    [SerializeField] float hp;
    [SerializeField] float speed = 1f;
    [SerializeField] float damage = 10f;
    [SerializeField] float attackCooldown = 1f;
    [SerializeField] private bool canAttack = true;

    private void Start()
    {
        canAttack = true;
    }


    public EnemyController(float _maxHp, float _spd, float _dmg, float _cd, PlayerController _controler)
    {
        this.maxHp = _maxHp;
        this.hp = maxHp;
        this.speed = _spd;
        this.damage = _dmg;
        this.attackCooldown = _cd;

        this.controller = _controler;
        this.canAttack = true;
    }

    private void OnTriggerEnter(Collider _collision)
    {
        StartCoroutine(Attack(_collision));
    }

    private void OnTriggerStay(Collider _collision)
    {
        StartCoroutine(Attack(_collision));
    }

    public void ModifyHp(float _value)
    {
        if (hp + _value >= maxHp)
            hp = maxHp;
        else if (hp + _value <= 0)
        {
            Debug.Log("Enemy is Dead");
            this.gameObject.SetActive(false);
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