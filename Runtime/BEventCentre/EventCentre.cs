using System.Collections.Generic;

namespace Game
{

    /// <summary>
    /// 简易的事件消息中心
    /// 张新宇
    /// 可自定义消息枚举枚举
    /// 可监视所有事件
    /// 2023 11 16
    /// </summary>
    public class EventCentre
    {

        #region 普通单例化
        private static EventCentre instance;
        public static EventCentre Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EventCentre();
                }
                return instance;
            }
        }
        private EventCentre()
        {
            eventDic = new Dictionary<string, Action>();
            eventDesDic = new Dictionary<string, string>();
        }
        #endregion

        #region 字典事件存储
        /// <summary>
        /// 万能参数委托
        /// </summary>
        /// <param name="args"></param>
        public delegate void Action(params object[] args);
        private Dictionary<string, Action> eventDic = null;//事件字典
        private Dictionary<string, string> eventDesDic = null;//事件信息字典
        #endregion

        #region 枚举封装
        /// <summary>
        /// 添加无参数消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventEnum">枚举</param>
        /// <param name="action">事件</param>
        /// <param name="des">事件信息</param>
        public void AddEvent<T>(T eventEnum, System.Action action, string des)
        {
            AddEvent(eventEnum, (o) => { if (action != null) action(); }, des);
        }
        /// <summary>
        /// 添加消息
        /// </summary>
        /// <param name="eventEnum">消息事件枚举</param>
        /// <param name="action">具体事件为万能参数委托类型</param>
        /// <param name="des">事件信息</param>
        public void AddEvent<T>(T eventEnum, Action action, string des)
        {
            AddEvent(eventEnum.ToString(), action, des);
        }
        /// <summary>
        /// 触发消息
        /// </summary>
        /// <param name="eventEnum">消息事件枚举</param>
        /// <param name="args">参数</param>
        public void TriggerEvent<T>(T eventEnum, params object[] args)
        {
            TriggerEvent(eventEnum.ToString(), args);
        }
        /// <summary>
        ///  移除消息
        /// </summary>
        /// <param name="eventEnum">消息事件枚举</param>
        /// <param name="action">具体事件（可选参数）</param>
        public void RemoveEvent<T>(T eventEnum, Action action = null)
        {
            RemoveEvent(eventEnum.ToString(), action);
        }
        #endregion

        /// <summary>
        /// 无参数消息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action"></param>
        /// <param name="des">事件信息</param>
        public void AddEvent(string name, System.Action action, string des)
        {
            AddEvent(name, (o) => { if (action != null) action(); }, des);
        }

        #region 正常API
        /// <summary>
        /// 添加万能参数消息
        /// </summary>
        /// <param name="name">消息名字</param>
        /// <param name="action">具体事件</param>
        /// <param name="des">事件信息</param>
        public void AddEvent(string name, Action action, string des)
        {
            if (eventDic.ContainsKey(name))
            {
                eventDic[name] += action;
            }
            else
            {
                eventDic.Add(name, action);
                eventDesDic.Add(name, des);
                Debug("添加事件：" + eventDesDic[name]);
            }
        }
        /// <summary>
        /// 触发消息
        /// </summary>
        /// <param name="name">消息名字</param>
        /// <param name="args">参数</param>
        public void TriggerEvent(string name, params object[] args)
        {
            if (eventDic.ContainsKey(name))
            {
                eventDic[name](args);
                Debug("触发事件：" + eventDesDic[name]);
            }
            else
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogError("触发事件：" + name + "。但是这事件没有被注册，不影响游戏");
#endif
            }
        }
        /// <summary>
        /// 移除消息
        /// </summary>
        /// <param name="name">名字</param>
        /// <param name="action">具体事件（可选参数）</param>
        public void RemoveEvent(string name, Action action = null)
        {
            if (eventDic.ContainsKey(name))
            {
                if (action == null)
                {
                    eventDic.Remove(name);
                    eventDesDic.Remove(name);
                    Debug("移除事件：" + eventDesDic[name]);
                }
                else
                {
                    eventDic[name] -= action;
                    Debug("取消一个事件：" + eventDesDic[name]);
                }
            }
        }
        #endregion

        #region 销毁析构
        /// <summary>
        /// 清楚所有事件数据
        /// </summary>
        public void Clear()
        {
            eventDic.Clear();
            eventDesDic.Clear();
        }
        ~EventCentre()
        {
            Clear();
        }
        #endregion

        #region 上帝窥视
        private void Debug(string str)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.Log(str);
#endif
        }
        public static void DebugAllEvent()
        {
            foreach (var item in Instance.eventDesDic)
            {
                UnityEngine.Debug.LogError(item.Key + ":\t\t\t\t" + item.Value);
            }
        }
        #endregion

    }

    /// <summary>
    /// 对object的拓展
    /// </summary>
    public static class ObjectTloos
    {
        public static int ASInt(this object self)
        {
            return (int)self;
        }
        public static bool ASBool(this object self)
        {
            return (bool)self;
        }
        public static float ASFloat(this object self)
        {
            return (float)self;
        }
        public static string ASString(this object self)
        {
            return (string)self;
        }
        public static T AS<T>(this object self)
        {
            return (T)self;
        }
    }

}