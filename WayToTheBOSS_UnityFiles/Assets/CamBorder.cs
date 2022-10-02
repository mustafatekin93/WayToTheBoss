using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamBorder : MonoBehaviour
{

    void OnTriggerEnter2D()
    {
        GameObject.FindGameObjectWithTag("cam").GetComponent<CamBorderSelector>().borderCollider = this.transform.gameObject.GetComponent<Collider2D>();
    }
}
