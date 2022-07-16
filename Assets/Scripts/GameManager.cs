using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool IsPlayerTurn;
    private bool PlayerTurnStarted;

    private void Start()
    {
        IsPlayerTurn = true;
        PlayerTurnStarted = false;
    }

    private void Update()
    {
        // player turn
        if (IsPlayerTurn && !PlayerTurnStarted)
        {
            GameObject.FindObjectOfType<UIManager>().StartPlayerTurn();

            Character[] players = GameObject.FindObjectsOfType<Character>();
            foreach (Character player in players)
            {
                player.StartTurn();
            }

            PlayerTurnStarted = true;
        }
        // enemy turn, implement later
        if (!IsPlayerTurn)
        {
            GameObject.FindObjectOfType<UIManager>().StartEnemyTurn();

            Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();
            foreach(Enemy enemy in enemies)
            {
                enemy.TakeTurn();
            }
        }

        // switching turns
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (IsPlayerTurn)
            {
                IsPlayerTurn = false;
            }
            else
            {
                IsPlayerTurn = true;
                PlayerTurnStarted = false;
            }
        }
    }
}
