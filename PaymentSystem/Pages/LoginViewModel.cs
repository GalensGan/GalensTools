using GalensSDK.Encrypt;
using Panuon.UI.Silver;
using PaymentSystem.Database;
using PaymentSystem.Datas;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PaymentSystem.Pages
{
    public class LoginViewModel : ScreenChild
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsRememberLoginInfo { get; set; } = false;

        public LoginViewModel(Store store) : base(store)
        {
            IsRememberLoginInfo = store.isRememberLoginInfo;

            // 加载上次登陆的用户
            if (store.isRememberLoginInfo && store.lastUser!=null)
            {
                // 读取上一次的数据
                this.UserName = store.lastUser.userId;
                this.Password = store.lastUser.password;
            }
        }

        public void PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox pb = sender as PasswordBox;
            Password = pb.Password;
        }

        private bool ValidateAccount()
        {
            if (string.IsNullOrEmpty(UserName))
            {
                MessageBoxX.Show("请输入用户名", "温馨提示");
                return false;
            }

            if (string.IsNullOrEmpty(Password))
            {
                MessageBoxX.Show("请输入密码", "温馨提示");
                return false;
            }
            return true;
        }

        public void Login()
        {
            if (!ValidateAccount()) return;

            // 从数据库获取用户
            User user = Store.DataBase.FindUser(this.UserName);

            if (user == null)
            {
                MessageBoxX.Show("用户名不存在，请联系管理员", "用户不存在");
                return;
            }

            if(user.password != MD5Ex.EncryptString(this.Password))
            {
                MessageBoxX.Show("密码有误，请重新输入", "密码错误");
                return;
            }

            // 记住登陆的账户
            if (this.IsRememberLoginInfo)
            {
                Store.isRememberLoginInfo = this.IsRememberLoginInfo;
                Store.lastUser = new User()
                {
                    userId = this.UserName,
                    password = this.Password
                };
                Store.Save();
            }

            Store.currentUser = user;
            this.RequestClose(true);
        }

        

        // 退出
        public void Quite()
        {
            System.Environment.Exit(0);
        }
    }
}
