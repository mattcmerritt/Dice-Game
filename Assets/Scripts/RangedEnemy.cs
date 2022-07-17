using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RangedEnemy : Enemy, IUnstackable
{
    public override void TakeTurn()
    {
        Die.SetActive(true);
        ActiveCoroutine = StartCoroutine(RollDie());
    }

    IEnumerator RollDie()
    {
        Die.GetComponent<Die>().StartRolling();
        isRolling = true;
        yield return new WaitForSeconds(2);
        if (isRolling)
        {
            isRolling = false;
            //StopCoroutine(ActiveCoroutine);
            Die.GetComponent<Die>().StopRolling();
            
            ActiveCoroutine = StartCoroutine(PerformAttacks());
        }
    }

    IEnumerator PerformAttacks()
    {
        // wait for die to finish rolling and despawn
        while (Die.activeSelf)
        {
            yield return null;
        }

        Debug.Log("roll=" + MovesRemaining);

        if (MovesRemaining > 0)
        {
            Character[] players = {FindObjectOfType<Fighter>(), FindObjectOfType<Ranger>()};
            foreach (Character player in players)
            {
                if (Mathf.RoundToInt(Mathf.Abs(player.transform.position.x - transform.position.x)) <= 3 && Mathf.RoundToInt(player.transform.position.y) == transform.position.y)
                {
                    while (Mathf.RoundToInt(Mathf.Abs(player.transform.position.x - transform.position.x)) * 2 <= MovesRemaining)
                    {
                        // animation?
                        Debug.Log(player.name + " got hit by a " + MovesRemaining + "!");
                        MovesRemaining -= Mathf.RoundToInt(Mathf.Abs(player.transform.position.x - transform.position.x)) * 2;
                    }
                }
                if (Mathf.RoundToInt(Mathf.Abs(player.transform.position.y - transform.position.y)) <= 3 && Mathf.RoundToInt(player.transform.position.x) == transform.position.x)
                {
                    while (Mathf.RoundToInt(Mathf.Abs(player.transform.position.y - transform.position.y)) * 2 <= MovesRemaining)
                    {
                        Debug.Log(player.name + " got hit by a " + MovesRemaining + "!");
                        MovesRemaining -= Mathf.RoundToInt(Mathf.Abs(player.transform.position.y - transform.position.y)) * 2;
                    }
                }
            }
        }
        yield return null;
    }
}
