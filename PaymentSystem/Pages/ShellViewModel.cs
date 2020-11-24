using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.OpenXmlFormats.Dml;
using Panuon.UI.Silver;
using Panuon.UI.Silver.Core;
using PaymentSystem.Database;
using PaymentSystem.Datas;
using Stylet;

namespace PaymentSystem.Pages
{
    /// <summary>
    /// 主窗体
    /// </summary>
    public class ShellViewModel : Conductor<Screen>.Collection.OneActive
    {
        #region 属性
        private bool _isUser = false;
        public bool IsUser
        {
            get => _isUser;
            set
            {
                base.SetAndNotify(ref _isUser, value);
                if (value) ActiveItemByDisplayName(Key.user_main.ToString());
            }
        }

        private bool _isQRCode = false;
        public bool IsQRCode
        {
            get => _isQRCode;
            set
            {
                base.SetAndNotify(ref _isQRCode, value);
                if (value) ActiveItemByDisplayName(Key.qRStore.ToString());
            }
        }

        private bool _isPayTask = false;
        public bool IsPayTask
        {
            get => _isPayTask;
            set
            {
                SetAndNotify(ref _isPayTask, value);
                if (value) ActiveItemByDisplayName(Key.payTask.ToString());
            }
        }


        private bool _isDashboard = true;
        public bool IsDashboard
        {
            get => _isDashboard;
            set
            {
                base.SetAndNotify(ref _isDashboard, value);
                if (value) ActiveItemByDisplayName("个人中心");
            }
        }




        private bool _isImportData;
        public bool IsImportData
        {
            get => _isImportData;
            set
            {
                base.SetAndNotify(ref _isImportData, value);
                if (value) ActiveItemByDisplayName("导入数据");
            }
        }

        private bool _isTemplate = false;
        public bool IsTemplate
        {
            get => _isTemplate;
            set
            {
                base.SetAndNotify(ref _isTemplate, value);
                if (value) ActiveItemByDisplayName("模板");
            }
        }

        private bool _isSend = false;
        public bool IsSend
        {
            get => _isSend;
            set
            {
                base.SetAndNotify(ref _isSend, value);
                if (value) ActiveItemByDisplayName("发送");
            }
        }

        private bool _isLog = false;
        public bool IsLog
        {
            get => _isLog;
            set
            {
                SetAndNotify(ref _isLog, value);
                if (value) ActiveItemByDisplayName("日志");
            }
        }

        private bool _isAboutMe = false;
        public bool IsAboutMe
        {
            get => _isAboutMe;
            set
            {
                SetAndNotify(ref _isAboutMe, value);
                if (value) ActiveItemByDisplayName("关于");
            }
        }

        public Store Store { get; private set; }

        public string Version
        {
            get
            {
                return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }
        #endregion

        private void ActiveItemByDisplayName(string displayName, Parameter arg = null)
        {
            Screen screen = this.Items.Where(item => item.DisplayName == displayName).FirstOrDefault();
            if (screen != null)
            {
                if (screen is IScreenNotify sn)
                {
                    sn.SetParameter(arg);
                }
                this.ActivateItem(screen);
            }
        }


        public ShellViewModel(IWindowManager windowManager)
        {
            Store = Store.GetConfigStore();
            Store.WindowManager = windowManager;
            // 连接数据库
            Store.DataBase = new Mongodb(Store.GetConnectionString(), Store.databaseName);
        }

        protected override void OnClose()
        {
            base.OnClose();
            if (Store.isAutoSave)
            {
                Store.Save();
                return;
            }
            // 保存Store数据
            System.Windows.MessageBoxResult dialogResult = MessageBoxX.Show("是否保存数据？", "保存", null, MessageBoxButton.YesNoCancel,
                new MessageBoxXConfigurations() { CancelButton = "自动保存" });
            if (dialogResult == MessageBoxResult.Yes)
            {
                Store.Save();
            }
            else if (dialogResult == MessageBoxResult.Cancel)
            {
                // 自动保存
                Store.isAutoSave = true;
                Store.Save();
            }
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();

            // 保存view
            Store.MainWindow = this.View as WindowX;

            // 另起一个线程检查更新
            Thread thread = new Thread(() =>
            {
                CheckVersion();
            });
            thread.IsBackground = true;
            thread.Start();
        }

        protected override void OnInitialActivate()
        {
            Screen loginVM = new LoginViewModel(Store);
            Store.WindowManager.ShowDialog(loginVM);

            // 初始化其它界面
            InitializeViewModel(new User_MainViewModel(Store)
            {
                DisplayName = Key.user_main.ToString(),
            });

            InitializeViewModel(new User_EditViewModel(Store)
            {
                DisplayName = Key.user_new.ToString(),
                Header = "新增用户",
            });

            InitializeViewModel(new User_EditViewModel(Store)
            {
                DisplayName = Key.user_edit.ToString(),
                Header = "编辑用户",
            });

            InitializeViewModel(new QRStorageViewModel(Store)
            {
                DisplayName = Key.qRStore.ToString(),
            });

            InitializeViewModel(new QRStorage_DetailViewModel(Store)
            {
                DisplayName = Key.qRStore_Detail.ToString(),
            });


            InitializeViewModel(new PayTaskViewModel(Store)
            {
                DisplayName = Key.payTask.ToString(),
            });

            InitializeViewModel(new PayTask_NewViewModel(Store)
            {
                DisplayName = Key.payTask_New.ToString(),
            });

            // 激活
            ActiveItemByDisplayName(Key.user_main.ToString());
        }

        private void InitializeViewModel(Screen screenChild)
        {
            if (screenChild is IScreenNotify sn) sn.ActiveScreen += ActiveItemByDisplayName;

            this.Items.Add(screenChild);
        }

        private async void CheckVersion()
        {
            // 从服务器获取更新的json文件
            string version = Store.DataBase.GetCurrentVersion();
            // 比较版本号
            System.Version serviceVersion = new Version(version);
            System.Version currentVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            if (serviceVersion > currentVersion)
            {

                // 显示到界面
                NewVersion = serviceVersion.ToString();
                IsNewVersion = Visibility.Visible;
            }
        }

        #region 新版本号
        public Visibility IsNewVersion { get; set; } = Visibility.Collapsed;
        public string NewVersion { get; set; } = string.Empty;

        public void DownLoadNewVersion()
        {
            // 下载version信息
            VersionInfo versionInfo = Store.DataBase.GetCurrentVersionInfo();
            // 跳转下载连接
            System.Diagnostics.Process.Start("explorer.exe", versionInfo.url);
        }
        #endregion
    }
}
