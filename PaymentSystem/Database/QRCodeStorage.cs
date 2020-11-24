using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Database
{
    /// <summary>
    /// 二维码仓库
    /// </summary>
    [BsonIgnoreExtraElements]
    public class QRCodeStorage
    {
        public string name { get; set; }
        public string description { get; set; }
        public string createUserId { get; set; }
        public string createUserName { get; set; }
        public DateTime dateTime { get; set; }
    }
}
