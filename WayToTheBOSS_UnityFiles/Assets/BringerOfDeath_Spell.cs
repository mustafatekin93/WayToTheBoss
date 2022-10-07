using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerOfDeath_Spell : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Collider2D HitCollider;
    [SerializeField] private float delayTime;
    GameObject Player;
    float range = 3f;

    void Awake()
    {
        Player = GameObject.Find("Player");
        float PosX = Random.Range(Player.transform.position.x - range, Player.transform.position.x + range);
        transform.position = new Vector3(PosX, Player.transform.position.y + 4.5f, 0);
        anim.SetTrigger("spell");
        StartCoroutine(HitColliderActivate());
        Destroy(gameObject, 3);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.tag == "Player")
        {
            PlayerControl hit = col.transform.GetComponent<PlayerControl>();
            hit.GetHit();
        }

    }

    IEnumerator HitColliderActivate()
    {
        yield return new WaitForSeconds(delayTime);
        HitCollider.enabled = true;
        yield return new WaitForSeconds(0.2f);
        HitCollider.enabled = false;
    }
}
