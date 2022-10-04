using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public Transform enemyTransform;
    [HideInInspector] public Transform playerTransform;
    public Transform swordPosition;
    public Animator enemyAnimator;
    //[SerializeField] private GameObject halfHP;
    public float moveableDistance;
    public float attackableDistance;
    public float attackTimer;
    [SerializeField] private int hitPoint;
    public LayerMask playerLayer;
    private Collider2D enemyCollider;
    private Rigidbody2D enemeyRigidbody;

    [HideInInspector] public float oldAttackTimer;
    [HideInInspector] public float distance;

    private int hitCounter;
    private PlayerControl playerControl;


    void Awake()
    {

        oldAttackTimer = attackTimer;
        attackTimer = 0;
        hitCounter = 0;

        enemyCollider = GetComponent<Collider2D>();
        enemeyRigidbody = GetComponent<Rigidbody2D>();
        enemyAnimator = gameObject.GetComponentInChildren<Animator>();
        playerTransform = GameObject.Find("Player").transform;
        playerControl = playerTransform.GetComponent<PlayerControl>();
    }

    void Update()
    {
        if (playerTransform == null || enemyTransform == null)
            return;

        distance = playerTransform.position.x - enemyTransform.position.x;

        if (!playerControl.inDialogue())
        {
            if (Mathf.Abs(distance) < moveableDistance && Mathf.Abs(distance) > attackableDistance && Mathf.Abs(playerTransform.position.y - enemyTransform.position.y) <= 3)
            {
                MoveToPlayer(true);
            }
            else if (Mathf.Abs(distance) < attackableDistance)
            {
                MoveToPlayer(false);
                AttackToPlayer();
            }
        }
        else
        {
            IdleAnimation();
        }
    }

    protected virtual void MoveToPlayer(bool condition)
    {
        if (condition == true)
        {
            if (distance > 0)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else if (distance < 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }

            enemyTransform.position = Vector2.MoveTowards(enemyTransform.position, playerTransform.position, 0.075f);
            enemyAnimator.SetBool("isWalking", true);
            enemyAnimator.SetFloat("playerSpeed", 7);
        }

        else if (condition == false)
        {
            IdleAnimation();
        }
    }

    protected virtual void IdleAnimation()
    {
        enemyAnimator.SetBool("isWalking", false);
        enemyAnimator.SetFloat("playerSpeed", 0);
    }

    protected virtual void AttackToPlayer()
    {
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            StartCoroutine(hitPlayer(0.25f));
            attackTimer = oldAttackTimer;
            int index = Random.Range(0, 2);
            switch (index)
            {
                case 0: enemyAnimator.SetTrigger("attack1"); break;
                case 1: enemyAnimator.SetTrigger("attack2"); break;
            }
        }
    }

    public void HitAnimation()
    {
        hitCounter++;
        if (hitCounter >= hitPoint)
        {
            Death();
            /*switch (Random.Range(0,2))
            {
                case 0:
                    //GameObject _halfHP = (GameObject)Instantiate(halfHP, transform.position, Quaternion.identity);
                    //_halfHP.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
                    break;
                default: break;
            }*/
            this.enabled = false;
            return;
        }
        else if (hitCounter > hitPoint)
        {
            return;
        }
        else
        {
            enemyAnimator.SetTrigger("hit");
            StartCoroutine(Hit());
        }
    }

    protected virtual IEnumerator hitPlayer(float attackDelay)
    {
        yield return new WaitForSeconds(attackDelay);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(swordPosition.position, 1, playerLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            PlayerControl hit = enemy.transform.GetComponent<PlayerControl>();
            hit.GetHit();
        }
    }

    IEnumerator Hit()
    {
        attackTimer = oldAttackTimer;
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        Color oldColor = sr.color;
        sr.color = Color.red;
        yield return new WaitForSeconds(0.25f);
        sr.color = oldColor;
    }

    protected virtual void Death()
    {
        enemyAnimator.SetTrigger("isDead");
        Destroy(enemeyRigidbody);
        Destroy(enemyCollider);
        //enemyCollider.isTrigger = true;
        //enemeyRigidbody.isKinematic = true;
        Destroy(gameObject, 3f);
    }
}
