using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorEndGame : MonoBehaviour
{
    [SerializeField] private Animator fadeAnimator;
    [SerializeField] private GameObject costImage;
    [SerializeField] private bool isUnlocked = false;
    [SerializeField] private int doorCost;

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.transform.tag == "Player" && Input.GetKeyDown(KeyCode.UpArrow))
        {
            PlayerControl playerControl = col.transform.gameObject.GetComponent<PlayerControl>();
            StartCoroutine(changeScene());


            IEnumerator changeScene()
            {
                fadeAnimator.SetTrigger("isStart");
                playerControl.stopMove();
                playerControl.enabled = false;
                yield return new WaitForSeconds(0.5f);
                SceneManager.LoadScene("gameEnding");
            }
        }
    }
}
