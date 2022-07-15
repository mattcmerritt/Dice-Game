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
    private Coroutine MovementRoutine;
    [SerializeField, Range(0f, 0.5f)] private float MoveDuration;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("Moving Right");
            CursorX++;

            if (MovementRoutine != null)
            {
                StopCoroutine(MovementRoutine);
            }
            MovementRoutine = StartCoroutine(Lerp(Cursor.transform.position, Vector3.right * CursorX + Vector3.up * CursorY + Vector3.forward * Cursor.transform.position.z, MoveDuration));
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Moving Left");
            CursorX--;

            if (MovementRoutine != null)
            {
                StopCoroutine(MovementRoutine);
            }
            MovementRoutine = StartCoroutine(Lerp(Cursor.transform.position, Vector3.right * CursorX + Vector3.up * CursorY + Vector3.forward * Cursor.transform.position.z, MoveDuration));
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("Moving Up");
            CursorY++;

            if (MovementRoutine != null)
            {
                StopCoroutine(MovementRoutine);
            }
            MovementRoutine = StartCoroutine(Lerp(Cursor.transform.position, Vector3.right * CursorX + Vector3.up * CursorY + Vector3.forward * Cursor.transform.position.z, MoveDuration));
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("Moving Down");
            CursorY--;

            if (MovementRoutine != null)
            {
                StopCoroutine(MovementRoutine);
            }
            MovementRoutine = StartCoroutine(Lerp(Cursor.transform.position, Vector3.right * CursorX + Vector3.up * CursorY + Vector3.forward * Cursor.transform.position.z, MoveDuration));
        }
    }

    IEnumerator Lerp(Vector3 position, Vector3 target, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            Cursor.transform.position = Vector3.Lerp(position, target, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
