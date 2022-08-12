using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") && NewPlayer.Instance.dashing)
        {
            GetComponent<AudioSource>().Play();
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<Animator>().SetTrigger("Break");
        }
    }
}
