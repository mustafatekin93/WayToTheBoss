using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;

    private float speed;
    private float moveInput;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpForce;
    private int playerHitPoint = 9;


    private bool isGrounded;
    [SerializeField] private Transform feetPosition;
    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask groundLayer;

    private bool isJumping;
    private float jumpTimeCounter;
    [SerializeField] private float jumpTime;

    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    private float jumpBufferTime = 0.3f;
    private float jumpBufferCounter;

    private float attackTime = 0.33f;
    private float attackTimeCounter;

    private float HitTime = 1f;
    private float HitTimeCounter = 0f;


    public static bool isPaused { get; private set; }

    [SerializeField] private Transform swordPoint;
    [SerializeField] private float swordRange;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverScreen;

    [SerializeField] private Image hpImage;
    [SerializeField] private Sprite[] hpSprites;
    private int hitCounter = 0;

    [SerializeField] private TMP_Text boneCounterText;
    private int boneCounter = 0;

    [SerializeField] private Image skullImage;
    [SerializeField] private Sprite[] skullSprites;
    private int skullCounter = 0;

    [SerializeField] private GameObject endGameDoor;

    [SerializeField] private AudioClip[] swordSwingSounds;
    [SerializeField] private AudioClip[] hitSounds;
    [SerializeField] private AudioClip[] pickUpSounds;

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(swordPoint.position, swordRange);
    }

    void Start()
    {
        isPaused = false;
    }

    public void stopMove()
    {
        rb.velocity = new Vector2(0, 0);
        IdleAnimation();
    }

    void FixedUpdate()
    {
        if (!DialogueSystem.inDialogue)
        {
            moveInput = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                pauseMenu.SetActive(false);
                Time.timeScale = 1;
            }
        }

        HitTimeCounter -= Time.deltaTime;
        if (HitTimeCounter <= 0)
            HitTimeCounter = 0;

        hpImage.sprite = hpSprites[hitCounter];

        boneCounterText.text = ": " + boneCounter.ToString();

        skullImage.sprite = skullSprites[skullCounter];
        if (Input.GetKeyDown(KeyCode.U))
        {
            skullCounter++;
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            skullCounter--;
        }

        if (!DialogueSystem.inDialogue)
        {
            CharacterMovement();
        }
        else
        {
            stopMove();
        }
    }


    //Karakter Hareketleri
    private void CharacterMovement()
    {
        // Karakterin sağa yada sola dönmesi
        if (moveInput > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (moveInput < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }


        // Karakter in yerde olup olmadığının kontrol edilmesi
        isGrounded = Physics2D.OverlapCircle(feetPosition.position, checkRadius, groundLayer);
        if (isGrounded)
        { coyoteTimeCounter = coyoteTime; }
        else
        { coyoteTimeCounter -= Time.deltaTime; }

        if (Input.GetButtonDown("Jump"))
        { jumpBufferCounter = jumpBufferTime; }
        else
        { jumpBufferCounter -= Time.deltaTime; }



        //Karakterin coyoteTime ve jumpBuffer ile zıplaması
        if (coyoteTimeCounter > 0 && jumpBufferCounter > 0)
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = Vector2.up * jumpForce;
            jumpBufferCounter = 0;
        }

        if (isJumping == true && Input.GetButton("Jump"))
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }


        //Attack tuşuna basılı tutunca karakterin koşması
        if (Input.GetButton("Attack"))
        {
            speed = runSpeed;
        }
        else
        {
            speed = walkSpeed;
        }


        //Ard arda yapılan ataklara aralık konması
        attackTimeCounter -= Time.deltaTime;
        if ((Input.GetButtonDown("Attack")) && attackTimeCounter <= 0)
        {
            Attack();
            attackTimeCounter = attackTime;
        }


        //Basılan tuşlara göre karakterin duruyor, yürüyor yada koşuyor olması
        if ((moveInput < 0 || moveInput > 0) && speed == runSpeed)
        {
            RunAnimation();
        }
        else if ((moveInput < 0 || moveInput > 0) && speed == walkSpeed)
        {
            WalkAnimation();
        }
        else if (moveInput == 0)
        {
            IdleAnimation();
        }
    }

    //animator koşma animasyonu
    private void RunAnimation()
    {
        animator.SetFloat("playerSpeed", speed);
        animator.SetBool("isRunning", true);
        animator.SetBool("isWalking", false);
    }

    //animator yürüme animasyonu
    private void WalkAnimation()
    {
        animator.SetFloat("playerSpeed", speed);
        animator.SetBool("isRunning", false);
        animator.SetBool("isWalking", true);
    }

    //animator durma animasyonu
    private void IdleAnimation()
    {
        animator.SetFloat("playerSpeed", 0);
        animator.SetBool("isRunning", false);
        animator.SetBool("isWalking", false);
    }

    //karakterin atak yapması
    void Attack()
    {
        rb.velocity = rb.velocity * new Vector2(0, 0.1f);
        int index = Random.Range(0, 2);
        switch (index)
        {
            case 0: animator.SetTrigger("attack1"); break;
            case 1: animator.SetTrigger("attack2"); break;
        }

        int soundIndex = Random.Range(0, swordSwingSounds.Length);
        SoundManager.instance.PlaySound(swordSwingSounds[soundIndex]);
        StartCoroutine(hitEnemy());
    }

    //karakterin hasar yemesi
    public void GetHit()
    {
        if (HitTimeCounter <= 0)
        {
            HitTimeCounter = HitTime;
            hitCounter++;
            if (hitCounter >= playerHitPoint)
            {

                gameOverScreen.SetActive(true);
                stopMove();
                animator.SetBool("isDead", true);
                Destroy(gameObject, 3);
                this.enabled = false;
            }
            else
            {
                animator.SetTrigger("hit");
                StartCoroutine(Hit());
            }
        }
    }

    IEnumerator Hit()
    {
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        sr.color = Color.red;
        yield return new WaitForSeconds(0.25f);
        sr.color = Color.white;
    }


    void PlayPickUpSound()
    {
        int randomIndex = Random.Range(0, pickUpSounds.Length);
        SoundManager.instance.PlaySound(pickUpSounds[randomIndex]);
    }
    //Karakterin yerden can ve kemik alması
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.tag == "HalfHp")
        {
            PlayPickUpSound();
            Destroy(col.gameObject);
            hitCounter--;
            if (hitCounter <= 0)
            {
                hitCounter = 0;
            }
        }

        if (col.transform.tag == "FullHp")
        {
            PlayPickUpSound();
            Destroy(col.gameObject);
            hitCounter -= 2;
            if (hitCounter <= 0)
            {
                hitCounter = 0;
            }
        }

        if (col.transform.tag == "Bone")
        {
            PlayPickUpSound();
            Destroy(col.gameObject);
            boneCounter++;
        }

        if (col.transform.tag == "Bones")
        {
            PlayPickUpSound();
            Destroy(col.gameObject);
            boneCounter += 2;
        }

        if (col.transform.tag == "Skull")
        {
            PlayPickUpSound();
            Destroy(col.gameObject);
            skullCounter++;
        }
        if (col.transform.tag == "Key")
        {
            PlayPickUpSound();
            endGameDoor.SetActive(true);
        }
    }

    public int getBoneCount()
    {
        return boneCounter;
    }

    public void subtractBones(int bone)
    {
        boneCounter -= bone;
    }

    public int getSkullCount()
    {
        return skullCounter;
    }



    //Karakterin düşmana hasar vemesi
    IEnumerator hitEnemy()
    {
        yield return new WaitForSeconds(0.25f);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(swordPoint.position, swordRange, enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.TryGetComponent<EnemyScript>(out EnemyScript es))
            {
                es = enemy.transform.GetComponent<EnemyScript>();
                int randomIndex = Random.Range(0, hitSounds.Length);
                SoundManager.instance.PlaySound(hitSounds[randomIndex]);
                es.HitAnimation();
            }
            /*if (enemy.TryGetComponent<FantasyWarrior>(out FantasyWarrior fw))
            {
                fw = enemy.transform.GetComponent<FantasyWarrior>();
                fw.HitAnimation();
            }
            if (enemy.TryGetComponent<SkeletonBoss>(out SkeletonBoss sb))
            {
                sb = enemy.transform.GetComponent<SkeletonBoss>();
                sb.HitAnimation();
            }*/
        }
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.transform.tag == "Enemy")
        {
            GetHit();
        }
    }
}


