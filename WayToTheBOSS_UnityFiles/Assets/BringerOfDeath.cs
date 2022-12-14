using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BringerOfDeath : EnemyScript
{
    [SerializeField] private float attackDelay;
    [SerializeField] private GameObject spell;
    [SerializeField] private GameObject hpBar;
    [SerializeField] private Image hpBarImage;
    [SerializeField] private GameObject endDialogue;

    float hpFill;
    float spellTimer = 3;

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
            spellTimer -= Time.deltaTime;
            if (spellTimer <= 0)
            {
                enemyAnimator.SetTrigger("spell");
                int randomAmount = Random.Range(1, 4);
                StartCoroutine(SpawnSpell(randomAmount));
                spellTimer = Random.Range(5, 10);
            }
        }
    }

    protected override void Death()
    {
        base.Death();
        hpBar.SetActive(false);
        endDialogue.SetActive(true);
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

    public override void HitAnimation()
    {
        base.HitAnimation();

        hpFill = (float)hitCounter / hitPoint;
        hpBarImage.fillAmount = (1 - hpFill);
    }

    IEnumerator SpawnSpell(float amount)
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i <= amount; i++)
        {
            GameObject _fireball = (GameObject)Instantiate(spell, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.15f);
        }
    }
}
