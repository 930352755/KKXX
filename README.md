# DebugSystem
DebugSystem
使用方法简介，直接注册，按钮或者Text
注册方法案例
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

