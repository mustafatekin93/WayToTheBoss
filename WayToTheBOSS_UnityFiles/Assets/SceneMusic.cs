using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMusic : MonoBehaviour
{
    [SerializeField] private AudioClip startingMusic;

    public void Start()
    {
        SoundManager.instance.PlaySound(startingMusic);
    }
}
