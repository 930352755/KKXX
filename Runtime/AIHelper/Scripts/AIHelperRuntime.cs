using System;
using System.Collections;
using System.Collections.Generic;
using AIHelp;
using UnityEngine;
using static AIHelp.AIHelpDefine;

 namespace AIHelp
{
    public class AIHelperRuntime 
    {

        private static AIHelperRuntime instance;
        public static AIHelperRuntime Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new AIHelperRuntime();
                }
                return instance;
            }
        }


        public const string version = "1.0.0";
        public class AIHelpUser
        {
            private string userId;
            private string userName;
            private string serverId;
            private string userTags;
            private string customData;

            public string UserId { get => userId; set => userId = value; }
            public string UserName { get => userName; set => userName = value; }
            public string ServerId { get => serverId; set => serverId = value; }
            public string UserTags { get => userTags; set => userTags = value; }
            public string CustomData { get => customData; set => customData = value; }

            public AIHelpUser(string userId, string userName, string serverId, string userTags, string customData)
            {
                this.userId = userId;
                this.userName = userName;
                this.serverId = serverId;
                this.userTags = userTags;
                this.customData = customData;
            }

            public override string ToString()
            {
                string aiHelpUserStr = "userID : {0}, userName：{1},userSeverID：{2},userTags：{3},customData：{4}";
                return string.Format(aiHelpUserStr, userId, userName, serverId, userTags, customData);
            }

        }

        #region 入口1
        private string entranceID001 = "E001";//入口ID
        private string welcomeMessageID001 = "welcome!!!";//欢迎词
        private string faqID = "dddd";//FAQ ID
        private ApiConfig _configForEntranceID001;
        private ApiConfig ConfigForEntranceID001
        {
            get
            {
                if (_configForEntranceID001 == null)
                {
                    _configForEntranceID001 = new ApiConfig.Builder()
                    .SetEntranceId(entranceID001)
                    .SetWelcomeMessage(welcomeMessageID001)
                    .build();
                }
                return _configForEntranceID001;

            }
        }
        #endregion


        private AIHelpLogData logData;
        /// <summary>
        /// 是否初始化成功
        /// </summary>
        private bool isInitSuccess;
        private string country;

        /// <summary>
        /// AIHelp初始化
        /// 通过指定国家或地区获取语言 如果要添加语言，语言对照码在这 https://docs.aihelp.net/zh/android/config/language.html#api
        /// </summary>
        /// <param name="appKey">appKey运营给</param>
        /// <param name="domain">运营给</param>
        /// <param name="appId">运营给</param>
        /// <param name="country">小写二位语言编码en ru pt id es</param>
        public void Initila(string appKey,string domain,string appId,string country)
        {
            this.country = country;
            /*为以下国家或地区提供额外的域名支持：

                印度地区 IN
             中国大陆地区 CN
              需要注意的是，此方法必须在 init 开始之前调用
                 */
            //AIHelpSupport.AdditionalSupportFor(PublishCountryOrRegion.CN);

            Debug.LogError("AIHelperRuntime版本号：V"+version +" | AIHelp初始化地区 ： " + country);
            AIHelpSupport.enableLogging(true);//是否启用   
            AIHelpSupport.Init(appKey, domain, appId, country);
            AIHelpSupport.SetOnAIHelpInitializedCallback(OnAIHelpInitializedCallback);
        }

        /// <summary>
        /// 初始化成功回调
        /// </summary>
        /// <param name="isSuccess"></param>
        /// <param name="message"></param>
        public void OnAIHelpInitializedCallback(bool isSuccess, string message)
        {
            Debug.LogError("AIHelp初始化成功 ： " + isSuccess + " | 消息： " + message);
            if (isSuccess)
            {
                //设置标记位
                isInitSuccess = true;
                Debug.LogError(string.Format("AIHelp初始化成功! 版本信息：{0}", GetSdkVersion()));
                //初始化log
                logData = AIHelpLogData.Instance;//获取数据
                logData.AddLog(DateTime.Now + " : 初始化成功!");//测试输出
                SetUploadLogPath(logData.FilePath);//设置log日志地址
                //设置用户信息
                AIHelpUser user = new AIHelpUser(SystemInfo.deviceUniqueIdentifier, "anonymous", "-1", "","");
                Debug.LogError("AIHelp用户信息："+user.ToString());
                SetUserInfo(user);
                //开始未读消息轮询
                StartUnreadMessageCountPolling();
                //更新SDK语言
                UpdateSDKLanguage(country);
            }
            else
            {
                //设置标记位
                isInitSuccess = false;
                Debug.LogError("AIHelp初始化失败!");
            }
        }

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="msg"></param>
        public void AddLog(string msg)
        {
            if (!isInitSuccess)
            {
                return;
            }
            logData.AddLog(msg);
        }

        /// <summary>
        /// 清除日志
        /// </summary>
        /// <param name="msg"></param>
        public void ClearLog()
        {
            if (!isInitSuccess)
            {
                return;
            }
            logData.ClearLog();
        }

        /// <summary>
        /// 更新SDk语言
        /// </summary>
        public void UpdateSDKLanguage(string country)
        {
            this.country = country;
            if (!isInitSuccess)
            {
                return;
            }
            Debug.LogError("更新SDk Language到" + country);
            AIHelpSupport.UpdateSDKLanguage(country);
        }

        /// <summary>
        /// 设置用户信息
        /// </summary>
        public void SetUserInfo(AIHelpUser user)
        {
            if (!isInitSuccess)
            {
                return;
            }
            if (user == null)
            {
                Debug.LogError("用户信息为null");
                return;
            }
            UserConfig userConfig = new UserConfig.Builder()
            .SetUserId(user.UserId)//可选参数 默认值：用户设备随机数  用户唯一标识，不可以设置为空字符串、0 或 -1
            .SetUserName(user.UserName)//可选参数 默认值：anonymous 用户名称
            .SetServerId(user.ServerId)//可选参数 默认值：-1 用户所在服务器ID
            .SetUserTags(user.UserTags)//可选参数 默认值:'' 用户标签，多个标签之间需要以「,」分隔。
            .SetCustomData(user.CustomData)//可选参数 格式：Json字符串
            .build();
            AIHelpSupport.UpdateUserInfo(userConfig);
        }

        /// <summary>
        /// 拉起入口1
        /// </summary>
        public void ShowEntranceID001()
        {
            if (!isInitSuccess)
            {
                return;
            }
            Debug.LogError("显示入口1");
            AIHelpSupport.Show(ConfigForEntranceID001);
        }

        /// <summary>
        /// 拉起单个FAQ
        /// </summary>
        private void ShowSingleFAQ()
        {
            if (!isInitSuccess)
            {
                return;
            }
            AIHelpSupport.ShowSingleFAQ(faqID, ConversationMoment.AFTER_MARKING_UNHELPFUL);
        }

        /// <summary>
        /// 当前显示的是否为AIHelp页面
        /// </summary>
        public bool IsAIHelpShow()
        {
            if (!isInitSuccess)
            {
                return false;
            }
            return AIHelpSupport.IsAIHelpShowing();
        }


        /// <summary>
        /// 方法内部每 5 分钟主动拉取一次当前用户的未读消息数量，并在以下两种情况时将结果返回给调用者：

        //1、有进行中客诉的用户收到新消息时，返回该用户累计未读的消息数量；

        //2、用户打开客服会话窗口时，返回数字 0 以标记用户已读当前消息。


        /// </summary>
        void StartUnreadMessageCountPolling()
        {
            if (!isInitSuccess)
            {
                return;
            }
            AIHelpSupport.StartUnreadMessageCountPolling(OnMessageCountArrivedCallback);
        }

        void OnMessageCountArrivedCallback(int msgCount)
        {
            Debug.LogError("AIHelp you have " + msgCount + " unread messages");
        }

      

        /// <summary>
        /// 接入上传日志接口后，系统会在合适的时机将接入方生成的日志上传到客诉中 系统将会在生成客诉时自动上传对应的日志文件
        /// AIHelp 目前只支持上传 .log / .bytes / .txt / .zip 格式的文件。
        /// </summary>
        public void SetUploadLogPath(string path)
        {
            if (!isInitSuccess)
            {
                return;
            }
            //AIHelpSupport.SetUploadLogPath("absolute/path/to/your/logFile.log");
            AIHelpSupport.SetUploadLogPath(path);
        }

        /// <summary>
        /// 关闭所有AIHelp页面
        /// </summary>
        public void Close()
        {
            if (!isInitSuccess)
            {
                return;
            }
            AIHelpSupport.Close();
        }

        /// <summary>
        /// 获取AIHelp版本信息
        /// </summary>
        /// <returns></returns>
        public string GetSdkVersion()
        {
            if (!isInitSuccess)
            {
                return "";
            }
            return AIHelpSupport.GetSDKVersion();
        }

        /// <summary>
        /// 利用此 API 来接管某个超链接的点击事件，具体配置方法如下：

        ///1、为超链接配置 js-bridge=enable 参数，如：https://www.google.com?js-bridge=enable

        ///2、设置回调来接管配置了 js-bridge=enable 的超链接的点击事件： 
        /// </summary>
        public void SetSpecificUrlClickedEvent(OnSpecificUrlClickedCallback callback)
        {
            if (!isInitSuccess)
            {
                return;
            }
            AIHelpSupport.SetOnSpecificUrlClickedCallback(callback);
        }

        /// <summary>
        /// 注册 AIHelp 页面打开的回调通知：
        /// </summary>
        public void SetOnAIHelpSessionOpenCallback(OnAIHelpSessionOpenCallback callback)
        {
            if (!isInitSuccess)
            {
                return;
            }
            Debug.LogError("设置AIHelp打开页面的回调通知");
            AIHelpSupport.SetOnAIHelpSessionOpenCallback(callback);
        }

        /// <summary>
        /// 注册 AIHelp 页面关闭的回调通知：
        /// </summary>
        public void SetOnAIHelpSessionCloseCallback(OnAIHelpSessionCloseCallback callback)
        {
            if (!isInitSuccess)
            {
                return;
            }
            Debug.LogError("设置AIHelp关闭页面的回调通知");
            AIHelpSupport.SetOnAIHelpSessionCloseCallback(callback);
        }



        /// <summary>
        /// 显示url吧
        /// </summary>
        public void ShowUrl(string url)
        {
            if (!isInitSuccess)
            {
                return;
            }
            Debug.LogError("打开url = " + url);
            AIHelpSupport.ShowUrl(url);

        }
        #region 第三方推送
        /// <summary>
        /// 设置第三方推送平台 AIHelp 支持五个推送平台，分别是 APNs、FireBase、极光、个推以及华为推送。
        /// </summary>
        public void SetPushTokenAndPlatform()
        {
            AIHelpSupport.SetPushTokenAndPlatform("第三方token", PushPlatform.FIREBASE);

        }
        #endregion

        /// <summary>
        /// 注册运营文章更新时的回调通知：unity没这个API
        /// </summary>
        public void SetOnOperationUnreadChangedCallback()
        {
            //java
            //AIHelpSupport.setOnOperationUnreadChangedCallback((hasUnreadArticles)-> {
            //    // do something you want according to the unread status received
            //});
        }


    }
}
