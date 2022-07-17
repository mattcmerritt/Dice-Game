using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementIndicator : MonoBehaviour
{
    private InputManager InputManager;

    [SerializeField] private Vector3 Displacement;

    private void Start()
    {
        InputManager = FindObjectOfType<InputManager>();

        // shifting down by displacement
        transform.position += Displacement;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector3.forward);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                InputManager.SelectedObject.GetComponent<Character>().EnqueueMove(hit.collider.name);
            }
        }
    }
}
