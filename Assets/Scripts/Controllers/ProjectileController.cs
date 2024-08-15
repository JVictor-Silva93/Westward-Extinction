using UnityEditor.Experimental.GraphView;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ProjectileController : MonoBehaviour
{
    protected float flySpeed = 1f;
    protected int damage = 10;
    protected float size = 1f;
    protected float range = 9f;
    protected Vector3 startPosition;
    protected Vector3 direction;

    public void Attack(Vector3 _pos, Vector3 _cursorPos)
    {
        transform.position = _pos;
        startPosition = _pos;

        direction = ((_cursorPos - _pos).normalized) * flySpeed;
    }

    public void Attack(float _fSpd, int _dmg, float _size, float _rng, Vector3 _pos, Vector3 _cursorPos)
    {
        this.flySpeed = _fSpd;
        this.damage = _dmg;
        this.size = _size;
        this.range = _rng;

        transform.position = _pos;
        startPosition = _pos;

        direction = (_cursorPos - _pos).normalized * flySpeed;
    }

    private void OnTriggerEnter(Collider _collision)
    {
        Debug.Log(_collision);

        if (_collision != null && _collision.CompareTag("Enemy"))
        {
            EnemyController enemy = _collision.GetComponent<EnemyController>();
            enemy.ModifyHp(damage * -1);

            this.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        transform.position += direction * Time.deltaTime * 10f;
        if (Mathf.Abs(Vector3.Distance(transform.position, startPosition)) > range) 
        {
            this.gameObject.SetActive(false);
        }
    }

}
