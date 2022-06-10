using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*Disables the cursor, freezes timeScale and contains functions that the pause menu button can use*/ 

public class PauseMenu : MonoBehaviour
{
    [SerializeField] AudioClip pressSound;
    [SerializeField] AudioClip openSound;
    [SerializeField] private Animator hud;

    // Use this for initialization
    void OnEnable()
    {
        Cursor.visible = true;
        GameManager.Instance.audioSource.PlayOneShot(openSound);
        Time.timeScale = 0f;
    }

    public void Unpause()
    {
        Cursor.visible = false;
        gameObject.SetActive(false);
        GameManager.Instance.audioSource.PlayOneShot(openSound);
        Time.timeScale = 1f;
    }

    public void Quit()
    {
        Time.timeScale = 1f;
        hud.SetTrigger("coverScreen");
        StartCoroutine(LoadMenu(2f));
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        hud.SetTrigger("coverScreen");
        StartCoroutine(LoadThis(2f));
    }
    
    IEnumerator LoadMenu(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene("Menu");
    } 
    
    IEnumerator LoadThis(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    } 
}
