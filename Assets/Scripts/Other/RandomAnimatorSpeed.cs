using UnityEngine;

public class RandomAnimatorSpeed : MonoBehaviour
{

    private Animator animator;
    [SerializeField] float low = .3f;
    [SerializeField] float high = 1.5f;

    void Start()
    {
        animator = GetComponent<Animator>() as Animator;
        animator.speed = Random.Range(low, high);
    }
}
