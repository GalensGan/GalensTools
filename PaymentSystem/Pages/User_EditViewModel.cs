using GalensSDK.Encrypt;
using GalensSDK.Enumerable;
using Panuon.UI.Silver;
using PaymentSystem.Database;
using PaymentSystem.Datas;
using Stylet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PaymentSystem.Pages
{
    class User_EditViewModel : ScreenChild
    {
        public bool IsNew => DisplayName == Key.user_new.ToString();

        public string Header { get; set; }

        public User User { get; set; } = new User();

        public BindableCollection<string> Genders { get; set; } = new Stylet.BindableCollection<string>() { "男", "女" };
        public string Gender
        {
            get => User.gender;
            set
            {
                User.gender = value;
                base.NotifyOfPropertyChange(() => this.Gender);
            }
        }

        public Visibility IsShowReset
        {
            get
            {
                if (IsNew) return Visibility.Collapsed;
                else return Visibility.Visible;
            }
        }

        public string AccessString
        {
            get => User.access;
            set
            {
                User.access = value;
                base.NotifyOfPropertyChange(() => this.AccessString);
                Gender = "女";
            }
        }

        public User_EditViewModel(Store store) : base(store) { }

        private DataRowView _dataRow;
        protected override void HandleParameter(Parameter arg)
        {
            if (arg.Command == Command.EditUser)
            {
                _dataRow = arg.Arg as DataRowView;

                // 将要编辑的user属性复制过去
                User = _dataRow.Row.ConvertToModel<User>();
            }

            if (arg.Command == Command.NewUser)
            {
                User = new User();
            }
        }

        /// <summary>
        /// 返回
        /// </summary>
        public void Back()
        {
            ActiveScreen?.Invoke(Key.user_main.ToString(), null);
        }

        // 保存
        public void Save()
        {
            // 更新到数据库
            Store.DataBase.UpdateOrInsertUser(User);

            // 更新到显示列表
            if (IsNew)
            {
                // 如果是新增，就需要添加用户到列表中
                ActiveScreen?.Invoke(Key.user_main.ToString(), new Parameter() { Command = Command.InsertUser, Arg = User });
            }
            else
            {
                _dataRow.Row.CopyFrom(User);
                // 跳转到主界面
                ActiveScreen?.Invoke(Key.user_main.ToString(), null);
            }
        }

        public void Reset()
        {
            MessageBoxResult result = MessageBoxX.Show("是否重置该账户密码", "密码重置", null, MessageBoxButton.OKCancel);
            if (result != MessageBoxResult.OK) return;

            User user = this._dataRow.Row.ConvertToModel<User>();
            user.password = MD5Ex.EncryptString(Store.defaultPassword);
            bool updateResult = Store.DataBase.UpdateUser(user);
            if (updateResult)
            {
                MessageBoxX.Show("密码重置成功!", "重置结果");
            }
        }
    }
}
