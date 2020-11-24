using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Database
{
    public interface IDataBase
    {
        #region 管理：设置
        // 获取最新版本号
        string GetCurrentVersion();

        // 获取新版本的下载链接
        VersionInfo GetCurrentVersionInfo();
        #endregion

        #region user表
        // 插入成员
        bool InsertUser(User user);

        bool DeleteUser(string userId);

        User FindUser(string userId);

        List<User> GetUsers();

        List<T> GetUsers<T>() where T : Payment;

        bool UpdateUser(User user);

        bool UpdateOrInsertUser(User user);
        #endregion

        #region QRCodeStorage 表
        bool InsertStorage(QRCodeStorage storage);
        QRCodeStorage FindStorage(string storageName);
        List<QRCodeStorage> GetStorages();
        bool UpdateOrInsertStorage(QRCodeStorage storage);
        bool DeleteStorage(string storageName);
        #endregion

        #region QRCode表
        bool InsertQRCode(QRCode qrCode);
        QRCode FindQRCode(string storageName,string qrCodeName);
        List<QRCode> GetQRCodes(string storageName);
        bool UpdateOrInsertQRCode(QRCode qrCode);
        bool DeleteQRCode(string storageName,string qrCodeName);
        #endregion
    }
}



