using UnityEngine;
using System;

public struct LogEntry
{
    public Guid id { get; private set; } // Modification : type Guid
    public string Message { get; private set; }
    public LogType Type { get; private set; }
    public DateTime Timestamp { get; private set; }

    public LogEntry(string message, LogType type)
    {
        id = Guid.NewGuid();
        Message = message;
        Type = type;
        Timestamp = DateTime.Now;
    }
}
