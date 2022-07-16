using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour, IUnstackable
{
    [SerializeField] private int Health;

    [SerializeField] private GameObject Die;
    private int RollValue;
    private int MovesRemaining;
    private Coroutine ActiveCoroutine;

    private bool isRolling;

    private void Start()
    {
        Die.GetComponent<Die>().SetParentEnemy(this);
    }

    public void TakeDamage(int damage)
    {
        Debug.Log(name + " took " + damage + " damage!");
        Health -= damage;
        if (Health < 0)
        {
            Destroy(gameObject);
        }
    }

    public bool CheckIfValidMove(int x, int y)
    {
        // gather all objects with interface IUnstackable by looking through all root objects
        List<GameObject> unstackables = new List<GameObject>();
        GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject root in rootObjects)
        {
            if (root.GetComponentInChildren<IUnstackable>() != null)
            {
                unstackables.Add(root);
            }
        }

        // check move against all objects with IUnstackable
        foreach (GameObject unstackable in unstackables)
        {
            if (Mathf.Abs(unstackable.transform.position.x - x) < 0.1f && Mathf.Abs(unstackable.transform.position.y - y) < 0.1f)
            {
                return false;
            }
        }

        // else nothing blocks so valid move
        return true;
    }

    public void SetRoll(int roll)
    {
        RollValue = roll;
        MovesRemaining = roll;
    }

    public void TakeTurn()
    {
        Die.SetActive(true);
        ActiveCoroutine = StartCoroutine(RollDie());
    }

    IEnumerator RollDie()
    {
        
        Die.GetComponent<Die>().StartRolling();
        isRolling = true;
        yield return new WaitForSeconds(4);
        if (isRolling)
        {
            isRolling = false;
            StopCoroutine(ActiveCoroutine);
            Die.GetComponent<Die>().StopRolling();
        }
    }

    IEnumerator PerformAttacks()
    {
        bool NoViableAttacks = false;
        while (MovesRemaining > 0 && !NoViableAttacks)
        {
            NoViableAttacks = true;
            Character[] players = FindObjectsOfType<Character>();
            foreach (Character player in players)
            {
                if (Mathf.Abs(player.transform.position.x - transform.position.x) - MovesRemaining < 0.1 && Mathf.RoundToInt(player.transform.position.y) == transform.position.y)
                {
                    if (Mathf.RoundToInt(Mathf.Abs(player.transform.position.x - transform.position.x)) <= MovesRemaining)
                    {
                        // do attack
                        MovesRemaining -= Mathf.RoundToInt(Mathf.Abs(player.transform.position.x - transform.position.x));
                        NoViableAttacks = false;
                    }
                }
                if (Mathf.Abs(player.transform.position.y - transform.position.y) - MovesRemaining < 0.1 && Mathf.RoundToInt(player.transform.position.x) == transform.position.x)
                {
                    if (Mathf.RoundToInt(Mathf.Abs(player.transform.position.y - transform.position.y)) <= MovesRemaining)
                    {
                        // do attack
                        MovesRemaining -= Mathf.RoundToInt(Mathf.Abs(player.transform.position.y - transform.position.y));
                        NoViableAttacks = false;
                    }
                }
            }
        }
        yield return null;
    }
}
