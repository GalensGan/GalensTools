using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Database
{
    /// <summary>
    /// 支付任务
    /// </summary>
    [BsonIgnoreExtraElements]
    public class PayTask
    {
        public string title { get; set; }

        public string description { get; set; }

        public string createUserId { get; set; }

        public string createUserName { get; set; }

        public string startDate { get; set; } = "2020-01-01";
        public string endDate { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");

        public DateTime createDate { get; set; }

        /// <summary>
        /// 二维码库
        /// </summary>
        public string qrCodeStorage { get; set; }

        /// <summary>
        /// 总金额
        /// </summary>
        public double receivable { get; set; }

        /// <summary>
        /// 当前已缴金额
        /// </summary>
        public double actualReceipt { get; set; }

        /// <summary>
        /// 当前任务是否完成
        /// </summary>
        public bool isFinish { get; set; }

        /// <summary>
        /// 是否发布
        /// </summary>
        public bool isPublish { get; set; }

    }
}
