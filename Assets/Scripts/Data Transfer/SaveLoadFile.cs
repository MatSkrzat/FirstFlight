using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public static class SaveLoadFile
{
    public static void SaveAsJSON<T>(T data, string path, string fileName)
    {
        var basicPath = string.Format("{0}/{1}", Application.streamingAssetsPath, path);
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

    public static T LoadFromJson<T>(string path, string fileName)
    {
        string filePath = Path.Combine(
            Application.streamingAssetsPath,
            path,
            string.Format("{0}.json", fileName)
        );
        //loading file for Android
        if (Application.platform == RuntimePlatform.Android)
        {
            var request = UnityWebRequest.Get(filePath);
            request.SendWebRequest();
            while (!request.isDone) { }
            return JsonUtility.FromJson<T>(request.downloadHandler.text);
        }
        //loading file for other platforms
        if (File.Exists(filePath))
            return JsonUtility.FromJson<T>(File.ReadAllText(filePath));
        throw new FileNotFoundException();
    }

    public static T LoadFromBinary<T>(string path)
    {
        throw new NotImplementedException();
    }
}
