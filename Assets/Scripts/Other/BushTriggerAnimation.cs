using UnityEngine;
using UnityEngine.Rendering;

public class BushTriggerAnimation : MonoBehaviour
{
    private Animator bushAnimator;
    private SortingGroup sr;
    [SerializeField] private AudioClip sound;

    void Start()
    {
        bushAnimator = gameObject.GetComponent<Animator>();
        sr = transform.parent.transform.parent.gameObject.GetComponent<SortingGroup>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (sr.sortingLayerName == "Layer0Before" || sr.sortingLayerName == "Layer0Behind")
        {
            bushAnimator.SetTrigger("Shake");
            if (sound != null && Random.Range(1, 3) == 1)
            {
                GameManager.Instance.audioSource.PlayOneShot(sound, Random.Range(.6f, 0.3f));
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (sr.sortingLayerName == "Layer0Before" || sr.sortingLayerName == "Layer0Behind")
        {
            bushAnimator.SetTrigger("Shake");
        }
    }
}
