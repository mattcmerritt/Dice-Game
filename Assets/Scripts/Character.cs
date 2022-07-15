using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, ISelectable, IMovable
{
    // instance data
    [SerializeField] private bool IsSelected;
    [SerializeField] private bool IsMoving;
    private bool UsedMovement;
    [SerializeField] private int MovesRemaining;
    private int X, Y;

    // coroutines for movement
    private Coroutine ActiveMovementRoutine;
    private Queue<IEnumerator> QueuedMoves;
    [SerializeField, Range(0f, 0.5f)] private float MoveDuration;

    public void Start()
    {
        X = (int) transform.position.x;
        Y = (int) transform.position.y;

        StartTurn();
    }

    public void StartTurn()
    {
        // roll a die
        MovesRemaining = Random.Range(1, 7);
    }

    public void Select()
    {
        IsSelected = true;
    }

    public void Deselect()
    {
        IsSelected = false;
    }

    public void StartMoving()
    {
        if (!UsedMovement)
        {
            IsMoving = true;
            // create empty move queue
            QueuedMoves = new Queue<IEnumerator>();
        }
        else
        {
            StopMoving();
        }
    }

    public void StopMoving()
    {
        IsMoving = false;
    }

    private void Update()
    {
        // Queueing Movement
        if (IsSelected && IsMoving)
        {
            if (MovesRemaining > 0)
            {
                if (Input.GetKeyDown(KeyCode.D))
                {
                    // player moves right
                    MovesRemaining--;

                    X++;
                    QueuedMoves.Enqueue(Lerp(new Vector3(X, Y, transform.position.z), MoveDuration));
                }
                if (Input.GetKeyDown(KeyCode.A))
                {
                    // player moves left
                    MovesRemaining--;

                    X--;
                    QueuedMoves.Enqueue(Lerp(new Vector3(X, Y, transform.position.z), MoveDuration));
                }
                if (Input.GetKeyDown(KeyCode.W))
                {
                    // player moves up
                    MovesRemaining--;

                    Y++;
                    QueuedMoves.Enqueue(Lerp(new Vector3(X, Y, transform.position.z), MoveDuration));
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    // player moves down
                    MovesRemaining--;

                    Y--;
                    QueuedMoves.Enqueue(Lerp(new Vector3(X, Y, transform.position.z), MoveDuration));
                }
            }
            else
            {
                UsedMovement = true;
                IsMoving = false;
            }
        }

        // work through the queue if it is not empty and another action is not occuring
        if (QueuedMoves != null && QueuedMoves.Count > 0 && ActiveMovementRoutine == null)
        {
            ActiveMovementRoutine = StartCoroutine(QueuedMoves.Dequeue());
        }

        // remove the queue when all actions have been completed and movement has stopped
        if (QueuedMoves != null && !IsMoving && QueuedMoves.Count == 0)
        {
            QueuedMoves = null;
        }
    }

    IEnumerator Lerp(Vector3 target, float duration)
    {
        Vector3 start = transform.position;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(start, target, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        ActiveMovementRoutine = null;
    }

    public string GetDetails()
    {
        return $"{gameObject.name}\nHealth: N/A\nMoves: {MovesRemaining}";
    }

    public int GetX()
    {
        return X;
    }

    public int GetY()
    {
        return Y;
    }

    public bool CheckIsMoving()
    {
        return IsMoving;
    }
}
