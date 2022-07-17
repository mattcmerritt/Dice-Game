using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    private bool IsPlayerTurn;
    private bool PlayerTurnStarted, EnemyTurnStarted;

    private Tilemap Trap;

    private void Start()
    {
        IsPlayerTurn = true;
        PlayerTurnStarted = false;

        Trap = GameObject.Find("Trap").GetComponent<Tilemap>();
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
        if (!IsPlayerTurn && !EnemyTurnStarted)
        {
            GameObject.FindObjectOfType<UIManager>().StartEnemyTurn();

            Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();
            foreach(Enemy enemy in enemies)
            {
                enemy.TakeTurn();
            }

            // check if character is on a trap and deal damage
            Character[] characters = FindObjectsOfType<Character>();
            foreach (Character ch in characters)
            {
                if (Trap.GetTile(new Vector3Int(ch.GetX(), ch.GetY(), 0)) != null && Trap.GetComponent<Trap>().CheckForDamage())
                {
                    ch.TakeDamage(Trap.GetComponent<Trap>().GetDamage());
                }
            }

            EnemyTurnStarted = true;
        }

        // switching turns
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (IsPlayerTurn)
            {
                IsPlayerTurn = false;
                EnemyTurnStarted = false;
            }
            else
            {
                IsPlayerTurn = true;
                PlayerTurnStarted = false;
            }
        }
    }

    public bool IsEnemyTurn()
    {
        return !IsPlayerTurn;
    }
}
