using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AnalyzeUI : MonoBehaviour
{
    public GameObject contentArea;
    public GameObject namePrefab;
    public LoggingService loggingServiceScript;

    [SerializeField] private Color infoColor = Color.white;
    [SerializeField] private Color warningColor = Color.yellow;
    [SerializeField] private Color errorColor = Color.red;

    // Dictionnaire pour suivre les logs déjà affichés
    private HashSet<Guid> displayedLogIds = new HashSet<Guid>();

    void Start()
    {
        // Afficher les logs existants au démarrage
        foreach (var log in LoggingService.Instance.GetAllLogs())
        {
            CreateLogEntry(log);
            displayedLogIds.Add(log.id);
        }

        // S'abonner à l'événement OnNewLog
        LoggingService.Instance.OnNewLog += HandleNewLog;
    }

    void OnDestroy()
    {
        // Se désabonner de l'événement pour éviter les fuites de mémoire
        if (LoggingService.Instance != null)
        {
            LoggingService.Instance.OnNewLog -= HandleNewLog;
        }
    }

    private void HandleNewLog(LogEntry log)
    {
        if (!displayedLogIds.Contains(log.id))
        {
            CreateLogEntry(log);
            displayedLogIds.Add(log.id);
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