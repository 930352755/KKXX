using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class DebugView : MonoBehaviour, IBeginDragHandler, IDragHandler
    {
        #region 日志Test事件
        private Reporter reporter = null;

        private void LogConsole()
        {
            if (reporter == null)
            {
                GameObject reporterGo = Resources.Load<GameObject>("DebugPrefab/Reporter");
                reporter = Instantiate(reporterGo).GetComponent<Reporter>();
            }
            reporter.show = false;
        }
        #endregion

        #region 数据
        [SerializeField]
        private Text _name;
        [SerializeField]
        private Button _BtnDebug;
        [SerializeField]
        private GameObject _Content;
        [SerializeField]
        private GameObject _BtnTemplate;
        [SerializeField]
        private GameObject _Mask;
        #endregion

        public void Init()
        {

            this._name.text = "作弊器";
            _BtnDebug.onClick.AddListener(OnClickDebug);

            //一开始便把Repot给弄出来
            if (reporter == null)
            {
                this.LogConsole();
            }
            DebugSet.RegistDebugMenu(new DebugSet.DebugMenu[] {
                new DebugSet.DebugMenu() {
                    Text = "日志开关",
                    Type = DebugSet.DebugMenuType.Button,
                    OnClick = (ref DebugSet.DebugMenu menu) =>
                    {
                        if (reporter == null)
                        {
                            this.LogConsole();
                        }
                        reporter.Show(() => {
                            _Mask.SetActive(false);
                        });
                        _Mask.SetActive(true);
                    }
                },
                new DebugSet.DebugMenu() {
                    Text = SystemInfo.deviceUniqueIdentifier,
                    Type = DebugSet.DebugMenuType.Text,
                    OnClick = (ref DebugSet.DebugMenu menu) =>
                    {
                        menu.Text = SystemInfo.deviceUniqueIdentifier;
                    }
                },
            });
            _Content.SetActive(false);

        }
        private void OnClickDebug()
        {
            InitialAllDebugButton();
            _Content.SetActive(!_Content.activeSelf);
        }

        private bool isInitialAllDebugButton = false;
        /// <summary>
        /// 第一次展示时调用，生成所有的测试按钮
        /// </summary>
        private void InitialAllDebugButton()
        {
            if (isInitialAllDebugButton) return;

            GameObject obj = null;
            for (int i = 0; i < DebugSet.MenuList.Count; i++)
            {
                var item = DebugSet.MenuList[i];
                obj = obj == null ? this._BtnTemplate : GameObject.Instantiate(this._BtnTemplate, this._BtnTemplate.transform.parent);
                Text text = obj.transform.GetChild(0).GetComponent<Text>();
                text.text = item.Text;

                if (item.Type == DebugSet.DebugMenuType.Button)
                {
                    obj.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        item.OnClick(ref item);
                        text.text = item.Text; // NOTE: 放在这里是因为回调中可能会更新文字
                    });
                }
                else if (item.Type == DebugSet.DebugMenuType.Switch)
                {
                    text.text = item.Text + ":" + item.Value;
                    obj.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        bool old = (bool)item.Value;
                        item.Value = (object)!old;
                        item.OnClick(ref item);
                        text.text = item.Text + ":" + item.Value;
                    });
                }
            }

            isInitialAllDebugButton = true;
        }

        private Vector3 dir;
        public void OnBeginDrag(PointerEventData eventData)
        {
            dir = (Vector2)_BtnDebug.transform.position - eventData.position;
        }
        public void OnDrag(PointerEventData eventData)
        {
            SetUILocaPos(_BtnDebug.transform, transform, eventData.position, eventData.pressEventCamera, dir);
        }

        /// <summary>
        /// 设置UI中的位置
        /// 湿滑的实现拖拽操作。
        /// </summary>
        /// <param name="self"></param>
        /// <param name="parentTransform">UI父物体</param>
        /// <param name="screenTargetPos">UI鼠标点的位置</param>
        /// <param name="camera">相机</param>
        /// <param name="dir">偏移量</param>
        public static void SetUILocaPos(Transform self, Transform parentTransform, Vector2 screenTargetPos, Camera camera, Vector2 dir)
        {
            Vector2 locaPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentTransform as RectTransform, screenTargetPos, camera, out locaPos);
            self.localPosition = locaPos + dir;
        }
    }
}