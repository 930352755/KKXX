using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public delegate void ActionRef<DebugMenu>(ref DebugMenu menu);
    /// <summary>
    /// 20220810
    /// DeBug测试系统。
    /// 适用任何游戏。
    /// </summary>
    public static class DebugSet
    {
        public enum DebugMenuType
        {
            Text,
            Button,
            Switch,
        }
        public struct DebugMenu
        {
            public string Text;
            public object Value;
            public DebugMenuType Type;
            public ActionRef<DebugMenu> OnClick;
            public string ID; // 可选, 用来区分不同的菜单
        }

        public static List<DebugMenu> MenuList = new List<DebugMenu>();
        private static DebugView _View = null;

        public static void RegistDebugMenu(DebugMenu[] menus)
        {
            DebugSet.MenuList.AddRange(menus);
        }

        public static void Log(params object[] o)
        {
            UnityEngine.Debug.Log(string.Join(" ", o));
        }
        public static void LogError(params object[] o)
        {
            UnityEngine.Debug.LogError(string.Join(" ", o));
        }


        #region Debug

        public static void InitDebugMenu()
        {
            if (_View == null)
            {
                GameObject instance = GameObject.Instantiate(Resources.Load<GameObject>("DebugPrefab/DebugViewCanvas"));
                instance.name = "DebugViewCanvas";
                _View = instance.GetComponent<DebugView>();
                _View.Init();
                GameObject.DontDestroyOnLoad(instance);
            }
        }

#if DEBUG
        /// <summary>
        /// 第一个场景加载之后调用
        /// </summary>
        [RuntimeInitializeOnLoadMethod]
        private static void OnRuntimeMethodLoad()
        {
            InitDebugMenu();
        }
#endif
        #endregion
    }
}