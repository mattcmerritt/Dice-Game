using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialText : MonoBehaviour
{
    [SerializeField] private List<string> Messages;
    private int Index;
    [SerializeField] private TMP_Text TutorialTextbox;

    private void Update()
    {
        TutorialTextbox.SetText(Messages[Index]);
        
        if (Index == 0 && Input.GetKeyDown(KeyCode.Space))
        {
            Index++;
        }
        if (Index == 1 && GameObject.FindObjectOfType<InputManager>().SelectedObject != null && GameObject.FindObjectOfType<InputManager>().SelectedObject.GetComponent<Character>() != null)
        {
            Index++;
        }
        if (Index == 2 && GameObject.FindObjectOfType<InputManager>().SelectedObject != null && GameObject.FindObjectOfType<InputManager>().SelectedObject.GetComponent<Character>().MovesRemaining == 0)
        {
            Index++;
        }
        if (Index == 3 && Input.GetKeyDown(KeyCode.Return))
        {
            Index++;
        }
        if (Index == 4 && Input.GetKeyDown(KeyCode.Space))
        {
            Index++;
        }
    }
}
