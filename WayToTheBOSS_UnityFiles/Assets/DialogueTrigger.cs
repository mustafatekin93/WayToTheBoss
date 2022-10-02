using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private Dialogue dialogue;
    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}

[System.Serializable]
public class Dialogue
{
    public Sprite npcSprite;
    public string npcName;

    [TextArea(3, 10)]
    public string[] sentences;
}
