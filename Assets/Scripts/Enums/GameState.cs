using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    menu,     // active when in menu screen
    combat,   // active when in a level
    upgrading // prevent player inputs, wait for everyone to select an upgrade
}
