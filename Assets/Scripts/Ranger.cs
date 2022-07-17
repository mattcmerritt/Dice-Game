using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranger : Character
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
                    if (Mathf.Abs(enemy.transform.position.x - X) - MovesRemaining < 0.1 && Mathf.RoundToInt(enemy.transform.position.y) == Y)
                    {
                        if (enemy.GetComponentInChildren<AttackIndicator>() == null)
                        {
                            GameObject attack = Instantiate(AttackIndicator, enemy.transform);
                            attack.GetComponent<AttackIndicator>().MovementCost = Mathf.RoundToInt(Mathf.Abs(enemy.transform.position.x - X));
                        }
                    }
                    if (Mathf.Abs(enemy.transform.position.y - Y) - MovesRemaining < 0.1 && Mathf.RoundToInt(enemy.transform.position.x) == X)
                    {
                        if (enemy.GetComponentInChildren<AttackIndicator>() == null)
                        {
                            GameObject attack = Instantiate(AttackIndicator, enemy.transform);
                            attack.GetComponent<AttackIndicator>().MovementCost = Mathf.RoundToInt(Mathf.Abs(enemy.transform.position.y - Y));
                        }
                    }
                }
            }
        }
    }

    public override int GenerateAttackDamage()
    {
        Ani.Play("RangerShoot");
        int damage = Random.Range(5, 10);
        if (InitialRoll <= 2)
        {
            damage *= 2;
        }
        return damage;
    }
}
