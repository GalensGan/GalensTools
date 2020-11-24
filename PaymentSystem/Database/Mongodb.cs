using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Database
{
    class Mongodb : IDataBase
    {
        private IMongoDatabase paySysDB;
        public Mongodb(string connectionString, string databaseName)
        {
            MongoClient dbClient = new MongoClient(connectionString);
            paySysDB = dbClient.GetDatabase(databaseName);
        }

        #region uer表
        public bool DeleteUser(string userId)
        {
            IMongoCollection<User> user = paySysDB.GetCollection<User>(Key.user.ToString());
            DeleteResult dr = user.DeleteMany(u => u.userId == userId);
            return dr.DeletedCount == 1;
        }

        public User FindUser(string userId)
        {
            IMongoCollection<User> user = paySysDB.GetCollection<User>(Key.user.ToString());
            return user.Find(item => item.userId == userId).FirstOrDefault();
        }

        public string GetCurrentVersion()
        {
            return "1.0.0.0";
        }

        public VersionInfo GetCurrentVersionInfo()
        {
            throw new NotImplementedException();
        }

        public List<User> GetUsers()
        {
            IMongoCollection<User> user = paySysDB.GetCollection<User>(Key.user.ToString());
            return user.Find(u => true).ToList();
        }

        public List<T> GetUsers<T>() where T : Payment
        {
            IMongoCollection<T> user = paySysDB.GetCollection<T>(Key.user.ToString());
            return user.Find(u => true).ToList();
        }

        public bool InsertUser(User user)
        {
            paySysDB.GetCollection<User>(Key.user.ToString()).InsertOne(user);
            return true;
        }

        // 插入或者更新
        public bool UpdateOrInsertUser(User user)
        {
            BsonDocument bd = new BsonDocument { { "$set", BsonExtensionMethods.ToBsonDocument(user) } };
            UpdateResult ur = paySysDB.GetCollection<User>(Key.user.ToString()).UpdateOne(item => item.userId == user.userId,
                new UpdateDocument(bd), new UpdateOptions() { IsUpsert = true });
            return ur.ModifiedCount == 1;
        }

        public bool UpdateUser(User user)
        {
            BsonDocument bd = new BsonDocument { { "$set", BsonExtensionMethods.ToBsonDocument(user) } };
            UpdateResult ur = paySysDB.GetCollection<User>(Key.user.ToString()).UpdateOne(item => item.userId == user.userId, new UpdateDocument(bd));
            return ur.ModifiedCount == 1;
        }
        #endregion

        #region QRCodeStorage 表
        public bool InsertStorage(QRCodeStorage storage)
        {
            paySysDB.GetCollection<QRCodeStorage>(Key.qrStorage.ToString()).InsertOne(storage);
            return true; 
        }
        public QRCodeStorage FindStorage(string storageName)
        {
            return paySysDB.GetCollection<QRCodeStorage>(Key.qrStorage.ToString()).Find(s => s.name == storageName).FirstOrDefault();
        }
        public List<QRCodeStorage> GetStorages()
        {
            return paySysDB.GetCollection<QRCodeStorage>(Key.qrStorage.ToString()).Find(s => true).ToList();
        }
        public bool UpdateOrInsertStorage(QRCodeStorage storage)
        {
            BsonDocument bd = new BsonDocument { { "$set", BsonExtensionMethods.ToBsonDocument(storage) } };
            UpdateResult ur = paySysDB.GetCollection<QRCodeStorage>(Key.qrStorage.ToString()).UpdateOne(item => item.name == storage.name, 
                new UpdateDocument(bd), new UpdateOptions() { IsUpsert = true });
            return ur.ModifiedCount == 1;
        }
        public bool DeleteStorage(string storageName)
        {
           DeleteResult dr =  paySysDB.GetCollection<QRCodeStorage>(Key.qrStorage.ToString()).DeleteMany(s => s.name == storageName);
            return dr.DeletedCount > 0;
        }
        #endregion

        #region QRCode表
        public bool InsertQRCode(QRCode qrCode)
        {
            paySysDB.GetCollection<QRCode>(Key.qrCode.ToString()).InsertOne(qrCode);
            return true;
        }
        public QRCode FindQRCode(string storageName, string qrCodeName)
        {
            return paySysDB.GetCollection<QRCode>(Key.qrCode.ToString()).Find(s => s.storageName == storageName && s.name==qrCodeName).FirstOrDefault();
        }
        public List<QRCode> GetQRCodes(string storageName)
        {
            return paySysDB.GetCollection<QRCode>(Key.qrCode.ToString()).Find(s => s.storageName== storageName).ToList();
        }
        public bool UpdateOrInsertQRCode(QRCode qrCode)
        {
            BsonDocument bd = new BsonDocument { { "$set", BsonExtensionMethods.ToBsonDocument(qrCode) } };
            UpdateResult ur = paySysDB.GetCollection<QRCode>(Key.qrCode.ToString()).UpdateOne(item => item.name == qrCode.name && item.storageName == qrCode.storageName,
                new UpdateDocument(bd), new UpdateOptions() { IsUpsert = true });
            return ur.ModifiedCount == 1;
        }
        public bool DeleteQRCode(string storageName,string qrCodeName)
        {
            DeleteResult dr = paySysDB.GetCollection<QRCode>(Key.qrCode.ToString()).DeleteMany(s => s.storageName == storageName && s.name == qrCodeName);
            return dr.DeletedCount > 0;
        }
        #endregion
    }
}
