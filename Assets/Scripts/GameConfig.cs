using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class ConfigValue
{
    public string startStr = null;
    public string reStartStr = null;
    public string exitStr = null;
    public float[] barColor = null;
    public float speed = 0;

    public ConfigValue(string startStr, string reStartStr, string exitStr, float[] barColor, float speed)
    {
        this.startStr = startStr;
        this.reStartStr = reStartStr;
        this.exitStr = exitStr;
        this.barColor = barColor;
        this.speed = speed;
    }
}

public static class GameConfig
{
    public static string startStr = "Start";
    public static string reStartStr = "reStart";
    public static string exitStr = "exit";
    public static Color barColor = Color.black;
    public static float speed;

    public static ConfigValue configValue;

    public static void LoadMods(string path)
    {
        string[] modFilePaths = Directory.GetDirectories(path);
        System.Array.Sort(modFilePaths);
        Debug.Log(modFilePaths);

        foreach (string modFilePath in modFilePaths)
        {
            string configJson = File.ReadAllText(modFilePath + "/modcontent.txt");
            Debug.Log(configJson);
            try
            {
                ConfigValue configValue = JsonUtility.FromJson<ConfigValue>(configJson);
                if (configValue.startStr != null)
                {
                    startStr = configValue.startStr;
                    Debug.Log("startStr is changed to" + startStr);
                }
                if(configValue.reStartStr != null)
                {
                    reStartStr = configValue.reStartStr;
                }
                if (configValue.exitStr != null)
                {
                    exitStr = configValue.exitStr;
                }
                if (configValue.barColor != null)
                {
                    barColor = new Color(configValue.barColor[0], configValue.barColor[1], configValue.barColor[2]);
                }
                if (configValue.speed != 0)
                {
                    speed = configValue.speed;
                }
            }
            catch
            {
                Debug.Log("wrong modcontent in mod: " + path);
            }
            
        }
    }
}
