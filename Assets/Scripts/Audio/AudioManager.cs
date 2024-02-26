using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    private GameObject player;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        Play("ThemeSong");
        Play("BackgroundNoise");
        player = GameObject.FindWithTag("Player");
    }

    public void Play (string name, Vector3? soundOrigin = null)
    {
        if (soundOrigin != null)
        {
            // Prevent Pie cannons from spamming fart noises
            if ((soundOrigin.Value - player.transform.position).magnitude > 20)
            {
                return;
            }
        }
        
        
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogError("Sound called " + name + " wasn't found!");
        }

        s.source.Play();
    }
}
