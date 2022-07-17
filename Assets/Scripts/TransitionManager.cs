using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    [SerializeField] private Animator Animator;
    [SerializeField] private float Duration;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            LoadRoom("Main Menu");
        }
    }

    public void LoadRoom(string name)
    {
        StartCoroutine(LoadScene(name));
    }

    IEnumerator LoadScene(string name)
    {
        Animator.SetTrigger("Leaving");
        yield return new WaitForSeconds(Duration);
        SceneManager.LoadScene(name);
    }
}
