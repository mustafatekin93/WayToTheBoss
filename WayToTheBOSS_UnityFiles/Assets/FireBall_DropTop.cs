using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall_DropTop : MonoBehaviour
{
    [SerializeField] private Animator fireballAnimator;
    [SerializeField] private Rigidbody2D fireballRB;
    [SerializeField] private Collider2D fireballCollider;
    private float playerPositionX;

    void Awake()
    {
        SpawnRandom();
        Destroy(gameObject, 7);
    }

    void SpawnRandom()
    {
        fireballRB.velocity = Vector2.zero;
        playerPositionX = GameObject.Find("Player").transform.position.x;
        float spawnPosition = Random.Range(playerPositionX - 7, playerPositionX + 7);
        transform.position = new Vector2(spawnPosition, -1);
    }

    void Update()
    {
        if (transform.position.y < -20)
        {
            SpawnRandom();
            //Destroy(gameObject);
        }
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
