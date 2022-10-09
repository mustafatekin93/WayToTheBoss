using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    [SerializeField] private Animator fadeAnimator;
    [SerializeField] private float waitTime;

    public void startAnimation()
    {
        StartCoroutine(LoadLevel());
        IEnumerator LoadLevel()
        {
            fadeAnimator.SetTrigger("isStart");
            yield return new WaitForSeconds(waitTime);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void restartAnimation()
    {
        StartCoroutine(reloadLevel());
        IEnumerator reloadLevel()
        {
            fadeAnimator.SetTrigger("isStart");
            yield return new WaitForSeconds(waitTime);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void startFadeInAndOut()
    {

        StartCoroutine(_startFadeInAndOut());

        IEnumerator _startFadeInAndOut()
        {
            fadeAnimator.SetTrigger("isStart");
            yield return new WaitForSeconds(waitTime);
            fadeAnimator.SetTrigger("isEnd");
        }
    }



}
