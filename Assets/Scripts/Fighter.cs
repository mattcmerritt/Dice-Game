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
                    if (((Mathf.Abs(enemy.transform.position.x - X) < 1.1) && (Mathf.Abs(enemy.transform.position.x - X) > 0.1)) !=
                        ((Mathf.Abs(enemy.transform.position.y - Y) < 1.1) && (Mathf.Abs(enemy.transform.position.y - Y) > 0.1)))
                    {
                        if(enemy.GetComponentInChildren<AttackIndicator>() == null)
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
        return Random.Range(8, 13);
    }
}
