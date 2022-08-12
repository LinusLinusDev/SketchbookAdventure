using UnityEngine;
using UnityEngine.UI;

public class SoundSlider : MonoBehaviour
{
    public SessionValues sesVal;

    public void Start()
    {
        GetComponent<Slider>().value = sesVal.SoundVolume;
    }

    public void changeVolume(Slider slider)
    {
        sesVal.SoundVolume = slider.value;
    }
}
