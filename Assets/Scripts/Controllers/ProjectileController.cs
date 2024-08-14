using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class ProjectileController : MonoBehaviour
{
    protected float flySpeed = 1f;
    protected float damage = 10f;
    protected float size = 1f;
    protected float range = 5f;
    protected Vector3 startPosition;

    public void Attack(float _fSpd, float _dmg, float _size, float _rng)
    {
        this.flySpeed = _fSpd;
        this.damage = _dmg;
        this.size = _size;
        this.range = _rng;
        startPosition = this.transform.position;
    }
    private void OnCollisionEnter2D(Collision2D _collision)
    {
        if (_collision != null && _collision.gameObject.CompareTag("Enemy"))
        {
            PlayerController player = _collision.gameObject.GetComponent<PlayerController>();
            player.ModifyHp(damage);

            Destroy(this.gameObject);
            // this.gameObject.SetActive(false);
        }
    }
}
