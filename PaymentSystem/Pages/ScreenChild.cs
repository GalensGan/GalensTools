using PaymentSystem.Datas;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Pages
{
    public abstract class ScreenChild : Screen, IScreenNotify
    {
        public ScreenChild(Store store)
        {
            Store = store;
        }
        public Store Store { get; private set; }

        /// <summary>
        /// 图标名称
        /// </summary>
        public string IconName { get; set; }

        /// <summary>
        /// 子类去重写，获取相应的参数
        /// </summary>
        /// <param name="arg"></param>
        protected virtual void HandleParameter(Parameter parameter) { }

        public void SetParameter(Parameter parameter)
        {
            if (parameter == null) return;
            HandleParameter(parameter);
        }

        public Action<string, Parameter> ActiveScreen { get; set; }
    }
}
