using System;
using System.IO;
using UnityEngine;
/// <summary>
/// Basic helper class for JSON configs
/// </summary>
public static class ConfigJSON
{
    
    public static void SaveToJSON<T>(string path, T data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);

    }
    public static T LoadFromJSON<T>(string path)
    {
        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<T>(json);
    }
}
/// <summary>
/// sample class how a interface for the config should look like just use descriptive public variables
/// </summary>
[Serializable]
public class SampleConfigData
{
    public int a;
    public float b;
    public string c;
}
