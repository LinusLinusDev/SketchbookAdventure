using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*Loads a new scene, while also clearing level-specific inventory!*/

public class Goal : MonoBehaviour
{

    [SerializeField] private SessionValues sv;
    [SerializeField] string loadSceneName;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject == NewPlayer.Instance.gameObject)
        {
            GameManager.Instance.hud.loadSceneName = loadSceneName;
            GameManager.Instance.inventory.Clear();
            sv.deleteCheckpoint();
            GameManager.Instance.hud.animator.SetTrigger("coverScreen");
            enabled = false;
            StartCoroutine(FinishFirst(2f, loadSceneName));
        }
    }
    
    IEnumerator FinishFirst(float waitTime, string scene) {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(scene);
    } 
}