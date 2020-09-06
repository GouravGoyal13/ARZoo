using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml;
using System.Reflection;
using System.Xml.Schema;
using System.IO;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Linq;

public class LoadingManager : MonoBehaviour
{
    string dataPath = "Assets/Data/";
    string filePath = "LevelData.xml";
    public static LoadingManager Instance = null;
    Dictionary<string, LevelData> m_levelData = null;
    List<LevelData> levelData;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        LoadXMLs();
        LoadDataTables();
    }

    void LoadXMLs()
    {
        levelData = XMLUtil.DeserializeList<LevelData>(Path.Combine(dataPath, filePath), "LevelData");
    }

    void LoadDataTables()
    {
        m_levelData = LoadTable<LevelData>(levelData, "LevelData", delegate(LevelData leveldata)
            {
                return leveldata.LevelID;
            });
    }

    static Dictionary<string, T> LoadTable<T>(List<T> importedData, string fileName, System.Func<T, string> getKey) where T : class
    {
        if (importedData == null)
        {
            Debug.LogError("Unable to load table from file " + fileName);
            return null;
        }

        Dictionary<string, T> data = new Dictionary<string, T>(importedData.Count);
        foreach (object row in importedData)
        {
            T rowData = row as T;
            string key = getKey(rowData);
            if (string.IsNullOrEmpty(key))
            {
                continue;
            }
            else if (!data.ContainsKey(key))
            {
                data.Add(getKey(rowData), rowData);
            }
        }

        return data;
    }

    #region UtilityMethods

    public IEnumerable<LevelData> AllLevelData
    {
        get
        {
            foreach (KeyValuePair<string, LevelData> kvp in m_levelData)
            {
                yield return kvp.Value; 
            }
        }
    }

    #endregion
}


