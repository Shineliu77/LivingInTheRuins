using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;
    public AudioClip ClickedSound;
    public AudioClip HoverSound;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic("HomeStartMouse");
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Music sound not found.");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    internal void HoverSoundVolume(float value)
    {
        throw new NotImplementedException();
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("SFX sound not found.");
        }
        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }

    public void PlayMusic(int index) //調用的音樂撥放
    {
        if (index == -1)  //無音樂
        {
            musicSource.Stop();
            musicSource.clip = null;
            return;
        }
        if (index < 0 || index >= musicSounds.Length)
        {
            Debug.Log("音樂長度不夠");
            return;
        }
        AudioClip nextClip = musicSounds[index].clip;
        if (musicSource.clip == nextClip && musicSource.isPlaying)
            musicSource.Stop();
        musicSource.clip = nextClip;
        musicSource.Play();
        //Sound MusicSound = musicSounds[index];
        //musicSource.PlayOneShot(MusicSound.clip);



    }
    public void PlaySfx(int index)  //調用的音效撥放
    {
        if (index > 0 || index >= sfxSounds.Length)
        {
            Debug.Log("音效陣列長度不夠");
        }
        Sound sfxSound = sfxSounds[index];
        sfxSource.PlayOneShot(sfxSound.clip);
    }

    public void ToggleClickedSound()
    {
        musicSource.mute = !musicSource.mute;
    }

    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = Mathf.Clamp01(volume);
    }

    public void SFXVolume(float volume)
    {
        sfxSource.volume = Mathf.Clamp01(volume);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (HoverSound != null)
        {
            sfxSource.PlayOneShot(HoverSound);
        }
    }

    public void PlayClickSound()
    {
        if (ClickedSound != null)
        {
            sfxSource.PlayOneShot(ClickedSound);
        }
    }
}
