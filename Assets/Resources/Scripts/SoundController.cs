using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class SoundController : MonoBehaviour
{
    #region Variables

    [SerializeField] private TextMeshProUGUI sfxValue;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private AudioSource SFXAudioSource;

    public AudioClip flipcard;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        // check for exist value of music slider
        if (PlayerPrefs.HasKey("SavedSFXVolume"))
        {
            LoadVolume();

        }
        else
        {
            SetVolume(PlayerPrefs.GetFloat("SavedSFXVolume", 100));
        }
        Slider_SFX();
    }

    #endregion

    #region Logic

    public void SetVolume(float volume)
    {
        if (volume < 1)
            volume = .001f;

        PlayerPrefs.SetFloat("SavedSFXVolume", volume);
       // musicMixser.SetFloat("MasterVolume", Mathf.Log10(volume / 100) * 20f);

    }

    private void LoadVolume()
    {
        sfxSlider.value = PlayerPrefs.GetFloat("SavedSFXVolume");
        SetVolume(sfxSlider.value);
    }

    public void Slider_SFX()
    {
        sfxValue.text = sfxSlider.GetComponent<Slider>().value.ToString();
    }

    public void flipCard()
    {
        SFXAudioSource.clip = flipcard;
        SFXAudioSource.Play();
    }
    #endregion
}
