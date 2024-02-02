using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSView : MonoBehaviour
{

    private Text fpsView;
    private float time;
    private int frameCount;

    private void Start()
    {
        fpsView = GetComponent<Text>();
    }

    private void Update()
    {
        time += Time.unscaledDeltaTime;
        frameCount++;
        if (time >= 1 && frameCount >= 1)
        {
            float fps = frameCount / time;
            time = 0;
            frameCount = 0;
            fpsView.text = "FPS:" + fps.ToString("f2");//#0.00
            fpsView.color = fps >= 20 ? Color.white : (fps > 15 ? Color.yellow : Color.red);
        }
    }

}
