using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

namespace Game
{


    /// <summary>
    /// 生成UI获取引用脚本
    /// 生成UI面板枚举
    /// </summary>
    public class CreateUICS
    {
        public const string UIDirectories = "UIPrefabsPanel";
        /// <summary>
        /// 生成脚本的Assets下位置
        /// </summary>
        private static string UICSPath = "Game/Scripts/GameUI/Other_UI";
        /// <summary>
        /// 生成枚举的Assets路径。
        /// </summary>
        private static string UIEnumPath = "Game/Scripts/GameUI/Other_UI";
        /// <summary>
        /// 所有UI的路径
        /// 还是资源根目录
        /// </summary>
        private static string UIPrefabPath = "/"+ AddressSet.aaRootAssetPath;
        /// <summary>
        /// 生成Json路径
        /// </summary>
        public static string JsonFilePath = "/Game/Resources/";

        #region UI饮用生成
        [MenuItem("Game/GameUI/生成UI资源引用(会自动生成UI面板枚举)")]
        private static void Start()
        {
            if (Selection.gameObjects.Length == 1)
            {
                GameObject ui = Selection.gameObjects[0];
                if (ui.GetComponent<UIView>() != null)
                {
                    SetContentCS(ui, ui.name);
                    SetUIEnum();
                }
                else
                {
                    Debug.LogError("选中对象有问题，没有UIView组件");
                }
            }
            else
            {
                Debug.LogError("未选中要生成的UI");
            }
        }

        /// <summary>
        /// 设置内容
        /// </summary>
        /// <param name="go">UI对象预制体</param>
        /// <param name="uiName">生成CS文件的名字</param>
        public static void SetContentCS(GameObject go, string uiName)
        {
            List<GameObject> allUI = GetAllUI(go);
            if (ISSameName(allUI))
            {
                Debug.LogError("有相同名字的UI请注意");
                return;
            }

            uiName += "_UI";
            string path = Application.dataPath + "/" + UICSPath + "/" + uiName + ".cs";

            StringBuilder areaString;
            areaString = new StringBuilder();
            //需要的命名空间引入
            areaString.Append("using UnityEngine;\n");
            areaString.Append("using UnityEngine.UI;\n");
            areaString.Append("using Game;\n");
            areaString.Append("using Spine.Unity;\n");

            areaString.Append("public class " + uiName + " \n{");

            for (int i = 0; i < allUI.Count; i++)
            {
                string[] info = allUI[i].name.Split('#');
                string name = info[0];
                string ui = info[1];
                areaString.Append("\n\tpublic " + ui + " " + name + ";");
            }

            areaString.Append("\n\tpublic " + uiName + " (GameObject go)\n\t{");

            int count = allUI.Count;
            for (int i = 0; i < count; i++)
            {
                string[] info = allUI[i].name.Split('#');
                string name = info[0];
                string ui = info[1];
                if (ui == "GameObject")
                {
                    areaString.Append("\n\t\t" + name + " = UIView.FindChildTransformByName(go.transform,\"" + allUI[i].name + "\").gameObject;");
                }
                else
                {
                    areaString.Append("\n\t\t" + name + " = UIView.FindChindComponentByName<" + ui + ">(go.transform,\"" + allUI[i].name + "\");");
                }
            }
            areaString.Append("\n\t}");

            string endContent = "\n}";
            areaString.Append(endContent);

            if (!Directory.Exists(Application.dataPath + "/" + UICSPath))
            {
                Directory.CreateDirectory(Application.dataPath + "/" + UICSPath);
            }
            File.WriteAllText(path, areaString.ToString());
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("生成完成", path, "确认");
        }

        /// <summary>
        /// 判断生成UI中是否有相同名字的。
        /// </summary>
        /// <param name="allUI"></param>
        /// <returns></returns>
        public static bool ISSameName(List<GameObject> allUI)
        {
            int count = allUI.Count;
            for (int i = 0; i < count - 1; i++)
            {
                for (int j = i + 1; j < count; j++)
                {
                    if (allUI[i].name == allUI[j].name)
                    {
                        Debug.LogError(allUI[j].name);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 获取到所有的UI
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        public static List<GameObject> GetAllUI(GameObject go)
        {
            List<GameObject> allUI = new List<GameObject>();
            int count = go.transform.childCount;
            for (int i = 0; i < count; i++)
            {
                GameObject goc = go.transform.GetChild(i).gameObject;
                if (goc.name.Contains("#"))
                {
                    allUI.Add(goc);
                }
                allUI.AddRange(GetAllUI(goc));
            }
            return allUI;
        }
        #endregion

        #region UIView枚举生成。
        [MenuItem("Game/GameUI/生成UI资源枚举")]
        private static void SetUIEnum()
        {
            string uiPath = Application.dataPath + UIPrefabPath;
            string JsonPath = Application.dataPath + JsonFilePath + UIManager.UIPanelPath + ".json";

            //该文件夹下所有文件夹
            string[] uiPaths = Directory.GetDirectories(uiPath);
            List<string> allEnum = new List<string>();
            List<string> allPath = new List<string>();
            for (int i = 0; i < uiPaths.Length; i++)
            {
                string sameUIPath = uiPaths[i] + "/"+ UIDirectories;
                if (!Directory.Exists(sameUIPath)) continue;
                string[] sameUI = Directory.GetFiles(sameUIPath);
                for (int j = 0; j < sameUI.Length; j++)
                {
                    string aUIPath = sameUI[j];
                    if (aUIPath.Contains(".meta")) continue;
                    //获取面板名字
                    int pos = aUIPath.LastIndexOf('/');
                    string key = aUIPath.Substring(pos + 1, aUIPath.Length - pos - 8);
                    //获取面板路径
                    string value = aUIPath.Substring(uiPath.Length + 1, aUIPath.Length - uiPath.Length - 8);

                    allEnum.Add(key);
                    allPath.Add(value);
                }
            }

            //int l = allEnum.Count;
            //if (l == (int)AllUIEnum.AllUIEnumNum) return;
            if (!Directory.Exists(Application.dataPath + "/" + UIEnumPath))
            {
                Directory.CreateDirectory(Application.dataPath + "/" + UIEnumPath);
            }
            string pathK = Application.dataPath + "/" + UIEnumPath + "/" + "AllUIEnum.cs";
            SetEnum(pathK, allEnum, "AllUIEnum", "Game");

            AllUIPathInfo allUIPathInfo = new AllUIPathInfo();
            allUIPathInfo.allUIEnums = allEnum;
            allUIPathInfo.allPath = allPath;

            if (!Directory.Exists(Application.dataPath + JsonFilePath + "JsonData"))
            {
                Directory.CreateDirectory(Application.dataPath + JsonFilePath + "JsonData");
            }
            string json = JsonUtility.ToJson(allUIPathInfo);
            File.WriteAllText(JsonPath, json);

            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("生成完成", pathK, "确认");
        }
        #endregion

        #region 工具
        /// <summary>
        /// 写入一个枚举文件
        /// </summary>
        /// <param name="enumFilePath">文件路径</param>
        /// <param name="enumList">枚举列表</param>
        /// <param name="enumName">枚举名字</param>
        /// <param name="nameSpace">（可选）命名空间</param>
        private static void SetEnum(string enumFilePath, List<string> enumList, string enumName, string nameSpace = null)
        {
            StringBuilder areaString;
            if (nameSpace != null)
            {
                areaString = new StringBuilder("namespace " + nameSpace + "\n{\n\tpublic enum " + enumName + "\n\t{");
                string endContent = "\n\t}\n}";

                int count = enumList.Count;
                for (int i = 0; i < count; i++)
                {
                    areaString.Append("\n\t\t" + enumList[i] + ",");
                }

                areaString.Append("\n\t\t" + enumName + "Num");
                areaString.Append(endContent);
            }
            else
            {
                areaString = new StringBuilder("public enum " + enumName + "\n{");
                string endContent = "\n}";

                int count = enumList.Count;
                for (int i = 0; i < count; i++)
                {
                    areaString.Append("\n\t" + enumList[i] + ",");
                }
                areaString.Append("\n\t" + enumName + "Num");
                areaString.Append(endContent);
            }

            File.WriteAllText(enumFilePath, areaString.ToString());
        }



        #endregion
    }
}


