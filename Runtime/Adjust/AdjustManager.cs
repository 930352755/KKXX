using System.Collections;
using System.Collections.Generic;
using com.adjust.sdk;
using UnityEngine;
using ThinkingAnalytics;

public class AdjustManager
{

    private static System.Action<string> attributionCallback = null;
    public static void Initial(string key,MonoBehaviour mono,System.Action<string> attributionCallback)
    {
        Debug.LogError("Adjust开始初始化:" + key +"\t\t"+ Time.time);
        string distinctId = ThinkingAnalyticsAPI.GetDistinctId();
        string accountId = ThinkingAnalyticsAPI.GetDeviceId();
        Adjust.addSessionCallbackParameter("ta_distinct_id", distinctId);
        Adjust.addSessionCallbackParameter("ta_account_id", accountId);
#if DEBUG
        AdjustConfig adjustConfig = new AdjustConfig(key, AdjustEnvironment.Sandbox);
#else
        AdjustConfig adjustConfig = new AdjustConfig(key, AdjustEnvironment.Production);
#endif
        adjustConfig.needsCost = true;
        AdjustManager.attributionCallback = attributionCallback;
        Adjust.start(adjustConfig);
       
#if UNITY_EDITOR
        Debug.LogError("编辑器不初始化Adjust回调");
        Debug.LogError("初始化Adjust完成:" + key + "\t\t" + Time.time);
        return;
#endif
        mono.StartCoroutine(GetADJCallBack());
        Debug.LogError("初始化Adjust完成:" + key + "\t\t" + Time.time);
    }

    private static IEnumerator GetADJCallBack()
    {
        while (true)
        {
            AdjustAttribution adjustAttribution = Adjust.getAttribution();
            if (adjustAttribution != null)
            {
                string channel = adjustAttribution.network;
                if (channel != "Organic")
                {
                    AdjustManager.channel = channel;
                    attributionCallback?.Invoke(channel);
                    ThinkingAnalyticsAPI.Track("AdjustChannel", new Dictionary<string, object>() { { "channel", channel } });
                    yield break;
                }
            }
            yield return null;
        }
    }


    #region 一些渠道的控制 对应一些需求 具体去问策划

    private static string channel = "Organic";


    private static string mtg1 = "Mintegral";
    private static string mtg2 = "mintegral";

    private static string unity1 = "Unity ads";
    private static string unity2 = "Unity_ads";
    private static string unity3 = "Unity Ads";
    private static string unity4 = "Unitymob";
    private static string unity5 = "UnityMob";

    public static bool ISTestChannel()
    {
        return AdjustManager.channel == AdjustManager.mtg1
                                || AdjustManager.channel == AdjustManager.mtg2
                                || AdjustManager.channel == AdjustManager.unity1
                                || AdjustManager.channel == AdjustManager.unity2
                                || AdjustManager.channel == AdjustManager.unity3
                                || AdjustManager.channel == AdjustManager.unity4
                                || AdjustManager.channel == AdjustManager.unity5;
    }

    #endregion


}
