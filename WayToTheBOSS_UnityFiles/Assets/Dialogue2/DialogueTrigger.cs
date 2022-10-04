using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public enum DialogueType { NPC, CutScene }

    public DialogueType dialogueType;
    public GameObject dialogueHolder;


    void OnTriggerStay2D(Collider2D col)
    {
        if (col.transform.tag == "Player")
        {
            switch (dialogueType)
            {
                case DialogueType.NPC:
                    if (Input.GetKeyDown(KeyCode.Z))
                    { dialogueHolder.SetActive(true); }
                    break;

                case DialogueType.CutScene:
                    dialogueHolder.SetActive(true);
                    Destroy(this);
                    break;
            }
        }
    }
}
