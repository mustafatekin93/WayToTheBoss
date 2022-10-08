using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorEnter_Boss : MonoBehaviour
{
    [SerializeField] private Animator fadeAnimator;
    [SerializeField] private Transform exitPosition;
    [SerializeField] private ParticleSystem unlockEffect;
    [SerializeField] private GameObject costImage;
    [SerializeField] private bool isUnlocked = false;
    [SerializeField] private int doorCost;

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.transform.tag == "Player" && Input.GetKeyDown(KeyCode.UpArrow))
        {
            PlayerControl playerControl = col.transform.gameObject.GetComponent<PlayerControl>();
            if (isUnlocked == false)
            {
                if (playerControl.getSkullCount() >= doorCost)
                {
                    isUnlocked = true;

                    if (costImage != null)
                        costImage.SetActive(false);
                    if (unlockEffect != null)
                        unlockEffect.Play(true);
                }

            }
            else if (isUnlocked == true)
            {
                StartCoroutine(changePosition());
            }


            IEnumerator changePosition()
            {
                fadeAnimator.SetTrigger("isStart");
                playerControl.stopMove();
                playerControl.enabled = false;
                yield return new WaitForSeconds(0.5f);
                col.transform.position = exitPosition.position;
                yield return new WaitForSeconds(0.5f);
                fadeAnimator.SetTrigger("isEnd");
                playerControl.enabled = true;
            }
        }
    }
}
