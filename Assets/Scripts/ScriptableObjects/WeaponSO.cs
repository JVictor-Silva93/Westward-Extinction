using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="WeaponSO", menuName = "Armory/Create new Weapon")]
public class WeaponSO : ObjectIDSO
{
    [SerializeField] private string weaponName;
    [SerializeField] private float cooldown;
    [SerializeField] private float damage;
    [SerializeField] private float speed;
    [SerializeField] private float range;
    [SerializeField] private float size;

    public WeaponSO ()
    {
        weaponName = "Default";
        cooldown = 0.5f;
        damage = 1;
        speed = 1;
        range = 1;
        size = 1;
    }

    public WeaponSO (string _name, float _cd, float _dmg, float _spd, float _rng, float _sz)
    {
        weaponName = _name;
        cooldown = _cd;
        damage = _dmg;
        speed = _spd;
        range = _rng;
        size = _sz;
    }
}

