using GalensSDK.Enumerable;
using Panuon.UI.Silver;
using PaymentSystem.Database;
using PaymentSystem.Datas;
using Stylet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace PaymentSystem.Pages
{
    class User_MainViewModel : ScreenChild
    {
        public User_MainViewModel(Store store) : base(store) { }

        public BindingSource BindingSource { get; set; }
        protected override void OnInitialActivate()
        {
            // 从服务器获取数据            
            List<User> users = Store.DataBase.GetUsers();
            BindingSource = new BindingSource
            {
                DataSource = users.ConvertToDt(),
            };            

            base.OnInitialActivate();
        }

        protected override void HandleParameter(Parameter arg)
        {
            // 当参数传递进来后，需要进行处理
            if (arg.Command == Command.InsertUser && arg.Arg is User user)
            {
                // 添加一行数据
               if( this.BindingSource.DataSource is DataTable dt)
                {
                    DataRow row = dt.NewRow();
                    row.CopyFrom(user);
                    dt.Rows.Add(row);
                }
            }
        }

        public void EditUser(DataRowView dataRowView)
        {
            ActiveScreen?.Invoke(Key.user_edit.ToString(), new Parameter()
            {
                Command = Command.EditUser,
                Arg = dataRowView
            });
        }

        public void DeleteUser(DataRowView dataRowView)
        {
            MessageBoxResult result = MessageBoxX.Show(string.Format("是否删除用户:{0}", dataRowView.Row[Key.name.ToString()]), "删除账户",null,MessageBoxButton.OKCancel);
            if (result != MessageBoxResult.OK) return;

            // 删除数据库
            Store.DataBase.DeleteUser(dataRowView.Row[Key.userId.ToString()].ToString());

            // 删除视图
            dataRowView.Delete();
        }

        public void ForbiddenUser(DataRowView dataRowView)
        {
            MessageBoxResult result = MessageBoxX.Show(string.Format("是否禁用用户:{0}", dataRowView.Row[Key.name.ToString()]), "禁用账户",null,MessageBoxButton.OKCancel);
            if (result != MessageBoxResult.OK) return;

            User user = dataRowView.Row.ConvertToModel<User>();
            user.forbidden = true;
            Store.DataBase.UpdateUser(user);
            dataRowView.Row[Key.forbidden.ToString()] = true;
        }

        public void AllowUser(DataRowView dataRowView)
        {
            User user = dataRowView.Row.ConvertToModel<User>();
            user.forbidden = false;
            Store.DataBase.UpdateUser(user);
            dataRowView.Row[Key.forbidden.ToString()] = false;
        }


        #region 过滤
        public string FilterText { get; set; }
        public void Filter()
        {
            // 获取所有的列头
            string sql = string.Empty;
            List<string> names = typeof(User).GetProperties().ToList().ConvertAll(item => item.Name);
            names.Remove(Key.forbidden.ToString());

            for (int i = 0; i < names.Count; i++)
            {
                if (i == 0)
                {
                    sql = string.Format("{0} LIKE '*{1}*'", names[i], FilterText);
                }
                else sql += string.Format(" OR {0} LIKE '*{1}*'", names[i], FilterText);
            }

            BindingSource.Filter = sql;
        }
        #endregion

        public void AddUser()
        {
            this.ActiveScreen?.Invoke(Key.user_new.ToString(), new Parameter() { Command = Command.NewUser });
        }

        public void AddUsers()
        {

        }
    }
}
