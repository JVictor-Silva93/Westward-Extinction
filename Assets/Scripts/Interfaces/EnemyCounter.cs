using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Import TextMeshPro

public class EnemyCounter : MonoBehaviour
{
    public TextMeshProUGUI enemyCounterText;
    public int enemiesDefeated = 0; // Initial count of enemies defeated

    public void IncrementEnemyCount()
    {
        enemiesDefeated++;
        enemyCounterText.text = "Enemies Defeated: " + enemiesDefeated.ToString();
    }
}
