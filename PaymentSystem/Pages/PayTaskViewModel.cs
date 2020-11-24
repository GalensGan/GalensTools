using PaymentSystem.Database;
using PaymentSystem.Datas;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Pages
{
    class PayTaskViewModel :ScreenChild
    { 
        public PayTaskViewModel(Store store) : base(store) { }

        public void Add()
        {
            // 触发
            this.ActiveScreen?.Invoke(Key.payTask_New.ToString(), null);
        }
    }
}
