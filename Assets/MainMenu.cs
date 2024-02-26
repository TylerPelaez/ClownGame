using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    [Header("Volume Setting")]
    [SerializeField] TMP_Text volumeTextValue = null;
    [SerializeField] Slider volumeSlider = null;

    public void Start()
    {
        // Find the EventSystem in the scene
        EventSystem eventSystem = FindObjectOfType<EventSystem>();

        // If EventSystem exists, set it as the current event system
        if (eventSystem != null)
        {
            EventSystem.current = eventSystem;
        }
        else
        {
            Debug.LogWarning("No EventSystem found in the scene.");
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("Quitted game.");
        Application.Quit();
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0.0");
        volumeSlider.value = volume;
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
    }
}
