using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyColor : MonoBehaviour
{
    [SerializeField] private Butterfly butterflyScript;
    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        switch (butterflyScript.color)
        {
            case 0:
                sr.color = new Color(0.25f, 0.7f, 0.25f, 1);
                break;
            case 1:
                sr.color = new Color(0.25f, 0.25f, 0.7f, 1);
                break;
            case 2:
                sr.color = new Color(0.7f, 0.25f, 0.25f, 1);
                break;
        }
    }
}
