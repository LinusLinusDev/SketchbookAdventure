using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicSlider : MonoBehaviour
{
    public SessionValues sesVal;

    private void Start()
    {
        GetComponent<Slider>().value = sesVal.MusicVolume;
    }

    public void changeVolume(Slider slider)
    {
        sesVal.MusicVolume = slider.value;
    }
}
