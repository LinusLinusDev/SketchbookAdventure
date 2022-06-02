using System;
using UnityEngine;

[CreateAssetMenu]
public class SessionValues : ScriptableObject
{
    public float MusicVolume;
    public float SoundVolume;

    private void Awake()
    {
        MusicVolume = 1f;
        SoundVolume = 0.5f;
    }
}
