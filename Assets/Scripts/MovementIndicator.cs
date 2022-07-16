using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementIndicator : MonoBehaviour
{
    private InputManager InputManager;

    private void Start()
    {
        InputManager = FindObjectOfType<InputManager>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition - 10f * Vector3.forward, mousePosition);

            if (hit.collider.gameObject == gameObject)
            {
                InputManager.SelectedObject.GetComponent<Character>().EnqueueMove(hit.collider.name);
                Debug.Log(hit.collider.name);
            }
        }
    }
}
