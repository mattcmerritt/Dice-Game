using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // selection data
    [SerializeField] private GameObject DetailsPanel;
    [SerializeField] private TMP_Text DetailsBox;
    [SerializeField] private Image Portrait;

    // cursor
    [SerializeField] private GameObject Cursor;

    // turn UI
    [SerializeField] private GameObject TurnBanner;
    [SerializeField] private TMP_Text TurnText;
    [SerializeField] private GameObject EndTurn;

    public void SelectCharacter(GameObject selected)
    {
        DetailsPanel.SetActive(true);
        DetailsBox.SetText(selected.GetComponent<Character>().GetDetails());
        Portrait.sprite = selected.GetComponent<SpriteRenderer>().sprite;

        Cursor.transform.position = selected.transform.position;

        // sending update to inputManager
        GameObject.FindObjectOfType<InputManager>().ChangeSelected(selected);
    }

    public void DeselectCharacter()
    {
        DetailsPanel.SetActive(false);
    }

    public void StartPlayerTurn()
    {
        TurnBanner.SetActive(true);
        TurnText.SetText("Player Turn");
    }

    public void StartEnemyTurn()
    {
        TurnBanner.SetActive(true);
        TurnText.SetText("Enemy Turn");
    }
}
