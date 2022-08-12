using UnityEngine;

[RequireComponent(typeof(RecoveryCounter))]

public class Breakable : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Sprite brokenSprite;
    [SerializeField] private GameObject deathParticles;
    [SerializeField] private bool destroyAfterDeath = true;
    public int health;
    [SerializeField] private Instantiator instantiator;
    [SerializeField] private AudioClip hitSound;
    private bool recovered;
    [SerializeField] private RecoveryCounter recoveryCounter;
    [SerializeField] private bool requireDownAttack;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        recoveryCounter = GetComponent<RecoveryCounter>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void GetHurt(int hitPower)
    {
        if (health > 0 && !recoveryCounter.recovering)
        {
            if (!requireDownAttack || (requireDownAttack && NewPlayer.Instance.pounding))
            {
                if (NewPlayer.Instance.pounding)
                {
                    NewPlayer.Instance.PoundEffect();
                }

                if (hitSound != null)
                {
                    GameManager.Instance.audioSource.PlayOneShot(hitSound);
                }
                
                recoveryCounter.counter = 0;

                StartCoroutine(NewPlayer.Instance.FreezeFrameEffect());

                health -= 1;
                animator.SetTrigger("hit");

                if (health <= 0)
                {
                    Die();
                }
            }
        }
    }

    public void Die()
    {
        Time.timeScale = 1;
        
        deathParticles.SetActive(true);
        deathParticles.transform.parent = null;

        if (instantiator != null)
        {
            instantiator.InstantiateObjects();
        }
        
        if (destroyAfterDeath)
        {
            Destroy(gameObject);
        }
        else
        {
            spriteRenderer.sprite = brokenSprite;
        }
    }
}
