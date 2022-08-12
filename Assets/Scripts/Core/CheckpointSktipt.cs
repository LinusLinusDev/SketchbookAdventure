using UnityEngine;

public class CheckpointSktipt : MonoBehaviour
{
    public int green = 1000;
    public int blue = 1000;
    public int red = 1000;
    [SerializeField] private AudioClip splashSound;
    [SerializeField] private AudioClip checkpointSound;
    private NewPlayer player;


    private void Awake()
    {
        if (NewPlayer.Instance.transform.position.x > transform.position.x - 1) Destroy(this);
    }

    private void Start()
    {
        player = NewPlayer.Instance;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            GetComponent<BoxCollider2D>().enabled = false;
            if (Mathf.RoundToInt(player.sesVal.spawnLocation[0]) == Mathf.RoundToInt(transform.position.x) &&
                Mathf.RoundToInt(player.sesVal.spawnLocation[1]) == Mathf.RoundToInt(transform.position.y))
                Destroy(this.gameObject);
            else
            {
                GameManager.Instance.audioSource.PlayOneShot(splashSound, 0.7f);
                GameManager.Instance.audioSource.PlayOneShot(checkpointSound, 0.2f);
                GetComponent<Animator>().SetTrigger("Splash");
                player.consumeColor = true;
                player.colorAmmo[0] = green;
                player.colorAmmo[1] = blue;
                player.colorAmmo[2] = red;
                player.sesVal.spawnCoins = new[] {player.coins};
                player.sesVal.spawnColorAmmo = new[] {green, blue, red};
                player.sesVal.spawnLocation = new[] {transform.position.x, transform.position.y, transform.position.z};
            }

        }
    }
}
