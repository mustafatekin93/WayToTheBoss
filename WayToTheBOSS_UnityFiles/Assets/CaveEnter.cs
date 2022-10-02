using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveEnter : MonoBehaviour
{
    [SerializeField] private Animator fadeAnimator;
    [SerializeField] private Transform exitPosition;

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.transform.tag == "Player" && Input.GetKeyDown(KeyCode.UpArrow))
        {
            PlayerControl playerControl = col.transform.gameObject.GetComponent<PlayerControl>();
            StartCoroutine(changePosition());
            IEnumerator changePosition()
            {
                fadeAnimator.SetTrigger("isStart");
                playerControl.stopMove();
                playerControl.enabled = false;
                yield return new WaitForSeconds(1.5f);
                col.transform.position = exitPosition.position;
                yield return new WaitForSeconds(0.75f);
                fadeAnimator.SetTrigger("isEnd");
                playerControl.enabled = true;
            }
        }
    }
}
