using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game
{
    /// <summary>
    ///  20220922
    ///  简易得声音管理系统
    ///  不用挂载,Loading加载时调用。
    /// </summary>
    public class AudioManager : MonoBehaviour
    {

        #region 对外部使用

        /// <summary>
        /// 第一个场景加载之后调用
        /// </summary>
        [RuntimeInitializeOnLoadMethod]
        private static void OnRuntimeMethodLoad()
        {
            Instance.StartAudioSystem();
        }

        /// <summary>
        /// 起始点
        /// 启动脚本
        /// 启动系统必须调用的方法Load加载中调用
        /// </summary>
        private void StartAudioSystem()
        {
            ChangeBGM(AudioControl.BGM);
            Debug.Log("声音系统初始化完成");
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="key">音效配置表中的Key,或者直接写声音的名字</param>
        /// <param name="isLoop">是否循环播放</param>
        public void PlaySound(string key, bool isLoop = false)
        {
            AudioData audioData = GetAudioData(key);
            string name = audioData.name;
            if (isLoop)
            {
                PlayLoopSound(name, SoundVolume * audioData.volume);
            }
            else
            {
                PlaySound(name, SoundVolume * audioData.volume);
            }
        }

        public void StopSound(string key)
        {
            AudioData audioData = GetAudioData(key);
            string name = audioData.name;
            StopLoopSound(name);
        }

        public void ChangeBGM(string key)
        {
            AudioData audioData = GetAudioData(key);
            string name = audioData.name;
            MusicVolume = audioData.volume;

            ISPlayMusic = false;
            curBGM = name;
            ISPlayMusic = true;
        }

        /// <summary>
        /// BGM控制属性(可读写)
        /// </summary>
        public bool ISPlayMusic
        {
            get
            {
                return isPlayMusic;
            }
            set
            {
                isPlayMusic = value;
                if (value)
                {
                    PlayMusic(curBGM, MusicVolume);
                    PlayerPrefs.SetInt("isPlayMusic", 1);
                }
                else
                {
                    StopMusic();
                    PlayerPrefs.SetInt("isPlayMusic", 0);
                }
            }
        }

        /// <summary>
        /// 音效控制属性(可读写)
        /// </summary>
        public bool ISPlaySound
        {
            get
            {
                return isPlaySound;
            }
            set
            {
                isPlaySound = value;
                if (value)
                {
                    PlayerPrefs.SetInt("isPlaySound", 1);
                }
                else
                {
                    PlayerPrefs.SetInt("isPlaySound", 0);
                    StopAllLoopSound();
                }
            }
        }

        /// <summary>
        /// 背景音乐大小的控制
        /// </summary>
        public float MusicVolume
        {
            get
            {
                return PlayerPrefs.GetFloat("MusicVolume", 1f);
            }
            set
            {
                PlayerPrefs.SetFloat("MusicVolume", value);
                if (aSMusic != null)
                {
                    aSMusic.volume = value;
                }
            }
        }

        /// <summary>
        /// 音效大小的控制
        /// </summary>
        public float SoundVolume
        {
            get
            {
                return PlayerPrefs.GetFloat("SoundVolume", 1f);
            }
            set
            {
                PlayerPrefs.SetFloat("SoundVolume", value);
            }
        }

        #endregion

        #region 单例化

        private static AudioManager instance = null;

        public static AudioManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject ins = new GameObject("AudioManager");
                    GameObject.DontDestroyOnLoad(ins);
                    instance = ins.AddComponent<AudioManager>();
                    instance.AAwake();
                }
                return instance;
            }
        }

        #endregion

        #region 声音数据表
        public class AudioData
        {
            public string name;
            public string des;
            public float volume;
        }
        public AudioData GetAudioData(string key)
        {
            AudioData audioData = new AudioData();
            audioData.name = key;
            audioData.volume = 1;
            return audioData;
        }
        #endregion

        #region 数据

        /// <summary>
        /// 当前的BGM
        /// </summary>
        private string curBGM;

        private bool isPlayMusic;

        private bool isPlaySound;

        #endregion

        #region 初始化

        /// <summary>
        /// 初始化
        /// </summary>
        private void AAwake()
        {
            isPlayMusic = PlayerPrefs.GetInt("isPlayMusic", 1) == 1;
            isPlaySound = PlayerPrefs.GetInt("isPlaySound", 1) == 1;
        }

        #endregion

        #region 控制

        /// <summary>
        /// 所有音乐得池子
        /// </summary>
        private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

        #region 普通音效

        /// <summary>
        /// 非循环音效池子
        /// </summary>
        private List<AudioSource> soundPool = new List<AudioSource>();

        /// <summary>
        /// 播放一个非循环音效
        /// </summary>
        /// <param name="name">名字</param>
        /// <param name="volume">声音大小默认为1</param>
        private void PlaySound(string name, float volume = 1f)
        {
            if (!audioClips.ContainsKey(name))
            {
                UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<AudioClip> async = Addressables.LoadAssetAsync<AudioClip>(AudioControl.AudioSoundPath + name);
                async.Completed += x =>
                {
                    if (!audioClips.ContainsKey(name))
                    {
                        AudioClip ac = async.Result;
                        if (ac == null)
                        {
                            Debug.LogError(AudioControl.AudioSoundPath+" 下没有" + name);
                            return;
                        }
                        audioClips.Add(name, ac);
                    }
                    PlaySound(name, volume);
                };
                return;
            };
            if (isPlaySound)
            {
                AudioSource aS = GetASound();
                aS.clip = audioClips[name];
                aS.volume = volume;
                aS.loop = false;
                aS.Play();
                RecycleSound(aS, audioClips[name].length);
            }
        }

        private AudioSource GetASound()
        {
            if (soundPool.Count > 0)
            {
                AudioSource aS = soundPool[0];
                soundPool.RemoveAt(0);
                aS.gameObject.SetActive(true);
                return aS;
            }
            else
            {
                AudioSource aS = (new GameObject("ASound")).AddComponent<AudioSource>();
                aS.transform.parent = transform;
                return aS;
            }
        }

        private void RecycleSound(AudioSource aS, float time)
        {
            StartCoroutine(DelayedCall(() =>
            {
                aS.gameObject.SetActive(false);
                soundPool.Add(aS);
            }, time));
        }

        private IEnumerator DelayedCall(System.Action action, float time)
        {
            yield return new WaitForSecondsRealtime(time);
            if (action != null)
            {
                action();
            }
        }

        #endregion

        #region 循环音效

        /// <summary>
        /// 循环音效的池子
        /// </summary>
        private Dictionary<string, AudioSource> soundLoopPool = new Dictionary<string, AudioSource>();

        /// <summary>
        /// 播放一个循环的音效
        /// </summary>
        /// <param name="name">名字</param>
        /// <param name="volume">声音大小默认为1</param>
        private void PlayLoopSound(string name, float volume = 1f)
        {
            if (!audioClips.ContainsKey(name))
            {
                UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<AudioClip> async = Addressables.LoadAssetAsync<AudioClip>(AudioControl.AudioSoundPath + name);
                async.Completed += x =>
                {
                    AudioClip ac = async.Result;
                    if (ac == null)
                    {
                        Debug.LogError(AudioControl.AudioSoundPath+" 下没有" + name);
                        return;
                    }
                    audioClips.Add(name, ac);
                    PlayLoopSound(name, volume);
                };
                return;
            };
            if (isPlaySound)
            {

                if (soundLoopPool.TryGetValue(name, out AudioSource aS))
                {
                    if (aS.isPlaying) return;
                    aS.gameObject.SetActive(true);
                    aS.clip = audioClips[name];
                    aS.volume = volume;
                    aS.loop = true;
                    aS.Play();
                }
                else
                {
                    AudioSource aSM = (new GameObject("LoopSound")).AddComponent<AudioSource>();
                    aSM.transform.parent = transform;
                    aSM.clip = audioClips[name];
                    aSM.volume = volume;
                    aSM.loop = true;
                    aSM.Play();
                    soundLoopPool.Add(name, aSM);
                }
            }
        }

        /// <summary>
        /// 停止一个循环音效
        /// </summary>
        /// <param name="name"></param>
        private void StopLoopSound(string name)
        {
            if (soundLoopPool.TryGetValue(name, out AudioSource aS))
            {
                aS.Stop();
                aS.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 停止所有循环音效
        /// </summary>
        private void StopAllLoopSound()
        {
            foreach (AudioSource aS in soundLoopPool.Values)
            {
                aS.Stop();
                aS.gameObject.SetActive(false);
            }
        }

        #endregion

        #region BGM
        /// <summary>
        /// BGM
        /// </summary>
        private AudioSource aSMusic = null;
        private void PlayMusic(string name, float volume = 1f)
        {
            if (!audioClips.ContainsKey(name))
            {
                UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<AudioClip> async = Addressables.LoadAssetAsync<AudioClip>(AudioControl.AudioMusicPath + name);
                async.Completed += x =>
                {
                    AudioClip ac = async.Result;
                    if (ac == null)
                    {
                        Debug.LogError(AudioControl.AudioMusicPath +" 下没有" + name);
                        return;
                    }
                    audioClips.Add(name, ac);
                    PlayMusic(name, volume);
                };
                return;
            };
            if (aSMusic != null)
            {
                if (aSMusic.isPlaying) return;
                aSMusic.gameObject.SetActive(true);
                aSMusic.clip = audioClips[name];
                aSMusic.volume = volume;
                aSMusic.loop = true;
                aSMusic.Play();
            }
            else
            {
                aSMusic = (new GameObject("Music")).AddComponent<AudioSource>();
                aSMusic.transform.parent = transform;
                aSMusic.clip = audioClips[name];
                aSMusic.volume = volume;
                aSMusic.loop = true;
                aSMusic.Play();
            }
        }

        private void StopMusic()
        {
            if (aSMusic != null)
            {
                aSMusic.Stop();
                aSMusic.gameObject.SetActive(false);
            };
        }

        #endregion

        #endregion
    }
}