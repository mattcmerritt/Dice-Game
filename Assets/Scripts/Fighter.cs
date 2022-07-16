using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : Character
{
    [SerializeField] private bool IsAttacking;

    protected override void Update()
    {
        base.Update();

        // Queueing Attacks
        if (IsSelected && IsAttacking)
        {
            if (MovesRemaining > 0)
            {

            }
        }
    }
}
