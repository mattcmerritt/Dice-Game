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

        while (MovesRemaining > 0)
        {
            Character[] players = {FindObjectOfType<Fighter>(), FindObjectOfType<Ranger>()};
            foreach (Character player in players)
            {
                if (Mathf.Abs(player.transform.position.x - transform.position.x) - 3 < 0.1 && Mathf.RoundToInt(player.transform.position.y) == transform.position.y)
                {
                    if (Mathf.RoundToInt(Mathf.Abs(player.transform.position.x - transform.position.x)) <= MovesRemaining)
                    {
                        int distance = Mathf.RoundToInt(Mathf.Abs(player.transform.position.x - transform.position.x));
                        if (distance == 1)
                        {
                            for (int i = 0; i < 3 || MovesRemaining <= 0; i++)
                            {
                                // attack
                                MovesRemaining -= 2;
                            }
                        } 
                        else if (distance == 2)
                        {

                        }



                        Debug.Log(player.name + " got hit!");
                        MovesRemaining -= Mathf.RoundToInt(Mathf.Abs(player.transform.position.x - transform.position.x)) * 2;
                    }
                }
                if (Mathf.Abs(player.transform.position.y - transform.position.y) - 3 < 0.1 && Mathf.RoundToInt(player.transform.position.x) == transform.position.x)
                {
                    if (Mathf.RoundToInt(Mathf.Abs(player.transform.position.y - transform.position.y)) <= MovesRemaining)
                    {
                        Debug.Log(player.name + " got hit!");
                        MovesRemaining -= Mathf.RoundToInt(Mathf.Abs(player.transform.position.y - transform.position.y)) * 2;
                    }
                }
            }
        }
        yield return null;
    }
}
