using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.XR.CoreUtils;

public class MainMenuUI : MonoBehaviour
{
    public SoundPlayer soundPlayerForHover;
    public XROrigin xrOrigin;
    public Camera menuCamera;
    public GameObject cameraOffset;

    public GameObject panel;
    public GameObject panelSearch;
    public GameObject panelSearch1;
    public GameObject panelSearch2;
    public GameObject panelAnalyze;

    private MonoBehaviour ARCameraManagerScriptComponent;
    private MonoBehaviour ARCameraBackgroundScriptComponent;
    private UnityEngine.InputSystem.XR.TrackedPoseDriver trackedPoseDriverComponent;

    private Button[] buttons;
    private Color originalColor;
    public Color hoverColor = Color.gray;
    public float animationDuration = 0.5f; // Durée de l'animation en secondes

    void Start()
    {
        if (xrOrigin != null)
        {
            Debug.Log("XROrigin trouvé : " + xrOrigin.name);

            Component[] components = xrOrigin.GetComponents<Component>();

            foreach (Component component in components)
            {
                if (component is MonoBehaviour script)
                {
                    script.enabled = false;
                    Debug.Log("Script désactivé : " + script.GetType().Name);
                }
                else
                {
                    Debug.Log("Composant non script trouvé : " + component.GetType().Name);
                }
            }
        }
        else
        {
            Debug.LogWarning("Aucun XROrigin trouvé dans la scène !");
        }

        if (menuCamera != null)
        {
            menuCamera.enabled = true;
        }
        else
        {
            Debug.LogError("Pas de menu camera dans la scène");
        }

        cameraOffset.SetActive(false);
        panel.SetActive(true);
        panelSearch.SetActive(false);
        panelSearch1.SetActive(false);
        panelSearch2.SetActive(false);
        panelAnalyze.SetActive(false);

        buttons = panel.GetComponentsInChildren<Button>();

        if (buttons == null || buttons.Length == 0)
        {
            Debug.LogError("Buttons not found in the menu panel!");
            return;
        }

        Debug.Log("Buttons count: " + buttons.Length);

        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;

            EventTrigger trigger = buttons[i].gameObject.AddComponent<EventTrigger>();

            EventTrigger.Entry entryEnter = new EventTrigger.Entry();
            entryEnter.eventID = EventTriggerType.PointerEnter;
            entryEnter.callback.AddListener((eventData) => OnPointerEnter(buttons[index]));
            trigger.triggers.Add(entryEnter);

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
        panelSearch1.SetActive(true);
        panelSearch2.SetActive(false);
        panelAnalyze.SetActive(false);
        menuCamera.enabled = true;
        cameraOffset.SetActive(false);
        StartCoroutine(AnimateButtonCoroutine(0));
    }

    public void scanButtonPressed()
    {
        panel.SetActive(false);
        panelSearch.SetActive(false);
        panelAnalyze.SetActive(false);
        menuCamera.enabled = false;
        cameraOffset.SetActive(true);
        StartCoroutine(AnimateButtonCoroutine(1));
    }

    public void analyzeButtonPressed()
    {
        panel.SetActive(false);
        panelSearch.SetActive(false);
        panelAnalyze.SetActive(true);
        menuCamera.enabled = true;
        cameraOffset.SetActive(false);
        StartCoroutine(AnimateButtonCoroutine(2));
    }

    private void OnPointerEnter(Button button)
    {
        soundPlayerForHover.PlaySound();
        var backgroundImage = button.GetComponent<Image>();
        originalColor = backgroundImage.color;
        backgroundImage.color = hoverColor;
    }

    private void OnPointerExit(Button button)
    {
        var backgroundImage = button.GetComponent<Image>();
        backgroundImage.color = originalColor;
    }

    private IEnumerator AnimateButtonCoroutine(int buttonIndex)
    {
        // Réinitialiser tous les boutons à blanc
        for (int i = 0; i < buttons.Length; i++)
        {
            var backgroundImage = buttons[i].GetComponent<Image>();
            backgroundImage.color = Color.white;
        }

        // Changer la couleur du bouton cliqué en gris
        var clickedButtonBackground = buttons[buttonIndex].GetComponent<Image>();
        clickedButtonBackground.color = Color.grey;

        // Attendre la durée de l'animation
        yield return new WaitForSeconds(animationDuration);

        // Remettre le bouton en blanc
        clickedButtonBackground.color = Color.white;
    }
}