using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuUI : MonoBehaviour
{

    public SoundPlayer soundPlayerForHover;

    private Camera mainCamera;

    public GameObject panel; // Assignation des 3 panels dans l'inspecteur Unity
    public GameObject panelSearch;
    public GameObject panelAnalyze;

    private MonoBehaviour ARCameraManagerScriptComponent;
    private MonoBehaviour ARCameraBackgroundScriptComponent;
    private UnityEngine.InputSystem.XR.TrackedPoseDriver trackedPoseDriverComponent;

    private Button[] buttons;
    private Color originalColor;
    public Color hoverColor = Color.gray;

    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera != null)
        {
            var cameraManager = mainCamera.GetComponent("ARCameraManager");
            if (cameraManager != null)
            {
                ARCameraManagerScriptComponent = (MonoBehaviour) cameraManager;
                ARCameraManagerScriptComponent.enabled = false;
            }

            var cameraBackground = mainCamera.GetComponent("ARCameraBackground");
            if (cameraBackground != null)
            {
                ARCameraBackgroundScriptComponent = (MonoBehaviour) cameraBackground;
                ARCameraBackgroundScriptComponent.enabled = false;
            }

            var trackedPoseDriver = mainCamera.GetComponent<UnityEngine.InputSystem.XR.TrackedPoseDriver>();
            if (trackedPoseDriver != null)
            {
                trackedPoseDriverComponent = trackedPoseDriver;
                trackedPoseDriverComponent.enabled = false;
            }
        }
        else
        {
            Debug.LogError("Pas de main camera dans la scène");
        }

        panel.SetActive(true);
        panelSearch.SetActive(false);
        panelAnalyze.SetActive(false);

        buttons = panel.GetComponentsInChildren<Button>();


        if (buttons == null || buttons.Length == 0)
        {
            Debug.LogError("Buttons not found in the menu panel!");
            return;
        }

        Debug.Log("Buttons count: " + buttons.Length);

        // Add listeners to buttons
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i; // Capture index in local scope

            // Attach hover event listeners
            EventTrigger trigger = buttons[i].gameObject.AddComponent<EventTrigger>();

            // Add OnPointerEnter (hover)
            EventTrigger.Entry entryEnter = new EventTrigger.Entry();
            entryEnter.eventID = EventTriggerType.PointerEnter;
            entryEnter.callback.AddListener((eventData) => OnPointerEnter(buttons[index]));
            trigger.triggers.Add(entryEnter);

            // Add OnPointerExit (hover)
            EventTrigger.Entry entryExit = new EventTrigger.Entry();
            entryExit.eventID = EventTriggerType.PointerExit;
            entryExit.callback.AddListener((eventData) => OnPointerExit(buttons[index]));
            trigger.triggers.Add(entryExit);
        }
    }

    void Update()
    {
        
    }

    public void searchButtonPressed()
    {
        panel.SetActive(false);
        panelSearch.SetActive(true);
        panelAnalyze.SetActive(false);
        AnimateButton(0);
    }

    public void scanButtonPressed()
    {
        panel.SetActive(false);
        panelSearch.SetActive(false);
        panelAnalyze.SetActive(false);
        ARCameraManagerScriptComponent.enabled = true;
        ARCameraBackgroundScriptComponent.enabled = true;
        trackedPoseDriverComponent.enabled = true;
        AnimateButton(1);
    }

    public void analyzeButtonPressed()
    {
        panel.SetActive(false);
        panelSearch.SetActive(false);
        panelAnalyze.SetActive(true);
        AnimateButton(2);
    }

    private void OnPointerEnter(Button button)
    {
        soundPlayerForHover.PlaySound();
        var backgroundImage = button.GetComponent<Image>();
        originalColor = backgroundImage.color;
        backgroundImage.color = hoverColor;
    }

    // Reset color when pointer exits the button
    private void OnPointerExit(Button button)
    {
        var backgroundImage = button.GetComponent<Image>();
        backgroundImage.color = originalColor;
    }

    private void AnimateButton(int buttonIndex)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            // Reset all button backgrounds to default (white in this example)
            var backgroundImage = buttons[i].GetComponent<Image>();
            backgroundImage.color = Color.white;
        }

        // Change the clicked button background to green
        var clickedButtonBackground = buttons[buttonIndex].GetComponent<Image>();
        clickedButtonBackground.color = Color.green;
    }
}
