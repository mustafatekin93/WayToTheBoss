using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    [SerializeField] private Slider MusicSlider;
    [SerializeField] private Slider SFX_Slider;
    [SerializeField] private AudioSource MusicAudio;
    [SerializeField] private AudioSource SFX_Audio;

    void Update()
    {
        PlayerPrefs.SetFloat("MusicVolume", MusicSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", SFX_Slider.value);



        if (MusicAudio != null)
            MusicAudio.volume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);

        if (SFX_Audio != null)
            SFX_Audio.volume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
    }

    void Start()
    {
        MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        SFX_Slider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
    }
}
