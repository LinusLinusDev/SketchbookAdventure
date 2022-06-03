using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TongueTip : MonoBehaviour
{
    public GameObject anchor;
    public GameObject line;
    public bool action;
    public bool stretch;
    public bool coll;

    private void Start()
    {
        action = false;
        stretch = true;
        coll = false;
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        coll = true;
    }

    private void Update()
    {
        if (!action)
        {
            transform.position = anchor.transform.position;
            stretch = true;
            line.SetActive(false);
            GetComponent<CircleCollider2D>().enabled = false;
        }
        else
        {
            line.SetActive(true);
            GetComponent<CircleCollider2D>().enabled = false;
            if (stretch)
            {
                
                if (Vector2.Distance(transform.position, anchor.transform.position) >= 8 || coll)
                {
                    stretch = false;
                }
                else transform.Translate(0.8f,0,0);
            }
            else
            {
                transform.Translate(-0.8f, 0, 0);
                if (Vector2.Distance(transform.position, anchor.transform.position) <= 0.1f)
                {
                    action = false;
                    stretch = true;
                    coll = false;
                    NewPlayer.Instance.EndTongue();
                }
            }
        }
    }
}
