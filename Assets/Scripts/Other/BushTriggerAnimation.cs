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
            int rnd = Random.Range(1, 3);
            print(rnd);
            if (rnd == 1)
            {
                AudioSource.PlayClipAtPoint(sound, transform.position);
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
