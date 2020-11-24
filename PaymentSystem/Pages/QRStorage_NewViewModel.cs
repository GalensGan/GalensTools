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
    class QRStorage_NewViewModel:ScreenChild
    {
        public QRCodeStorage Storage { get; set; }

        private bool _isNew = true;

        public string Header { get; set; }

        public QRStorage_NewViewModel(Store store) : base(store) 
        {
            Storage = new QRCodeStorage()
            {
                createUserId = store.currentUser.userId,
                createUserName = store.currentUser.name,
                dateTime = DateTime.Now,
            };
            _isNew = true;
            Header = "新建二维码仓库";
        }

        private DataRowView _dataRowView;
        public QRStorage_NewViewModel(Store store,DataRowView row):base(store)
        {
            _dataRowView = row;
            Storage = row.Row.ConvertToModel<QRCodeStorage>();
            _isNew = false;
            Header = "编辑仓库";
        }

        public void Save()
        {
            if (_isNew)
            {
                // 新建仓库
                QRCodeStorage findStorage = this.Store.DataBase.FindStorage(Storage.name);
                if (findStorage != null)
                {
                    MessageBoxX.Show("当前仓库已经存在，请更改仓库名称", "名称重复");
                    return;
                }

                Store.DataBase.InsertStorage(Storage);
            }
            else
            {
                // 编辑仓库
                Store.DataBase.UpdateOrInsertStorage(this.Storage);

                // 将仓库信息更新
                _dataRowView.Row.CopyFrom(Storage);
            }

            this.RequestClose(true);
        }

        public void Cancle()
        {
            this.RequestClose(false);
        }
    }
}
