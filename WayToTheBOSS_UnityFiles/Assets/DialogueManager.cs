using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Image image;
    [SerializeField] private Animator dialogueAnimation;
    private float sentenceTimer = 4f;
    private float sentenceTimerCounter;
    private bool isDialogue;
    private Queue<string> sentences;

    void Start()
    {
        sentences = new Queue<string>();
        sentenceTimerCounter = sentenceTimer;
        isDialogue = false;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        isDialogue = true;
        dialogueAnimation.SetBool("isShow", true);
        nameText.text = dialogue.npcName;
        image.sprite = dialogue.npcSprite;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void Update()
    {
        if (isDialogue)
        {
            sentenceTimer -= Time.deltaTime;
            if (sentenceTimer <= 0)
            {
                sentenceTimer = sentenceTimerCounter;
                DisplayNextSentence();
            }
        }
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();

        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        isDialogue = false;
        dialogueAnimation.SetBool("isShow", false);
        //Debug.Log("end dialogue");
    }
}
