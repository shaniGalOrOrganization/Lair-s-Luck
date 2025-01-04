using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicController : MonoBehaviour
{
    #region Variables

    [SerializeField] Slider musicSlider;
    [SerializeField] AudioMixer musicMixser;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        // check for exist value of music slider
        if (PlayerPrefs.HasKey("SavedMusicVolume"))
        {
            LoadVolume();
            Slider_Music();
        }
        else
        {
            SetVolume(PlayerPrefs.GetFloat("SavedMusicVolume", 100));
        }
    }

    #endregion

    #region Logic

    public void SetVolume(float volume)
    {
        if(volume < 1)
            volume = .001f;

        PlayerPrefs.SetFloat("SavedMusicVolume", volume);
        musicMixser.SetFloat("MasterVolume", Mathf.Log10(volume / 100) * 20f);

        // save the value of the slider
        PlayerPrefs.SetFloat("SavedMusicVolume", volume);
    }

    // method to save the value of the slider
    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("SavedMusicVolume");
        SetVolume(musicSlider.value);
    }

    public void SetVolumeFromSlider()
    {
        SetVolume(musicSlider.value);
    }

    public void Slider_Music()
    {
        GameObject.Find("Text_MusicValue").GetComponent<TextMeshProUGUI>().text = musicSlider.GetComponent<Slider>().value.ToString();
    }

    #endregion
}
