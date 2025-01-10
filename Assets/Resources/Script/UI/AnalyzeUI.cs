using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalyzeUI : MonoBehaviour
{
    public GameObject contentArea; // Content GameObject inside of ScrollView
    public GameObject namePrefab; // Prefab to display text inside

    private GameObject selectedObjectPrefab; // To store the selected object prefab

    private string[] textsToDisplay = new string[]
   {
        "Texte 1",
        "Texte 2",
        "Texte 3",
        "Texte 1",
        "Texte 2",
        "Texte 3",
        "Texte 1",
        "Texte 2",
        "Texte 3",
        "Texte 1",
        "Texte 2",
        "Texte 3",
        "Texte 1",
        "Texte 2",
        "Texte 3",
        "Texte 1",
        "Texte 2",
        "Texte 3",
        "Texte 1",
        "Texte 2",
        "Texte 1",
        "Texte 2",
        "Texte 1",
        "Texte 2",
        "Texte 3",
        "Texte 3",
        "Texte 3",
   };

    // Start is called before the first frame update
    void Start()
    {
        foreach (string text in textsToDisplay)
        {
            // Instancier le prefab
            GameObject newTMP = Instantiate(namePrefab, contentArea.transform);

            // Récupérer le composant TextMeshPro et définir son texte
            TMPro.TextMeshProUGUI tmpText = newTMP.GetComponent<TMPro.TextMeshProUGUI>();
            if (tmpText != null)
            {
                tmpText.text = text;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
