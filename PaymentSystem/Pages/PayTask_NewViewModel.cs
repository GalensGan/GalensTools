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
    class PayTask_NewViewModel:Conductor<Screen>.Collection.OneActive,IScreenNotify
    {
        private Store _store;
        public PayTask_NewViewModel(Store store)
        {
            _store = store;
            _store.PayTask = new PayTask()
            {
                createUserId = _store.currentUser.userId,
                createUserName = _store.currentUser.name,
            };
        }

        public Action<string, Parameter> ActiveScreen { get; set; }

        // 接收跳转时的参数
        public void SetParameter(Parameter parameter)
        {
            
        }

        private void ChildCallBack(string displayName, Parameter arg = null)
        {
            if (arg != null)
            {
                switch (arg.Command)
                {
                    case Command.Back:
                        this.ActiveScreen(Key.payTask.ToString(), null);
                        break;

                    // 重置数据
                    case Command.Reset:
                        foreach (Screen item in Items)
                        {
                            ScreenChild sc = item as ScreenChild;
                            sc.SetParameter(arg);
                        }
                        break;

                    default: break;
                }
            }

            if (!string.IsNullOrEmpty(displayName))
            {
                Screen sc = Items.Where(item => item.DisplayName == displayName).FirstOrDefault();
                if(sc!=null) base.ActivateItem(sc);
            }
        }

        protected override void OnInitialActivate()
        {
            // 初始化
            ScreenChild sc1 = new PayTask_BasicViewModel(_store);
            sc1.ActiveScreen += ChildCallBack;
            sc1.SetParameter(new Parameter() { Command = Command.Reset });
            this.Items.Add(sc1);

            ScreenChild sc2 = new PayTask_UserViewModel(_store);
            sc1.ActiveScreen += ChildCallBack;
            sc2.SetParameter(new Parameter() { Command = Command.Reset });
            this.Items.Add(sc2);

            base.OnInitialActivate();
        }
    }
}
