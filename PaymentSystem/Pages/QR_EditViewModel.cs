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

namespace PaymentSystem.Pages
{
    class QR_EditViewModel:ScreenChild
    {
        public QRCode QRCode { get; set; }

        private bool _isNew = true;

        public string Header { get; set; }

        // 添加
        public QR_EditViewModel(Store store,string storageName) : base(store)
        {
            QRCode = new QRCode()
            {
                storageName = storageName,
            };
            _isNew = true;
            Header = "新增";
        }

        private DataRowView _dataRowView;
        // 编辑
        public QR_EditViewModel(Store store, DataRowView row) : base(store)
        {
            _dataRowView = row;
            QRCode = row.Row.ConvertToModel<QRCode>();
            _isNew = false;
            Header = "编辑";
        }

        public void Save()
        {
            if (_isNew)
            {
                // 新增二维码
                QRCode findStorage = this.Store.DataBase.FindQRCode(QRCode.storageName,QRCode.name);
                if (findStorage != null)
                {
                    MessageBoxX.Show("当前二维码已经存在，请更改二维码名称", "名称重复");
                    return;
                }

                Store.DataBase.InsertQRCode(QRCode);
            }
            else
            {
                // 编辑仓库
                Store.DataBase.UpdateOrInsertQRCode(QRCode);

                // 将仓库信息更新
                _dataRowView.Row.CopyFrom(QRCode);
            }

            this.RequestClose(true);
        }

        public void Cancle()
        {
            this.RequestClose(false);
        }
    }
}
