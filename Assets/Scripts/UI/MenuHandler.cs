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
    [SerializeField] AudioClip openSound;
    [SerializeField] private GameObject submenu;
    [SerializeField] private GameObject mainmenu;
    [SerializeField] private GameObject credits;

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
    
    public void GoToOptions()
    {
        GameManager.Instance.audioSource.PlayOneShot(openSound);
        submenu.SetActive(true);
        mainmenu.SetActive(false);
    }
    
    public void BackToMain()
    {
        GameManager.Instance.audioSource.PlayOneShot(openSound);
        mainmenu.SetActive(true);
        submenu.SetActive(false);
    }
    
    public void GoToCredits()
    {
        GameManager.Instance.audioSource.PlayOneShot(openSound);
        credits.SetActive(true);
        submenu.SetActive(false);
    }
    
    public void BackToSub()
    {
        GameManager.Instance.audioSource.PlayOneShot(openSound);
        submenu.SetActive(true);
        credits.SetActive(false);
    }
}
