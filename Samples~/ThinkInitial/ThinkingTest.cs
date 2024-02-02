using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThinkingTest : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        //初始化数数
        string thinkingAppkey = "a6cb9f585c9a433789a950c60ba0cc69";
        string thinkingUrl = "https://f96cce.kpkk.online";
        Debug.LogError("数数的Key:" + thinkingAppkey);
        Debug.LogError("数数的地址:" + thinkingUrl);
        RewardCardSDK.Thinking.ThinkingManager.Instance.Initial(thinkingAppkey, thinkingUrl);
    }
}
