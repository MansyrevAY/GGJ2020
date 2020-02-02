using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    [HideInInspector]
    public Sound currentBackgroundMusic;

    void Awake()
    {
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume;
            sound.source.loop = sound.loop;
        }

        currentBackgroundMusic = null;
    }

    void Start()
    {
        Play("Music");
        Play("Engines");
    }

    public void SetBackgroundMusicAndPlayIt(string trackName)
    {
        Sound s = Array.Find(sounds, sound => sound.name.Equals(trackName));
        if (s == null)
            return;
        currentBackgroundMusic = s;
        currentBackgroundMusic.volume = s.volume;
        Debug.Log($"Now playing: {currentBackgroundMusic.name}");
        currentBackgroundMusic.source.Play();
    }

    public void Play(string trackName)
    {
        Sound s = Array.Find(sounds, sound => sound.name.Equals(trackName));
        s?.source.Play();
    }
    public void PlayLoop(string trackName)
    {
        Sound s = Array.Find(sounds, sound => sound.name.Equals(trackName));
        if (s.source != null)
        {
            if (!s.source.isPlaying)
            {
                s?.source.Play();
            }
        }
    }

    public void StopLoop(string trackName)
    {
        Sound s = Array.Find(sounds, sound => sound.name.Equals(trackName));
        s?.source.Stop();
    }

    public static class FadeAudioSource
    {

        public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
        {
            float currentTime = 0;
            float start = audioSource.volume;

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
                yield return null;
            }
            
            yield break;
        }
    }

    public void FinishAudioSource()
    {
        currentBackgroundMusic.source.Stop();
        currentBackgroundMusic.source.volume = currentBackgroundMusic.volume;
    }
}