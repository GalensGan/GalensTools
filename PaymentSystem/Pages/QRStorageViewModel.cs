using GalensSDK.Enumerable;
using Panuon.UI.Silver;
using PaymentSystem.Database;
using PaymentSystem.Datas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace PaymentSystem.Pages
{ 
    class QRStorageViewModel:ScreenChild
    {
        public QRStorageViewModel(Store store) : base(store) { }

        public BindingSource BindingSource { get; set; }
        protected override void OnInitialActivate()
        {
            // 从服务器获取数据            
            List<QRCodeStorage> storages = Store.DataBase.GetStorages();
            BindingSource = new BindingSource
            {
                DataSource = storages.ConvertToDt(),
            };

            base.OnInitialActivate();
        }

        public void Add()
        {
            Store.MainWindow.IsMaskVisible = true;

            Store.WindowManager.ShowDialog(new QRStorage_NewViewModel(Store));

            Store.MainWindow.IsMaskVisible = false;

            // 重新拉取仓库
            List<QRCodeStorage> storages = Store.DataBase.GetStorages();
            BindingSource.DataSource = storages.ConvertToDt();
        }

        public void Edit(DataRowView dataRowView)
        {
            Store.MainWindow.IsMaskVisible = true;

            Store.WindowManager.ShowDialog(new QRStorage_NewViewModel(Store,dataRowView));

            Store.MainWindow.IsMaskVisible = false;
        }

        public void Delete(DataRowView dataRowView)
        {
            string storageName = dataRowView.Row[Key.name.ToString()].ToString();
            MessageBoxResult result = MessageBoxX.Show("是否删除仓库: " + storageName, "删除确认", null, System.Windows.MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.Cancel) return;

            // 开始删除
            Store.DataBase.DeleteStorage(storageName);
            dataRowView.Delete();
        }
       

        public void Detail(DataRowView dataRowView)
        {
            // 跳转到二维码仓库详细
            this.ActiveScreen?.Invoke(Key.qRStore_Detail.ToString(), new Parameter() { Command = Command.None, Arg = dataRowView.Row[Key.name.ToString()].ToString()});
        }

        #region 过滤
        public string FilterText { get; set; }
        public void Filter()
        {
            // 获取所有的列头
            string sql = string.Empty;
            List<string> names = typeof(QRCodeStorage).GetProperties().ToList().ConvertAll(item => item.Name);
            names.Remove(Key.dateTime.ToString());

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
    }
}
