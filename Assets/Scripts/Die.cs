using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Die : MonoBehaviour
{
    [SerializeField, Range(-1f, 1f)] private float XSpeed, YSpeed, ZSpeed;
    private bool isRolling;
    private int RollValue;

    private Character AssociatedPlayer;

    private void Start()
    {
        isRolling = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            isRolling = false;

            Vector3 targetRotation = new Vector3(
                Mathf.RoundToInt(transform.eulerAngles.x / 90f),
                Mathf.RoundToInt(transform.eulerAngles.y / 90f),
                Mathf.RoundToInt(transform.eulerAngles.z / 90f)
            ) * 90f;

            // Debug.Log($"Rotation {targetRotation}");

            StartCoroutine(Lerp(targetRotation, 0.1f));
        }
        else if (isRolling)
        {
            XSpeed = Mathf.Sin(Time.time);
            YSpeed = Mathf.Sin(1.7f * Time.time);
            ZSpeed = Mathf.Sin(0.33f * Time.time);

            transform.Rotate(XSpeed, YSpeed, ZSpeed);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            isRolling = true;
        }
    }

    IEnumerator Lerp(Vector3 target, float duration)
    {
        Vector3 start = transform.eulerAngles;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            transform.eulerAngles = Vector3.Lerp(start, target, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position - 10f * Vector3.forward, Vector3.forward);
        RollValue = int.Parse(hit.collider.name);

        // Debug.Log($"Rolled {RollValue}");
        float delayElapsed = 0f;

        while (delayElapsed < 0.5f)
        {
            delayElapsed += Time.deltaTime;
            yield return null;
        }

        AssociatedPlayer.SetRoll(RollValue);
        gameObject.SetActive(false);
    }

    public void SetPlayer(Character ch)
    {
        AssociatedPlayer = ch;
    }

    public void StartRolling()
    {
        isRolling = true;
    }
}
