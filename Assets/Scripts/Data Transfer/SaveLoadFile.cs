using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveLoadFile
{

    public static void SaveAsJSON<T>(T data, string path, string fileName)
    {
        if(!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            Debug.Log("Path doesn't exist: " + path + " Creating one");
        }
        File.WriteAllText(String.Format("Assets/Resources/{0}/{1}.json", path, fileName), JsonUtility.ToJson(data));
    }

    public static void SaveAsBinary<T>(T data, string path, string fileName)
    {
        throw new NotImplementedException();
    }

    public static T LoadFromJson<T>(string path, string fileName)
    {
        string filePath = String.Format("Assets/Resources/{0}/{1}.json", path, fileName);
        if (File.Exists(filePath))
        {
            return JsonUtility.FromJson<T>(File.ReadAllText(filePath));
        }
        throw new Exception("File not found!");
    }

    public static T LoadFromBinary<T>(string path)
    {
        throw new NotImplementedException();
    }
}
