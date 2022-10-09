using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameText : MonoBehaviour
{
    [SerializeField] private GameObject QuitButton;
    void Start()
    {
        StartCoroutine(ShowQuitButton());
    }
    IEnumerator ShowQuitButton()
    {
        yield return new WaitForSeconds(28);
        QuitButton.SetActive(true);
    }
    void Update()
    {
        transform.position += new Vector3(0, 2*Time.deltaTime, 0);
    }
}
