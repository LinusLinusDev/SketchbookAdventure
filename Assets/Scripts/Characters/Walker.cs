using UnityEngine;

public class Walker : PhysicsObject
{
    [Header ("Reference")]
    public EnemyBase enemyBase;
    [SerializeField] private GameObject graphic;

    [Header ("Properties")]
    [SerializeField] private LayerMask layerMask; 
    [SerializeField] enum EnemyType { Bug, Zombie };
    [SerializeField] EnemyType enemyType;
   
    public float attentionRange;
    public float changeDirectionEase = 1;
    [System.NonSerialized] public float direction = 1;
    private Vector2 distanceFromPlayer;
    [System.NonSerialized] public float directionSmooth = 1;
    [SerializeField] private bool followPlayer;
    [SerializeField] private bool flipWhenTurning = false;
    private RaycastHit2D ground;
    public float hurtLaunchPower = 10;
    [SerializeField] private bool jumping;
    public float jumpPower = 7;
    [System.NonSerialized] public bool jump = false;
    [System.NonSerialized] public float launch = 1;
    public float maxSpeed = 7;
    [SerializeField] private float maxSpeedDeviation;
    [SerializeField] private bool neverStopFollowing = false;
    private Vector3 origScale;
    [SerializeField] private Vector2 rayCastSize = new Vector2(1.5f, 1);
    private Vector2 rayCastSizeOrig;
    [SerializeField] private Vector2 rayCastOffset;
    private RaycastHit2D rightWall;
    private RaycastHit2D leftWall;
    private RaycastHit2D rightLedge;
    private RaycastHit2D leftLedge;

    private float sitStillMultiplier = 1;
    [SerializeField] private bool sitStillWhenNotFollowing = false;

    [Header("Sounds")]
    public AudioClip jumpSound;
    public AudioClip stepSound;
    
    void Start()
    {
        enemyBase = GetComponent<EnemyBase>();
        origScale = transform.localScale;
        rayCastSizeOrig = rayCastSize;
        maxSpeed -= Random.Range(0, maxSpeedDeviation);
        launch = 0;
        if (enemyType == EnemyType.Zombie)
        {
            direction = 0;
            directionSmooth = 0;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attentionRange);
    }

    private void Update()
    {
        ComputeVelocity();
    }

    protected void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        if (!NewPlayer.Instance.frozen)
        {
            distanceFromPlayer = new Vector2 (NewPlayer.Instance.gameObject.transform.position.x - transform.position.x, NewPlayer.Instance.gameObject.transform.position.y - transform.position.y);
            directionSmooth += ((direction * sitStillMultiplier) - directionSmooth) * Time.deltaTime * changeDirectionEase;
            move.x = (1 * directionSmooth) + launch;
            launch += (0 - launch) * Time.deltaTime;
            
            if (move.x < 0)
            {
                transform.localScale = new Vector3(-origScale.x, origScale.y, origScale.z);
            }
            else
            {
                transform.localScale = new Vector3(origScale.x, origScale.y, origScale.z);
            }

            if (!enemyBase.recoveryCounter.recovering)
            {
                if (move.x > 0.01f)
                {
                    if (graphic.transform.localScale.x == -1)
                    {
                        graphic.transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
                    }
                }
                else if (move.x < -0.01f)
                {
                    if (graphic.transform.localScale.x == 1)
                    {
                        graphic.transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
                    }
                }
                
                ground = Physics2D.Raycast(transform.position, -Vector2.up);
                Debug.DrawRay(transform.position, -Vector2.up, Color.green);
                
                if (enemyType == EnemyType.Zombie)
                {
                    if ((Mathf.Abs(distanceFromPlayer.x) < attentionRange) && (Mathf.Abs(distanceFromPlayer.y) < attentionRange))
                    {
                        followPlayer = true;
                        sitStillMultiplier = 1;

                        if (neverStopFollowing)
                        {
                            attentionRange = 10000000000;
                        }
                    }
                    else
                    {
                        if (sitStillWhenNotFollowing)
                        {
                            sitStillMultiplier = 0;
                        }
                        else
                        {
                            sitStillMultiplier = 1;
                        }
                    }
                }

                if (followPlayer)
                {
                    rayCastSize.y = 200;
                    if (distanceFromPlayer.x < 0)
                    {
                        direction = -1;
                    }
                    else
                    {
                        direction = 1;
                    }
                }
                else
                {
                    rayCastSize.y = rayCastSizeOrig.y;
                }
                
                rightWall = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + rayCastOffset.y), Vector2.right, rayCastSize.x, layerMask);
                Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + rayCastOffset.y), Vector2.right * rayCastSize.x, Color.yellow);

                if (rightWall.collider != null)
                {
                    if (!followPlayer)
                    {
                        direction = -1;
                    }
                    else if (direction == 1)
                    {
                        Jump();
                    }

                }

                leftWall = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + rayCastOffset.y), Vector2.left, rayCastSize.x, layerMask);
                Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + rayCastOffset.y), Vector2.left * rayCastSize.x, Color.blue);

                if (leftWall.collider != null)
                {
                    if (!followPlayer)
                    {
                        direction = 1;
                    }
                    else if (direction == -1)
                    {
                        Jump();
                    }
                }
                
                rightLedge = Physics2D.Raycast(new Vector2(transform.position.x + rayCastOffset.x, transform.position.y), Vector2.down, rayCastSize.y, layerMask);
                Debug.DrawRay(new Vector2(transform.position.x + rayCastOffset.x, transform.position.y), Vector2.down * rayCastSize.y, Color.blue);
                if ((rightLedge.collider == null || rightLedge.collider.gameObject.layer == 14) && direction == 1)
                {
                    direction = -1;
                }

                leftLedge = Physics2D.Raycast(new Vector2(transform.position.x - rayCastOffset.x, transform.position.y), Vector2.down, rayCastSize.y, layerMask);
                Debug.DrawRay(new Vector2(transform.position.x - rayCastOffset.x, transform.position.y), Vector2.down * rayCastSize.y, Color.blue);
                if ((leftLedge.collider == null || leftLedge.collider.gameObject.layer == 14) && direction == -1)
                {
                    direction = 1;
                }
            }
        }

        enemyBase.animator.SetBool("grounded", grounded);
        enemyBase.animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
        targetVelocity = move * maxSpeed;
    }

    public void Jump()
    {
        if (grounded)
        {
            velocity.y = jumpPower;
            PlayJumpSound();
            PlayStepSound();
        }
    }

    public void PlayStepSound()
    {
        enemyBase.audioSource.pitch = (Random.Range(0.6f, 1f));
        enemyBase.audioSource.PlayOneShot(stepSound);
    }

    public void PlayJumpSound()
    {
        enemyBase.audioSource.pitch = (Random.Range(0.6f, 1f));
        enemyBase.audioSource.PlayOneShot(jumpSound);
    }

}