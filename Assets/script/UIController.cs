using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIcontroller : MonoBehaviour
{
    public Slider _musicSlider, _sfxSlider;
   

    public void ToggleMusic()

    {
        AudioManager.Instance.ToggleClickedSound();
    }
    public void ToggleSFX()
    {
        AudioManager.Instance.ToggleSFX();
    }
    public void HoverSound()

    {
        AudioManager.Instance.ToggleClickedSound();
    }
    public void ClickedSound()

    {
        AudioManager.Instance.ToggleClickedSound();
    }
    public void MusicVolume()
    {
        AudioManager.Instance.MusicVolume(_musicSlider.value);
    }
    public void SFXVolume()
    {
        AudioManager.Instance.SFXVolume(_sfxSlider.value);
    }
    
    public void HoverSoundVolume()
    {
        AudioManager.Instance.HoverSoundVolume(_sfxSlider.value);
    }
    public void ClickedSoundVolume()
    {
        AudioManager.Instance.HoverSoundVolume(_sfxSlider.value);
    }
}

