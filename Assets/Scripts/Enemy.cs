using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public abstract class Enemy : MonoBehaviour, IUnstackable
{
    [SerializeField] protected int Health;

    [SerializeField] protected GameObject Die;
    [SerializeField] protected int RollValue;
    [SerializeField] protected int MovesRemaining;
    protected Coroutine ActiveCoroutine;

    protected bool isRolling;

    [SerializeField] protected Tilemap Floor;

    private void Start()
    {
        Die.GetComponent<Die>().SetParentEnemy(this);
        Floor = GameObject.Find("Floor").GetComponent<Tilemap>();
    }

    public void TakeDamage(int damage)
    {
        Debug.Log(name + " took " + damage + " damage!");
        Health -= damage;
        if (Health < 0)
        {
            DestroyImmediate(gameObject);
            FindObjectOfType<InputManager>().SelectedObject.GetComponent<Character>().CreateMovementIndicators();
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

        // check for valid floor tile
        if (Floor.GetTile(new Vector3Int(x, y, 0)) == null)
        {
            return false;
        }

        // else nothing blocks so valid move
        return true;
    }

    public void SetRoll(int roll)
    {
        RollValue = roll;
        MovesRemaining = roll;
    }

    public abstract void TakeTurn();
}
