using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.AddressableAssets;

namespace Game
{
    /// <summary>
    /// 简单的UI系统，UI管理者，管理着所有UI面板2022.11.08
    /// 资源没加载好，Loading可别结束啊
    /// </summary>
    public class UIManager
    {
        #region 资源加载，配合Loading
        /// <summary>
        /// 是否已经加载资源完成
        /// </summary>
        public static bool IsLoadOver = false;

        /// <summary>
        /// Resources下UIPanel的Json信息路径
        /// </summary>
        public const string UIPanelPath = "JsonData/GameUIPathInfo";

        #region 启动方法
        private Dictionary<UILayers, Transform> allUILayers;
        /// <summary>
        /// 场景切换后需要设置新场景的Canvas，为UI的父物体
        /// </summary>
        /// <param name="uiCanvas"></param>
        public void SetCanvas(Transform uiCanvas, MonoBehaviour mono)
        {
            uiViewDic.Clear();
            allUILayers.Clear();
            allCurOpenPanel.Clear();
            for (int i = 0; i < (int)UILayers.UILayersCount; i++)
            {
                RectTransform uiRoot = new GameObject(((UILayers)i).ToString(), typeof(RectTransform)).transform as RectTransform;
                uiRoot.SetParent(uiCanvas);
                uiRoot.localScale = Vector3.one;
                uiRoot.anchorMin = Vector2.zero;
                uiRoot.anchorMax = Vector2.one;
                uiRoot.sizeDelta = Vector2.zero;
                uiRoot.localPosition = Vector3.zero;
                allUILayers.Add((UILayers)i, uiRoot);
                allCurOpenPanel.Add((UILayers)i, null);
            }
            mono.StartCoroutine(LoadResources());
        }
        #endregion

        #region 初始化逻辑
        /// <summary>
        /// 所有预制体加载
        /// </summary>
        private GameObject[] allUIPanel;
        /// <summary>
        /// 异步执行加载操作
        /// </summary>
        /// <returns></returns>
        private IEnumerator LoadResources()
        {
            Debug.LogError("开始加载UI");
            IsLoadOver = false;
            AllUIPathInfo allUIPathInfo = JsonUtility.FromJson<AllUIPathInfo>(Resources.Load<TextAsset>(UIPanelPath).text);
            yield return null;
            float t = Time.time;
            int count = allUIPathInfo.allUIEnums.Count;
            List<GameObject> gs = new List<GameObject>();
            for (int i = 0; i < count; i++)
            {
                string path = allUIPathInfo.allPath[i];
                UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> async = Addressables.LoadAssetAsync<GameObject>(path);
                yield return new WaitUntil(() => { return async.IsDone; });
                gs.Add(async.Result);
                yield return null;
            }
            allUIPanel = gs.ToArray();
            yield return null;
            //初始化执行所有面板的AStart方法
            //将所有的UI面板都实例化出来，然后存到池子里
            int length = allUIPanel.Length;
            for (int i = 0; i < length; i++)
            {
                GameObject prefab = allUIPanel[i];
                GameObject UIPrefab = GameObject.Instantiate(prefab);
                Transform uiType = null;
                UIView uiView = UIPrefab.GetComponent<UIView>();
                if (uiView == null)
                {
                    Debug.LogError("预制体不是UI加载失败+" + prefab.name);
                    continue;
                }
                UILayers uiLayer = uiView.selfLayer;
                allUILayers.TryGetValue(uiLayer, out uiType);
                if (uiType == null)
                {
                    Debug.LogError("将UICanvas脚本挂载到Canvas下");
                    yield break;
                }
                UIPrefab.transform.SetParent(uiType);
                //面板的大小初始化
                UIPrefab.name = prefab.name;
                UIPrefab.transform.localScale = Vector3.one;
                RectTransform rt = UIPrefab.transform as RectTransform;
                rt.localPosition = Vector3.zero;
                rt.anchoredPosition = Vector2.zero;
                rt.offsetMax = Vector2.zero;
                rt.offsetMin = Vector2.zero;
                uiViewDic.Add(prefab.name, uiView);
                uiView.OnStart();
                yield return null;
            }
            //加载完成
            yield return null;
            IsLoadOver = true;
            Debug.LogError("UI加载完成");
        }
        #endregion

        #endregion

        #region 标准单例化
        private static UIManager instance;
        public static UIManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UIManager();
                }
                return instance;
            }
        }
        private UIManager()
        {
            allUILayers = new Dictionary<UILayers, Transform>();
            allCurOpenPanel = new Dictionary<UILayers, UIView>();
            uiViewDic = new Dictionary<string, UIView>();
        }
        #endregion

        #region 内部管理逻辑

        #region 池子
        /// <summary>
        /// 存放所有面板的键值对
        /// </summary>
        private Dictionary<string, UIView> uiViewDic = new Dictionary<string, UIView>();
        #endregion

        #region 展示
        /// <summary>
        /// 限制仅仅在UI框架中使用
        /// </summary>
        /// <param name="name"></param>
        /// <param name="hideCallBack"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public UIView ShowUIView(string name, params object[] msg)
        {
            UIView uiView = null;
            if (!uiViewDic.TryGetValue(name, out uiView))
            {
                Debug.LogError("加载失败" + name);
                return null;
            }
            //这一层的当前面板
            UIView curPanel = allCurOpenPanel[uiView.selfLayer];
            if (curPanel != null)
            {//如果当前要展示的面板所在层级 已经有了其他面板
                DelayUI delayUI = new DelayUI(name, msg);
                curPanel.panelStack.Push(delayUI);
                return null;
            }
            //
            allCurOpenPanel[uiView.selfLayer] = uiView;
            uiView.Show(msg);//调用展示触发方法
            return uiView;
        }
        #endregion

        #region 关闭
        /// <summary>
        /// 框架内部使用
        /// </summary>
        /// <param name="uiView"></param>
        public void HideUIView(UIView uiView)
        {
            uiView.Hide();
        }
        #endregion

        #region 查找
        private UIView FindView(string name)
        {
            UIView uiView = null;
            if (!uiViewDic.TryGetValue(name, out uiView))
            {
                return null;
            }
            return uiView;
        }
        #endregion

        #region 永久销毁某个面板
        private void DestroyUIView(string name)
        {
            UIView uiView = null;
            if (!uiViewDic.TryGetValue(name, out uiView))
            {
                Debug.LogError("面板没有" + name);
                return;
            }
            uiViewDic.Remove(name);
            GameObject.Destroy(uiView.gameObject);
        }
        #endregion

        #endregion

        #region 对外方法

        #region 当前存在的UI 同一级的面板只能同时存在一个
        public Dictionary<UILayers, UIView> allCurOpenPanel;
        #endregion

        #region 展示
        /// <summary>
        /// 展示一个面板
        /// </summary>
        /// <typeparam name="T">弹板类型</typeparam>
        /// <param name="name">弹板名字</param>
        /// <param name="msg">参数</param>
        public void ShowUI(string name, params object[] msg)
        {
            ShowUIView(name, msg);
        }

        /// <summary>
        /// 展示一个面板
        /// </summary>
        /// <typeparam name="T">弹板类型</typeparam>
        /// <param name="name">弹板名字</param>
        /// <param name="msg">参数</param>
        public void ShowUI<T>(T name, params object[] msg)
        {
            ShowUIView(name.ToString(), msg);
        }
        #endregion

        #region 关闭
        /// <summary>
        /// 隐藏面板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void HideUI(string name)
        {
            UIView uiView = null;
            if (!uiViewDic.TryGetValue(name, out uiView))
            {
                Debug.LogError("面板没有" + name);
                return;
            }
            HideUIView(uiView);
        }
        /// <summary>
        /// 隐藏面板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void HideUI<T>(T name)
        {
            HideUI(name.ToString());
        }
        #endregion

        #region 查找
        /// <summary>
        /// 获取UIPanel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T FindUI<T>(string name) where T : UIView
        {
            UIView uIView = FindView(name);
            if (uIView == null) return null;
            return uIView as T;
        }

        /// <summary>
        /// 获取UIPanel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T FindUI<T,R>(R name) where T : UIView
        {
            return FindUI<T>(name.ToString());
        }
        #endregion

        #region 永久销毁某个面板
        private void DestroyUI(string name)
        {
            DestroyUIView(name);
        }
        #endregion

        #endregion
    }

    [System.Serializable]
    public class AllUIPathInfo
    {
        public List<string> allUIEnums;
        public List<string> allPath;
    }
}