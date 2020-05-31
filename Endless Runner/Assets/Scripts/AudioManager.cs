using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    private static bool isSoundTurnedOn = true;
    public Button soundOnOffBtn;
    public Sprite soundOnImg;
    public Sprite soundOffImg;

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

        if (!isSoundTurnedOn)
        {
            PauseAudio();
        }           
        else
        {
            PlaySound("MainTheme");
        }
    }

    public void PlaySound(string name)
    {
        if (isSoundTurnedOn)
        {
            sounds.ToList()
                  .Find(x => x.name.Equals(name))
                  .source.Play();
        }
    }

    public void ChangeMainThemeVolume(float newVolume)
    {
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

    private void PauseAudio()
    {
        gameObject.GetComponent<AudioSource>().Pause();
        this.soundOnOffBtn.GetComponent<Image>().sprite = this.soundOffImg;
    }

    private void ResumeAudio()
    {
        gameObject.GetComponent<AudioSource>().Play();
        this.soundOnOffBtn.GetComponent<Image>().sprite = this.soundOnImg;
    }

    public void ToggleSoundPlaying()
    {
        if (isSoundTurnedOn)
        {
            PauseAudio();           
        } 
        else
        {
            ResumeAudio();
        }

        isSoundTurnedOn = !isSoundTurnedOn;
    }
}
