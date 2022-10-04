using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    enum DialogueType { NPC, BOSS, CutScene }

    [SerializeField] private GameObject dialogue;
    [SerializeField] private float dialogueDelay;
    [SerializeField] private DialogueType dialogueType;
    private bool isActivated = false;
    private bool isBoss = false;
    private bool isCutScene = false;
    private bool isNPC = false;


    public void Awake()
    {
        switch (dialogueType)
        {
            case DialogueType.NPC: isNPC = true; break;
            case DialogueType.BOSS: isBoss = true; break;
            case DialogueType.CutScene: isCutScene = true; break;
        }

    }

    public void ActivateDialogue()
    {
        if (isNPC)
        {
            dialogue.SetActive(true);
        }
        else if (isCutScene)
        {
            dialogue.SetActive(true);
        }
        else if (isBoss && !isActivated)
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
        return isBoss;
    }

    public bool isThisCutScene()
    {
        return isCutScene;
    }

    private IEnumerator startBossDialogue()
    {
        yield return new WaitForSeconds(dialogueDelay);
        dialogue.SetActive(true);
    }



}
