using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    

    bool isMainMenu = false;

    private void Awake()
    {
        Scene currentScene = SceneManager.GetActiveScene();
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

        if (currentScene.name == "MainMenu")
        {
            isMainMenu = true;
        }

        if (isMainMenu)
        {
            Play("MainMenuSong");
        }
        else
        {
            Play("ThemeSong");
            Play("BackgroundNoise");
        }
    }

    private void Start()
    {
         
    }

    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogError("Sound called " + name + " wasn't found!");
        }

        s.source.Play();
    }
}
