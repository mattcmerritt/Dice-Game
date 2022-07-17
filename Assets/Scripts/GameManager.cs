using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    private bool IsPlayerTurn;
    private bool PlayerTurnStarted, EnemyTurnStarted;

    // level data
    public static int CurrentLevel = 1; // 1 is the tutorial level
    private bool Cleared;

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
        if (!IsPlayerTurn && !EnemyTurnStarted)
        {
            GameObject.FindObjectOfType<UIManager>().StartEnemyTurn();

            Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();
            foreach(Enemy enemy in enemies)
            {
                enemy.TakeTurn();
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

                if (GameObject.FindObjectOfType<InputManager>().SelectedObject != null)
                {
                    GameObject.FindObjectOfType<InputManager>().SelectedObject.GetComponent<ISelectable>().Deselect();
                }

                Character[] characters = FindObjectsOfType<Character>();
                foreach (Character ch in characters)
                {
                    ch.MovesRemaining = 0;
                    ch.DeactivateDie();
                }
            }
            else
            {
                IsPlayerTurn = true;
                PlayerTurnStarted = false;

                if (GameObject.FindObjectOfType<InputManager>().SelectedObject != null)
                {
                    GameObject.FindObjectOfType<InputManager>().SelectedObject.GetComponent<ISelectable>().Deselect();
                }
            }
        }

        // win/loss conditions
        Character[] chs = FindObjectsOfType<Character>();
        Enemy[] ens = FindObjectsOfType<Enemy>();
        if (chs.Length < 2)
        {
            FindObjectOfType<TransitionManager>().LoadRoom(CurrentLevel);
        }
        
        if (ens.Length == 0 && !Cleared)
        {
            Cleared = true;
            CurrentLevel++;
            FindObjectOfType<TransitionManager>().LoadRoom(CurrentLevel);
        }
    }

    public bool IsEnemyTurn()
    {
        return !IsPlayerTurn;
    }
}
