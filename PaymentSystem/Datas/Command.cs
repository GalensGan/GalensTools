using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Datas
{
    public enum Command
    {
        None,
        /// <summary>
        /// 插入用户
        /// </summary>
        InsertUser, 

        EditUser,

        NewUser,

        Reset,

        Back,
    }
}
