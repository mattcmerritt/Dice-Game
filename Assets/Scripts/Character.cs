using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class Character : MonoBehaviour, ISelectable, IMovable, IUnstackable
{
    // instance data
    [SerializeField] protected bool IsSelected;
    [SerializeField] protected bool IsMoving;
    private bool UsedMovement;
    [SerializeField] public int MovesRemaining;
    [SerializeField] protected int X, Y;
    private int Health = 50;

    // coroutines for movement
    protected Coroutine ActiveMovementRoutine;
    private Queue<IEnumerator> QueuedMoves;
    [SerializeField, Range(0f, 0.5f)] private float MoveDuration;

    // die information
    [SerializeField] private GameObject Die;
    [SerializeField] private Die DieScript;

    // animator
    [SerializeField] protected Animator Ani;
    [SerializeField] private string AnimationClipPrefix;

    // movement indicators
    [SerializeField] private GameObject Left, Right, Up, Down;

    // tilemaps
    [SerializeField] private Tilemap Floor;
    [SerializeField] private Tilemap[] Traps;

    public int InitialRoll;

    [SerializeField] private GameObject DamageIndicator;

    public void Start()
    {
        X = (int) transform.position.x;
        Y = (int) transform.position.y;

        DieScript = Die.GetComponent<Die>();
        DieScript.SetPlayer(this);

        Floor = GameObject.Find("Floor").GetComponent<Tilemap>();
    }

    public void StartTurn()
    {
        // roll a die
        Die.SetActive(true);
        DieScript.StartRolling();

        UsedMovement = false;
    }

    public void Select()
    {
        IsSelected = true;
        GameObject.FindObjectOfType<UIManager>().SelectCharacter(gameObject);

        // activate movement indicators
        CreateMovementIndicators();
        StartMoving();
    }

    public void Deselect()
    {
        IsSelected = false;
        GameObject.FindObjectOfType<UIManager>().DeselectCharacter();

        // destroy movement indicators
        MovementIndicator[] moveIndicators = FindObjectsOfType<MovementIndicator>();
        foreach (MovementIndicator m in moveIndicators)
        {
            Destroy(m.gameObject);
        }

        // destroy attack indicators
        AttackIndicator[] attackIndicators = FindObjectsOfType<AttackIndicator>();
        foreach (AttackIndicator m in attackIndicators)
        {
            Destroy(m.gameObject);
        }
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

    private void OnMouseDown()
    {
        // Deselect other characters in scene
        Character[] chars = GameObject.FindObjectsOfType<Character>();
        foreach (Character ch in chars)
        {
            ch.Deselect();
        }

        Select();
    }

    protected virtual void Update()
    {
        // Queueing Movement
        if (IsSelected && IsMoving)
        {
            if (MovesRemaining > 0)
            {
                // OUTDATED KEYBOARD CONTROLS
                /*
                if (Input.GetKeyDown(KeyCode.D))
                {
                    EnqueueMove("Right");
                }
                if (Input.GetKeyDown(KeyCode.A))
                {
                    EnqueueMove("Left");
                }
                if (Input.GetKeyDown(KeyCode.W))
                {
                    EnqueueMove("Up");
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    EnqueueMove("Down");
                }
                */
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
        /*
        if (QueuedMoves != null && !IsMoving && QueuedMoves.Count == 0)
        {
            QueuedMoves = null;
        }
        */
    }

    IEnumerator Lerp(Vector3 target, float duration)
    {
        // destroy attack indicators before starting move
        AttackIndicator[] indicators = FindObjectsOfType<AttackIndicator>();
        foreach (AttackIndicator i in indicators)
        {
            Destroy(i.gameObject);
        }

        // destroy movement indicators
        MovementIndicator[] moveIndicators = FindObjectsOfType<MovementIndicator>();
        foreach (MovementIndicator m in moveIndicators)
        {
            Destroy(m.gameObject);
        }

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

        // adding new movement UI
        CreateMovementIndicators();
    }

    public string GetDetails()
    {
        return $"{gameObject.name}\nHealth: {Health}\nMoves: {MovesRemaining}";
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
        foreach(GameObject root in rootObjects)
        {
            if (root.GetComponentInChildren<IUnstackable>() != null)
            {
                unstackables.Add(root);
            }
        }

        // check move against all objects with IUnstackable
        foreach(GameObject unstackable in unstackables)
        {
            if(Mathf.Abs(unstackable.transform.position.x - x) < 0.1f && Mathf.Abs(unstackable.transform.position.y - y) < 0.1f)
            {
                return false;
            }
        }

        // check for valid floor tile
        if (Floor.GetTile(new Vector3Int(x, y, 0)) == null)
        {
            return false;
        }

        // else nothing blocks so valid move
        return true;
    }

    public virtual int GenerateAttackDamage()
    {
        return 0; // should be overwritten
    }

    public void SetRoll(int roll)
    {
        InitialRoll = roll;
        MovesRemaining = roll;
        if (IsSelected)
        {
            CreateMovementIndicators();
        }
    }

    public void EnqueueMove(string direction)
    {
        if (direction.Contains("Up"))
        {
            if (CheckIfValidMove(X, Y + 1))
            {
                // player moves right
                Y++;
            }
        }
        if (direction.Contains("Down"))
        {
            if (CheckIfValidMove(X, Y - 1))
            {
                // player moves right
                Y--;
            }
        }
        if (direction.Contains("Left"))
        {
            if (CheckIfValidMove(X - 1, Y))
            {
                // player moves right
                X--;
            }
        }
        if (direction.Contains("Right"))
        {
            if (CheckIfValidMove(X + 1, Y))
            {
                // player moves right
                X++;
            }
        }

        MovesRemaining--;
        QueuedMoves.Enqueue(Lerp(new Vector3(X, Y, transform.position.z), MoveDuration));

        // check if character is on a trap
        foreach (Tilemap trap in Traps)
        if (trap.GetTile(new Vector3Int(X, Y, 0)) != null && trap.GetComponent<Trap>().CheckForDamage())
        {
            TakeDamage(trap.GetComponent<Trap>().GetDamage());
        }
    }

    public void CreateMovementIndicators()
    {
        if (MovesRemaining > 0)
        {
            if (CheckIfValidMove(X - 1, Y))
            {
                Instantiate(Left, transform.position + Vector3.left, Quaternion.identity);
            }
            if (CheckIfValidMove(X + 1, Y))
            {
                Instantiate(Right, transform.position + Vector3.right, Quaternion.identity);
            }
            if (CheckIfValidMove(X, Y + 1))
            {
                Instantiate(Up, transform.position + Vector3.up, Quaternion.identity);
            }
            if (CheckIfValidMove(X, Y - 1))
            {
                Instantiate(Down, transform.position + Vector3.down, Quaternion.identity);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

        GameObject dmg = Instantiate(DamageIndicator, transform.position + Vector3.left * 0.25f + Vector3.up * 0.75f, Quaternion.identity);
        dmg.GetComponent<DamageIndicator>().SetDetails("Player", damage);

        if (Health < 0)
        {
            Destroy(gameObject);
        } 
    }

    public void DeactivateDie()
    {
        Die.SetActive(false);
    }
}
