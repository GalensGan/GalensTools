using GalensSDK.Enumerable;
using PaymentSystem.Database;
using PaymentSystem.Datas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaymentSystem.Pages
{
    class PayTask_UserViewModel:ScreenChild
    {
        public PayTask_UserViewModel(Store store) : base(store) 
        {
            DisplayName = "2.添加人员";
        }

        public BindingSource BindingSource { get; set; } = new BindingSource();

        protected override void OnInitialActivate()
        {
            // 读取人员
            List<PaymentUser> users = Store.DataBase.GetUsers<PaymentUser>();
            BindingSource.DataSource = users.ConvertToDt();

            base.OnInitialActivate();
        }
    }
}
