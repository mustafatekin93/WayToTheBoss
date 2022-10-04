using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWorm : EnemyScript
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float attackDelay;

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
}
