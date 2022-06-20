using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") && NewPlayer.Instance.dashing)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<Animator>().SetTrigger("Break");
        }
    }
}
