using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    // stage bounds for cursor
    // [SerializeField] private int MinX, MaxX, MinY, MaxY;

    // cursor data
    [SerializeField] private int CursorX, CursorY;
    [SerializeField] private GameObject Cursor;
    private Coroutine MovementRoutine;
    [SerializeField, Range(0f, 0.5f)] private float MoveDuration;

    // selection data
    [SerializeField] private GameObject SelectedObject;
    [SerializeField] private GameObject DetailsPanel;
    [SerializeField] private TMP_Text DetailsBox;
    [SerializeField] private Image Portrait;

    // movement data
    private bool CurrentlyMoving;

    private void Update()
    {
        // cursor movement
        if (Cursor.activeSelf)
        {
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
                    Debug.Log("Nothing to select");

                    DetailsPanel.SetActive(false);
                }
                else if (hit.GetComponent<ISelectable>() != null)
                {
                    hit.GetComponent<ISelectable>().Select();
                    SelectedObject = hit.gameObject;

                    Debug.Log($"Selected {SelectedObject.name}");

                    DetailsPanel.SetActive(true);
                }
                else
                {
                    Debug.LogWarning($"Unimplemented selection occured when attempting to select {hit.name}.");
                }
            }
            else
            {
                SelectedObject.GetComponent<ISelectable>().Deselect();
                SelectedObject = null;
                Debug.Log("Deselected");

                DetailsPanel.SetActive(false);
            } 
        }

        // character movement
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (SelectedObject.GetComponent<Character>() != null)
            {
                if (!CurrentlyMoving)
                {
                    SelectedObject.GetComponent<Character>().StartMoving();
                    CurrentlyMoving = true;
                    Cursor.SetActive(false);
                }
                else
                {
                    SelectedObject.GetComponent<Character>().StopMoving();
                    CurrentlyMoving = false;
                    Cursor.transform.position = new Vector3(CursorX = SelectedObject.GetComponent<Character>().GetX(), CursorY = SelectedObject.GetComponent<Character>().GetY(), SelectedObject.transform.position.z);
                    Cursor.SetActive(true);
                }
            } 
        }
        // extra movement check
        if (CurrentlyMoving && !SelectedObject.GetComponent<Character>().CheckIsMoving())
        {
            SelectedObject.GetComponent<Character>().StopMoving();
            CurrentlyMoving = false;
            Cursor.transform.position = new Vector3(CursorX = SelectedObject.GetComponent<Character>().GetX(), CursorY = SelectedObject.GetComponent<Character>().GetY(), SelectedObject.transform.position.z);
            Cursor.SetActive(true);
        }

        // UI
        if (DetailsPanel.activeSelf)
        {
            DetailsBox.SetText(SelectedObject.GetComponent<ISelectable>().GetDetails());
            Portrait.sprite = SelectedObject.GetComponent<SpriteRenderer>().sprite;
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
