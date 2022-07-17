using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private bool TrapActive;
    [SerializeField] private int Damage;

    public void Activate()
    {
        TrapActive = true;
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        TrapActive = false;
        gameObject.SetActive(false);
    }

    // check for characters on the trap when it is activated
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Character character = collision.gameObject.GetComponent<Character>();
        if (FindObjectOfType<GameManager>().IsEnemyTurn() && character != null)
        {
            character.TakeDamage(Damage);
        }
    }

    public bool CheckForDamage()
    {
        return TrapActive;
    }

    public int GetDamage()
    {
        return Damage;
    }
}
