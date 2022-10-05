using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    [Header("Dialogue Areas")]
    public DialogueLines[] dialogueLines;
    [Header("Options")]
    public AudioClip DialogueSound;
    public float textSpeed;
    public float dialogueDelay;
    public GameObject activeEndOfDialogue;

    public static bool inDialogue { get; private set; } //dialoğun aktif olup olmadığını görmek için static olarak kullanıldı, player ve enemy scriptlerinde eğer dialog varsa, hareket ettirilmeyecek

    private int index;
    private TMP_Text textComponent;
    private Image spriteComponent;

    // Aktif olduğunda komponentleri bulma
    void OnEnable()
    {
        textComponent = GetComponentInChildren<TMP_Text>();
        spriteComponent = GameObject.Find("CharacterSprite").GetComponentInChildren<Image>();
        textComponent.text = "";
        StartDialogue();
    }

    //Z tuşuna basarak dialogtaki cümleleri geçme
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (textComponent.text == dialogueLines[index].DialogueLine)
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = dialogueLines[index].DialogueLine;
            }
        }
    }

    //Dialog başlatıcı
    void StartDialogue()
    {
        inDialogue = true;
        index = 0;
        StartCoroutine(TypeLine());
    }

    //Dialogta sonraki cümleye geçme
    void NextLine()
    {
        if (index < dialogueLines.Length - 1)
        {
            index++;
            StartCoroutine(TypeLine());
        }
        else
        {
            if (activeEndOfDialogue != null)
            {
                activeEndOfDialogue.SetActive(true);
            }
            inDialogue = false;
            gameObject.SetActive(false);
        }
    }

    //Harflerin tek tek yazılması
    IEnumerator TypeLine()
    {
        textComponent.text = "";
        spriteComponent.sprite = dialogueLines[index].CharacterSprite;
        foreach (char c in dialogueLines[index].DialogueLine.ToCharArray())
        {
            textComponent.text += c;
            SoundManager.instance.PlaySound(DialogueSound);
            yield return new WaitForSeconds(textSpeed);
        }
    }

    //Dialog sprite ve yazıları için basic class
    [System.Serializable]
    public class DialogueLines
    {
        public Sprite CharacterSprite;
        [TextArea(3, 10)] public string DialogueLine;
    }
}

