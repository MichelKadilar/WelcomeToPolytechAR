using UnityEngine;
using System;
using System.Collections.Generic;

public struct LogEntry
{
    public string Message { get; private set; }
    public LogType Type { get; private set; }
    public DateTime Timestamp { get; private set; }

    public LogEntry(string message, LogType type)
    {
        Message = message;
        Type = type;
        Timestamp = DateTime.Now;
    }
}
