using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    // Start is called before the first frame update
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
}
