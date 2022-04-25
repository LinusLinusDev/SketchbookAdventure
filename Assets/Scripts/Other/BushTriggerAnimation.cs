using UnityEngine;

public class BushTriggerAnimation : MonoBehaviour
{
    private Animator bushAnimator;

    void Start()
    {
        bushAnimator = gameObject.GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        bushAnimator.SetTrigger("Shake");
    }

    void OnTriggerExit2D(Collider2D other)
    {
        bushAnimator.SetTrigger("Shake");
    }
}
