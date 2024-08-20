using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] PlayerController controller;

    [SerializeField] int maxHp = 10;
    [SerializeField] int hp;
    [SerializeField] float speed = 1f;
    [SerializeField] int damage = 10;
    
    public void InstantiateEnemy(EnemySO _enemy, PlayerController _controler)
    {
        this.maxHp = _enemy.maxHp;
        this.hp = maxHp;
        this.speed = _enemy.speed;
        this.damage = _enemy.damage;

        this.controller = _controler;
    }

    private void OnTriggerEnter(Collider _collision)
    {
        Attack(_collision);
    }

    public void ModifyHp(int _value)
    {
        if (hp + _value >= maxHp)
            hp = maxHp;
        else if (hp + _value <= 0)
        {
            Debug.Log("Enemy is Dead");
            EnemyCounter.Instance.IncrementEnemiesDefeated();
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

    private void Attack(Collider _collision)
    {
        if (_collision != null && _collision.CompareTag("Player"))
        {
            PlayerController player = _collision.GetComponent<PlayerController>();
            player.ModifyHp(damage * -1);

            // knock the player back when collided with
            Vector3 knockBack = (player.transform.position - transform.position).normalized * 0.000002f;
            _collision.attachedRigidbody.AddForce(knockBack, ForceMode.Impulse);
            StartCoroutine(ZeroPlayerVelocity(_collision));
        }
    }

    private IEnumerator ZeroPlayerVelocity(Collider _collision)
    {
        yield return new WaitForSeconds(0.1f);
        if (_collision != null)
            _collision.attachedRigidbody.velocity = Vector3.zero;
    }
}