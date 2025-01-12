using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalyzeUI : MonoBehaviour
{
    public GameObject contentArea; // Content GameObject inside of ScrollView
    public GameObject namePrefab; // Prefab to display text inside

    public LoggingService loggingServiceScript;

    private GameObject selectedObjectPrefab; // To store the selected object prefab
    [SerializeField] private Color infoColor = Color.white;
    [SerializeField] private Color warningColor = Color.yellow;
    [SerializeField] private Color errorColor = Color.red;

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
        /*foreach (string text in textsToDisplay)
        {
            // Instancier le prefab
            GameObject newTMP = Instantiate(namePrefab, contentArea.transform);

            // Récupérer le composant TextMeshPro et définir son texte
            TMPro.TextMeshProUGUI tmpText = newTMP.GetComponent<TMPro.TextMeshProUGUI>();
            if (tmpText != null)
            {
                tmpText.text = text;
            }
        }*/

        foreach (var log in LoggingService.Instance.GetAllLogs())
        {
            CreateLogEntry(log);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateLogEntry(LogEntry log)
    {
        // Instancier le prefab
        GameObject newLogObject = Instantiate(namePrefab, contentArea.transform);

        // Récupérer et configurer le TextMeshPro
        TMPro.TextMeshProUGUI tmpText = newLogObject.GetComponent<TMPro.TextMeshProUGUI>();
        if (tmpText != null)
        {
            // Formatter le texte avec timestamp et préfixe
            string prefix = GetLogPrefix(log.Type);
            tmpText.text = $"[{log.Timestamp:HH:mm:ss}] {prefix}{log.Message}";

            // Appliquer la couleur selon le type
            tmpText.color = GetLogColor(log.Type);
        }
    }

    private string GetLogPrefix(LogType type)
    {
        return type switch
        {
            LogType.Warning => "[WARN] ",
            LogType.Error => "[ERROR] ",
            _ => "[INFO] "
        };
    }

    private Color GetLogColor(LogType type)
    {
        return type switch
        {
            LogType.Warning => warningColor,
            LogType.Error => errorColor,
            _ => infoColor
        };
    }
}
