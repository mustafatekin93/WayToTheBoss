using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPick : MonoBehaviour
{
    [SerializeField] private GameObject pickupEffect;
    [SerializeField] private Collider2D keyCol;
    [SerializeField] private SpriteRenderer keySprite;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.tag == "Player")
        {
            keySprite.enabled = false;
            keyCol.enabled = false;
            Destroy(gameObject, 2);
            pickupEffect.SetActive(true);
        }
    }
}
