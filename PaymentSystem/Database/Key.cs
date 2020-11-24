using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Database
{
    public enum Key
    {
        #region displayName
        user_main,
        user_new,
        user_importFromExcel,
        user_edit,
        qRStore,
        qRStore_Detail,
        payTask,
        payTask_New,
        #endregion

        #region 数据库
        user,
        name,
        userId,
        forbidden,

        qrCode,
        qrStorage,
        storageName,
        dateTime,
        acount,
        #endregion
    }
}
