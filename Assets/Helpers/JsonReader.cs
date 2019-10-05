using System.Collections.Generic;
using UnityEngine;

public static class JsonReader
{
    public static List<T> readJsonArray<T>(string json)
    {
        json = json.Substring(1, json.Length-1);
        int curlCounter = 0;
        List<T> items = new List<T>();
        string current = "";
        foreach (char c in json) {
            if (c == ',' && curlCounter == 0) continue;
            current += c;
            if (c == '}') {
                curlCounter -= 1;
                if (curlCounter == 0) {
                    items.Add(JsonUtility.FromJson<T>(current));
                    current = "";
                }
            }
            else if (c == '{') curlCounter += 1;
        }
        return items;
    }
}