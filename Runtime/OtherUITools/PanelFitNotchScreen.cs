using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    /// <summary>
    /// 刘海屏幕的适配除去背景，下移有效显示区域。
    /// 适应本UI框架进行修改。
    /// </summary>
    public class PanelFitNotchScreen : MonoBehaviour
    {
        private RectTransform panelRectTransform;
        private Vector2 originalAnchorMin;
        private Vector2 originalAnchorMax;

        private ScreenOrientation curOrientation;
        private void Start()
        {
            //UI搭建时需要注意
            panelRectTransform = transform.GetChild(1) as RectTransform;
            originalAnchorMin = panelRectTransform.anchorMin;
            originalAnchorMax = panelRectTransform.anchorMax;
            curOrientation = Screen.orientation;
            FitNotchScreen();
        }

        /// <summary>
        /// 这里只设计支持正竖屏幕，和左右横屏幕
        /// </summary>
        private void FitNotchScreen()
        {
            if (Screen.orientation == ScreenOrientation.Portrait)
            {
                float cd = Screen.height - Screen.safeArea.height;
                if (cd > 120) cd = 120;

                float offset = cd / Screen.height;
                panelRectTransform.anchorMin = originalAnchorMin;
                panelRectTransform.anchorMax = new Vector2(originalAnchorMax.x, originalAnchorMax.y - offset);
            }
        }
        private void Update()
        {
            if (curOrientation != Screen.orientation)
            {
                FitNotchScreen();
                curOrientation = Screen.orientation;
            }
        }
    }
}
