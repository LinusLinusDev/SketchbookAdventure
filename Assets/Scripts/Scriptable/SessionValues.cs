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
        MusicVolume = 0.6f;
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
