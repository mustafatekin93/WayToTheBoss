using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu_Button : MonoBehaviour
{
    [SerializeField] private Sprite[] buttonSprites;
    [SerializeField] private Image buttonImage;
    [SerializeField] private TMP_Text buttonText;
    bool done = false;

    public void OnMouseOver()
    {
        buttonImage.color = new Color(0.75f, 1f, 0.75f);
        if (Input.GetButtonDown("MouseClickLeft") && !done)
        {
            done = true;
            buttonImage.sprite = buttonSprites[1];
            buttonText.fontSize *= 0.9f;
            StartCoroutine(backNormal());
        }

        IEnumerator backNormal()
        {
            yield return new WaitForSeconds(0.05f);
            buttonText.fontSize *= 1.1f;
            buttonImage.sprite = buttonSprites[0];
            done = false;
        }
    }

    public void OnMouseExit()
    {
        buttonImage.sprite = buttonSprites[0];
        buttonImage.color = Color.white;
    }


}
