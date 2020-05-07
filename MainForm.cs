using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Newtonsoft.Json;

namespace WindowsFormsApp1
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] dzs = richTextBox1.Text.Split();
            foreach (var dz in dzs)
            {
                // http://api.tianditu.gov.cn/geocoder?ds={"keyWord":"延庆区北京市延庆区延庆镇莲花池村前街50夕阳红养老院"}&tk=您的密钥             

                //  string m_CurrentSearchUri = string.Format(@"http://api.tianditu.gov.cn/geocoder?ds={""keyWord"":{0}}&tk={1}",dz,"d8b05b26b44eb9007c645c51d20a0128") ;

                string a = @"http://api.tianditu.gov.cn/geocoder?ds=";
                string b = @"{""keyWord"":""xxxx""}";
                b= b.Replace("xxxx", dz);
                string c = @"&tk=d8b05b26b44eb9007c645c51d20a0128";
                string m_CurrentSearchUri = string.Format("{0}{1}{2}",a,b,c);

             
                // 发送要求
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(m_CurrentSearchUri);
                //下面的代码一定要加上，在天地图网站会发生"远程服务器返回错误: (403) 已禁止。"
                request.Method = "GET";
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                request.Headers.Add("Accept-Language", "zh-cn,zh;q=0.8,en-us;q=0.5,en;q=0.3");
                request.UserAgent = "Mozilla/5.0 (Windows NT 5.2; rv:12.0) Gecko/20100101 Firefox/12.0";

                // 取得响应
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream());
                //得到一个json字符串
                string joResultTemp = sr.ReadToEnd().ToString();
                //将返回的json数据转为JSON对象
                RecordResultTDT joResult = JsonConvert.DeserializeObject<RecordResultTDT>(joResultTemp);
                //返回天地图的Json对象
                string lonlat = "\n";
                
                if (joResult.Msg != null&&joResult.Msg.ToString().ToUpper() == "OK")
                {
                    lonlat = joResult.Location.Lon.ToString() + "," + joResult.Location.Lat.ToString() + "\n";
                }

                richTextBox2.Text += lonlat;
            }
           
      
        }
    }

    public class LocationTDT
    {
        [JsonProperty("level")]
        public string Level { get; set; }

        [JsonProperty("lon")]
        public double Lon { get; set; }

        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("keyWord")]
        public string KeyWord { get; set; }
    }
    class RecordResultTDT
    {
        [JsonProperty("msg")]
        public string Msg { get; set; }

        [JsonProperty("location")]
        public LocationTDT Location { get; set; }

        [JsonProperty("searchVersion")]
        public string SearchVersion { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }
    }
}
