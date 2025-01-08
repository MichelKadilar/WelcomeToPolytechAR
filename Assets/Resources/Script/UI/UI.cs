using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    private Camera mainCamera;

    public GameObject panel; // Assignation des 3 panels dans l'inspecteur Unity
    public GameObject panelSearch;
    public GameObject panelAnalyze;

    private MonoBehaviour ARCameraManagerScriptComponent;
    private MonoBehaviour ARCameraBackgroundScriptComponent;
    private UnityEngine.InputSystem.XR.TrackedPoseDriver trackedPoseDriverComponent;

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
    }

    void Update()
    {
        
    }

    public void searchButtonPressed()
    {
        panel.SetActive(false);
        panelSearch.SetActive(true);
        panelAnalyze.SetActive(false);
    }

    public void scanButtonPressed()
    {
        panel.SetActive(false);
        panelSearch.SetActive(false);
        panelAnalyze.SetActive(false);
        ARCameraManagerScriptComponent.enabled = true;
        ARCameraBackgroundScriptComponent.enabled = true;
        trackedPoseDriverComponent.enabled = false;
    }

    public void analyzeButtonPressed()
    {
        panel.SetActive(false);
        panelSearch.SetActive(false);
        panelAnalyze.SetActive(true);
    }
}
