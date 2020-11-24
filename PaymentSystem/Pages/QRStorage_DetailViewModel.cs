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
    class QRStorage_DetailViewModel:ScreenChild
    {
        public QRStorage_DetailViewModel(Store store) : base(store) { }

        private string _storageName;
        protected override void HandleParameter(Parameter parameter)
        {
            _storageName = parameter.Arg as string;
            // 从服务器获取数据            
            List<QRCode> result = Store.DataBase.GetQRCodes(_storageName);
            BindingSource.DataSource = result.ConvertToDt();
        }

        public BindingSource BindingSource { get; set; } = new BindingSource();

        public void Back()
        {
            this.ActiveScreen?.Invoke(Key.qRStore.ToString(), null);
        }

        public void Add()
        {
            Store.MainWindow.IsMaskVisible = true;

            Store.WindowManager.ShowDialog(new QR_EditViewModel(Store, _storageName));

            Store.MainWindow.IsMaskVisible = false;

            // 重新拉取仓库
            List<QRCode> storages = Store.DataBase.GetQRCodes(_storageName);
            BindingSource.DataSource = storages.ConvertToDt();
        }

        public void Edit(DataRowView dataRowView)
        {
            Store.MainWindow.IsMaskVisible = true;

            Store.WindowManager.ShowDialog(new QR_EditViewModel(Store, dataRowView));

            Store.MainWindow.IsMaskVisible = false;
        }

        public void Delete(DataRowView dataRowView)
        {
            string name = dataRowView.Row[Key.name.ToString()].ToString();
            MessageBoxResult result = MessageBoxX.Show("是否二维码: " + name, "删除确认", null, System.Windows.MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.Cancel) return;

            // 开始删除
            Store.DataBase.DeleteQRCode(_storageName,name);
            dataRowView.Delete();
        }


        public void Detail(DataRowView dataRowView)
        {
            // 查看具体的二维码
        }

        #region 过滤
        public string FilterText { get; set; }
        public void Filter()
        {
            // 获取所有的列头
            string sql = string.Empty;
            List<string> names = typeof(QRCode).GetProperties().ToList().ConvertAll(item => item.Name);
            names.Remove(Key.acount.ToString());
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
