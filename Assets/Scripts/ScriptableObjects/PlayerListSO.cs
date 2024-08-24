using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerListSO", menuName = "Players/Create new Player List")]
public class PlayerListSO : ScriptableObject
{
    public PlayerSO[] playerSOs;
}
