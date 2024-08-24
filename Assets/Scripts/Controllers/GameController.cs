using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private PlayerListSO playerList;
    [SerializeField] private PlayableDirector gameOverTimeline;

    private void Awake()
    {
        foreach (PlayerSO playerSO in playerList.playerSOs)
        {
            playerSO.gameController = this;
        }
    }

    // Game Over setup, Works in conjunction with NotifyOnFinish
    private void GameOver()
    {
        StartCoroutine(NotifyOnFinish());
    }

    private IEnumerator NotifyOnFinish()
    {
        gameOverTimeline.Play();
        while (gameOverTimeline.state == PlayState.Playing)
        {
            yield return null;
        }
        SceneManager.LoadScene("MainMenu");
    }

    public void CheckIfGameOver()
    {
        int numDeadPlayers = 0;
        int numActivePlayers = 0;

        foreach (PlayerSO playerSO in playerList.playerSOs)
        {
            if (playerSO.isActive)
            {
                numActivePlayers++;

                if (playerSO.hp < 0)
                    numDeadPlayers++;
            }
        }

        if (numDeadPlayers == numActivePlayers)
            GameOver();
    }
}
