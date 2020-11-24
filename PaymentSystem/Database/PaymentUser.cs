using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Database
{
    [BsonIgnoreExtraElements]
    class PaymentUser:Payment
    {
        // 姓名
        public string name { get; set; }

        // 部门
        public string department { get; set; }

        // 标签
        public string tag { get; set; }

    }
}
