using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour, ISelectable, IMovable, IUnstackable
{
    // instance data
    [SerializeField] protected bool IsSelected;
    [SerializeField] private bool IsMoving;
    private bool UsedMovement;
    [SerializeField] protected int MovesRemaining;
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

    protected virtual void Update()
    {
        // Queueing Movement
        if (IsSelected && IsMoving)
        {
            if (MovesRemaining > 0)
            {
                if (Input.GetKeyDown(KeyCode.D))
                {
                    if(CheckIfValidMove(X + 1, Y))
                    {
                        // player moves right
                        MovesRemaining--;

                        X++;
                        QueuedMoves.Enqueue(Lerp(new Vector3(X, Y, transform.position.z), MoveDuration));
                    }
                }
                if (Input.GetKeyDown(KeyCode.A))
                {
                    if (CheckIfValidMove(X - 1, Y))
                    {
                        // player moves left
                        MovesRemaining--;

                        X--;
                        QueuedMoves.Enqueue(Lerp(new Vector3(X, Y, transform.position.z), MoveDuration));
                    }
                }
                if (Input.GetKeyDown(KeyCode.W))
                {
                    if (CheckIfValidMove(X, Y + 1))
                    {
                        // player moves up
                        MovesRemaining--;

                        Y++;
                        QueuedMoves.Enqueue(Lerp(new Vector3(X, Y, transform.position.z), MoveDuration));
                    }
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    if (CheckIfValidMove(X, Y - 1))
                    {
                        // player moves down
                        MovesRemaining--;

                        Y--;
                        QueuedMoves.Enqueue(Lerp(new Vector3(X, Y, transform.position.z), MoveDuration));
                    }
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

    public bool CheckIfValidMove(int x, int y)
    {
        // gather all objects with interface IUnstackable by looking through all root objects
        List<GameObject> unstackables = new List<GameObject>();
        GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach(var root in rootObjects)
        {
            if (root.GetComponentInChildren<IUnstackable>() != null)
            {
                unstackables.Add(root);
            }
        }

        Debug.Log(unstackables.Count);

        // check move against all objects with IUnstackable
        foreach(var unstackable in unstackables)
        {
            if(Mathf.Abs(unstackable.transform.position.x - x) < 0.1f && Mathf.Abs(unstackable.transform.position.y - y) < 0.1f)
            {
                return false;
            }
        }

        // else nothing blocks so valid move
        return true;
    }
}
