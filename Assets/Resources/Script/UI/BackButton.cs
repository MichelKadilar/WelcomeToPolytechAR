using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;
using Unity.VisualScripting;

public class BackButton : MonoBehaviour
{
    public XROrigin xrOrigin;
    public GameObject backButton;
    public GameObject mainMenuPanel;
    public SearchForm panelSearch;
    public GameObject panelSearch1;
    public GameObject panelSearch2;
    public GameObject panelRoom;
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
            Debug.Log("XROrigin trouv� : " + xrOrigin.name);
            Component[] components = xrOrigin.GetComponents<Component>();
            foreach (Component component in components)
            {
                if (component is MonoBehaviour script)
                {
                    script.enabled = false;
                    Debug.Log("Script d�sactiv� : " + script.GetType().Name);
                }
                else
                {
                    Debug.Log("Composant non script trouv� : " + component.GetType().Name);
                }
            }
        }
        else
        {
            Debug.LogWarning("Aucun XROrigin trouv� dans la sc�ne !");
        }
    }

    private void EnableXRComponents()
    {
        if (xrOrigin != null)
        {
            Debug.Log("XROrigin trouv� : " + xrOrigin.name);
            Component[] components = xrOrigin.GetComponents<Component>();
            foreach (Component component in components)
            {
                if (component is MonoBehaviour script)
                {
                    script.enabled = true;
                    Debug.Log("Script activé : " + script.GetType().Name);
                }
                else
                {
                    Debug.Log("Composant non script trouv� : " + component.GetType().Name);
                }
            }
        }
        else
        {
            Debug.LogWarning("Aucun XROrigin trouv� dans la sc�ne !");
        }
    }

    private void backButtonActivation()
    {
        if (IsActive(mainMenuPanel)) {
            backButton.SetActive(false);
        }
        else
        {
            backButton.SetActive(true);
        }
    }

    public void goBack()
    {
        if (IsActive(panelSearch2))
        {
            mainMenuPanel.SetActive(false);
            panelSearch.gameObject.SetActive(true);
            panelSearch1.SetActive(true);
            panelSearch2.SetActive(false);
            panelRoom.SetActive(false);
            panelAnalyze.SetActive(false);
            DisableXRComponents();
            panelSearch.ReturnToForm();
        }
        else if (IsActive(panelRoom))
        {
            mainMenuPanel.SetActive(false);
            panelSearch.gameObject.SetActive(false);
            panelSearch1.SetActive(false);
            panelSearch2.SetActive(false);
            panelRoom.SetActive(false);
            panelAnalyze.SetActive(false);
            EnableXRComponents();
        }
        else
        {
            mainMenuPanel.SetActive(true);
            panelSearch.gameObject.SetActive(false);
            panelSearch1.SetActive(false);
            panelSearch2.SetActive(false);
            panelRoom.SetActive(false);
            panelAnalyze.SetActive(false);
            DisableXRComponents();
        }
    }




}
