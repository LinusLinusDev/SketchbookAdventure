using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setColor : MonoBehaviour
{
    private SpriteRenderer sr;
    private NewPlayer playerScript;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        playerScript = GameObject.FindWithTag("Player").GetComponent<NewPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (playerScript.color)
        {
            case 0:
                sr.color = Color.green;
                break;
            case 1:
                sr.color = Color.yellow;
                break;
            case 2:
                sr.color = Color.red;
                break;
            case 3:
                sr.color = Color.blue;
                break;
        }
    }
}
