using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireWorm : EnemyScript
{
    [SerializeField] private float attackDelay;
    [SerializeField] private GameObject fireballDropTop;
    [SerializeField] private GameObject fireballProjectile;
    [SerializeField] private GameObject endDoor;
    [SerializeField] private Image hpBarImage;

    float hpFill;
    float fireballTimer = 5;
    float projectileTimer = 10;

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

    protected override void Death()
    {
        base.Death();
        StartCoroutine(endDoorActivate());
    }

    public override void HitAnimation()
    {
        base.HitAnimation();

        hpFill = (float)hitCounter / hitPoint;
        hpBarImage.fillAmount = (1 - hpFill);
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

    IEnumerator endDoorActivate()
    {
        yield return new WaitForSeconds(1f);
        endDoor.SetActive(true);
    }
}
