using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkeletonSeeker : EnemyScript
{
    [SerializeField] private GameObject hordePF;
    [SerializeField] private Collider2D bossCollider;
    [SerializeField] private Rigidbody2D bossRigidbody;
    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private Transform RandomPos1;
    [SerializeField] private Transform RandomPos2;

    [SerializeField] private Image hpBarImage;

    float hpFill;

    private bool canMove = false;
    private float HordeTimer = 3;

    private void OnEnable()
    {
        StartCoroutine(StartDelay());
    }
    protected override void WalkAnimation()
    {
        enemyAnimator.SetBool("isWalk", true);
    }
    protected override void IdleAnimation()
    {
        enemyAnimator.SetBool("isWalk", false);
    }
    protected override void AttackAnimation()
    {
        enemyAnimator.SetTrigger("attack");
    }

    public override void HitAnimation()
    {
        base.HitAnimation();

        hpFill = (float)hitCounter / hitPoint;
        hpBarImage.fillAmount = (1 - hpFill);
    }

    protected override void Update()
    {
        Debug.Log(canMove);

        if (!canMove)
        {
            StartCoroutine(SearchEnemies());
            return;
        }

        base.Update();

        if (!DialogueSystem.inDialogue)
        {
            HordeTimer -= Time.deltaTime;
            if (HordeTimer <= 0 && canMove)
            {
                bossCollider.enabled = false;
                bossRigidbody.isKinematic = true;
                bossRigidbody.velocity = Vector2.zero;
                canMove = false;
                IdleAnimation();
                enemyAnimator.SetTrigger("goGround");
                int randomAmount = Random.Range(1, 4);
                StartCoroutine(SpawnHorde(randomAmount));
                HordeTimer = Random.Range(5, 10);
            }
        }
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 20);
    }

    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(1f);
        canMove = true;
    }

    IEnumerator GoUp()
    {
        enemyAnimator.SetTrigger("goUp");
        bossCollider.enabled = true;
        bossRigidbody.isKinematic = false;
        yield return new WaitForSeconds(1.5f);
        canMove = true;
    }

    IEnumerator SearchEnemies()
    {
        yield return new WaitForSeconds(1f);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, 20, enemyLayer);
        if (hitEnemies.Length == 0)
        {
            StartCoroutine(GoUp());
        }
    }

    IEnumerator SpawnHorde(float amount)
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i <= amount; i++)
        {
            GameObject horde = (GameObject)Instantiate(hordePF, new Vector2(Random.Range(RandomPos1.position.x, RandomPos2.position.x), Random.Range(RandomPos1.position.y, RandomPos2.position.y)), Quaternion.identity);
            yield return new WaitForSeconds(0.25f);
            Destroy(horde);
        }
    }

}
