using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageIndicator : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] private float Duration;
    private float ElapsedTime = 0f;
    private Vector3 Direction;
    [SerializeField, Range(0f, 5f)] private float Speed;

    [SerializeField] private TMP_Text Textbox;

    private void Update()
    {
        ElapsedTime += Time.deltaTime;
        if (ElapsedTime >= Duration)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.position += Direction * Speed * Time.deltaTime;
        }
    }

    public void SetDetails(string name, int damage)
    {
        if (name == "Player")
        {
            Direction = Vector3.up + Vector3.left;
        }
        else
        {
            Direction = Vector3.up + Vector3.right;
        }
        Textbox.SetText($"-{damage}");
    }
}
