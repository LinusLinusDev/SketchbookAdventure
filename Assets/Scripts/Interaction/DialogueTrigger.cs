using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    [Header ("References")]
    [SerializeField] private GameObject finishTalkingActivateObject;
    [SerializeField] private Animator iconAnimator; 

    [Header("Trigger")]
    [SerializeField] private bool autoHit;
    public bool completed;
    [SerializeField] private bool repeat;
    [SerializeField] private bool sleeping;

    [Header ("Dialogue")]
    [SerializeField] private string characterName;
    [SerializeField] private string dialogueStringA; 
    [SerializeField] private string dialogueStringB;
    [SerializeField] private AudioClip[] audioLinesA;
    [SerializeField] private AudioClip[] audioLinesB;
    [SerializeField] private AudioClip[] audioChoices;

    [Header ("Fetch Quest")]
    [SerializeField] private GameObject deleteGameObject;
    [SerializeField] private string getWhichItem; 
    [SerializeField] private int getCoinAmount;
    [SerializeField] private string finishTalkingAnimatorBool; 
    [SerializeField] private string finishTalkingActivateObjectString; 
    [SerializeField] private Sprite getItemSprite;
    [SerializeField] private AudioClip getSound;
    [SerializeField] private bool instantGet; 
    [SerializeField] private string requiredItem;
    [SerializeField] private int requiredCoins; 
    public Animator useItemAnimator;
    [SerializeField] private string useItemAnimatorBool; 

    void OnTriggerStay2D(Collider2D col)
    {
        if (instantGet)
        {
            InstantGet();
        }

        if (col.gameObject == NewPlayer.Instance.gameObject && !sleeping && !completed && NewPlayer.Instance.grounded)
        {
            iconAnimator.SetBool("active", true);
            if (autoHit || (Input.GetAxis("Submit") > 0))
            {
                iconAnimator.SetBool("active", false);
                if (requiredItem == "" && requiredCoins == 0 || !GameManager.Instance.inventory.ContainsKey(requiredItem) && requiredCoins == 0 || (requiredCoins != 0 && NewPlayer.Instance.coins < requiredCoins))
                {
                    GameManager.Instance.dialogueBoxController.Appear(dialogueStringA, characterName, this, false, audioLinesA, audioChoices, finishTalkingAnimatorBool, finishTalkingActivateObject, finishTalkingActivateObjectString, repeat);
                }
                else if (requiredCoins == 0 && GameManager.Instance.inventory.ContainsKey(requiredItem) || (requiredCoins != 0 && NewPlayer.Instance.coins >= requiredCoins))
                {
                    if (dialogueStringB != "")
                    {
                        GameManager.Instance.dialogueBoxController.Appear(dialogueStringB, characterName, this, true, audioLinesB, audioChoices, "", null, "", repeat);
                    }
                    else
                    {
                        UseItem();
                    }
                }
                sleeping = true;
            }
        }
        else
        {
            iconAnimator.SetBool("active", false);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject == NewPlayer.Instance.gameObject)
        {
            iconAnimator.SetBool("active", false);
            sleeping = completed;
        }
    }

    public void UseItem()
    {
        if (!completed)
        {
            if (useItemAnimatorBool != "")
            {
                useItemAnimator.SetBool(useItemAnimatorBool, true);
            }

            if (deleteGameObject)
            {
                Destroy(deleteGameObject);
            }

            Collect();

            if (GameManager.Instance.inventory.ContainsKey(requiredItem))
            {
                GameManager.Instance.RemoveInventoryItem(requiredItem);
            }
            else
            {
                NewPlayer.Instance.coins -= requiredCoins;
            }

            repeat = false;
        }
    }

    public void Collect()
    {
        if (!completed)
        {
            if (getWhichItem != "")
            {
                GameManager.Instance.GetInventoryItem(getWhichItem, getItemSprite);
            }

            if (getCoinAmount != 0)
            {
                NewPlayer.Instance.coins += getCoinAmount;
            }

            if (getSound != null)
            {
                GameManager.Instance.audioSource.PlayOneShot(getSound);
            }

            completed = true;
        }
    }

    public void InstantGet()
    {
        GameManager.Instance.GetInventoryItem(getWhichItem, null);
        instantGet = false;
    }
}