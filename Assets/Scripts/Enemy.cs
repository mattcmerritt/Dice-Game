using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour, IUnstackable
{
    [SerializeField] private int Health;

    public void TakeDamage(int damage)
    {
        Debug.Log(name + " took " + damage + " damage!");
        Health -= damage;
        if (Health < 0)
        {
            Destroy(gameObject);
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

        // else nothing blocks so valid move
        return true;
    }
}
