using PaymentSystem.Datas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Pages
{
    public interface IScreenNotify
    {
        void SetParameter(Parameter parameter);

        Action<string, Parameter> ActiveScreen { get; set; }
    }
}
