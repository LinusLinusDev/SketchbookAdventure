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
    private float tongueLength;
    public List<GameObject> collectables;
    private float tongueSpeed = 0.8f;
    private NewPlayer player;

    private void Start()
    {
        action = false;
        stretch = true;
        coll = false;
        player = NewPlayer.Instance;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Solid")) coll = true;
    }

    private void FixedUpdate()
    {
        if (!action)
        {
            transform.position = anchor.transform.position;
            stretch = true;
            coll = false;
            line.SetActive(false);
            GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            line.SetActive(true);
            GetComponent<BoxCollider2D>().enabled = true;
            
            if (stretch)
            {
                if (Vector2.Distance(transform.position, anchor.transform.position) >= tongueLength || coll)
                {
                    stretch = false;
                }
                else transform.Translate(tongueSpeed,0,0);
            }
            else
            {
                transform.Translate(-1 * tongueSpeed, 0, 0);
                if (!player.animator.GetBool("grounded") && coll && player.color == 0)
                {
                   player.transform.Translate(tongueSpeed* 5 * NewPlayer.Instance.transform.localScale.x, 0, 0);
                }
                if (Vector2.Distance(transform.position, anchor.transform.position) <= 0.1f)
                {
                    action = false;
                    stretch = true;
                    coll = false;
                    NewPlayer.Instance.EndTongue();
                    collectables.ForEach(x => x.GetComponent<CollectWithTongue>().CollectThis());
                    collectables.Clear();
                }
            }
        }
    }

    public void activate(float length)
    {
        action = true;
        tongueLength = length;
    }
}
