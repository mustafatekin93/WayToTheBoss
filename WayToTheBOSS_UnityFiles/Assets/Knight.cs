﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : EnemyScript
{
    [SerializeField] private float attackDelay;
    [SerializeField] GameObject dialogueTrigger;

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

    protected override void Death()
    {
        base.Death();
        dialogueTrigger.SetActive(true);
    }
}
