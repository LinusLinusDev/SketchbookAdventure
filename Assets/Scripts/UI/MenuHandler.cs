using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*Allows buttons to fire various functions, like QuitGame and LoadScene*/

public class MenuHandler : MonoBehaviour {

	[SerializeField] private string whichScene;
    [SerializeField] private Animator hud;

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadScene()
    {
        hud.SetTrigger("coverScreen");
        StartCoroutine(FinishFirst(2f));
    }
    
    IEnumerator FinishFirst(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(whichScene);
    } 
}
