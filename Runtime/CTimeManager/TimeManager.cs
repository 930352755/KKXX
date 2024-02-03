using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 比较好用的计时器功能
    /// 自动生成，定时，计时，轮训都可
    /// 2023 11 16
    /// </summary>
    public class TimeManager : MonoBehaviour
    {
        #region 自动挂载单例化,自动启动
        private static TimeManager instance = null;
        public static TimeManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject ins = new GameObject("TimeManager");
                    GameObject.DontDestroyOnLoad(ins);
                    instance = ins.AddComponent<TimeManager>();
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
        private void StartSystem() { }
        #endregion
        //-------------------------事件事件-------------------------
        private Dictionary<string, Coroutine> allTimerEvent;
        private void AAwake()
        {
            allTimerEvent = new Dictionary<string, Coroutine>();
        }

        /// <summary>
        /// 事件计数器
        /// </summary>
        private int count = 0;
        /// <summary>
        /// 添加延时事件
        /// </summary>
        /// <param name="action"></param>
        /// <param name="time">时间不会小于一渲染帧</param>
        /// <returns></returns>
        public string AddDelayedEvent(System.Action action, float time)
        {
            count += 1;
            string key = "Time" + count + TimeStamp.GetTimeStamp();
            Coroutine coroutine = StartCoroutine(DelayedCall(action, time, key));
            allTimerEvent.Add(key, coroutine);
            return key;
        }
        /// <summary>
        /// 添加一个间隔触发事件
        /// </summary>
        /// <param name="action"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public string AddIntervalEvent(System.Action action, float time)
        {
            count += 1;
            string key = "Interval" + count + TimeStamp.GetTimeStamp();
            Coroutine coroutine = StartCoroutine(IntervalCall(action, time));
            allTimerEvent.Add(key, coroutine);
            return key;
        }
        /// <summary>
        /// 停止一个时间事件
        /// </summary>
        /// <param name="key"></param>
        public void StopTimerEvent(string key)
        {
            if (allTimerEvent.ContainsKey(key))
            {
                Coroutine coroutine = allTimerEvent[key];
                allTimerEvent.Remove(key);
                StopCoroutine(coroutine);
            }
        }

        private IEnumerator DelayedCall(System.Action action, float delayed, string key)
        {
            if (delayed < 0.02)
            {
                yield return null;
            }
            else
            {
                yield return new WaitForSeconds(delayed);
            }

            if (allTimerEvent.ContainsKey(key))
            {
                if (allTimerEvent.TryGetValue(key, out Coroutine coroutine))
                {
                    StopCoroutine(coroutine);
                }
                allTimerEvent.Remove(key);
                action?.Invoke();
            }
        }
        private IEnumerator IntervalCall(System.Action action, float delayed)
        {
            while (true)
            {
                yield return new WaitForSeconds(delayed);
                action?.Invoke();
            }
        }
        //------------------------------//------------------------

    }
}