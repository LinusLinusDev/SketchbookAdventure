using UnityEngine;

public class Collectable : MonoBehaviour
{

    enum ItemType { InventoryItem, Coin, Health, Ammo };
    [SerializeField] ItemType itemType;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip bounceSound;
    [SerializeField] private AudioClip[] collectSounds;
    [SerializeField] private int itemAmount;
    [SerializeField] private string itemName;
    [SerializeField] private Sprite UIImage;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject == NewPlayer.Instance.gameObject)
        {
            Collect();
        }

        if (col.gameObject.layer == 14)
        {
            Collect();
        }
    }

    public void Collect()
    {
        if (itemType == ItemType.InventoryItem)
        {
            if (itemName != "")
            {
                GameManager.Instance.GetInventoryItem(itemName, UIImage);
            }
        }
        else if (itemType == ItemType.Coin)
        {
            NewPlayer.Instance.coins += itemAmount;
        }
        else if (itemType == ItemType.Health)
        {
            
            if (NewPlayer.Instance.health < NewPlayer.Instance.maxHealth)
            {
                GameManager.Instance.hud.HealthBarHurt();
                NewPlayer.Instance.health += itemAmount;
            }
        }
        else if (itemType == ItemType.Ammo)
        {
            if (NewPlayer.Instance.ammo < NewPlayer.Instance.maxAmmo)
            {
                Butterfly bf = GetComponent<Butterfly>();
                if (!NewPlayer.Instance.consumeColor)
                {
                    NewPlayer.Instance.colorAmmo[0] = 0;
                    NewPlayer.Instance.colorAmmo[1] = 0;
                    NewPlayer.Instance.colorAmmo[2] = 0;
                }
                if (NewPlayer.Instance.colorAmmo[bf.color] + bf.amount >= NewPlayer.Instance.maxColor)
                    NewPlayer.Instance.colorAmmo[bf.color] = NewPlayer.Instance.maxColor;
                else NewPlayer.Instance.colorAmmo[bf.color] += bf.amount;
            }
        }

        GameManager.Instance.audioSource.PlayOneShot(collectSounds[Random.Range(0, collectSounds.Length)], Random.Range(.6f, 1f));

        if (transform.parent.GetComponent<Ejector>() != null)
        {
            Destroy(transform.parent.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
}
