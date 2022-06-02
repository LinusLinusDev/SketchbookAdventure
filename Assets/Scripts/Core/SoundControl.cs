using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundControl : MonoBehaviour
{
    public SessionValues sesVal;
    public GameObject music;
    public GameObject sound;
    void Update()
    {
        music.GetComponent<AudioTrigger>().maxVolume = sesVal.MusicVolume;
        sound.GetComponent<AudioTrigger>().maxVolume = sesVal.SoundVolume;
        GameManager.Instance.audioSource.volume = sesVal.SoundVolume;
    }
}
