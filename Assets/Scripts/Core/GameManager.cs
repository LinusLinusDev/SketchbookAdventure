using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public AudioSource audioSource;
    public DialogueBoxController dialogueBoxController;
    public HUD hud;
    public Dictionary<string, Sprite> inventory = new Dictionary<string, Sprite>();
    private static GameManager instance;
    [SerializeField] public AudioTrigger gameMusic;
    [SerializeField] public AudioTrigger gameAmbience;
    
    public static GameManager Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<GameManager>();
            return instance;
        }
    }
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    public void GetInventoryItem(string name, Sprite image)
    {
        inventory.Add(name, image);

        if (image != null)
        {
            hud.SetInventoryImage(inventory[name]);
        }
    }

    public void RemoveInventoryItem(string name)
    {
        inventory.Remove(name);
        hud.SetInventoryImage(hud.blankUI);
    }

    public void ClearInventory()
    {   
        inventory.Clear();
        hud.SetInventoryImage(hud.blankUI);
    }

}
