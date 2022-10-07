using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall_Projectile : MonoBehaviour
{

    [SerializeField] private Animator fireballAnimator;
    [SerializeField] private Rigidbody2D fireballRB;
    [SerializeField] private Collider2D fireballCollider;
    private GameObject Player;
    private Vector2 currentPosition;

    void Awake()
    {
        Player = GameObject.Find("Player");
        Destroy(gameObject, 10);
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, 4f * Time.deltaTime);
        transform.up = Player.transform.position - transform.position;
        //transform.LookAt(playerPosition);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.tag == "Player")
        {
            PlayerControl hit = col.transform.GetComponent<PlayerControl>();
            hit.GetHit();
            fireballRB.velocity = Vector2.zero;
            fireballRB.isKinematic = true;
            fireballAnimator.SetTrigger("Explotion");
            fireballCollider.enabled = false;
            Destroy(gameObject, 0.5f);
        }
    }

}
