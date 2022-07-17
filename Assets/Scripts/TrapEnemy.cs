using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrapEnemy : Enemy, IUnstackable
{
    [System.Serializable]
    public class CoordinateArray
    {
        public Vector2Int[] coords;
    }

    public int ChosenIndex; // button to press
    public Vector2Int[] Targets; // the buttons to move to
    public CoordinateArray[] AreasOfInterest; // the tiles that the enemy will watch for each button
    public Trap[] Traps; // the traps for each button when pressed

    // animator
    [SerializeField] private Animator Ani;
    [SerializeField] private string AnimationClipPrefix;

    private Queue<IEnumerator> QueuedMoves;
    private Coroutine ActiveMovementRoutine;

    public void Update()
    {
        if(ActiveMovementRoutine == null && QueuedMoves != null && QueuedMoves.Count != 0)
        {
            ActiveMovementRoutine = StartCoroutine(QueuedMoves.Dequeue());
        }
    }

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
                    foreach (Vector2Int tile in AreasOfInterest[i].coords)
                    {
                        if (Mathf.RoundToInt(player.transform.position.x) == tile.x && Mathf.RoundToInt(player.transform.position.y) == tile.y)
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
                    bool duplicate = false;
                    // check to see if there are multiple max candidates and pick the current one if so
                    for (int remaining = i + 1; remaining < populations.Length; remaining++)
                    {
                        if (populations[i] == populations[remaining])
                        {
                            duplicate = true;

                            /*
                            // useless code - oops
                            
                            Vector2Int currentPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));

                            int xdiffi = Mathf.Abs(currentPos.x - Targets[i].x);
                            int xdiffremaining = Mathf.Abs(currentPos.x - Targets[remaining].x);
                            int ydiffi = Mathf.Abs(currentPos.y - Targets[i].y);
                            int ydiffremaining = Mathf.Abs(currentPos.y - Targets[remaining].y);
                            int diffi = xdiffi + ydiffi;
                            int diffremaining = xdiffremaining + ydiffremaining;

                            if (diffi >= diffremaining)
                            {
                                ChosenIndex = i;
                            }
                            else
                            {
                                ChosenIndex = remaining;
                            }
                            */

                        }
                    }
                    // otherwise pick one with uncontested max
                    if (!duplicate)
                    {
                        ChosenIndex = i;
                    }
                }
            }
            if (Targets[ChosenIndex] == new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)))
            {
                Traps[ChosenIndex].Activate();
                MovesRemaining = 0;
            }
            else
            {
                // deactivate all traps
                foreach (Trap trap in Traps)
                {
                    trap.Deactivate();
                }

                // move toward chosen target
                Vector2Int currentPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
                QueuedMoves = new Queue<IEnumerator>();
                while (MovesRemaining > 0)
                {
                    if (MovesRemaining > 0 && currentPos.x > Targets[ChosenIndex].x && CheckIfValidMove(currentPos.x - 1, currentPos.y))
                    {
                        // move left
                        QueuedMoves.Enqueue(Lerp(new Vector3(currentPos.x - 1, currentPos.y, 0), 0.5f));
                        currentPos.x--;
                        
                    }
                    else if (MovesRemaining > 0 && currentPos.x < Targets[ChosenIndex].x && CheckIfValidMove(currentPos.x + 1, currentPos.y))
                    {
                        // move right
                        QueuedMoves.Enqueue(Lerp(new Vector3(currentPos.x + 1, currentPos.y, 0), 0.5f));
                        currentPos.x++;
                        
                    }
                    else if (MovesRemaining > 0 && currentPos.y > Targets[ChosenIndex].y && CheckIfValidMove(currentPos.x, currentPos.y - 1))
                    {
                        // move down
                        QueuedMoves.Enqueue(Lerp(new Vector3(currentPos.x, currentPos.y - 1, 0), 0.5f));
                        currentPos.y--;
                        
                    }
                    else if (MovesRemaining > 0 && currentPos.y < Targets[ChosenIndex].y && CheckIfValidMove(currentPos.x, currentPos.y + 1))
                    {
                        // move up
                        QueuedMoves.Enqueue(Lerp(new Vector3(currentPos.x, currentPos.y + 1, 0), 0.5f));
                        currentPos.y++;
                        
                    }
                    MovesRemaining--;
                }
            }
            yield return null;
        }

        IEnumerator Lerp(Vector3 target, float duration)
        {
            Vector3 start = transform.position;
            float elapsedTime = 0f;

            // playing walk animations
            if (Ani != null)
            {
                if (target.x > start.x)
                {
                    Ani.Play(AnimationClipPrefix + "WalkRight");
                }
                else
                {
                    Ani.Play(AnimationClipPrefix + "WalkLeft");
                }
            }

            while (elapsedTime < duration)
            {
                transform.position = Vector3.Lerp(start, target, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // returning to the idle state
            if (Ani != null)
            {
                Ani.Play(AnimationClipPrefix + "Idle");
            }
            ActiveMovementRoutine = null;
        }
    }
}

