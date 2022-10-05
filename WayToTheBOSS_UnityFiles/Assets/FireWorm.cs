using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWorm : EnemyScript
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float attackDelay;
    [SerializeField] private GameObject fireballDropTop;
    [SerializeField] private GameObject fireballProjectile;
    [SerializeField] private GameObject endDoor;

    float fireballTimer = 5;
    float projectileTimer = 10;

    protected override void MoveToPlayer(bool condition)
    {
        //base.MoveToPlayer();

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

            enemyTransform.position = Vector2.MoveTowards(enemyTransform.position, playerTransform.position, 0.1f * moveSpeed);
            enemyAnimator.SetBool("isWalk", true);
        }

        else if (condition == false)
        {
            enemyAnimator.SetBool("isWalk", false);
        }
    }

    protected override void Update()
    {
        base.Update();
        if (!DialogueSystem.inDialogue)
        {
            fireballTimer -= Time.deltaTime;
            if (fireballTimer <= 0)
            {
                int randomAmount = Random.Range(3, 6);
                StartCoroutine(SpawnFireball(randomAmount));
                fireballTimer = Random.Range(5, 10);
            }

            projectileTimer -= Time.deltaTime;
            if ((projectileTimer <= 0 && Mathf.Abs(distance) < moveableDistance && Mathf.Abs(playerTransform.position.y - enemyTransform.position.y) > 3))
            {
                StartCoroutine(SpawnProjectile());
                enemyAnimator.SetTrigger("attack");
                projectileTimer = Random.Range(8, 15);
            }
        }
    }

    protected override void AttackToPlayer()
    {
        //base.AttackToPlayer();
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            StartCoroutine(hitPlayer(attackDelay));
            enemyAnimator.SetTrigger("attack");
            attackTimer = oldAttackTimer;
        }
    }

    protected override void IdleAnimation()
    {
        //base.IdleAnimation();
        enemyAnimator.SetBool("isWalk", false);
    }

    protected override void Death()
    {
        base.Death();
        endDoor.SetActive(true);
    }

    IEnumerator SpawnFireball(float amount)
    {

        for (int i = 0; i <= amount; i++)
        {
            GameObject _fireball = (GameObject)Instantiate(fireballDropTop, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.15f);
        }
    }

    IEnumerator SpawnProjectile()
    {
        yield return new WaitForSeconds(1f);
        GameObject _fireball = (GameObject)Instantiate(fireballProjectile, swordPosition.position, Quaternion.identity);
    }
}
