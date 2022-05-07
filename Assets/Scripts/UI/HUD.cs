using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

/*Manages and updates the HUD, which contains your health bar, coins, etc*/

public class HUD : MonoBehaviour
{
    [Header ("Reference")]
    public Animator animator;
    [SerializeField] private GameObject ammoBar;
    public TextMeshProUGUI coinsMesh;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject redColorBar;
    [SerializeField] private GameObject greenColorBar;
    [SerializeField] private GameObject blueColorBar;
    [SerializeField] private Image inventoryItemGraphic;
    [SerializeField] private GameObject startUp;

    private float ammoBarWidth;
    private float ammoBarWidthEased; //Easing variables slowly ease towards a number
    [System.NonSerialized] public Sprite blankUI; //The sprite that is shown in the UI when you don't have any items
    private float coins;
    private float coinsEased;
    private float healthBarWidth;
    private float healthBarWidthEased;
    private float redBarWidth;
    private float redBarWidthEased;
    private float greenBarWidth;
    private float greenBarWidthEased;
    private float blueBarWidth;
    private float blueBarWidthEased;
    
    [System.NonSerialized] public string loadSceneName;
    [System.NonSerialized] public bool resetPlayer;

    void Start()
    {
        //Set all bar widths to 1, and also the smooth variables.
        healthBarWidth = 1;
        healthBarWidthEased = healthBarWidth;
        redBarWidth = 1;
        redBarWidthEased = redBarWidth;
        greenBarWidth = 1;
        greenBarWidthEased = greenBarWidth;
        blueBarWidth = 1;
        blueBarWidthEased = blueBarWidth;
        ammoBarWidth = 1;
        ammoBarWidthEased = ammoBarWidth;
        coins = (float)NewPlayer.Instance.coins;
        coinsEased = coins;
        blankUI = inventoryItemGraphic.GetComponent<Image>().sprite;
    }

    void Update()
    {
        //Update coins text mesh to reflect how many coins the player has! However, we want them to count up.
        coinsMesh.text = Mathf.Round(coinsEased).ToString();
        coinsEased += ((float)NewPlayer.Instance.coins - coinsEased) * Time.deltaTime * 5f;

        if (coinsEased >= coins)
        {
            animator.SetTrigger("getGem");
            coins = coinsEased + 1;
        }
        /*
        //Controls the width of the health bar based on the player's total health
        healthBarWidth = (float)NewPlayer.Instance.health / (float)NewPlayer.Instance.maxHealth;
        healthBarWidthEased += (healthBarWidth - healthBarWidthEased) * Time.deltaTime * healthBarWidthEased;
        healthBar.transform.localScale = new Vector2(healthBarWidthEased, 1);
        */
        
        //Color Bars
        redBarWidth = (float)NewPlayer.Instance.redColor / (float)NewPlayer.Instance.maxColor;
        redBarWidthEased += (redBarWidth - redBarWidthEased) * Time.deltaTime * redBarWidthEased;
        redColorBar.transform.localScale = new Vector2(redBarWidthEased, 1);
        
        greenBarWidth = (float)NewPlayer.Instance.greenColor / (float)NewPlayer.Instance.maxColor;
        greenBarWidthEased += (greenBarWidth - greenBarWidthEased) * Time.deltaTime * greenBarWidthEased;
        greenColorBar.transform.localScale = new Vector2(greenBarWidthEased, 1);
        
        blueBarWidth = (float)NewPlayer.Instance.blueColor / (float)NewPlayer.Instance.maxColor;
        blueBarWidthEased += (blueBarWidth - blueBarWidthEased) * Time.deltaTime * blueBarWidthEased;
        blueColorBar.transform.localScale = new Vector2(blueBarWidthEased, 1);

        //Controls the width of the ammo bar based on the player's total ammo
        if (ammoBar)
        {
            ammoBarWidth = (float)NewPlayer.Instance.ammo / (float)NewPlayer.Instance.maxAmmo;
            ammoBarWidthEased += (ammoBarWidth - ammoBarWidthEased) * Time.deltaTime * ammoBarWidthEased;
            ammoBar.transform.localScale = new Vector2(ammoBarWidthEased, transform.localScale.y);
        }
        
    }

    public void HealthBarHurt()
    {
        animator.SetTrigger("hurt");
    }

    public void SetInventoryImage(Sprite image)
    {
        inventoryItemGraphic.sprite = image;
    }

    void ResetScene()
    {
        if (GameManager.Instance.inventory.ContainsKey("reachedCheckpoint"))
        {
            //Send player back to the checkpoint if they reached one!
            NewPlayer.Instance.ResetLevel();
        }
        else
        {
            //Reload entire scene
            SceneManager.LoadScene(loadSceneName);
        }
    }

}
