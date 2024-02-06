using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class AudioControl
    {

        private static AudioControl instance = null;
        public static AudioControl Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AudioControl();
                }
                return instance;
            }
        }
        private AudioControl() { }


        #region 快速数据处理
        /// <summary>
        /// BGM 初始播放的BGM
        /// </summary>
        public readonly static string BGM = "BGM";
        /// <summary>
        /// 按钮声音的名字
        /// </summary>
        public readonly static string ButtonAudio = "ButtonClick";
        /// <summary>
        /// 关闭按钮的音效
        /// </summary>
        public readonly static string ButtonClose = "ButtonClose";
        /// <summary>
        /// 按钮事件的名字
        /// </summary>
        public readonly static string ButtonEvent = "ButtonClick";

        /// <summary>
        /// 音效的路径
        /// (用的Addressable加载资源，填写根目录后的路径)
        /// </summary>
        public const string AudioSoundPath = "Audios/";
        /// <summary>
        /// 音乐的路径
        /// (用的Addressable加载资源，填写根目录后的路径)
        /// </summary>
        public const string AudioMusicPath = "Audios/BGM/";
        #endregion


        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="key">音效的Key,或者直接写声音的名字,要符合路径</param>
        /// <param name="isLoop">是否循环播放</param>
        public void PlaySound(string key, bool isLoop = false)
        {
            AudioManager.Instance.PlaySound(key, isLoop);
        }
        /// <summary>
        /// 停止一个循环音效
        /// </summary>
        /// <param name="key"></param>
        public void StopSound(string key)
        {
            AudioManager.Instance.StopSound(key);
        }
        /// <summary>
        /// 改变一个BGM
        /// </summary>
        /// <param name="key"></param>
        public void ChangeBGM(string key)
        {
            AudioManager.Instance.ChangeBGM(key);
        }


        /// <summary>
        /// BGM控制属性(可读写)
        /// </summary>
        public bool ISPlayMusic
        {
            get
            {
                return AudioManager.Instance.ISPlayMusic;
            }
            set
            {
                AudioManager.Instance.ISPlayMusic = value;
            }
        }

        /// <summary>
        /// 音效控制属性(可读写)
        /// </summary>
        public bool ISPlaySound
        {
            get
            {
                return AudioManager.Instance.ISPlaySound;
            }
            set
            {
                AudioManager.Instance.ISPlaySound = value;
            }
        }

        /// <summary>
        /// 背景音乐大小的控制
        /// </summary>
        public float MusicVolume
        {
            get
            {
                return AudioManager.Instance.MusicVolume;
            }
            set
            {
                AudioManager.Instance.MusicVolume = value;
            }
        }

        /// <summary>
        /// 音效大小的控制
        /// </summary>
        public float SoundVolume
        {
            get
            {
                return AudioManager.Instance.SoundVolume;
            }
            set
            {
                AudioManager.Instance.SoundVolume = value;
            }
        }

    }
}