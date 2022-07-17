using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrapEnemy : Enemy, IUnstackable
{
    public Vector2Int ChosenTarget; // button to press
    public Vector2Int[] Targets; // the buttons to move to
    public Vector2Int[][] AreasOfInterest; // the tiles that the enemy will watch for each button

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

        if (MovesRemaining > 0)
        {
            Character[] players = { FindObjectOfType<Fighter>(), FindObjectOfType<Ranger>() };
            // search to see which button to press
            int[] populations = new int[AreasOfInterest.Length];
            foreach (Character player in players)
            {
                for (int i = 0; i < AreasOfInterest.Length; i++)
                {
                    foreach(Vector2Int tile in AreasOfInterest[i])
                    {
                        if(Mathf.RoundToInt(player.transform.position.x) == tile.x && Mathf.RoundToInt(player.transform.position.y) == tile.y)
                        {
                            populations[i]++;
                        }
                    }
                }
            }
            for (int i = 0; i < populations.Length; i++)
            {
                if (Mathf.Max(populations) == populations[i])
                {
                    for (int pre = 0; pre < i; pre++)
                    {
                        if(populations[i] == populations[pre])
                        {
                            // calc distance and go to closer one
                        }
                    }
                    for (int post = i+1; post < populations.Length; post++)
                    {
                        if (populations[i] == populations[post])
                        {
                            // calc distance and go to closer one
                        }
                    }
                }
            }
        }
        yield return null;
    }
}

