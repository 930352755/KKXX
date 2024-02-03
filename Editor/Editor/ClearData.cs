using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

namespace Game
{
    public class ClearData
    {
        [UnityEditor.MenuItem("Game/清除有存档数据")]
        private static void ClearAllData()
        {
            Game.PPData.DeleteAll();
            if (Directory.Exists(Application.persistentDataPath)) Directory.Delete(Application.persistentDataPath, true);
            EditorUtility.DisplayDialog("清除数据完成", "清除数据完成", "OK");
        }
    }
}
