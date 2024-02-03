using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// Unity MonoBehaviour 的所有事件管理。
    /// 渲染帧事件
    /// 物理帧事件
    /// </summary>
    public class UpdataManager : MonoBehaviour
    {
        #region 自动挂载单例化,自动启动
        private static UpdataManager instance = null;
        public static UpdataManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject ins = new GameObject("UpdataManager");
                    GameObject.DontDestroyOnLoad(ins);
                    instance = ins.AddComponent<UpdataManager>();
                    instance.AAwake();
                }
                return instance;
            }
        }

        /// <summary>
        /// 第一个场景加载之后调用
        /// </summary>
        [RuntimeInitializeOnLoadMethod]
        private static void OnRuntimeMethodLoad()
        {
            Instance.StartSystem();
        }
        #endregion

        private void StartSystem() { }
        private void AAwake()
        {
            allUpdataEvent = new Dictionary<string, System.Action>();
            allFixedUpdataEvent = new Dictionary<string, System.Action>();
            allOnApplicationQuitEvent = new Dictionary<string, System.Action>();
            allOnApplicationPauseEvent = new Dictionary<string, System.Action<bool>>();
        }

        #region 渲染帧事件
        private Dictionary<string, System.Action> allUpdataEvent;
        /// <summary>
        /// 事件计数器
        /// </summary>
        private int updataEventCount = 0;
        /// <summary>
        /// 添加一个渲染帧事件
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public string AddUptataEvent(System.Action action)
        {
            updataEventCount++;
            string key = "Uptata" + updataEventCount + TimeStamp.GetTimeStamp();
            return AddUptataEvent(key, action);
        }
        /// <summary>
        /// 添加一个渲染帧事件
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public string AddUptataEvent(string key, System.Action action)
        {
            if (allUpdataEvent.ContainsKey(key))
            {
                return key;
            }
            allUpdataEvent.Add(key, action);
            return key;
        }
        /// <summary>
        /// 移除一个渲染帧事件
        /// </summary>
        /// <param name="key">对应的Key</param>
        public void RomoveUptataEvent(string key)
        {
            if (allUpdataEvent.ContainsKey(key))
            {
                allUpdataEvent.Remove(key);
            }
        }
        private void Update()
        {
            foreach (System.Action action in allUpdataEvent.Values)
            {
                action?.Invoke();
            }
        }
        #endregion

        #region 物理贞事件
        private Dictionary<string, System.Action> allFixedUpdataEvent;
        /// <summary>
        /// 事件计数器
        /// </summary>
        private int fixedUpdataCount = 0;
        /// <summary>
        /// 添加一个物理帧事件
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public string AddFixedUpdataEvent(System.Action action)
        {
            fixedUpdataCount += 1;
            string key = "FixedUpdata" + fixedUpdataCount + TimeStamp.GetTimeStamp();
            return AddFixedUpdataEvent(key, action);
        }
        /// <summary>
        /// 添加一个物理帧事件
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public string AddFixedUpdataEvent(string key, System.Action action)
        {
            if (allFixedUpdataEvent.ContainsKey(key))
            {
                return key;
            }
            allFixedUpdataEvent.Add(key, action);
            return key;
        }
        /// <summary>
        /// 移除一个物理帧事件
        /// </summary>
        /// <param name="key">对应的Key</param>
        public void RomoveFixedUpdataEvent(string key)
        {
            if (allFixedUpdataEvent.ContainsKey(key))
            {
                allFixedUpdataEvent.Remove(key);
            }
        }
        private void FixedUpdate()
        {
            foreach (System.Action action in allFixedUpdataEvent.Values)
            {
                action?.Invoke();
            }
        }
        #endregion

        #region 游戏退出事件
        private Dictionary<string, System.Action> allOnApplicationQuitEvent;
        /// <summary>
        /// 事件计数器
        /// </summary>
        private int onApplicationQuitCount = 0;
        /// <summary>
        /// 添加一个游戏退出事件
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public string AddOnApplicationQuitEvent(System.Action action)
        {
            onApplicationQuitCount += 1;
            string key = "OnApplicationQuit" + onApplicationQuitCount + TimeStamp.GetTimeStamp();
            return AddOnApplicationQuitEvent(key, action);
        }
        /// <summary>
        /// 添加一个游戏退出事件
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public string AddOnApplicationQuitEvent(string key, System.Action action)
        {
            if (allOnApplicationQuitEvent.ContainsKey(key))
            {
                return key;
            }
            allOnApplicationQuitEvent.Add(key, action);
            return key;
        }
        /// <summary>
        /// 移除一个游戏退出事件
        /// </summary>
        /// <param name="key">对应的Key</param>
        public void RomoveOnApplicationQuitEvent(string key)
        {
            if (allOnApplicationQuitEvent.ContainsKey(key))
            {
                allOnApplicationQuitEvent.Remove(key);
            }
        }
        private void OnApplicationQuit()
        {
            foreach (System.Action action in allOnApplicationQuitEvent.Values)
            {
                action?.Invoke();
            }
        }
        #endregion

        #region 游戏暂停继续事件

        private Dictionary<string, System.Action<bool>> allOnApplicationPauseEvent;
        /// <summary>
        /// 事件计数器
        /// </summary>
        private int onApplicationPauseCount = 0;
        /// <summary>
        /// 添加一个游戏暂停继续事件
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public string AddOnApplicationPauseEvent(System.Action<bool> action)
        {
            onApplicationPauseCount += 1;
            string key = "OnApplicationPause" + onApplicationPauseCount + TimeStamp.GetTimeStamp();
            return AddOnApplicationPauseEvent(key, action);
        }
        /// <summary>
        /// 添加一个游戏暂停继续事件
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public string AddOnApplicationPauseEvent(string key, System.Action<bool> action)
        {
            if (allOnApplicationPauseEvent.ContainsKey(key))
            {
                return key;
            }
            allOnApplicationPauseEvent.Add(key, action);
            return key;
        }
        /// <summary>
        /// 移除一个游戏暂停继续事件
        /// </summary>
        /// <param name="key">对应的Key</param>
        public void RomoveOnApplicationPauseEvent(string key)
        {
            if (allOnApplicationPauseEvent.ContainsKey(key))
            {
                allOnApplicationPauseEvent.Remove(key);
            }
        }
        private void OnApplicationPause(bool pause)
        {
            foreach (System.Action<bool> action in allOnApplicationPauseEvent.Values)
            {
                action?.Invoke(pause);
            }
        }

        #endregion

        #region 拓展游戏退出的存档操作
        /// <summary>
        /// 通常用于游戏退出，保存数据
        /// </summary>
        /// <param name="save"></param>
        public void AddSaveEvent(System.Action save)
        {
            AddOnApplicationQuitEvent(save);
            AddOnApplicationPauseEvent((b) => { if (b) save?.Invoke(); });
        }
        #endregion
    }
}
