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

    // Start is called before the first frame update
    void Start()
    {
        foreach (var log in LoggingService.Instance.GetAllLogs())
        {
            CreateLogEntry(log);
        }
    }

    private void CreateLogEntry(LogEntry log)
    {
        GameObject newLogObject = Instantiate(namePrefab, contentArea.transform);

        TMPro.TextMeshProUGUI tmpText = newLogObject.GetComponent<TMPro.TextMeshProUGUI>();
        if (tmpText != null)
        {
            string prefix = GetLogPrefix(log.Type);
            tmpText.text = $"[{log.Timestamp:HH:mm:ss}] {prefix}{log.Message}";

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
