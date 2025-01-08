using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioClip soundClip;

    // Play the sound when this method is called
    public void PlaySound()
    {
        // If soundClip is set, assign it to the AudioSource
        if (soundClip != null)
        {
            audioSource.clip = soundClip;
        }

        // Play the sound
        audioSource.Play();
    }

    public GameObject panel; // Assign the Panel in the Inspector
    public GameObject panelSearch; // Assign the Panel in the Inspector
    public GameObject panelAnalyze; // Assign the Panel in the Inspector

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void scanButtonPressed()
    {
        panel.SetActive(false);
        panelSearch.SetActive(false);
        panelAnalyze.SetActive(false);
    }

    public void searchButtonPressed()
    {
        panel.SetActive(false);
        panelSearch.SetActive(true);
        panelAnalyze.SetActive(false);
    }


    public void analyzeButtonPressed()
    {
        panel.SetActive(false);
        panelSearch.SetActive(false);
        panelAnalyze.SetActive(true);
    }
}
