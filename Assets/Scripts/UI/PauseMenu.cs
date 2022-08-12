using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] AudioClip pressSound;
    [SerializeField] AudioClip openSound;
    [SerializeField] private Animator hud;

    private bool canUnpause = true;
    
    void OnEnable()
    {
        Cursor.visible = true;
        GameManager.Instance.audioSource.PlayOneShot(openSound);
        Time.timeScale = 0f;
    }

    public void Unpause()
    {
        if (canUnpause)
        {
            Cursor.visible = false;
            gameObject.SetActive(false);
            GameManager.Instance.audioSource.PlayOneShot(openSound);
            Time.timeScale = 1f;
        }
    }

    public void Quit()
    {
        canUnpause = false;
        Time.timeScale = 1f;
        hud.SetTrigger("coverScreen");
        StartCoroutine(LoadMenu(2f));
    }

    public void RestartLevel()
    {
        canUnpause = false;
        NewPlayer.Instance.sesVal.deleteCheckpoint();
        NewPlayer.Instance.Freeze(true);
        Time.timeScale = 1f;
        hud.SetTrigger("coverScreen");
        StartCoroutine(LoadThis(2f));
    }
    
    public void LastCheckpoint()
    {
        canUnpause = false;
        NewPlayer.Instance.Freeze(true);
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
