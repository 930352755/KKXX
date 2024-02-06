using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using Game;


namespace KKXX.ZXY.APKTool
{
    /// <summary>
    /// 密钥数据
    /// </summary>
    [System.Serializable]
    public class ConfigKeystore
    {
        public string store_path = "";
        public string store_password = "";
        public string key_alias = "";
        public string key_password = "";
    }

    /// <summary>
    /// 我还有IOS版本的，这里还没结合
    /// 依赖激励卡生成的配置文件Json
    /// </summary>
    public class APKToolWindow : EditorWindow
    {
        private static EditorWindow _window;

        [UnityEditor.MenuItem("Game/打包配置")]
        private static void OpenConfigWindow()
        {
            string targetName = EditorUserBuildSettings.activeBuildTarget.ToString();//开发平台
            _window = (APKToolWindow)GetWindow(typeof(APKToolWindow), false, $"打包配置({targetName})", true);//创建窗口
            _window.minSize = new Vector2(700, 250);
            _window.Show();//展示
            isFixkeyStore = false;
            isSeekeyStore = false;
            key = keyStorePath;
            if (string.IsNullOrEmpty(keyStorePath))
            {
                EditorUtility.DisplayDialog("请设置密钥", "请设置密钥", "OK");
            }
            else
            {
                string keyPath = Path.Combine(Path.GetDirectoryName(Application.dataPath), keyStorePath);
                string keyInfoPath = keyPath.Substring(0, keyPath.Length - 8) + "json";
                if (!File.Exists(keyPath) || !File.Exists(keyInfoPath))
                {
                    EditorUtility.DisplayDialog("请按照要求设置密钥", $"密钥位置: ${keyPath}\n$\"密钥信息位置: ${keyInfoPath}", "OK");
                }
                else
                {
                    string keyStoreInfo = File.ReadAllText(keyInfoPath);
                    if (string.IsNullOrEmpty(keyStoreInfo))
                    {
                        EditorUtility.DisplayDialog("密钥配置出了问题", $"密钥位置: ${keyPath}\n$\"密钥信息位置: ${keyInfoPath}", "OK");
                    }
                    else
                    {
                        ConfigKeystore configData = JsonUtility.FromJson<ConfigKeystore>(keyStoreInfo);
                        key_password = configData.store_password;
                        key_alias = configData.key_alias;
                        store_password = configData.key_password;
                    }
                }
            }
        }
        private static bool isFixkeyStore;
        private static bool isSeekeyStore;
        private void OnGUI()
        {
#if UNITY_ANDROID
            GUILayout.Label($"安卓配置说明：自动设置安卓的版本A21-A33，自动设置IL2CPP。和只设置ARM64");
            GUILayout.Label($"安卓打包说明：打包前请设置密钥。");


            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical();
                GUILayout.Label($"版本号");
                PlayerSettings.bundleVersion = GUILayout.TextField(Application.version, GUILayout.Width(100));

                GUILayout.Label($"产品名字");
                PlayerSettings.productName = EditorGUILayout.TextField(PlayerSettings.productName, GUILayout.Width(300));


                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                GUILayout.Label($"安卓的bundleVersionCode");
                PlayerSettings.Android.bundleVersionCode = EditorGUILayout.IntField(PlayerSettings.Android.bundleVersionCode, GUILayout.Width(100));
                GUILayout.Label($"产品包名");
                PlayerSettings.applicationIdentifier = EditorGUILayout.TextField(PlayerSettings.applicationIdentifier, GUILayout.Width(300));
                GUILayout.EndVertical();





                GUILayout.BeginVertical();
                if (GUILayout.Button("打测试Apk"))
                {
                    SetAndroidConfigInfo();
                    BuildApk(true);
                }
                if (GUILayout.Button("打正式Apk"))
                {
                    SetAndroidConfigInfo();
                    BuildApk(false);
                }
                if (GUILayout.Button("打正式aab"))
                {
                    SetAndroidConfigInfo();
                    BuildApk(false, true);
                }
                if (GUILayout.Button("打安卓工程"))
                {
                    SetAndroidConfigInfo();
                    BuildApk(false, false, true);
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();

            if (GUILayout.Button("修改/设置密钥信息"))
            {
                isFixkeyStore = !isFixkeyStore;
            }
            if (GUILayout.Button("查看密钥(无法修改，只能查看)"))
            {
                if (!string.IsNullOrEmpty(keyStorePath))
                {
                    isSeekeyStore = !isSeekeyStore;
                }
            }
            if (isFixkeyStore)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label($"密钥在项目中的路径");
                key = GUILayout.TextField(key, GUILayout.Width(300));
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.Label($"密码");
                key_password = GUILayout.TextField(key_password, GUILayout.Width(300));
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.Label($"别名");
                key_alias = GUILayout.TextField(key_alias, GUILayout.Width(300));
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.Label($"别名密码");
                store_password = GUILayout.TextField(store_password, GUILayout.Width(300));
                GUILayout.EndHorizontal();
                if (GUILayout.Button("生成密钥数据"))
                {
                    string path = Path.Combine(Path.GetDirectoryName(Application.dataPath), "keyPath.json");
                    File.WriteAllText(path, key);
                    AddKeyStore();
                    isFixkeyStore = false;
                }
            }
            if (isSeekeyStore)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label($"密钥在项目中的路径");
                GUILayout.TextField(keyStorePath, GUILayout.Width(300));
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.Label($"密码");
                GUILayout.TextField(key_password, GUILayout.Width(300));
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.Label($"别名");
                GUILayout.TextField(key_alias, GUILayout.Width(300));
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.Label($"别名密码");
                GUILayout.TextField(store_password, GUILayout.Width(300));
                GUILayout.EndHorizontal();
            }
#elif UNITY_IOS
            if (GUILayout.Button("打XCode DeBug"))
            {
                //这里是自动打出Addressables资源
                ResEditorUtility.AddGroups();
                ResEditorUtility.AutoBuildDefaultAddressables();

                string path = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "Build"));
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string apkName = Path.Combine(path, "IOS" + PlayerSettings.bundleVersion + TimeStamp.GetTimeStamp()); ;
                if (EditorUtility.DisplayDialog("开始打包", apkName, "确认", "取消"))
                {
                    BuildTarget target = BuildTarget.iOS;
                    string[] scenes = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes);

                    BuildPipeline.BuildPlayer(scenes, apkName, target, BuildOptions.Development);
                }
            }

            if (GUILayout.Button("打XCode 正式"))
            {
                //这里是自动打出Addressables资源
                ResEditorUtility.AddGroups();
                ResEditorUtility.AutoBuildDefaultAddressables();

                string path = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "Build"));
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string apkName = Path.Combine(path, "IOS" + PlayerSettings.bundleVersion + TimeStamp.GetTimeStamp()); ;
                if (EditorUtility.DisplayDialog("开始打包", apkName, "确认", "取消"))
                {
                    BuildTarget target = BuildTarget.iOS;
                    string[] scenes = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes);
                    BuildPipeline.BuildPlayer(scenes, apkName, target, BuildOptions.None);
                }
            }
#endif

        }

        #region 属性信息配置
        private void SetAndroidConfigInfo()
        {
#if UNITY_ANDROID
            //设置适应安卓版本
            PlayerSettings.Android.minSdkVersion = (AndroidSdkVersions)22;
            PlayerSettings.Android.targetSdkVersion = (AndroidSdkVersions)33;

            //设置IL2CPP
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;//设置为ARMv7已经淘汰  只设置ARM64
#endif
        }
        #endregion

        #region 打包
        /// <summary>
        /// 快速安全的打包
        /// </summary>
        /// <param name="isDev"></param>
        /// <param name="isBuildAppBundle"></param>
        private void BuildApk(bool isDev, bool isBuildAppBundle = false, bool isAndroid = false)
        {
            bool isCanApk = true;

            #region 下载Keystore,以及更新Keystore
            if (!IsConfigKeystore())
            {
                isCanApk = false;
            }
            #endregion

            //这里是自动打出Addressables资源
            AddressSet.AddGroups();
            AddressSet.AutoBuildDefaultAddressables();

            if (isCanApk)
            {
                Build(isDev, isBuildAppBundle, isAndroid);
            }
        }

        #region 打包检测核对信息
        private bool IsConfigKeystore()
        {
            if (!PlayerSettings.Android.useCustomKeystore ||
               string.IsNullOrEmpty(PlayerSettings.Android.keystoreName) ||
               string.IsNullOrEmpty(PlayerSettings.Android.keystorePass) ||
               string.IsNullOrEmpty(PlayerSettings.Android.keyaliasName) ||
               string.IsNullOrEmpty(PlayerSettings.Android.keyaliasPass))
            {
                return ConfigKeystore();
            }
            return true;
        }
        #endregion

        #region Build
        private void Build(bool isDevBuild, bool isBuildAppBundle, bool isAndroiud)
        {
            if (BuildPipeline.isBuildingPlayer)
                return;
            string apkName = GetBuildPath(isDevBuild, isBuildAppBundle, isAndroiud);
            if (EditorUtility.DisplayDialog("开始打包", apkName, "确认", "取消"))
            {
                BuildTarget target = BuildTarget.Android;
                string[] scenes = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes);
                EditorUserBuildSettings.buildAppBundle = isBuildAppBundle;//是否是aab文件
                EditorUserBuildSettings.androidCreateSymbolsZip = false;//设置打出SymbolsZip 压缩文件
                EditorUserBuildSettings.exportAsGoogleAndroidProject = isAndroiud;
                BuildPipeline.BuildPlayer(scenes, apkName, target, isDevBuild ? BuildOptions.Development : BuildOptions.None);

            }
        }
        private static string GetBuildPath(bool isDevBuild, bool isBuildAppBundle, bool isAndroid)
        {
            string path = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "Build"));
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            long timestamp = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            if (isAndroid) return Path.Combine(path, "AndroidPro" + PlayerSettings.bundleVersion + timestamp);
            if (isBuildAppBundle) return Path.Combine(path, "AAB" + "-" + PlayerSettings.bundleVersion + "(" + PlayerSettings.Android.bundleVersionCode + ")" + "-" + timestamp + "-" + (isDevBuild ? "Debug" : "Release") + ".aab");
            else return Path.Combine(path, "APK" + "-" + PlayerSettings.bundleVersion + "(" + PlayerSettings.Android.bundleVersionCode + ")" + "-" + timestamp + "-" + (isDevBuild ? "Debug" : "Release") + ".apk");
        }
        #endregion
        #endregion

        #region 密钥设置
        /// <summary>
        /// 设置密钥
        /// </summary>
        /// <returns>是否成功</returns>
        public bool ConfigKeystore()
        {
            //密钥路径
            string keyPath = Path.Combine(Path.GetDirectoryName(Application.dataPath), keyStorePath);
            string keyInfoPath = keyPath.Substring(0, keyPath.Length - 8) + "json";
            if (!File.Exists(keyPath) || !File.Exists(keyInfoPath))
            {
                EditorUtility.DisplayDialog("请按照要求设置密钥", $"密钥位置: ${keyPath}\n$\"密钥信息位置: ${keyInfoPath}", "OK");
                return false;
            }
            string keyStoreInfo = File.ReadAllText(keyInfoPath);
            if (string.IsNullOrEmpty(keyStoreInfo))
            {
                return false;
            }
            if (!PlayerSettings.Android.useCustomKeystore ||
                string.IsNullOrEmpty(PlayerSettings.Android.keystoreName) ||
                string.IsNullOrEmpty(PlayerSettings.Android.keystorePass) ||
                string.IsNullOrEmpty(PlayerSettings.Android.keyaliasName) ||
                string.IsNullOrEmpty(PlayerSettings.Android.keyaliasPass))
            {
                ConfigKeystore configData = JsonUtility.FromJson<ConfigKeystore>(keyStoreInfo);
                PlayerSettings.Android.useCustomKeystore = true;
                PlayerSettings.Android.keystoreName = Path.Combine(Path.GetDirectoryName(Application.dataPath), configData.store_path);
                PlayerSettings.Android.keystorePass = configData.store_password;
                PlayerSettings.Android.keyaliasName = configData.key_alias;
                PlayerSettings.Android.keyaliasPass = configData.key_password;
                AssetDatabase.SaveAssets();
                EditorUtility.DisplayDialog("签名配置成功", $"密钥位置: ${keyPath}", "OK");
            }
            return true;
        }


        private static string keyStorePath
        {
            get
            {
                string path = Path.Combine(Path.GetDirectoryName(Application.dataPath), "keyPath.json");
                if (File.Exists(path))
                {
                    return File.ReadAllText(path);
                }
                return "";
            }
        }
        private static string key;
        private static string key_password;
        private static string key_alias;
        private static string store_password;

        /// <summary>
        /// 没有服务器，手动配置
        /// 生成可以读取的Json
        /// </summary>
        private static void AddKeyStore()
        {
            //密钥路径
            string keyPath = Path.Combine(Path.GetDirectoryName(Application.dataPath), keyStorePath);
            if (!File.Exists(keyPath))
            {
                EditorUtility.DisplayDialog("请按照要求设置密钥", $"密钥位置: ${keyPath}", "OK");
                return;
            }
            //设置路径名称
            string keyInfoPath = keyPath.Substring(0, keyPath.Length - 8) + "json";
            ConfigKeystore configKeystore = new ConfigKeystore();
            configKeystore.store_path = keyStorePath;
            //密码s
            configKeystore.key_password = key_password;
            //别名
            configKeystore.key_alias = key_alias;
            //别名密码
            configKeystore.store_password = store_password;
            string json = JsonUtility.ToJson(configKeystore);
            File.WriteAllText(keyInfoPath, json);
            EditorUtility.DisplayDialog("签名配置生成成功", $"密钥配置位置: ${keyInfoPath}", "OK");
        }
        #endregion


    }
}