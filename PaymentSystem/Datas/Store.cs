using Newtonsoft.Json;
using Panuon.UI.Silver;
using PaymentSystem.Database;
using Stylet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Datas
{
    public class Store
    {
        private static readonly string configName = "paymentSys.conf";
        #region 构造函数
        /// <summary>
        /// 获取本地配置
        /// </summary>
        /// <returns></returns>
        public static Store GetConfigStore()
        {
            // 从当前目录下读取配置文件，如果没有，则直接返回
            if (File.Exists(configName))
            {
                using (StreamReader reader = new StreamReader(configName))
                {
                    string content = reader.ReadToEnd();
                    Store store = JsonConvert.DeserializeObject<Store>(content);
                    reader.Close();
                    return store;
                }
            }
            return new Store();
        }
        private Store() { }
        #endregion

        #region 上次登陆的信息
        public User lastUser;

        public bool isRememberLoginInfo;

        public bool isAutoSave = false;
        #endregion

        #region 当前登陆信息
        public User currentUser;
        #endregion

        #region 公共方法
        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
            string content = JsonConvert.SerializeObject(this);
            using (Stream stream = File.Create(configName))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(content);
                    writer.Close();
                }
                stream.Close();
            }
            return true;
        }
        #endregion

        #region 数据库
        public string databaseType = "mongodb";
        public string databaseName = "paymentSystem";
        public string connectionString = "mongodb://root:whfy8888@35b59s4020.zicp.vip:26621/paymentSystem?authSource=admin";
        public string connectionStringLAN = "mongodb://root:whfy8888@192.168.19.220:27017/paymentSystem?authSource=admin";
        public string GetConnectionString()
        {
            string tempIp = string.Empty;
            try
            {
                WebRequest wr = WebRequest.Create("https://www.ipip5.com/");
                Stream s = wr.GetResponse().GetResponseStream();
                StreamReader sr = new StreamReader(s, Encoding.Default);
                string all = sr.ReadToEnd();//读取网站的数据
                int first = all.IndexOf("<span class=\"c-ip\">") + 19;
                int last = all.IndexOf("</span>", first);
                tempIp = all.Substring(first, last - first);
                sr.Close();
                s.Close();
            } 
            catch (Exception)
            {
                // 获取id失败
            }

            if (string.IsNullOrEmpty(tempIp)) return connectionString;

            if (tempIp.Contains("223.75.251.233")) return connectionStringLAN;

            return connectionString;
        }
        [JsonIgnore]
        public IDataBase DataBase { get; set; }
        #endregion

        #region 窗体
        [JsonIgnore]
        public WindowX MainWindow { get; set; }

        [JsonIgnore]
        public IWindowManager WindowManager { get; set; }

        [JsonIgnore]
        public PayTask PayTask { get; set; }
        #endregion

        public string defaultPassword = "123";
    }
}
