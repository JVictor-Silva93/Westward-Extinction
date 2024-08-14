using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player
{
    [SerializeField] private string playerName;         // Logs the name of this player
    public float moveSpeed = 5f;                        // Controls the speed at which the player moves
    public float aimSpeed = 1f;                         // Controlls speed of directional rotation
    public float maxHp = 100f;                          // Controls the cap of the player's hp
    public float hp;                                    // Controls the player's current hp

    public Player()
    {
        this.playerName = "Default";
    }

    public Player(string _playerName, float _moveSpeed, float _aimSpeed, float _maxHp)
    {
        this.playerName = _playerName;
        this.moveSpeed = _moveSpeed;
        this.aimSpeed = _aimSpeed;
        this.maxHp = _maxHp;
        this.hp = maxHp;
    }
}
