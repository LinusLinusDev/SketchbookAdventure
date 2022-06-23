using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

/*Adds player functionality to a physics object*/

[RequireComponent(typeof(RecoveryCounter))]

public class NewPlayer : PhysicsObject
{
    public SessionValues sesVal;
    [Header ("Reference")]
    public AudioSource audioSource;
    [SerializeField] public Animator animator;
    private AnimatorFunctions animatorFunctions;
    public GameObject attackHit;
    private CapsuleCollider2D capsuleCollider;
    public CameraEffects cameraEffects;
    [SerializeField] private GameObject tongueTip;
    [SerializeField] private ParticleSystem deathParticles;
    [SerializeField] private AudioSource flameParticlesAudioSource;
    [SerializeField] private GameObject graphic;
    [SerializeField] private Component[] graphicSprites;
    [SerializeField] private ParticleSystem jumpParticles;
    [SerializeField] private GameObject pauseMenu;
    public RecoveryCounter recoveryCounter;
    public AudioClip poofSound;
    public AudioClip tongueSound;
    
    // Singleton instantiation
    private static NewPlayer instance;
    public static NewPlayer Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<NewPlayer>();
            return instance;
        }
    }

    [Header("Properties")] private Rigidbody2D rdb;
    [SerializeField] private string[] cheatItems;
    public bool dead = false;
    public bool frozen = false;
    private float fallForgivenessCounter; //Counts how long the player has fallen off a ledge
    [SerializeField] private float fallForgiveness = .2f; //How long the player can fall from a ledge and still jump
    [System.NonSerialized] public string groundType = "grass";
    [System.NonSerialized] public RaycastHit2D ground; 
    [SerializeField] Vector2 hurtLaunchPower; //How much force should be applied to the player when getting hurt?
    public float launch; //The float added to x and y moveSpeed. This is set with hurtLaunchPower, and is always brought back to zero
    [SerializeField] private float launchRecovery; //How slow should recovering from the launch be? (Higher the number, the longer the launch will last)
    private bool jumping;
    public bool climbing;
    public bool dashing;
    public bool atClimbable;
    private int collidersAtClimbable = 0;
    private bool doubleJump;
    private bool dash;
    private float verticalInput;
    private float horizontalInput;
    public int color;
    private Vector3 origLocalScale;
    [System.NonSerialized] public bool pounded;
    [System.NonSerialized] public bool pounding;
    [System.NonSerialized] public bool shooting = false;

    [Header ("Skills")]
    public float maxSpeed = 7;
    public float jumpPower = 17;
    public float dashPower = 20;
    public float tongueLength = 8;
    public float climbingSpeed = 0.01f;
    public int maxColor = 1000;
    
    [Header ("Inventory")]
    public float ammo;
    public int coins;
    public int health;
    public int maxHealth;
    public int maxAmmo;
    public int[] colorAmmo;
    public GameObject whitePoof;
    public GameObject greenPoof;
    public GameObject bluePoof;
    public GameObject redPoof;

    [Header ("Sounds")]
    public AudioClip deathSound;
    public AudioClip equipSound;
    public AudioClip grassSound;
    public AudioClip hurtSound;
    public AudioClip[] hurtSounds;
    public AudioClip holsterSound;
    public AudioClip jumpSound;
    public AudioClip landSound;
    public AudioClip poundSound;
    public AudioClip punchSound;
    public AudioClip[] poundActivationSounds;
    public AudioClip outOfAmmoSound;
    public AudioClip stepSound;
    [System.NonSerialized] public int whichHurtSound;

    void Start()
    {
        Cursor.visible = false;
        rdb = GetComponent<Rigidbody2D>();
        SetUpCheatItems();
        if ( sesVal.spawnCoins[0] == -1 && sesVal.spawnColorAmmo[0] == -1)
        {
            if (!(colorAmmo is {Length: 3})) colorAmmo = new[] {maxColor, maxColor, maxColor};
        }
        else
        {
            transform.position = new Vector3(sesVal.spawnLocation[0], sesVal.spawnLocation[1], sesVal.spawnLocation[2]);
            colorAmmo = new[] {sesVal.spawnColorAmmo[0], sesVal.spawnColorAmmo[1], sesVal.spawnColorAmmo[2]};
            coins = sesVal.spawnCoins[0];
        }
        if (colorAmmo[0] > 0) color = 0;
        else if (colorAmmo[1] > 0) color = 1;
        else if (colorAmmo[2] > 0) color = 2;
        else color = 3;
        health = maxHealth;
        animatorFunctions = GetComponent<AnimatorFunctions>();
        origLocalScale = transform.localScale;
        recoveryCounter = GetComponent<RecoveryCounter>();
        
        //Find all sprites so we can hide them when the player dies.
        graphicSprites = GetComponentsInChildren<SpriteRenderer>();

        SetGroundType();
    }

    private void Update()
    {
        ComputeVelocity();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Climb"))
        {
            collidersAtClimbable++;
            atClimbable = true;
        }
        if (!grounded && color == 0 && other.gameObject.CompareTag("Climb"))
        {
            climbing = true;
        }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Climb"))
        {
            collidersAtClimbable--;
            if (collidersAtClimbable == 0)
            {
                if(verticalInput > 0 && climbing) Jump(0.8f);
                atClimbable = false;
                climbing = false;
            }
        }
    }

    protected void ComputeVelocity()
    {
        //Player movement & attack
        Vector2 move = Vector2.zero;
        ground = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), -Vector2.up);

        //Lerp launch back to zero at all times
        launch += (0 - launch) * Time.deltaTime * launchRecovery;

        if (Input.GetButtonDown("Cancel"))
        {
            if (!pauseMenu.activeInHierarchy) pauseMenu.SetActive(true);
            else pauseMenu.GetComponent<PauseMenu>().Unpause();
        }

        if (!frozen && !pauseMenu.activeInHierarchy)
        {
            // Move   
            if (math.abs(launch) < 0.6f)
            {
                horizontalInput = Input.GetAxis("Horizontal");
                if (dashing)
                {
                    launch = 0;
                    dashing = false;
                    animator.SetBool("dash",false);
                }
            }
            else if(dashing) velocity.y = 0;
            move.x = horizontalInput + launch;

            // Jump
            if (Input.GetButtonDown("Jump") && animator.GetBool("grounded") == true && !jumping)
            {
                animator.SetBool("pounded", false);
                Jump(1f);
            }
            
            // Doublejump
            else if (Input.GetButtonDown("Jump") && !shooting && doubleJump && color == 1 && colorAmmo[color] >= 2)
            {
                poof(1);
                colorAmmo[color] -= 200;
                doubleJump = false;
                Jump(0.9f);
                animator.SetTrigger("doubleJump");
            }
            
            // Dash
            if (Input.GetKeyDown(KeyCode.LeftShift) && !shooting && dash && color == 2 && colorAmmo[color] >= 2)
            {
                poof(2);
                colorAmmo[color] -= 200;
                dash = false;
                Dash(0.7f);
            }
            
            // Tongue
            if ((Input.GetKeyDown(KeyCode.LeftControl) || Input.GetMouseButtonDown(2)) && !shooting && !dashing && !climbing &&
                !animator.GetCurrentAnimatorStateInfo(0).IsName("DoubleJump"))
            {
                StartTongue();
            }

            // Climb
            if (climbing)
            {
                launch = 0;
                verticalInput = Input.GetAxis("Vertical");
                transform.Translate(0,verticalInput*climbingSpeed,0);
                if (verticalInput != 0) colorAmmo[color] -= 1;
                velocity.y = 0;
                gravityModifier = 0;
                doubleJump = true;
                dash = true;
                if ((transform.localScale.x > 0 && horizontalInput < 0) || (transform.localScale.x < 0 && horizontalInput > 0)) climbing = false;
                if (Input.GetButtonDown("Jump"))
                {
                    poof(0);
                    climbing = false;
                    launch = -transform.localScale.x*15;
                    Jump(1f);
                    colorAmmo[color] -= 200;
                }
            }
            else
            {
                gravityModifier = 3.2f;
            }
            
            //Flip the graphic's localScale
            if (!shooting)
            {
                if (move.x > 0.01f)
                {
                    graphic.transform.localScale = new Vector3(origLocalScale.x, transform.localScale.y, transform.localScale.z);
                }
                else if (move.x < -0.01f)
                {
                    graphic.transform.localScale = new Vector3(-origLocalScale.x, transform.localScale.y, transform.localScale.z);
                }
            }

            if (color != 3)
            {
                if (Input.GetKeyDown("e") || Input.GetMouseButtonDown(1))
                {
                    int prevColor = color;
                    if(colorAmmo[(color + 1) % 3] > 0)color = (color + 1) % 3;
                    else if(colorAmmo[(color + 2) % 3] > 0)color = (color + 2) % 3;
                    if (!grounded && atClimbable && color == 0)
                    {
                        colorAmmo[color] -= 100;
                        climbing = true;
                    }
                    if(color != prevColor)poof(color);
                }
            
                if (Input.GetKeyDown("q") || Input.GetMouseButtonDown(0))
                {
                    int prevColor = color;
                    if(colorAmmo[(color + 2) % 3] > 0)color = (color + 2) % 3;
                    else if(colorAmmo[(color + 1) % 3] > 0)color = (color + 1) % 3;
                    if (atClimbable && color == 0)
                    {
                        climbing = true;
                    }
                    if(color != prevColor)poof(color);
                }

                if (colorAmmo[0] < 0) colorAmmo[0] = 0;
                else if (colorAmmo[1] < 0) colorAmmo[1] = 0;
                else if (colorAmmo[2] < 0) colorAmmo[2] = 0;

                if (colorAmmo.All(val => val <= 0))
                {
                    color = 3;
                    poof(color);
                }
                else if (colorAmmo[color] <= 0)
                {
                    color = (color + 1) % 3;
                    if(colorAmmo[color] > 0)poof(color);
                }
            }
            else
            {
                if (colorAmmo[0] > 0)
                {
                    color = 0;
                    poof(color);
                }
                else if (colorAmmo[1] > 0)
                {
                    color = 1;
                    poof(color);
                }
                else if (colorAmmo[2] > 0)
                {
                    color = 2;
                    poof(color);
                }
            }

            if (color != 0) climbing = false;
            
            // Cheat
            if (Input.GetKeyDown("g"))
            {
                colorAmmo[0] += 200;
            }
            
            if (Input.GetKeyDown("b"))
            {
                colorAmmo[1] += 200;
            }
            
            if (Input.GetKeyDown("r"))
            {
                colorAmmo[2] += 200;
            }
            //

            //Allow the player to jump even if they have just fallen off an edge ("fall forgiveness")
            if (!grounded)
            {
                if (fallForgivenessCounter < fallForgiveness && !jumping)
                {
                    fallForgivenessCounter += Time.deltaTime;
                }
                else
                {
                    animator.SetBool("grounded", false);
                }
            }
            else
            {
                climbing = false;
                doubleJump = true;
                dash = true;
                fallForgivenessCounter = 0;
                animator.SetBool("grounded", true);
            }

            //Set each animator float, bool, and trigger to it knows which animation to fire
            animator.SetBool("climb", climbing);
            animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
            animator.SetFloat("velocityY", velocity.y);
            animator.SetInteger("attackDirectionY", (int)Input.GetAxis("VerticalDirection"));
            animator.SetInteger("moveDirection", (int)Input.GetAxis("HorizontalDirection"));
            animator.SetBool("hasChair", GameManager.Instance.inventory.ContainsKey("chair"));
            targetVelocity = move * maxSpeed;
            
        }
        else
        {
            //If the player is set to frozen, his launch should be zeroed out!
            launch = 0;
        }
    }

    public void SetGroundType()
    {
        //If we want to add variable ground types with different sounds, it can be done here
        switch (groundType)
        {
            case "Grass":
                stepSound = grassSound;
                break;
        }
    }

    public void Freeze(bool freeze)
    {
        //Set all animator params to ensure the player stops running, jumping, etc and simply stands
        if (freeze)
        {
            animator.SetInteger("moveDirection", 0);
            animator.SetBool("grounded", true);
            animator.SetFloat("velocityX", 0f);
            animator.SetFloat("velocityY", 0f);
            GetComponent<PhysicsObject>().targetVelocity = Vector2.zero;
        }

        frozen = freeze;
        shooting = false;
        launch = 0;
    }


    public void GetHurt(int hurtDirection, int hitPower)
    {
        //If the player is not frozen (ie talking, spawning, etc), recovering, and pounding, get hurt!
        if (!frozen && !recoveryCounter.recovering && !pounding)
        {
            HurtEffect();
            cameraEffects.Shake(100, 1);
            animator.SetTrigger("hurt");
            velocity.y = hurtLaunchPower.y;
            launch = hurtDirection * (hurtLaunchPower.x);
            recoveryCounter.counter = 0;

            GameManager.Instance.hud.HealthBarHurt();
        }
    }

    private void HurtEffect()
    {
        GameManager.Instance.audioSource.PlayOneShot(hurtSound);
        StartCoroutine(FreezeFrameEffect());
        GameManager.Instance.audioSource.PlayOneShot(hurtSounds[whichHurtSound]);

        if (whichHurtSound >= hurtSounds.Length - 1)
        {
            whichHurtSound = 0;
        }
        else
        {
            whichHurtSound++;
        }
        cameraEffects.Shake(100, 1f);
    }

    public IEnumerator FreezeFrameEffect(float length = .007f)
    {
        Time.timeScale = .1f;
        yield return new WaitForSeconds(length);
        Time.timeScale = 1f;
    }
    
    public void ResetLevel()
    {
        Freeze(true);
        dead = false;
        health = maxHealth;
        for (int i = 0; i < colorAmmo.Length; i++) colorAmmo[i] = maxColor;
    }

    public void SubtractAmmo()
    {
        if (ammo > 0)
        {
            ammo -= 20 * Time.deltaTime;
        }
    }

    public void Jump(float jumpMultiplier)
    {
        if (velocity.y != jumpPower)
        {
            velocity.y = jumpPower * jumpMultiplier; //The jumpMultiplier allows us to use the Jump function to also launch the player from bounce platforms
            PlayJumpSound();
            PlayStepSound();
            JumpEffect();
            jumping = true;
        }
    }
    
    public void Dash(float dashMultiplier)
    {
        //PlayDashSound();
        PlayStepSound();
        JumpEffect();
        dashing = true;
        launch = dashPower*dashMultiplier*transform.localScale.x;
        animator.SetBool("dash",true);
    }

    public void PlayStepSound()
    {
        //Play a step sound at a random pitch between two floats, while also increasing the volume based on the Horizontal axis
        audioSource.pitch = (Random.Range(0.9f, 1.1f));
        audioSource.PlayOneShot(stepSound, Mathf.Abs(Input.GetAxis("Horizontal") / 10)*GameManager.Instance.audioSource.volume);
    }

    public void PlayJumpSound()
    {
        audioSource.pitch = (Random.Range(1f, 1f));
        GameManager.Instance.audioSource.PlayOneShot(jumpSound, .1f * GameManager.Instance.audioSource.volume);
    }


    public void JumpEffect()
    {
        jumpParticles.Emit(1);
        audioSource.pitch = (Random.Range(0.6f, 1f));
        audioSource.PlayOneShot(landSound,GameManager.Instance.audioSource.volume);
    }

    public void LandEffect()
    {
        if (jumping)
        {
            jumpParticles.Emit(1);
            audioSource.pitch = (Random.Range(0.6f, 1f));
            audioSource.PlayOneShot(landSound, GameManager.Instance.audioSource.volume);
            jumping = false;
        }
    }

    public void PunchEffect()
    {
        GameManager.Instance.audioSource.PlayOneShot(punchSound);
        cameraEffects.Shake(100, 1f);
    }

    public void ActivatePound()
    {
        //A series of events needs to occur when the player activates the pound ability
        if (!pounding)
        {
            animator.SetBool("pounded", false);

            if (velocity.y <= 0)
            {
                velocity = new Vector3(velocity.x, hurtLaunchPower.y / 2, 0.0f);
            }

            GameManager.Instance.audioSource.PlayOneShot(poundActivationSounds[Random.Range(0, poundActivationSounds.Length)]);
            pounding = true;
            FreezeFrameEffect(.3f);
        }
    }
    public void PoundEffect()
    {
        //As long as the player as activated the pound in ActivatePound, the following will occur when hitting the ground.
        if (pounding)
        {
            animator.ResetTrigger("attack");
            velocity.y = jumpPower / 1.4f;
            animator.SetBool("pounded", true);
            GameManager.Instance.audioSource.PlayOneShot(poundSound);
            cameraEffects.Shake(200, 1f);
            pounding = false;
            recoveryCounter.counter = 0;
            animator.SetBool("pounded", true);
        }
    }

    public void FlashEffect()
    {
        //Flash the player quickly
        animator.SetTrigger("flash");
    }

    public void Hide(bool hide)
    {
        Freeze(hide);
        foreach (SpriteRenderer sprite in graphicSprites)
            sprite.gameObject.SetActive(!hide);
    }

    public void Shoot(bool equip)
    {
        //Flamethrower ability
        if (GameManager.Instance.inventory.ContainsKey("flamethrower"))
        {
            if (equip)
            {
                if (!shooting)
                {
                    animator.SetBool("shooting", true);
                    GameManager.Instance.audioSource.PlayOneShot(equipSound);
                    flameParticlesAudioSource.Play();
                    shooting = true;
                }
            }
            else
            {
                if (shooting)
                {
                    animator.SetBool("shooting", false);
                    flameParticlesAudioSource.Stop();
                    GameManager.Instance.audioSource.PlayOneShot(holsterSound);
                    shooting = false;
                }
            }
        }
    }

    public void SetUpCheatItems()
    {
        //Allows us to get various items immediately after hitting play, allowing for testing. 
        for (int i = 0; i < cheatItems.Length; i++)
        {
            GameManager.Instance.GetInventoryItem(cheatItems[i], null);
        }
    }
    
    public void poof(int color)
    {
        GameManager.Instance.audioSource.PlayOneShot(poofSound, 3);
        switch (color) 
        {
            case 0:
                Instantiate(greenPoof, new Vector3(transform.position.x,transform.position.y + 2,transform.position.z), Quaternion.identity);
                break;
            case 1:
                Instantiate(bluePoof, new Vector3(transform.position.x,transform.position.y + 2,transform.position.z), Quaternion.identity);
                break;
            case 2:
                Instantiate(redPoof, new Vector3(transform.position.x,transform.position.y + 2,transform.position.z), Quaternion.identity);
                break;
            case 3:
                Instantiate(whitePoof, new Vector3(transform.position.x,transform.position.y + 2,transform.position.z), Quaternion.identity);
                break;
        }
    }

    public void StartTongue()
    {
        GameManager.Instance.audioSource.PlayOneShot(tongueSound, 0.5f);
        animator.SetBool("mouthOpen",true);
        tongueTip.GetComponent<TongueTip>().activate(tongueLength);
        shooting = true;
    }

    public void EndTongue()
    {
        animator.SetBool("mouthOpen",false);
        shooting = false;
    }
}