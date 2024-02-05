using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    /// <summary>
    /// 简单的UI面板展示，隐藏动画
    /// 跟据层级可设置不同的动画效果
    /// </summary>
    public class UIAnimation : MonoBehaviour
    {
        protected Dictionary<UILayers, System.Action<System.Action>> allOpenAnima;
        protected Dictionary<UILayers, System.Action<System.Action>> allCloseAnima;
        public void OpenAnima(UILayers uILayers, System.Action action, bool isUseAnima)
        {
            if (allOpenAnima[uILayers] != null && isUseAnima)
            {
                allOpenAnima[uILayers](action);
            }
            else
            {
                action();
            }
        }
        public void CloseAnima(UILayers uILayers, System.Action action, bool isUseAnima)
        {
            if (allCloseAnima[uILayers] != null && isUseAnima)
            {
                allCloseAnima[uILayers](action);
            }
            else
            {
                action();
            }
        }
        public void InitialUIAnima()
        {
            allOpenAnima = new Dictionary<UILayers, System.Action<System.Action>>();
            allCloseAnima = new Dictionary<UILayers, System.Action<System.Action>>();
            for (int i = 0; i < (int)UILayers.UILayersCount; i++)
            {
                allOpenAnima.Add((UILayers)i, null);
                allCloseAnima.Add((UILayers)i, null);
            }
            allOpenAnima[UILayers.Pop1] = OpenPopAnima;
            allCloseAnima[UILayers.Pop1] = ClosePopAnima;
            allOpenAnima[UILayers.Pop2] = OpenPopAnima;
            allCloseAnima[UILayers.Pop2] = ClosePopAnima;
            allOpenAnima[UILayers.Pop3] = OpenPopAnima;
            allCloseAnima[UILayers.Pop3] = ClosePopAnima;
            allOpenAnima[UILayers.Pop4] = OpenPopAnima;
            allCloseAnima[UILayers.Pop4] = ClosePopAnima;
            allOpenAnima[UILayers.Pop5] = OpenPopAnima;
            allCloseAnima[UILayers.Pop5] = ClosePopAnima;
            allOpenAnima[UILayers.Pop6] = OpenPopAnima;
            allCloseAnima[UILayers.Pop6] = ClosePopAnima;
        }

        /// <summary>
        /// 动画速度
        /// </summary>
        [HideInInspector]
        public float Speed = 0.07f;
        //弹板层的动画
        private void OpenPopAnima(System.Action openCallBack)
        {
            Transform maskbg = transform.GetChild(0);
            CanvasGroup maskBGcg = maskbg.GetComponent<CanvasGroup>();
            if (maskBGcg == null)
            {
                maskBGcg = maskbg.gameObject.AddComponent<CanvasGroup>();
            }
            maskBGcg.alpha = 0;
            maskBGcg.DOFade(1, Speed*5);

            Transform ts = transform.GetChild(1);//这个动画对面板的搭建有一定的要求
            ts.localScale = Vector3.zero;
            Sequence quence = DOTween.Sequence();
            quence.Append(ts.DOScale(Vector3.one * 1.05f, Speed * 3).SetEase(Ease.OutQuad));
            quence.Append(ts.DOScale(Vector3.one, Speed * 2).SetEase(Ease.InQuad));
            quence.AppendCallback(() =>
            {
                if (openCallBack != null)
                {
                    openCallBack();
                }
            });
            quence.Play();
        }
        private void ClosePopAnima(System.Action closeCallBack)
        {
            Transform maskbg = transform.GetChild(0);
            CanvasGroup maskBGcg = maskbg.GetComponent<CanvasGroup>();
            if (maskBGcg == null)
            {
                maskBGcg = maskbg.gameObject.AddComponent<CanvasGroup>();
            }
            maskBGcg.alpha = 1;
            maskBGcg.DOFade(0, Speed*5);

            Transform ts = transform.GetChild(1);//这个动画对面板的搭建有一定的要求
            ts.localScale = Vector3.one;
            Sequence quence = DOTween.Sequence();
            quence.Append(ts.DOScale(Vector3.one * 1.05f, Speed * 2).SetEase(Ease.OutQuad));
            quence.Append(ts.DOScale(0, Speed * 3).SetEase(Ease.InQuad));
            quence.AppendCallback(() =>
            {
                if (closeCallBack != null)
                {
                    closeCallBack();
                }
                ts.localScale = Vector3.one;
            }
            );
            quence.Play();
        }
    }
}