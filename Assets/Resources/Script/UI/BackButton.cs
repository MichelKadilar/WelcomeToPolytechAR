using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;

public class BackButton : MonoBehaviour
{
    public XROrigin xrOrigin;
    public GameObject backButton;
    public GameObject mainMenuPanel;
    public GameObject panelSearch;
    public GameObject panelSearch1;
    public GameObject panelSearch2;
    public GameObject panelAnalyze;

    // Start is called before the first frame update
    void Start()
    {
        backButtonActivation();
    }

    // Update is called once per frame
    void Update()
    {
        backButtonActivation();
    }

    private bool IsActive(GameObject targetObject)
    {
        return targetObject != null && targetObject.activeSelf;
    }

    private void DisableXRComponents()
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
    }

    private void backButtonActivation()
    {
        if (IsActive(mainMenuPanel)) {
            backButton.SetActive(false);
        }
        else if (IsActive(panelSearch))
        {
            backButton.SetActive(true);
        }
        else if (IsActive(panelAnalyze))
        {
            backButton.SetActive(true);
        }
        else
        {
            backButton.SetActive(true);
        }
    }

    public void goBack()
    {
        if (IsActive(panelSearch1)){
            mainMenuPanel.SetActive(true);
            panelSearch.SetActive(false);
            panelSearch1.SetActive(false);
            panelSearch2.SetActive(false);
            panelAnalyze.SetActive(false);
            DisableXRComponents();
        }
        else if (IsActive(panelSearch2))
        {
            mainMenuPanel.SetActive(false);
            panelSearch.SetActive(true);
            panelSearch1.SetActive(true);
            panelSearch2.SetActive(false);
            panelAnalyze.SetActive(false);
            DisableXRComponents();
        }
        else if (IsActive(panelAnalyze))
        {
            mainMenuPanel.SetActive(true);
            panelSearch.SetActive(false);
            panelSearch1.SetActive(false);
            panelSearch2.SetActive(false);
            panelAnalyze.SetActive(false);
            DisableXRComponents();
        }
        else
        {
            mainMenuPanel.SetActive(true);
            panelSearch.SetActive(false);
            panelSearch1.SetActive(false);
            panelSearch2.SetActive(false);
            panelAnalyze.SetActive(false);
            DisableXRComponents();
        }
    }




}
