using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setColorNPC : MonoBehaviour
{
    void Start()
    {
        GetComponent<SpriteRenderer>().color = transform.parent.GetComponent<colorNPC>().color;
    }
}