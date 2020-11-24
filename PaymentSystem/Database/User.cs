using GalensSDK.Encrypt;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Database
{
    /// <summary>
    /// 党员信息
    /// </summary>
    [BsonIgnoreExtraElements]
    public class User
    {
        // 姓名
        public string name { get; set; }

        // 性别
        public string gender { get; set; }

        // 电话号码
        public string tell { get; set; }

        // 身份证号码
        public string idNumber { get; set; }

        // 用户id
        public string userId { get; set; }

        // 密码
        public string password { get; set; } = MD5Ex.EncryptString("123");

        // 功能码
        public string access { get; set; } = "普通用户";

        // 部门
        public string department { get; set; }

        // 标签
        public string tag { get; set; } 

        // 禁用
        public bool forbidden { get; set; } = false;

        public void CopyFrom(User user)
        {
            name = user.name;
            gender = user.gender;
            tell = user.tell;
            idNumber = user.idNumber;
            userId = user.userId;
            password = user.password;
            access = user.access;
            department = user.department;
            tag = user.tag;
            forbidden = user.forbidden;
        }
    }
}
