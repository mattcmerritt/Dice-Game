using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // stage bounds for cursor
    // [SerializeField] private int MinX, MaxX, MinY, MaxY;

    // cursor data
    private int CursorX, CursorY;
    [SerializeField] private GameObject Cursor;
    private Coroutine MovementRoutine;
    [SerializeField, Range(0f, 0.5f)] private float MoveDuration;

    // selection data
    [SerializeField] private GameObject SelectedObject;

    private void Update()
    {
        // cursor movement
        if (Input.GetKeyDown(KeyCode.D))
        {
            CursorX++;

            if (MovementRoutine != null)
            {
                StopCoroutine(MovementRoutine);
            }
            MovementRoutine = StartCoroutine(Lerp(Cursor.transform.position, new Vector3(CursorX, CursorY, transform.position.z), MoveDuration));
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            CursorX--;

            if (MovementRoutine != null)
            {
                StopCoroutine(MovementRoutine);
            }
            MovementRoutine = StartCoroutine(Lerp(Cursor.transform.position, new Vector3(CursorX, CursorY, transform.position.z), MoveDuration));
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            CursorY++;

            if (MovementRoutine != null)
            {
                StopCoroutine(MovementRoutine);
            }
            MovementRoutine = StartCoroutine(Lerp(Cursor.transform.position, new Vector3(CursorX, CursorY, transform.position.z), MoveDuration));
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            CursorY--;

            if (MovementRoutine != null)
            {
                StopCoroutine(MovementRoutine);
            }
            MovementRoutine = StartCoroutine(Lerp(Cursor.transform.position, new Vector3(CursorX, CursorY, transform.position.z), MoveDuration));
        }

        // selection
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (SelectedObject == null)
            {
                Collider2D hit = Physics2D.OverlapPoint(new Vector2(CursorX, CursorY));
                if (hit == null)
                {
                    SelectedObject = null;
                }
                else if (hit.GetComponent<Character>() != null)
                {
                    hit.GetComponent<Character>().Select();
                    SelectedObject = hit.gameObject;

                    Debug.Log($"Selected {SelectedObject.name}");
                }
                else
                {
                    Debug.LogWarning($"Unimplemented selection occured when attempting to select {hit.name}.");
                }
            }
            else
            {
                SelectedObject = null;
                Debug.Log("Deselected");
            } 
        }

        // character movement
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (SelectedObject.GetComponent<Character>() != null)
            {
                SelectedObject.GetComponent<Character>().StartMoving();
            }
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
