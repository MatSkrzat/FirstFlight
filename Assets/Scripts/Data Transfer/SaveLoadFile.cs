using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public static class SaveLoadFile
{
    public static void SaveAsJSON<T>(T data, string path, string fileName, bool isReadOnly = false)
    {
        var basicPath = isReadOnly
            ? string.Format("{0}/{1}", Application.streamingAssetsPath, path)
            : string.Format("{0}/{1}", Application.persistentDataPath, path);
        if (!Directory.Exists(basicPath))
        {
            Directory.CreateDirectory(basicPath);
            Debug.Log("Path doesn't exist: " + basicPath + " Creating one");
        }
        File.WriteAllText(string.Format("{0}{1}.json", basicPath, fileName), JsonUtility.ToJson(data));
    }

    public static void SaveAsBinary<T>(T data, string path, string fileName)
    {
        throw new NotImplementedException();
    }

    public static T LoadFromJson<T>(string path, string fileName, bool isReadOnly = false)
    {
        string filePath = isReadOnly
            ? Path.Combine(
                Application.streamingAssetsPath,
                path,
                string.Format("{0}.json", fileName))
            : Path.Combine(
                Application.persistentDataPath,
                path,
                string.Format("{0}.json", fileName));
        //loading file for Android
        if (Application.platform == RuntimePlatform.Android && isReadOnly)
        {
            Debug.Log("ANDROID path: " + filePath);
            var request = UnityWebRequest.Get(filePath);
            request.SendWebRequest();
            while (!request.isDone) { }
            if (string.IsNullOrWhiteSpace(request.downloadHandler.text)) throw new FileNotFoundException();
            return JsonUtility.FromJson<T>(request.downloadHandler.text);
        }
        //loading file for other platforms
        Debug.Log("DEVICE path: " + filePath);
        if (File.Exists(filePath))
            return JsonUtility.FromJson<T>(File.ReadAllText(filePath));
        throw new FileNotFoundException();
    }

    public static T LoadFromBinary<T>(string path)
    {
        throw new NotImplementedException();
    }
}
