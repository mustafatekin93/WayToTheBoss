using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DialogueSystem
{
    public class DialogueBaseClass : MonoBehaviour
    {
        public bool finished { get; protected set; }

        protected IEnumerator WriteText(string input, TMP_Text textHolder, float delay, AudioClip sound)
        {
            for (int i = 0; i < input.Length; i++)
            {
                textHolder.text += input[i];
                SoundManager.instance.PlaySound(sound);
                yield return new WaitForSeconds(delay);
            }
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.C));
            finished = true;
        }
    }
}