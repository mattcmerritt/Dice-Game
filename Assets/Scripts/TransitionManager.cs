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
        // potentially remove
        if (Input.GetKeyDown(KeyCode.P))
        {
            LoadRoom(0);
        }
    }

    public void LoadRoom(int index)
    {
        StartCoroutine(LoadScene(index));
    }

    IEnumerator LoadScene(int index)
    {
        Animator.SetTrigger("Leaving");
        yield return new WaitForSeconds(Duration);
        SceneManager.LoadScene(index);
    }
}
