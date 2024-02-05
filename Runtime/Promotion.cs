using UnityEngine;
using UnityEngine.UI;

public class Promotion : MonoBehaviour
{
    private static Promotion instance = null;
    public static Promotion Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject go = Resources.Load<GameObject>("UWV/Promotion");
                go = GameObject.Instantiate(go);
                go.name = "Promotion";
                GameObject.DontDestroyOnLoad(go);
                go.SetActive(true);
                instance = go.GetComponent<Promotion>();
            }
            return instance;
        }
    }
    private void StartSystem() { Debug.LogError("Promotion系统自动初始化完成"); }

    [SerializeField]
    private Texture imgGui;
    public GameObject prefab;
    public RectTransform showImg;
    public GameObject childPanel;


    private UniWebView webView;
    public void Close()
    {
        Debug.Log("Promotion:Close");
        if (webView != null)
        {
            webView.CleanCache();
            webView.Hide();
        }
        this.childPanel.SetActive(false);
        h5SuccCellback = null;
    }

    private System.Action h5SuccCellback = null;


    public void Show(string url)
    {
        UniWebViewLogger.Instance.LogLevel = UniWebViewLogger.Level.Debug;
        Screen.orientation = ScreenOrientation.Portrait;
        if (webView != null)
        {
            Destroy(webView.gameObject);
        }
        this.childPanel.SetActive(true);
        if (webView == null)
        {
            GameObject go1 = Instantiate(prefab);
            webView = go1.GetComponent<UniWebView>();
            webView.Load(url);
            webView.ReferenceRectTransform = showImg;
            webView.SetShowSpinnerWhileLoading(true);
            webView.Show();
        }
    }

    public void Show(string url,System.Action h5SuccCellback,string token,int appid)
    {
        Token = token;
        APP_ID = appid;
        this.h5SuccCellback = h5SuccCellback;
        UniWebViewLogger.Instance.LogLevel = UniWebViewLogger.Level.Debug;
        Screen.orientation = ScreenOrientation.Portrait;
        if (webView != null)
        {
            Destroy(webView.gameObject);
        }
        this.childPanel.SetActive(true);

        if (webView == null)
        {
            GameObject go1 = Instantiate(prefab);
            webView = go1.GetComponent<UniWebView>();
            webView.SetHeaderField("token", Token);
            webView.SetHeaderField("appId", APP_ID.ToString());
            webView.Load(url);
            webView.ReferenceRectTransform = showImg;
            webView.OnPageFinished += OnPageLoadFinished;
            webView.OnShouldClose += OnShouldClose;
            webView.OnKeyCodeReceived += OnKeyCodeReceived;
            webView.OnMessageReceived += OnMessageReceived;
            webView.SetShowSpinnerWhileLoading(true);
            webView.Show();
        }

    }

    private void OnKeyCodeReceived(UniWebView webView, int keyCode)
    {
        Debug.Log("Promotion:OnKeyCodeReceived");
    }

    private void OnMessageReceived(UniWebView webView, UniWebViewMessage message)
    {
        Debug.Log("Promotion:OnMessageReceived" + message.Path);
        if (message.Args.ContainsKey("key") && message.Args["key"]=="success")
        {
            Debug.LogError("Promotion:  payout信息填写完成");
            h5SuccCellback?.Invoke();
            Close();
        }
    }

    private bool OnShouldClose(UniWebView view)
    {
        Close();
        return true;
    }

    private void OnPageLoadFinished(UniWebView webView, int statusCode, string url)
    {
        Debug.Log("Promotion:OnPageLoadFinished");
    }
    private string Token;
    public void getToken()
    {
        webView.EvaluateJavaScript($"receiveToken('{Token}');", (payload) => {
            if (payload.resultCode.Equals("0"))
            {
                print("succ");
            }
        });
    }
    private int APP_ID;
    public void getAppId()
    {
        webView.EvaluateJavaScript($"receiveToken('{APP_ID}');", (payload) => {
            if (payload.resultCode.Equals("0"))
            {
                print("succ");
            }
        });
    }

    private void OnGUI()
    {
        if (childPanel.gameObject.activeSelf)
        {
            if (GUI.Button(new Rect(50, 35, 100, 100), imgGui, GUIStyle.none))
            {
                Close();
            }
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


}
