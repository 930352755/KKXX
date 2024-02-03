using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ToolsClass
{
    #region UnitySelfExpand
    #region UI

    #region RectTransform
    #region AnchoredPosition
    /// <summary>
    /// 设置anchoredPosition通过一个向量坐标
    /// </summary>
    /// <param name="self"></param>
    /// <param name="anchoredPosition">坐标位置</param>
    /// <returns></returns>
    public static RectTransform AnchoredPosition(this RectTransform self, Vector3 anchoredPosition)
    {
        self.anchoredPosition = anchoredPosition;
        return self;
    }
    /// <summary>
    /// 设置anchoredPosition通过一个 X坐标 与Y坐标
    /// </summary>
    /// <param name="self"></param>
    /// <param name="x">X坐标</param>
    /// <param name="y">Y坐标</param>
    /// <returns></returns>
    public static RectTransform AnchoredPosition(this RectTransform self, float x, float y)
    {
        Vector2 anchoredPosition = self.anchoredPosition;
        anchoredPosition.x = x;
        anchoredPosition.y = y;
        self.anchoredPosition = anchoredPosition;
        return self;
    }
    /// <summary>
    /// 设置anchoredPosition X坐标值.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="x">X坐标值</param>
    /// <returns></returns>
    public static RectTransform AnchoredPositionX(this RectTransform self, float x)
    {
        Vector2 anchoredPosition = self.anchoredPosition;
        anchoredPosition.x = x;
        self.anchoredPosition = anchoredPosition;
        return self;
    }
    /// <summary>
    /// 设置anchoredPosition Y坐标值.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="y">Y坐标值</param>
    /// <returns></returns>
    public static RectTransform AnchoredPositionY(this RectTransform self, float y)
    {
        Vector2 anchoredPosition = self.anchoredPosition;
        anchoredPosition.y = y;
        self.anchoredPosition = anchoredPosition;
        return self;
    }
    #endregion
    #region OffsetMax
    /// <summary>
    /// Set the offset max.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="offsetMax"></param>
    /// <returns></returns>
    public static RectTransform OffsetMax(this RectTransform self, Vector2 offsetMax)
    {
        self.offsetMax = offsetMax;
        return self;
    }
    /// <summary>
    /// Set the offset max.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static RectTransform OffsetMax(this RectTransform self, float x, float y)
    {
        Vector2 offsetMax = self.offsetMax;
        offsetMax.x = x;
        offsetMax.y = y;
        self.offsetMax = offsetMax;
        return self;
    }
    /// <summary>
    /// Set the offset max x value.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="x"></param>
    /// <returns></returns>
    public static RectTransform OffsetMaxX(this RectTransform self, float x)
    {
        Vector2 offsetMax = self.offsetMax;
        offsetMax.x = x;
        self.offsetMax = offsetMax;
        return self;
    }
    /// <summary>
    /// Set the offset max y value.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static RectTransform OffsetMaxY(this RectTransform self, float y)
    {
        Vector2 offsetMax = self.offsetMax;
        offsetMax.y = y;
        self.offsetMax = offsetMax;
        return self;
    }
    #endregion
    #region OffsetMin
    /// <summary>
    /// Set the offset min.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="offsetMin"></param>
    /// <returns></returns>
    public static RectTransform OffsetMin(this RectTransform self, Vector2 offsetMin)
    {
        self.offsetMin = offsetMin;
        return self;
    }
    /// <summary>
    /// Set the offset min.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static RectTransform OffsetMin(this RectTransform self, float x, float y)
    {
        Vector2 offsetMin = self.offsetMin;
        offsetMin.x = x;
        offsetMin.y = y;
        self.offsetMin = offsetMin;
        return self;
    }
    /// <summary>
    /// Set the offset min x value.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="x"></param>
    /// <returns></returns>
    public static RectTransform OffsetMinX(this RectTransform self, float x)
    {
        Vector2 offsetMin = self.offsetMin;
        offsetMin.x = x;
        self.offsetMin = offsetMin;
        return self;
    }
    /// <summary>
    /// Set the offset min y value.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static RectTransform OffsetMinY(this RectTransform self, float y)
    {
        Vector2 offsetMin = self.offsetMin;
        offsetMin.y = y;
        self.offsetMin = offsetMin;
        return self;
    }
    #endregion
    #region AnchoredPosition3D
    /// <summary>
    /// Set the anchored position 3d.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="anchoredPosition3D"></param>
    /// <returns></returns>
    public static RectTransform AnchoredPosition3D(this RectTransform self, Vector2 anchoredPosition3D)
    {
        self.anchoredPosition3D = anchoredPosition3D;
        return self;
    }
    /// <summary>
    /// Set the anchored position 3d.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static RectTransform AnchoredPosition3D(this RectTransform self, float x, float y)
    {
        Vector2 anchoredPosition3D = self.anchoredPosition3D;
        anchoredPosition3D.x = x;
        anchoredPosition3D.y = y;
        self.anchoredPosition3D = anchoredPosition3D;
        return self;
    }
    /// <summary>
    /// Set the anchored position 3d x value.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="x"></param>
    /// <returns></returns>
    public static RectTransform AnchoredPosition3DX(this RectTransform self, float x)
    {
        Vector2 anchoredPosition3D = self.anchoredPosition3D;
        anchoredPosition3D.x = x;
        self.anchoredPosition3D = anchoredPosition3D;
        return self;
    }
    /// <summary>
    /// Set the anchored position 3d y value.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static RectTransform AnchoredPosition3DY(this RectTransform self, float y)
    {
        Vector2 anchoredPosition3D = self.anchoredPosition3D;
        anchoredPosition3D.y = y;
        self.anchoredPosition3D = anchoredPosition3D;
        return self;
    }
    #endregion
    #region AnchorMin
    /// <summary>
    /// Set the anchor min.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="anchorMin"></param>
    /// <returns></returns>
    public static RectTransform AnchorMin(this RectTransform self, Vector2 anchorMin)
    {
        self.anchorMin = anchorMin;
        return self;
    }
    /// <summary>
    /// Set the anchor min.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static RectTransform AnchorMin(this RectTransform self, float x, float y)
    {
        Vector2 anchorMin = self.anchorMin;
        anchorMin.x = x;
        anchorMin.y = y;
        self.anchorMin = anchorMin;
        return self;
    }
    /// <summary>
    /// Set the anchor min x value.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="x"></param>
    /// <returns></returns>
    public static RectTransform AnchorMinX(this RectTransform self, float x)
    {
        Vector2 anchorMin = self.anchorMin;
        anchorMin.x = x;
        self.anchorMin = anchorMin;
        return self;
    }
    /// <summary>
    /// Set the anchor min y value.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static RectTransform AnchorMinY(this RectTransform self, float y)
    {
        Vector2 anchorMin = self.anchorMin;
        anchorMin.y = y;
        self.anchorMin = anchorMin;
        return self;
    }
    #endregion
    #region AnchorMax
    /// <summary>
    /// Set the anchor max.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="anchorMax"></param>
    /// <returns></returns>
    public static RectTransform AnchorMax(this RectTransform self, Vector2 anchorMax)
    {
        self.anchorMax = anchorMax;
        return self;
    }
    /// <summary>
    /// Set the anchor max.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static RectTransform AnchorMax(this RectTransform self, float x, float y)
    {
        Vector2 anchorMax = self.anchorMax;
        anchorMax.x = x;
        anchorMax.y = y;
        self.anchorMax = anchorMax;
        return self;
    }
    /// <summary>
    /// Set the anchor max x value.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="x"></param>
    /// <returns></returns>
    public static RectTransform AnchorMaxX(this RectTransform self, float x)
    {
        Vector2 anchorMax = self.anchorMax;
        anchorMax.x = x;
        self.anchorMax = anchorMax;
        return self;
    }
    /// <summary>
    /// Set the anchor max y value.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static RectTransform AnchorMaxY(this RectTransform self, float y)
    {
        Vector2 anchorMax = self.anchorMax;
        anchorMax.y = y;
        self.anchorMax = anchorMax;
        return self;
    }
    #endregion
    #region Pivot
    /// <summary>
    /// Set the pivot.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="pivot"></param>
    /// <returns></returns>
    public static RectTransform Pivot(this RectTransform self, Vector2 pivot)
    {
        self.pivot = pivot;
        return self;
    }
    /// <summary>
    /// Set the pivot.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static RectTransform Pivot(this RectTransform self, float x, float y)
    {
        Vector2 pivot = self.pivot;
        pivot.x = x;
        pivot.y = y;
        self.pivot = pivot;
        return self;
    }
    /// <summary>
    /// Set the pivot x value.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="x"></param>
    /// <returns></returns>
    public static RectTransform PivotX(this RectTransform self, float x)
    {
        Vector2 pivot = self.pivot;
        pivot.x = x;
        self.pivot = pivot;
        return self;
    }
    /// <summary>
    /// Set the pivot y value.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static RectTransform PivotY(this RectTransform self, float y)
    {
        Vector2 pivot = self.pivot;
        pivot.y = y;
        self.pivot = pivot;
        return self;
    }
    #endregion
    #region SizeDelta
    /// <summary>
    /// Set the size delta.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="sizeDelta"></param>
    /// <returns></returns>
    public static RectTransform SizeDelta(this RectTransform self, Vector2 sizeDelta)
    {
        self.sizeDelta = sizeDelta;
        return self;
    }
    /// <summary>
    /// Set the size delta.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static RectTransform SizeDelta(this RectTransform self, float x, float y)
    {
        Vector2 sizeDelta = self.sizeDelta;
        sizeDelta.x = x;
        sizeDelta.y = y;
        self.sizeDelta = sizeDelta;
        return self;
    }
    /// <summary>
    /// Set the size delta x value.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="x"></param>
    /// <returns></returns>
    public static RectTransform SizeDeltaX(this RectTransform self, float x)
    {
        Vector2 sizeDelta = self.sizeDelta;
        sizeDelta.x = x;
        self.sizeDelta = sizeDelta;
        return self;
    }
    /// <summary>
    /// Set the size delta y value.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static RectTransform SizeDeltaY(this RectTransform self, float y)
    {
        Vector2 sizeDelta = self.sizeDelta;
        sizeDelta.y = y;
        self.sizeDelta = sizeDelta;
        return self;
    }
    #endregion
    #region Size
    /// <summary>
    /// Set width with current anchors.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="width"></param>
    /// <returns></returns>
    public static RectTransform SetSizeWidth(this RectTransform self, float width)
    {
        self.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        return self;
    }
    /// <summary>
    /// Set height with current anchors.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public static RectTransform SetSizeHeight(this RectTransform self, float height)
    {
        self.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        return self;
    }

    #endregion
    #endregion

    /// <summary>
    /// 按钮添加点击事件
    /// </summary>
    /// <param name="self">UI的按纽组件</param>
    /// <param name="action">无参事件</param>
    public static void AddClick(this Button self, System.Action action)
    {
        self.onClick.AddListener(() => { action(); });
    }

    /// <summary>
    /// 设置UI中的位置
    /// </summary>
    /// <param name="self">UI的Transform</param>
    /// <param name="parentTransform">UI父物体</param>
    /// <param name="screenTargetPos">UI鼠标点的位置</param>
    /// <param name="camera">相机</param>
    /// <param name="offset">偏移量</param>
    public static void SetUILocaPos(this Transform self, Transform parentTransform, Vector2 screenTargetPos, Camera camera, Vector2 offset)
    {
        Vector2 locaPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentTransform as RectTransform, screenTargetPos, camera, out locaPos);
        self.localPosition = locaPos + offset;
    }
    #endregion

    /// <summary>
    /// 未知层级查找对象。
    /// </summary>
    /// <param name="self"></param>
    /// <param name="childName"></param>
    /// <returns></returns>
    public static Transform FindChildTransformByName(this Transform self, string childName)
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
    public static T FindChindComponentByName<T>(this Transform self, string childName) where T : class
    {
        Transform tr = self.FindChildTransformByName(childName);
        T t = tr.GetComponent<T>();
        if (t == null)
        {
            Debug.LogError(string.Format("在{0}没找到组件{1}", self.name, typeof(T)));
            return null;
        }
        return t;
    }

    /// <summary>
    /// 获得一个对象下的所有T组件（包括自己与子物体）
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static List<T> GetMono<T>(this Transform self) where T : MonoBehaviour
    {
        List<T> monoList = new List<T>();

        T mono = self.GetComponent<T>();
        if (mono != null)
        {
            monoList.Add(mono);
        }

        int count = self.childCount;
        for (int i = 0; i < count; i++)
        {
            Transform trc = self.GetChild(i);
            List<T> monoListN = GetMono<T>(trc);
            if (monoListN.Count > 0)
            {
                monoList.AddRange(monoListN);
            }
        }
        return monoList;
    }

    #endregion
    #region 对链表的拓展
    /// <summary>
    /// 从链表中筛选出一个符合某个条件的一个元素
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="self"></param>
    /// <param name="func">某个元素符合某个条件</param>
    /// <returns></returns>
    public static T GetOneByList<T>(this List<T> self, System.Func<T, bool> func)
    {
        int count = self.Count;
        for (int i = 0; i < count; i++)
        {
            if (func(self[i]))
            {
                return self[i];
            }
        }
        return default;
    }
    /// <summary>
    /// 直接拿到一个链表中符合某个条件的所有元素
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="self"></param>
    /// <param name="func">某个元素符合某个条件</param>
    /// <returns></returns>
    public static List<T> GetAllByList<T>(this List<T> self, System.Func<T, bool> func)
    {
        List<T> ts = new List<T>();

        int count = self.Count;
        for (int i = 0; i < count; i++)
        {
            if (func(self[i]))
            {
                ts.Add(self[i]);
            }
        }
        return ts;
    }

    /// <summary>
    /// 从数组中筛选出一个最符合某个条件的一个元素
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="self"></param>
    /// <param name="func">某个元素符合某个条件</param>
    /// <returns></returns>
    public static T GetOneByList<T>(this List<T> self, System.Func<T, T, bool> func)
    {
        T t = self[0];

        int count = self.Count;
        for (int i = 1; i < count; i++)
        {
            if (func(t, self[i]))
            {
                t = self[i];
            }
        }

        return t;
    }

    /// <summary>
    /// 随机打乱某个列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="self"></param>
    public static void RandomDisturbList<T>(this List<T> self)
    {
        List<T> oldList = new List<T>();

        int count = self.Count;
        for (int i = 0; i < count; i++)
        {
            oldList.Add(self[i]);
        }

        for (int i = 0; i < count; i++)
        {
            int r = UnityEngine.Random.Range(0, oldList.Count);
            self[i] = oldList[r];
            oldList.RemoveAt(r);
        }
    }

    public static string GetClipboard()
    {
        return GUIUtility.systemCopyBuffer;
    }
    #endregion


    public static Color GetColor(this string color)
    {
        ColorUtility.TryParseHtmlString(color, out Color colorGold);
        return colorGold;
    }


}
