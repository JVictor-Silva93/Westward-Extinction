using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSO", menuName = "Players/Create new Player")]
public class PlayerSO : ScriptableObject
{
    public HealthBar healthBar;
    public GameController gameController;

    [SerializeField] private string playerName;         // Logs the name of this player
    public float moveSpeed = 5f;                        // Controls the speed at which the player moves
    public float aimSpeed = 1f;                         // Controlls speed of directional rotation
    public int maxHp = 100;                             // Controls the cap of the player's hp
    public int hp;                                      // Controls the player's current hp
    public bool isActive = false;

    public void Init(string _playerName = "Default", float _moveSpeed = 5f, float _aimSpeed = 1f, int _maxHp = 100)
    {
        playerName = _playerName;
        moveSpeed = _moveSpeed;
        aimSpeed = _aimSpeed;
        maxHp = _maxHp;
        hp = maxHp;
        isActive = true;
    }

    public void Death()
    {
        gameController.CheckIfGameOver();
    }
}
