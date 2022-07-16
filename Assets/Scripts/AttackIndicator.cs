using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackIndicator : MonoBehaviour
{
    [SerializeField] private InputManager Inputs;
    private GameObject Parent;
    public int MovementCost;

    private void Start()
    {
        Parent = transform.parent.gameObject;
        Inputs = FindObjectOfType<InputManager>();
    }

    private void OnMouseDown()
    {
        Parent.GetComponent<Enemy>().TakeDamage(Inputs.SelectedObject.GetComponent<Character>().GenerateAttackDamage());
        Inputs.SelectedObject.GetComponent<Character>().MovesRemaining -= MovementCost;

        // destroy attack indicators before starting move
        AttackIndicator[] indicators = FindObjectsOfType<AttackIndicator>();
        foreach (AttackIndicator i in indicators)
        {
            Destroy(i.gameObject);
        }
    }
}
