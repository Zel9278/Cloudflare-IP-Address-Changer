using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Configuration;

namespace CF_IP_Address_Changer
{
    [JsonObject("Data")]
    class Data
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("ttl")]
        public int TTL { get; set; }

        [JsonProperty("proxied")]
        public bool Proxied { get; set; }
    }

    public class Program
    {
        static async Task Main(string[] args)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    Console.WriteLine("Call Get Amazon Check IP");
                    var resultIP = await client.GetAsync(@"https://checkip.amazonaws.com");
                    var ip = await resultIP.Content.ReadAsStringAsync();
                    Console.WriteLine("Get IP: " + ip);

                    string zoneID = Properties.Settings.Default.zoneID;
                    string dnsRecord = Properties.Settings.Default.dnsRecord;
                    string mailAddress = Properties.Settings.Default.mailAddress;
                    string authKey = Properties.Settings.Default.authKey;

                    client.DefaultRequestHeaders.Add("X-Auth-Email", mailAddress);
                    client.DefaultRequestHeaders.Add("X-Auth-Key", authKey);

                    Console.WriteLine("Call Get Cloudflare");
                    var getResponse = await client.GetAsync(@"https://api.cloudflare.com/client/v4/zones/" + zoneID + "/dns_records?type=A&name=" + dnsRecord);
                    var json = await getResponse.Content.ReadAsStringAsync();
                    JObject jsonObj = JObject.Parse(json);
                    string recordID = (string)jsonObj["result"][0]["id"];
                    Console.WriteLine("Get RecordID: " + recordID);

                    Data data = new Data();
                    data.Type = (string)jsonObj["result"][0]["type"];
                    data.Name = dnsRecord;
                    data.Content = ip;
                    data.TTL = (int)jsonObj["result"][0]["ttl"];
                    data.Proxied = (bool)jsonObj["result"][0]["proxied"];

                    var jsonData = JsonConvert.SerializeObject(data);
                    var reqData = new StringContent(jsonData, Encoding.UTF8, mediaType: "application/json");
                    Console.WriteLine("Call PUT Cloudflare");
                    var putResponse = await client.PutAsync(@"https://api.cloudflare.com/client/v4/zones/"+ zoneID +"/dns_records/" + recordID, reqData);
                    putResponse.EnsureSuccessStatusCode();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                Console.WriteLine("End");
            }
        }
    }
}
