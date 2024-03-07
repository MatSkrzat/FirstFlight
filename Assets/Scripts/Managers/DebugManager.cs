using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Application;

public class DebugManager : MonoBehaviour
{
    public static DebugManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public bool isDebugEnabled = false;
    public bool isLogExceptionOnlyEnabled = false;
    private string fileName = string.Empty;
    LogListModel logsList = null;
    void OnEnable()
    {
        if (!isDebugEnabled) return;
        fileName = DateTime.Now.ToString("yyyyMMddHHmmss");
        logsList = new LogListModel
        {
            logs = new List<LogModel>()
        };
        SaveLoadFile.SaveAsJSON(logsList, PathsDictionary.LOGS, FilenameDictionary.LOGS + fileName);
        logMessageReceived += LogCallback;
    }

    void OnDisable()
    {
        if (!isDebugEnabled) return;
        logMessageReceived -= LogCallback;
    }

    //Called when there is an exception
    void LogCallback(string condition, string stackTrace, LogType type)
    {
        if (!isDebugEnabled) return;
        if (GameManager.UI != null) GameManager.UI.UpdateDebugText(condition, stackTrace, type);
        if (isLogExceptionOnlyEnabled)
        {
            if (type == LogType.Error || type == LogType.Exception)
            {
                logsList.logs.Add(new LogModel() { LogType = (int)type, Message = condition, StackTrace = stackTrace });
            }
        }
        else
        {
            logsList.logs.Add(new LogModel() { LogType = (int)type, Message = condition, StackTrace = stackTrace });
        }
        SaveLoadFile.SaveAsJSON(logsList, PathsDictionary.LOGS, FilenameDictionary.LOGS + fileName);
    }
}
