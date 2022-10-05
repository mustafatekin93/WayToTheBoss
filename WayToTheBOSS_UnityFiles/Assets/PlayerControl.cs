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
    [SerializeField] private int playerHitPoint;


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

    private float attackTime = 0.25f;
    private float attackTimeCounter;

    public static bool isPaused { get; private set; }

    [SerializeField] private Transform swordPoint;
    [SerializeField] private float swordRange;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverScreen;

    private int hitCounter = 0;

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
        StartCoroutine(hitEnemy());
    }

    //karakterin hasar yemesi
    public void GetHit()
    {
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

    IEnumerator Hit()
    {
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        sr.color = Color.red;
        yield return new WaitForSeconds(0.25f);
        sr.color = Color.white;
    }

    /*void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.tag == "halfHp")
        {
            Destroy(col.gameObject);
            hitCounter--;
            if (hitCounter <= 0)
            {
                hitCounter = 0;
            }
        }
    }*/


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
            StartCoroutine(Hit());
        }
    }
}


