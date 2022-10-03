using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private GameObject dialogue;
    [SerializeField] private bool isBossArea;
    private bool isActivated = false;

    public void ActivateDialogue()
    {
        if (!isBossArea)
        {
            dialogue.SetActive(true);
        }
        else if (isBossArea && !isActivated)
        {
            StartCoroutine(startBossDialogue());
            isActivated = true;
        }
    }

    public bool DialogueActive()
    {
        return dialogue.activeInHierarchy;
    }

    public bool isThisBossArea()
    {
        return isBossArea;
    }

    private IEnumerator startBossDialogue()
    {
        yield return new WaitForSeconds(2f);
        dialogue.SetActive(true);
    }

}
