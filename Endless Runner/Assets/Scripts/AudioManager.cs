using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    void Start()
    {
        foreach (var s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.isLooped;
            s.source.name = s.name;
            s.source.volume = s.volume;
        }

        PlaySound("MainTheme");
    }

    public void PlaySound(string name)
    {
        sounds.ToList()
              .Find(x => x.name.Equals(name))
              .source.Play();
    }

    public void ChangeMainThemeVolume(float newVolume)
    { //dlatego bo tutaj jest wywolywane maintheme, DO ZMIANY
        gameObject.GetComponent<AudioSource>().volume = newVolume;
    }

    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime, float endLevel)
    {
        float startVolume = audioSource.volume;
        while (audioSource.volume > endLevel)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }

        if (endLevel == 0.0f)
        {
            audioSource.Stop();
        }
    }
}
