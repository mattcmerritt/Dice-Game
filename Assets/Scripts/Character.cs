using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, ISelectable, IMovable
{
    // instance data
    private bool IsSelected;
    private bool IsMoving;
    private bool HasMoved;
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
        IsMoving = true;
        if (!HasMoved)
        {
            // roll a die for movement
            MovesRemaining = Random.Range(1, 7);
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
        QueuedMoves = null;
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
                HasMoved = true;
            }
        }

        if (QueuedMoves != null && QueuedMoves.Count > 0 && ActiveMovementRoutine == null)
        {
            ActiveMovementRoutine = StartCoroutine(QueuedMoves.Dequeue());
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
        return $"{gameObject.name}\nHealth: N/A\n{(IsMoving ? $"Moves: {MovesRemaining}" : (HasMoved ? "Unable to move" : "Able to move"))}";
    }
}
