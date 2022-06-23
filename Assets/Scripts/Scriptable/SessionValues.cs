using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SessionValues : ScriptableObject
{
    public float MusicVolume;
    public float SoundVolume;
    public int[] spawnCoins ;
    public int[] spawnColorAmmo;
    public float[] spawnLocation;

    private void Awake()
    {
        MusicVolume = 1f;
        SoundVolume = 0.5f;
        deleteCheckpoint();
    }

    public void deleteCheckpoint()
    {
        spawnCoins = new []{-1};
        spawnColorAmmo = new []{-1};
        spawnLocation = new[] {-1f};
    }
}
