using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // stage bounds for cursor
    [SerializeField] private int MinX, MaxX, MinY, MaxY;

    // cursor data
    private int CursorX, CursorY;
    [SerializeField] private GameObject Cursor;
    private float MoveElapsedTime;

    // UNUSED: arrows for movement
    // [SerializeField] private GameObject Right, Left, Up, Down;   // arrows
    // [SerializeField] private GameObject RL, UD, UR, UL, DR, DL;  // connections

    // UNUSED: movement tracking
    // private char PrevDirection, PrevPrevDirection;
    // private List<GameObject> MovementArrows;
    // private List<char> Moves;

    private void Update()
    {
        /*
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            Debug.Log("Moving Right");
            CursorX++;
        } 
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            Debug.Log("Moving Left");
            CursorX--;
        }
        else if (Input.GetAxisRaw("Vertical") > 0)
        {
            Debug.Log("Moving Up");
            CursorY++;
        }
        else if (Input.GetAxisRaw("Vertical") < 0)
        {
            Debug.Log("Moving Down");
            CursorY--;
        }
        */

        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("Moving Right");
            CursorX++;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Moving Left");
            CursorX--;
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("Moving Up");
            CursorY++;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("Moving Down");
            CursorY--;
        }

        Cursor.transform.position = Vector3.right * CursorX + Vector3.up * CursorY + Vector3.forward * Cursor.transform.position.z;

        // smoothing cursor movement
        /*
        Vector3 targetPosition = Vector3.right * CursorX + Vector3.up * CursorY + Vector3.forward * Cursor.transform.position.z;
        Vector3.Lerp(Cursor.transform.position, targetPosition, MoveElapsedTime);
        MoveElapsedTime += Time.deltaTime;
        */
    }
}
