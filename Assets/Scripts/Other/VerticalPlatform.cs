using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalPlatform : MonoBehaviour
{
    private PlatformEffector2D effector;
    public float waitTime;
    private float time = 0.2f;

    private void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
        waitTime = time;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
            {
                waitTime = time;
            }

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                if (waitTime <= 0)
                {
                    collision.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
                    collision.gameObject.GetComponent<NewPlayer>().animator.SetTrigger("fallThrough");
                    waitTime = time;
                    StartCoroutine(EnableCollider(collision));
                }
                else
                {
                    waitTime -= Time.deltaTime;
                }
            }
        }
    }

    private IEnumerator EnableCollider(Collision2D collision)
    {
        yield return new WaitForSeconds(0.3f);
        collision.gameObject.GetComponent<CapsuleCollider2D>().enabled = true;
    }
}
