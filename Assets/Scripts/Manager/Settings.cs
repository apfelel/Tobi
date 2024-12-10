using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private Slider _master, _sfx, _music, _ambience;

    [SerializeField] private AudioMixer _mainMixer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _master.value = PlayerPrefs.GetFloat("MasterVolume", 0.5f);
        OnMasterSliderChanged(_master.value);
        _sfx.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        OnSFXSliderChanged(_sfx.value);
        _music.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        OnMusicSliderChanged(_music.value);
        _ambience.value = PlayerPrefs.GetFloat("AmbienceVolume", 0.5f);
        OnAmbienceSliderChanged(_ambience.value);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat("MasterVolume", _master.value);
        PlayerPrefs.SetFloat("SFXVolume", _sfx.value);
        PlayerPrefs.SetFloat("MusicVolume", _music.value);
        PlayerPrefs.SetFloat("AmbienceVolume", _ambience.value);
    }

    public void OnMasterSliderChanged(float value)
    {
        _mainMixer.SetFloat("Master", Mathf.Log10(value) * 20);
    }
    public void OnSFXSliderChanged(float value)
    {
        _mainMixer.SetFloat("SFX", Mathf.Log10(value) * 20);
    }
    public void OnMusicSliderChanged(float value)
    {
        _mainMixer.SetFloat("Music", Mathf.Log10(value) * 20);
    }
    public void OnAmbienceSliderChanged(float value)
    {
        _mainMixer.SetFloat("Ambience", Mathf.Log10(value) * 20);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
