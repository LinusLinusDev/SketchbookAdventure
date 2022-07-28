using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class killWall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            NewPlayer.Instance.Freeze(true);
            Time.timeScale = 1f;
            GameManager.Instance.hud.animator.SetTrigger("coverScreen");
            StartCoroutine(LoadThis(2f));
        }
    }

    IEnumerator LoadThis(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    } 
}
