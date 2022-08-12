using UnityEngine;

public class CollectWithTongue : MonoBehaviour
{
    private Transform follow;
    [SerializeField] private Collectable c;

    private void Start()
    {
        follow = transform;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TongueTip"))
        {
            follow=other.transform;
            if (!other.GetComponent<TongueTip>().collectables.Contains(this.gameObject))
                other.GetComponent<TongueTip>().collectables.Add(this.gameObject);
        }
    }

    public void CollectThis()
    {
        c.Collect();
    }
    
    void Update()
    {
        transform.position = follow.position;
    }
}
