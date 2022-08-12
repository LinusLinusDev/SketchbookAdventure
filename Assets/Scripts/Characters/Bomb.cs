using UnityEngine;
public class Bomb : MonoBehaviour
{

    [Header ("References")]
    private Rigidbody2D rigidbody2D;
    [SerializeField] private GameObject bomb;
    [System.NonSerialized] public EnemyBase enemyBase;
    private Transform lookAtTarget;

    [Header ("Ground Avoidance")]
    [SerializeField] private float rayCastWidth = 5;
    [SerializeField] private float rayCastOffsetY = 1;
    [SerializeField] private LayerMask layerMask;
    private RaycastHit2D rayCastHit;

    [Header ("Flight")]
    [SerializeField] private bool avoidGround;
    private Vector3 distanceFromPlayer;
    [SerializeField] private float maxSpeedDeviation;
    [SerializeField] private float easing = 1;
    private float bombCounter = 0;
    [SerializeField] private float bombCounterMax = 5;
    public float attentionRange; 
    public float lifeSpan;
    [System.NonSerialized] public float lifeSpanCounter;
    private bool sawPlayer = false;
    [SerializeField] private float speedMultiplier; 
    [System.NonSerialized] public Vector3 speed;
    [System.NonSerialized] public Vector3 speedEased;
    [SerializeField] private bool shootsBomb;
    [SerializeField] private Vector2 targetOffset = new Vector2(0, 2);

    void Start()
    {
        enemyBase = GetComponent<EnemyBase>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        if (enemyBase.isBomb)
        {
            lookAtTarget = NewPlayer.Instance.gameObject.transform;
        }

        speedMultiplier += Random.Range(-maxSpeedDeviation, maxSpeedDeviation);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attentionRange);
    }
    
    void Update()
    {
        distanceFromPlayer.x = NewPlayer.Instance.transform.position.x + targetOffset.x - transform.position.x;
        distanceFromPlayer.y = NewPlayer.Instance.transform.position.y + targetOffset.y - transform.position.y;
        speedEased += (speed - speedEased) * (Time.deltaTime * easing);
        transform.position += speedEased * Time.deltaTime;

        if (Mathf.Abs(distanceFromPlayer.x) <= attentionRange && Mathf.Abs(distanceFromPlayer.y) <= attentionRange || lookAtTarget != null)
        {
            sawPlayer = true;
            speed.x = (Mathf.Abs(distanceFromPlayer.x) / distanceFromPlayer.x) * speedMultiplier;
            speed.y = (Mathf.Abs(distanceFromPlayer.y) / distanceFromPlayer.y) * speedMultiplier;

            if (!NewPlayer.Instance.frozen)
            {
                if (shootsBomb)
                {
                    if (bombCounter > bombCounterMax)
                    {
                        ShootBomb();
                        bombCounter = 0;
                    }
                    else
                    {
                        bombCounter += Time.deltaTime;
                    }
                }
            }
            else
            {
                speedEased = Vector3.zero;
            }
        }
        else
        {
            speed = Vector2.zero;
            if (transform.position.y > (NewPlayer.Instance.transform.position.y + targetOffset.y) && sawPlayer)
            {
                speed = new Vector2(0f, -.05f);
            }

        }
        
        if (avoidGround)
        {
            rayCastHit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + rayCastOffsetY), Vector2.right, rayCastWidth, layerMask);
            Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + rayCastOffsetY), Vector2.right * rayCastWidth, Color.yellow);

            if (rayCastHit.collider != null)
            {
                speed.x = -(Mathf.Abs(speed.x));

            }

            rayCastHit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + rayCastOffsetY), Vector2.left, rayCastWidth, layerMask);
            Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + rayCastOffsetY), Vector2.left * rayCastWidth, Color.blue);

            if (rayCastHit.collider != null)
            {
                speed.x = Mathf.Abs(speed.x);

            }

            rayCastHit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + rayCastOffsetY), Vector2.down, rayCastWidth, layerMask);
            Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + rayCastOffsetY), Vector2.down * rayCastWidth, Color.red);

            if (rayCastHit.collider != null)
            {
                speed.y = Mathf.Abs(speed.x);

            }
        }

        if (lookAtTarget != null)
        {
            LookAt2D();
        }
      
        if (lifeSpan != 0)
        {
            if (lifeSpanCounter < lifeSpan)
            {
                lifeSpanCounter += Time.deltaTime;
            }
            else
            {
                enemyBase.Die();
            }
        }
    }

    void LookAt2D()
    {
        float angle = Mathf.Atan2(speedEased.y, speedEased.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
    }

    public void ShootBomb()
    {
        GameObject bombClone;
        bombClone = Instantiate(bomb, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity, null);
    }
}
