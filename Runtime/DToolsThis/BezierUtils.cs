using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class BezierUtils
{  
 
    /// <summary>
    /// 线性贝塞尔曲线，根据T值，计算贝塞尔曲线上面相对应的点
    /// </summary>
    /// <param name="t"></param>T值
    /// <param name="p0"></param>起始点
    /// <param name="p1"></param>控制点
    /// <returns></returns>根据T值计算出来的贝赛尔曲线点
    private static Vector3 CalculateLineBezierPoint(float t, Vector3 p0, Vector3 p1)
    {
        float u = 1 - t;
         
        Vector3 p = u * p0;
        p +=  t * p1;
    
 
        return p;
    }
 
    /// <summary>
    /// 二次贝塞尔曲线，根据T值，计算贝塞尔曲线上面相对应的点
    /// </summary>
    /// <param name="t"></param>T值
    /// <param name="p0"></param>起始点
    /// <param name="p1"></param>控制点
    /// <param name="p2"></param>目标点
    /// <returns></returns>根据T值计算出来的贝赛尔曲线点
    private static Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
 
        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;
 
        return p;
    }
 
    /// <summary>
    /// 三次贝塞尔曲线，根据T值，计算贝塞尔曲线上面相对应的点
    /// </summary>
    /// <param name="t">插量值</param>
    /// <param name="p0">起点</param>
    /// <param name="p1">控制点1</param>
    /// <param name="p2">控制点2</param>
    /// <param name="p3">尾点</param>
    /// <returns></returns>
    private static Vector3 CalculateThreePowerBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float ttt = tt * t;
        float uuu = uu * u;
 
        Vector3 p = uuu * p0;
        p += 3 * t * uu * p1;
        p += 3 * tt * u * p2;
        p += ttt * p3;
 
        return p;
    }
 
 
    /// <summary>
    /// 获取存储贝塞尔曲线点的数组
    /// </summary>
    /// <param name="startPoint"></param>起始点
    /// <param name="controlPoint"></param>控制点
    /// <param name="endPoint"></param>目标点
    /// <param name="segmentNum"></param>采样点的数量
    /// <returns></returns>存储贝塞尔曲线点的数组
    public static Vector3[] GetLineBeizerList(Vector3 startPoint,  Vector3 endPoint, int segmentNum)
    {
        Vector3[] path = new Vector3[segmentNum];
        for (int i = 1; i <= segmentNum; i++)
        {
            float t = i / (float)segmentNum;
            Vector3 pixel = CalculateLineBezierPoint(t, startPoint, endPoint);
            path[i - 1] = pixel;
            Debug.Log(path[i - 1]);
        }
        return path;
 
    }
 
    /// <summary>
    /// 获取存储的二次贝塞尔曲线点的数组
    /// </summary>
    /// <param name="startPoint"></param>起始点
    /// <param name="controlPoint"></param>控制点
    /// <param name="endPoint"></param>目标点
    /// <param name="segmentNum"></param>采样点的数量
    /// <returns></returns>存储贝塞尔曲线点的数组
    public static Vector3[] GetCubicBeizerList(Vector3 startPoint, Vector3 controlPoint, Vector3 endPoint, int segmentNum)
    {
        Vector3[] path = new Vector3[segmentNum];
        for (int i = 1; i <= segmentNum; i++)
        {
            float t = i / (float)segmentNum;
            Vector3 pixel = CalculateCubicBezierPoint(t, startPoint,
                controlPoint, endPoint);
            path[i - 1] = pixel;
        }
        return path;
 
    }
 
    /// <summary>
    /// 获取存储的三次贝塞尔曲线点的数组
    /// </summary>
    /// <param name="startPoint"></param>起始点
    /// <param name="controlPoint1"></param>控制点1
    /// <param name="controlPoint2"></param>控制点2
    /// <param name="endPoint"></param>目标点
    /// <param name="segmentNum"></param>采样点的数量
    /// <returns></returns>存储贝塞尔曲线点的数组
    public static Vector3[] GetThreePowerBeizerList(Vector3 startPoint, Vector3 controlPoint1, Vector3 controlPoint2 , Vector3 endPoint, int segmentNum)
    {
        Vector3[] path = new Vector3[segmentNum];
        for (int i = 1; i <= segmentNum; i++)
        {
            float t = i / (float)segmentNum;
            Vector3 pixel = CalculateThreePowerBezierPoint(t, startPoint,
                controlPoint1, controlPoint2, endPoint);
            path[i - 1] = pixel;
            Debug.Log(path[i - 1]);
        }
        return path;
 
    }


    // n阶曲线，递归实现
    public static Vector3 Bezier(float t, List<Vector3> p)
    {
        if (p.Count < 2)
            return p[0];
        List<Vector3> newp = new List<Vector3>();
        for (int i = 0; i < p.Count - 1; i++)
        {
            Debug.DrawLine(p[i], p[i + 1]);
            Vector3 p0p1 = (1 - t) * p[i] + t * p[i + 1];
            newp.Add(p0p1);
        }
        return Bezier(t, newp);
    }
    // transform转换为vector3，在调用参数为List<Vector3>的Bezier函数
    public static Vector3 Bezier(float t, List<Transform> p)
    {
        if (p.Count < 2)
            return p[0].position;
        List<Vector3> newp = new List<Vector3>();
        for (int i = 0; i < p.Count; i++)
        {
            newp.Add(p[i].position);
        }
        return Bezier(t, newp);
    }

    public static Vector3[] GetBeizerList(int segmentNum, List<Vector3>  allPoint)
    {
        Vector3[] path = new Vector3[segmentNum];
        for (int i = 1; i <= segmentNum; i++)
        {
            float t = i / (float)segmentNum;
            Vector3 pixel = Bezier(t, allPoint);
            path[i - 1] = pixel;
            Debug.Log(path[i - 1]);
        }
        return path;
    }
    public static Vector3[] GetBeizerList(int segmentNum, List<Transform> allPoint)
    {
        Vector3[] path = new Vector3[segmentNum];
        for (int i = 1; i <= segmentNum; i++)
        {
            float t = i / (float)segmentNum;
            Vector3 pixel = Bezier(t, allPoint);
            path[i - 1] = pixel;
            Debug.Log(path[i - 1]);
        }
        return path;
    }
}