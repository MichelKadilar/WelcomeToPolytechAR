using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class LoggingService : MonoBehaviour
{
    private static LoggingService instance;

    public static LoggingService Instance
    {
        get
        {
            if (instance == null)
            {
                var go = new GameObject("LoggingService");
                instance = go.AddComponent<LoggingService>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    // Cache circulaire pour stocker les logs
    private readonly Queue<LogEntry> logCache = new Queue<LogEntry>();
    private const int MAX_CACHE_SIZE = 100; // Nombre maximum de logs conservés

    // Event pour notifier les listeners des nouveaux logs
    public event Action<LogEntry> OnNewLog;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    // Méthodes pour ajouter des logs de différents types
    public void LogInfo(string message)
    {
        AddLog(message, LogType.Log);
    }

    public void LogWarning(string message)
    {
        AddLog(message, LogType.Warning);
    }

    public void LogError(string message)
    {
        AddLog(message, LogType.Error);
    }

    private void AddLog(string message, LogType type)
    {
        var logEntry = new LogEntry(message, type);

        // Gestion du cache circulaire
        if (logCache.Count >= MAX_CACHE_SIZE)
        {
            logCache.Dequeue();
        }
        logCache.Enqueue(logEntry);

        // Notification des listeners
        OnNewLog?.Invoke(logEntry);
    }

    // Méthode pour récupérer tous les logs
    public IEnumerable<LogEntry> GetAllLogs()
    {
        return logCache.ToList();
    }

    // Méthode pour vider le cache
    public void ClearLogs()
    {
        logCache.Clear();
    }
}
