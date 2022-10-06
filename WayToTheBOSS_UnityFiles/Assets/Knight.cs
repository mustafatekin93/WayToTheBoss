using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Knight : EnemyScript
{
    [SerializeField] private float attackDelay;
    [SerializeField] GameObject dialogueTrigger;
    [SerializeField] private Image hpBarImage;

    float hpFill;

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
        int index = Random.Range(0, 2);
        switch (index)
        {
            case 0: enemyAnimator.SetTrigger("attack1"); break;
            case 1: enemyAnimator.SetTrigger("attack2"); break;
        }
    }

    protected override void Death()
    {
        base.Death();
        dialogueTrigger.SetActive(true);
    }

    public override void HitAnimation()
    {
        base.HitAnimation();

        hpFill = (float)hitCounter / hitPoint;
        hpBarImage.fillAmount = (1 - hpFill);
    }
}
