using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Database
{
    /// <summary>
    /// 二维码
    /// </summary>
    [BsonIgnoreExtraElements]
    public class QRCode
    {
        public string storageName { get; set; }
        public string name { get; set; }

        // 0 代表任意
        public double amount { get; set; }
        public string url { get; set; }
        public string tag { get; set; }
    }
}
