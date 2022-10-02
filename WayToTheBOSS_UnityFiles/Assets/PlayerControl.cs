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

    //[SerializeField] private Transform swordPoint;
    //[SerializeField] private float swordRange;
    //[SerializeField] private LayerMask enemyLayer;


    public void stopMove()
    {
        rb.velocity = new Vector2(0, 0);
    }

    void FixedUpdate()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            Debug.Log("Paused");
        }

        isGrounded = Physics2D.OverlapCircle(feetPosition.position, checkRadius, groundLayer);

        if (isGrounded)
        { coyoteTimeCounter = coyoteTime; }
        else
        { coyoteTimeCounter -= Time.deltaTime; }

        if (Input.GetButtonDown("Jump"))
        { jumpBufferCounter = jumpBufferTime; }
        else
        { jumpBufferCounter -= Time.deltaTime; }

        if (moveInput > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (moveInput < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        if (coyoteTimeCounter > 0 && jumpBufferCounter > 0)
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = Vector2.up * jumpForce;
            jumpBufferCounter = 0;
        }

        if (isJumping == true && Input.GetButton("Jump"))
        {
            Debug.Log("Jump");
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

        if (Input.GetButton("Attack"))
        {
            speed = runSpeed;
        }
        else
        {
            speed = walkSpeed;
        }

        attackTimeCounter -= Time.deltaTime;
        if ((Input.GetButtonDown("Attack")) && attackTimeCounter <= 0)
        {
            Attack();
            attackTimeCounter = attackTime;
        }

        if ((moveInput < 0 || moveInput > 0) && speed == runSpeed)
        {
            animator.SetFloat("playerSpeed", speed);
            animator.SetBool("isRunning", true);
            animator.SetBool("isWalking", false);
        }

        else if ((moveInput < 0 || moveInput > 0) && speed == walkSpeed)
        {
            animator.SetFloat("playerSpeed", speed);
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", true);
        }
        else if (moveInput == 0)
        {
            animator.SetFloat("playerSpeed", 0);
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", false);
        }
    }

    void Attack()
    {
        rb.velocity = rb.velocity * new Vector2(0, 0.05f);
        int index = Random.Range(0, 2);
        switch (index)
        {
            case 0: animator.SetTrigger("attack1"); break;
            case 1: animator.SetTrigger("attack2"); break;
        }
        //StartCoroutine(hitEnemy());
    }


    /*public void GetHit()
    {
        hitCounter++;
        if (hitCounter >= 7)
        {
            StartCoroutine(GameOverScreen());
            animator.SetBool("isDead", true);
            Destroy(gameObject, 3);
            this.enabled = false;
        }
        else
        {
            animator.SetTrigger("hit");
            StartCoroutine(Hit());
        }
    }*/

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(feetPosition.position, checkRadius);
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
    /*IEnumerator hitEnemy()
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
            if (enemy.TryGetComponent<FantasyWarrior>(out FantasyWarrior fw))
            {
                fw = enemy.transform.GetComponent<FantasyWarrior>();
                fw.HitAnimation();
            }
            if (enemy.TryGetComponent<SkeletonBoss>(out SkeletonBoss sb))
            {
                sb = enemy.transform.GetComponent<SkeletonBoss>();
                sb.HitAnimation();
            }
        }
    }*/

    /*IEnumerator Hit()
    {
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        sr.color = Color.red;
        yield return new WaitForSeconds(0.25f);
        sr.color = Color.white;
    }*/
}


