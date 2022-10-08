using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkeletonBoss : EnemyScript
{
    [SerializeField] private GameObject spawner1;
    [SerializeField] private GameObject spawner2;
    [SerializeField] private GameObject backDoor;

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
        enemyAnimator.SetTrigger("attack");
    }

    public override void HitAnimation()
    {
        base.HitAnimation();

        hpFill = (float)hitCounter / hitPoint;
        hpBarImage.fillAmount = (1 - hpFill);
    }

    protected override void DropOnDeath()
    {
        //base.DropOnDeath();
        Rigidbody2D rigidbody;
        Collider2D collider;

        GameObject _skull = (GameObject)Instantiate(dropOnDeath[0], transform.position, Quaternion.identity);
        rigidbody = _skull.GetComponent<Rigidbody2D>();
        collider = _skull.GetComponent<Collider2D>();
        rigidbody.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
        StartCoroutine(dropKinematic());

        IEnumerator dropKinematic()
        {
            yield return new WaitForSeconds(0.5f);
            rigidbody.isKinematic = true;
            rigidbody.velocity = Vector2.zero;
            collider.isTrigger = true;
        }
    }

    protected override void Death()
    {
        base.Death();
        backDoor.SetActive(true);
        Destroy(spawner1);
        Destroy(spawner2);
    }

}
