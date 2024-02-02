using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Reflection;

namespace AIHelp
{
    [Serializable]
    public class  DataBase<T> where T : DataBase<T>
    {
        private static T dataBase = default;
        public static T Instance
        {
            get
            {
                if (dataBase == null)
                { 
                    //先获取非Public的构造方法
                    ConstructorInfo[] ctors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
                    //从ctors中获取无惨的构造方法
                    ConstructorInfo ctor = Array.Find(ctors, (c) => c.GetParameters().Length == 0);
                    if (ctor == null)
                    {
                        throw new Exception("该类没有私有构造函数" + typeof(T));
                    }
                    //调用构造方法
                    dataBase = ctor.Invoke(null) as T;
                    dataBase.LoadData();
                }
                return dataBase;
            }
        }
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
