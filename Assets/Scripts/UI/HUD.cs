using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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
    private float ammoBarWidthEased; 
    [System.NonSerialized] public Sprite blankUI;
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
        healthBarWidth = 1;
        healthBarWidthEased = healthBarWidth;
        redBarWidth = 1;
        redBarWidthEased = 0;
        greenBarWidth = 1;
        greenBarWidthEased = 0;
        blueBarWidth = 1;
        blueBarWidthEased = 0;
        ammoBarWidth = 1;
        ammoBarWidthEased = ammoBarWidth;
        coins = (float)NewPlayer.Instance.coins;
        coinsEased = coins;
        blankUI = inventoryItemGraphic.GetComponent<Image>().sprite;
    }

    void Update()
    {
        coinsMesh.text = Mathf.Round(coinsEased).ToString();
        coinsEased += ((float)NewPlayer.Instance.coins - coinsEased) * Time.deltaTime * 5f;

        if (coinsEased >= coins)
        {
            animator.SetTrigger("getGem");
            coins = coinsEased + 1;
        }
        
        redBarWidth = (float)NewPlayer.Instance.colorAmmo[2] / (float)NewPlayer.Instance.maxColor;
        redBarWidthEased += (redBarWidth - redBarWidthEased) * Time.deltaTime;
        redColorBar.transform.localScale = new Vector2(redBarWidthEased, 1);
        
        greenBarWidth = (float)NewPlayer.Instance.colorAmmo[0] / (float)NewPlayer.Instance.maxColor;
        greenBarWidthEased += (greenBarWidth - greenBarWidthEased) * Time.deltaTime;
        greenColorBar.transform.localScale = new Vector2(greenBarWidthEased, 1);
        
        blueBarWidth = (float)NewPlayer.Instance.colorAmmo[1] / (float)NewPlayer.Instance.maxColor;
        blueBarWidthEased += (blueBarWidth - blueBarWidthEased) * Time.deltaTime;
        blueColorBar.transform.localScale = new Vector2(blueBarWidthEased, 1);

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
            NewPlayer.Instance.ResetLevel();
        }
        else
        {
            SceneManager.LoadScene(loadSceneName);
        }
    }

}
