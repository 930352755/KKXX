using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Reflection;

namespace AIHelp
{
    [Serializable]
    public class  DataBase
    {

        public virtual string DataPath => GetType().Name;
        public string FilePath => Path.Combine(UnityEngine.Application.persistentDataPath, "GameData", DataPath);
        public string DirectoryPath => Path.Combine(UnityEngine.Application.persistentDataPath, "GameData");
        public void LoadData()
        {

            if (!Directory.Exists(DirectoryPath))
            {
                Directory.CreateDirectory(DirectoryPath);
            }
            if (!File.Exists(FilePath))
            {
                StreamWriter stream = File.CreateText(FilePath);
                stream.Close();
            }

            string path = Path.Combine(UnityEngine.Application.persistentDataPath, FilePath);
            string text = File.ReadAllText(path);
            try
            {
                if (string.IsNullOrEmpty(text))
                {
                    text = JsonUtility.ToJson(this);
                }
                JsonUtility.FromJsonOverwrite(text, this);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"直接解析失败，清档:{e}");
            }

            OnLoad();
        }
        public void Save()
        {
            if (!Directory.Exists(DirectoryPath))
            {
                Directory.CreateDirectory(DirectoryPath);
            }
            StreamWriter stream = File.CreateText(FilePath);
            stream.Write(CreateSaveString());
            stream.Close();
        }
        private string CreateSaveString()
        {
            string jsonText = JsonUtility.ToJson(this);
            Debug.LogError(jsonText);
            return jsonText;
        }
        protected virtual void OnLoad() { }
    }
}
