using PaymentSystem.Database;
using PaymentSystem.Datas;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Pages
{
    class PayTask_BasicViewModel : ScreenChild
    {
        public PayTask PayTask { get => Store.PayTask; set => Store.PayTask = value; }
        public PayTask_BasicViewModel(Store store) : base(store)
        {
            DisplayName = "1.基本信息";
        }

        protected override void OnActivate()
        {
            // 获取二维码仓库
            List<string> storages = Store.DataBase.GetStorages().ConvertAll(item => item.name).ToList();
            QRStorages = new BindingList<string>(storages);

            base.OnActivate();
        }

        public BindingList<string> QRStorages { get; set; }
        public string QRStorage
        {
            get => PayTask.qrCodeStorage;
            set
            {
                PayTask.qrCodeStorage = value;
                base.NotifyOfPropertyChange(() => this.QRStorage);
            }
        }

        public string StartDate
        {
            get => PayTask.startDate;
            set
            {
                PayTask.startDate = value;
                base.NotifyOfPropertyChange(() => this.StartDate);
            }
        }

        public string EndDate
        {
            get => PayTask.endDate;
            set
            {
                PayTask.endDate = value;
                base.NotifyOfPropertyChange(() => this.EndDate);
            }
        }

        public void Back()
        {
            this.ActiveScreen?.Invoke(string.Empty, new Parameter() { Command = Command.Back });
        }

        public void Next()
        {
            this.ActiveScreen?.Invoke("2.添加人员", null);
        }
    }
}
