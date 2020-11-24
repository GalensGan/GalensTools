using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Database
{
    /// <summary>
    /// 一条支付信息
    /// </summary>
    [BsonIgnoreExtraElements]
    public class Payment
    {
        /// <summary>
        /// 缴费任务的id
        /// </summary>
        public string payTaskId { get; set; }
        
        public string userId { get; set; }

        public string userName { get; set; }

        /// <summary>
        /// 基数
        /// </summary>
        public double factor { get; set; }

        /// <summary>
        /// 倍数
        /// </summary>
        public double time { get; set; }

        public bool isPaid { get; set; }

        public DateTime paidDate { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        // 支付方式：比如支付宝，微信等等
        public string payPattern { get; set; }
    }
}
