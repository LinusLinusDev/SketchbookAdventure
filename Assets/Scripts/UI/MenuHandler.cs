using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*Allows buttons to fire various functions, like QuitGame and LoadScene*/

public class MenuHandler : MonoBehaviour {

	[SerializeField] private string whichScene;
    [SerializeField] private Animator hud;
    [SerializeField] private SessionValues sv;

    private void Start()
    {
        sv.deleteCheckpoint();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadScene(string scene)
    {
        hud.SetTrigger("coverScreen");
        StartCoroutine(FinishFirst(2f, scene));
    }

    public void LoadScene()
    {
        hud.SetTrigger("coverScreen");
        StartCoroutine(FinishFirst(2f, whichScene));
    }
    
    IEnumerator FinishFirst(float waitTime, string scene) {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(scene);
    } 
}
