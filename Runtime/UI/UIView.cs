using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    /// <summary>
    /// UI面板以及UI的视图显示控制2022.11.29
    /// </summary>
    public abstract class UIView : UIAnimation
    {
        /// <summary>
        /// 自身的UI层级
        /// </summary>
        [Tooltip("UI的层级")]
        public UILayers selfLayer = UILayers.Zero;

        /// <summary>
        /// 该面板的状态
        /// </summary>
        [HideInInspector]
        public UIState selfState = UIState.Hide;

        /// <summary>
        /// 是否使用该层级的动画
        /// </summary>
        protected bool isUseAnima = true;

        #region 需要继承，不需要调用的参数，在管理器中用
        /// <summary>
        /// 将要展示的面板的队列
        /// </summary>
        [HideInInspector]
        public Stack<DelayUI> panelStack = new Stack<DelayUI>();
        /// <summary>
        /// 栈展示
        /// </summary>
        private void QueueShow()
        {
            UIManager.Instance.allCurOpenPanel[selfLayer] = null;
            if (panelStack.Count > 0)
            {
                DelayUI panel = panelStack.Pop();
                UIView uiView = UIManager.Instance.ShowUIView(panel.panelName, panel.panelParameter);
                for (int i = 0; i < panelStack.Count; i++)
                {
                    uiView.panelStack.Push(panelStack.Pop());
                }
            }
        }
        /// <summary>
        /// 整个UI的CG
        /// </summary>
        private CanvasGroup cg = null;
        /// <summary>
        /// UI的点击屏蔽
        /// </summary>
        private CanvasGroup cgUI = null;

        public void OnStart()
        {
            InitialUIAnima();
            OnAStart();
            if (cg == null)
            {
                cg = gameObject.GetComponent<CanvasGroup>();
                if (cg == null)
                {
                    cg = gameObject.AddComponent<CanvasGroup>();
                }
            }
            cg.alpha = 0;
            cg.blocksRaycasts = false;
            selfState = UIState.Hide;
        }

        public void Show(params object[] msg)
        {
            OnShowBefore(msg);
            OnShowStart();
        }
        public void Hide()
        {
            OnHideBefore();
            OnHideStart();
        }

        private void OnShowBefore(params object[] msg)
        {
            selfState = UIState.Showing;
            if (cg == null)
            {
                cg = gameObject.GetComponent<CanvasGroup>();
                if (cg == null)
                {
                    cg = gameObject.AddComponent<CanvasGroup>();
                }
            }
            cg.alpha = 1;
            cg.blocksRaycasts = true;

            //动画中，不能乱点击别的UI
            if (cgUI == null)
            {
                cgUI = transform.GetChild(1).GetComponent<CanvasGroup>();
                if (cgUI == null)
                {
                    cgUI = transform.GetChild(1).gameObject.AddComponent<CanvasGroup>();
                }
            }
            cgUI.blocksRaycasts = false;

            OnBShowBefore(msg);
        }
        private void OnShowStart()
        {
            OpenAnima(selfLayer, OnShowLater, isUseAnima);
        }
        private void OnShowLater()
        {
            cgUI.blocksRaycasts = true;
            selfState = UIState.Show;
            OnBShowLater();
        }

        private void OnHideBefore()
        {
            selfState = UIState.Hideing;
            if (cg == null)
            {
                cg = gameObject.GetComponent<CanvasGroup>();
                if (cg == null)
                {
                    cg = gameObject.AddComponent<CanvasGroup>();
                }
            }
            cg.alpha = 1;

            if (cgUI == null)
            {
                cgUI = transform.GetChild(1).GetComponent<CanvasGroup>();
                if (cgUI == null)
                {
                    cgUI = transform.GetChild(1).gameObject.AddComponent<CanvasGroup>();
                }
            }
            cgUI.blocksRaycasts = false;
            OnCHideBefore();
        }
        private void OnHideStart()
        {
            CloseAnima(selfLayer, OnHideLater, isUseAnima);
        }
        private void OnHideLater()
        {
            selfState = UIState.Hide;
            cg.blocksRaycasts = false;
            cgUI.blocksRaycasts = true;
            cg.alpha = 0;
            OnCHideLater();
            QueueShow();
        }
        #endregion

        #region 生命周期
        /// <summary>
        /// 面板的初始化
        /// </summary>
        protected abstract void OnAStart();
        /// <summary>
        /// 面板的展示调用方法
        /// 面板在展示之前调用
        /// </summary>
        /// <param name="msg"></param>
        protected abstract void OnBShowBefore(params object[] msg);
        /// <summary>
        /// 展示之后调用的方法
        /// </summary>
        protected virtual void OnBShowLater()
        {
            EventCentre.Instance.TriggerEvent("OpenUI", transform.name);
        }
        /// <summary>
        /// 隐藏之前调用的方法
        /// </summary>
        protected virtual void OnCHideBefore()
        {
            EventCentre.Instance.TriggerEvent("CloseUI", transform.name);
        }
        /// <summary>
        /// 面板的隐藏调用方法
        /// 面板在隐藏之后调用
        /// </summary>
        protected abstract void OnCHideLater();
        #endregion

        /// <summary>
        /// 关闭自己
        /// </summary>
        protected virtual void CloseSelf()
        {
            if (selfState == UIState.Show)
            {
                UIManager.Instance.HideUIView(this);
            }
        }
        /// <summary>
        /// 未知层级查找对象。
        /// </summary>
        /// <param name="self"></param>
        /// <param name="childName"></param>
        /// <returns></returns>
        public static Transform FindChildTransformByName(Transform self, string childName)
        {
            Transform c = self.Find(childName);
            if (c != null) return c;
            for (int i = 0; i < self.childCount; i++)
            {
                c = FindChildTransformByName(self.GetChild(i), childName);
                if (c != null) return c;
            }
            return null;
        }
        /// <summary>
        /// 未知层级获得对象的组件。
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <param name="self"></param>
        /// <param name="childName"></param>
        /// <returns></returns>
        public static T FindChindComponentByName<T>(Transform self, string childName) where T : class
        {
            Transform tr = FindChildTransformByName(self, childName);
            T t = tr.GetComponent<T>();
            if (t == null)
            {
                Debug.LogError(string.Format("在{0}没找到组件{1}", self.name, typeof(T)));
                return null;
            }
            return t;
        }
    }

    /// <summary>
    /// UI面板的层级
    /// 最后一个为层级的数量
    /// </summary>
    public enum UILayers
    {
        /// <summary>
        /// 备用0层级
        /// </summary>
        Zero,
        /// <summary>
        /// 场景层级
        /// </summary>
        Scenes,
        /// <summary>
        /// 顶部资源层级
        /// </summary>
        TopResources,
        /// <summary>
        /// 备用1层级
        /// </summary>
        One,
        /// <summary>
        /// 弹版级别
        /// </summary>
        Pop1,
        Pop2,
        Pop3,
        Pop4,
        Pop5,
        Pop6,
        /// <summary>
        /// 备用2层级
        /// </summary>
        Two,
        /// <summary>
        /// 引导层级
        /// </summary>
        Guide,
        /// <summary>
        /// 最高遮照层
        /// </summary>
        Mask,

        UILayersCount
    }
    public class DelayUI
    {
        public string panelName;
        public object[] panelParameter;

        public DelayUI(string panelName, object[] panelParameter)
        {
            this.panelName = panelName;
            this.panelParameter = panelParameter;
        }
    }
    public enum UIState
    {
        Hide,
        Show,
        Hideing,
        Showing
    }

}