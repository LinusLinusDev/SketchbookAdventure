using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TongueAnchor : MonoBehaviour
{
    [SerializeField] private Transform[] points;
    [SerializeField] private TongueLine line;

    private void Start()
    {
        line.SetUpLine(points);
    }
}
