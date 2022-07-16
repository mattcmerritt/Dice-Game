using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackIndicator : MonoBehaviour
{
    [SerializeField] private InputManager Inputs;
    private GameObject Parent;

    private void Start()
    {
        Parent = transform.parent.gameObject;
        Inputs = FindObjectOfType<InputManager>();
    }

    private void OnMouseDown()
    {
        Parent.GetComponent<Enemy>().TakeDamage(Inputs.SelectedObject.GetComponent<Character>().GenerateAttackDamage());
    }
}
