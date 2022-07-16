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

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector3.forward);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                Parent.GetComponent<Enemy>().TakeDamage(Inputs.SelectedObject.GetComponent<Character>().GenerateAttackDamage());
                Inputs.SelectedObject.GetComponent<Character>().MovesRemaining -= MovementCost;

                // destroy attack indicators before starting move
                AttackIndicator[] indicators = FindObjectsOfType<AttackIndicator>();
                foreach (AttackIndicator i in indicators)
                {
                    Destroy(i.gameObject);
                }

                if (Inputs.SelectedObject.GetComponent<Character>().MovesRemaining <= 0)
                {
                    // destroy movement indicators
                    MovementIndicator[] moveIndicators = FindObjectsOfType<MovementIndicator>();
                    foreach (MovementIndicator m in moveIndicators)
                    {
                        Destroy(m.gameObject);
                    }
                }
            }
        }
    }

    /*
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
    */
}
