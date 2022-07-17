using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : Character
{
    [SerializeField] private GameObject AttackIndicator;

    protected override void Update()
    {
        base.Update();

        // Queueing Attacks
        if (IsSelected && IsMoving && ActiveMovementRoutine == null)
        {
            if (MovesRemaining > 0)
            {
                Enemy[] enemies = FindObjectsOfType<Enemy>();
                foreach (Enemy enemy in enemies)
                {
                    //Debug.Log("1 " + (Mathf.RoundToInt(Mathf.Abs(enemy.transform.position.x - X)) == 1));
                    //Debug.Log("2 " + (Mathf.RoundToInt(enemy.transform.position.y) == Y));
                    //Debug.Log("3 " + (Mathf.RoundToInt(Mathf.Abs(enemy.transform.position.y - Y)) == 1));
                    //Debug.Log("4 " + (Mathf.RoundToInt(enemy.transform.position.x) == X));
                    if (Mathf.RoundToInt(Mathf.Abs(enemy.transform.position.x - X)) == 1 && Mathf.RoundToInt(enemy.transform.position.y) == Y)
                    {
                        if(enemy.GetComponentInChildren<AttackIndicator>() == null)
                        {
                            GameObject attack = Instantiate(AttackIndicator, enemy.transform);
                            attack.GetComponent<AttackIndicator>().MovementCost = 1;
                        }
                    }
                    if (Mathf.RoundToInt(Mathf.Abs(enemy.transform.position.y - Y)) == 1 && Mathf.RoundToInt(enemy.transform.position.x) == X)
                    {
                        if (enemy.GetComponentInChildren<AttackIndicator>() == null)
                        {
                            GameObject attack = Instantiate(AttackIndicator, enemy.transform);
                            attack.GetComponent<AttackIndicator>().MovementCost = 1;
                        }
                    }
                }
            }
        }
    }

    public override int GenerateAttackDamage()
    {
        Ani.Play("FighterAttack");
        int damage = Random.Range(8, 13);
        if (InitialRoll <= 2)
        {
            damage *= 2;
        }
        return damage;
    }
}
