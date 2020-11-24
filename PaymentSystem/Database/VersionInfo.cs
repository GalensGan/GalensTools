using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Database
{
    [BsonIgnoreExtraElements]
    public class VersionInfo
    {
        public string version;
        public string description;
        public string url;
        public long datestamp;
    }
}
