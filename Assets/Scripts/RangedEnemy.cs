using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RangedEnemy : Enemy, IUnstackable
{
    [SerializeField] private Animator Ani;
    private Queue<IEnumerator> QueuedAttacks;
    private Coroutine ActiveAttackRoutine;

    public void Update()
    {
        if (ActiveAttackRoutine == null && QueuedAttacks != null && QueuedAttacks.Count != 0)
        {
            ActiveAttackRoutine = StartCoroutine(QueuedAttacks.Dequeue());
        }
    }

    public override void TakeTurn()
    {
        Die.SetActive(true);
        QueuedAttacks = new Queue<IEnumerator>();
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
            Debug.LogWarning("die rolling");
            yield return null;
        }

        if (MovesRemaining > 0)
        {
            Character[] players = {FindObjectOfType<Fighter>(), FindObjectOfType<Ranger>()};
            foreach (Character player in players)
            {
                Debug.Log(Mathf.RoundToInt(Mathf.Abs(player.transform.position.x - transform.position.x)));
                if (Mathf.RoundToInt(Mathf.Abs(player.transform.position.x - transform.position.x)) <= 3 && Mathf.RoundToInt(player.transform.position.y) == transform.position.y)
                {
                    while (Mathf.RoundToInt(Mathf.Abs(player.transform.position.x - transform.position.x)) * 2 <= MovesRemaining)
                    {
                        Debug.Log("checking horiz moves");
                        QueuedAttacks.Enqueue(Attack(player));
                        MovesRemaining -= Mathf.RoundToInt(Mathf.Abs(player.transform.position.x - transform.position.x)) * 2;
                    }
                }
                if (Mathf.RoundToInt(Mathf.Abs(player.transform.position.y - transform.position.y)) <= 3 && Mathf.RoundToInt(player.transform.position.x) == transform.position.x)
                {
                    while (Mathf.RoundToInt(Mathf.Abs(player.transform.position.y - transform.position.y)) * 2 <= MovesRemaining)
                    {
                        Debug.Log("checking vert moves");
                        QueuedAttacks.Enqueue(Attack(player));
                        MovesRemaining -= Mathf.RoundToInt(Mathf.Abs(player.transform.position.y - transform.position.y)) * 2;
                    }
                }
            }
        }
        yield return null;
    }

    IEnumerator Attack(Character player)
    {
        Debug.Log(player.name + " got hit by a " + MovesRemaining + "!");
        Ani.Play("EnemyShoot");
        yield return new WaitForSeconds(1.55f);
        player.TakeDamage(Random.Range(5, 11));
        ActiveAttackRoutine = null;
    }
}
