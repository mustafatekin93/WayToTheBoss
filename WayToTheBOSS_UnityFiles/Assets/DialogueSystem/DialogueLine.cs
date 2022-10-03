using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DialogueSystem
{
    public class DialogueLine : DialogueBaseClass
    {
        private bool delayControl = false;
        private TMP_Text textHolder;
        private Image characterImage;
        [SerializeField][TextArea(2, 8)] private string dialogueText;
        [SerializeField] private float delay;
        [SerializeField] private AudioClip sound;
        [SerializeField] private Sprite characterSprite;

        private IEnumerator lineAppear;

        private void OnEnable()
        {
            ResetLine();
            lineAppear = WriteText(dialogueText, textHolder, delay, sound);
            StartCoroutine(lineAppear);
            StartCoroutine(DelayControl());
        }

        private void Awake()
        {
            characterImage = GetComponentInChildren<Image>();
            characterImage.sprite = characterSprite;
            characterImage.preserveAspect = true;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z) && delayControl)
            {
                if (textHolder.text != dialogueText)
                {
                    StopCoroutine(lineAppear);
                    textHolder.text = dialogueText;
                }
                else
                    finished = true;
            }
        }

        private void ResetLine()
        {
            textHolder = GetComponent<TMP_Text>();
            textHolder.text = "";
            finished = false;
            delayControl = false;
        }

        private IEnumerator DelayControl()
        {
            yield return new WaitForSeconds(0.1f);
            delayControl = true;
        }

    }
}
