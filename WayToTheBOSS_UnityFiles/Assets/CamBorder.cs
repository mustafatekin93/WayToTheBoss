using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamBorder : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.tag == "Player")
            GameObject.FindGameObjectWithTag("cam").GetComponent<CamBorderSelector>().borderCollider = this.transform.gameObject.GetComponent<Collider2D>();
    }
}
