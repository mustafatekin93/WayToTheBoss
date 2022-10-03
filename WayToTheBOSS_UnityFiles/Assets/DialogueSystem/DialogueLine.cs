using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DialogueSystem
{
    public class DialogueLine : DialogueBaseClass
    {
        private TMP_Text textHolder;
        private Image characterImage;
        [SerializeField][TextArea(2, 8)] private string dialogueText;
        [SerializeField] private float delay;
        [SerializeField] private AudioClip sound;
        [SerializeField] private Sprite characterSprite;

        private void Start()
        {
            StartCoroutine(WriteText(dialogueText, textHolder, delay, sound));
        }

        private void Awake()
        {
            characterImage = GetComponentInChildren<Image>();
            characterImage.sprite = characterSprite;
            characterImage.preserveAspect = true;
            textHolder = GetComponent<TMP_Text>();
            textHolder.text = "";


        }
    }
}
