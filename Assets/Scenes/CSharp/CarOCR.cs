using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Baidu.Aip.Ocr;
using System.IO;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Text;
using System.Web;
using UnityEngine.UI;
using ChensDll;
using Newtonsoft.Json;

public class CarOCR : MonoBehaviour {

    /// 设置APPID/AK/SK
    const string APP_ID = "19915588";
    const string API_KEY = "YsaUnsQGFgrolLALiD2Q0Gmn";
    const string SECRET_KEY = "08BvVPAxvNdhobet9Wm9rV6qCfGQjHov";
    string HOST;
    string TOKEN;
    //输出识别结果
    public Text AnalyzText;
    //输出匹配结果
    public Text MatchText;

    Ocr client;
    void Awake()
    {
        TOKEN = AccessToken.getAccessToken(API_KEY, SECRET_KEY);
        string accessToken = TokenData.CreateFromJSON(TOKEN).access_token;
        HOST = "https://aip.baidubce.com/rest/2.0/ocr/v1/license_plate?access_token=" + accessToken;
    }
    void Start()
    {
        
    }

    void Update()
    {
 
    }


    /// <summary>
    /// Generals the basic demo.
    /// </summary>
    public  void ActivateAY()
    {
        //读取对应"图片文件路径"的图片文件
        string Imgpath = CText.PathNow + "/AyAche/ImgTemp.jpg";
        string result = "";
        // 调用通用文字识别, 图片参数为本地图片，可能会抛出网络等异常，请使用try/catch捕获
        try
        {
            //发送Http请求
            Encoding encoding = Encoding.Default;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(HOST);
            request.Method = "post";
            request.KeepAlive = true;

            //调取API识别图片转化文字
            string base64 = getFileBase64(Imgpath);
            String str = "image=" + HttpUtility.UrlEncode(base64);
            byte[] buffer = encoding.GetBytes(str);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
            result = BasicStrReaderFromJson(reader.ReadToEnd(), "number");

            AnalyzText.text = "<color=#00C4FF><b>" + result +"</b></color>";
        }
        catch (Exception e)
        {
            //打印异常信息
            Debug.Log("异常：" + e);
        }
        //识别完成，开始匹配操作
        GameObject.Find("AYTip").GetComponent<Text>().text = "识别完成！";
        GameObject.Find("MTTip").GetComponent<Text>().text = "匹配中...";
        GameObject.Find("GameManager").GetComponent<list>().OnMatch(result);
        
    }

    // 图片的base64编码
    public static String getFileBase64(String fileName)
    {
        FileStream filestream = new FileStream(fileName, FileMode.Open);
        byte[] arr = new byte[filestream.Length];
        filestream.Read(arr, 0, (int)filestream.Length);
        string baser64 = Convert.ToBase64String(arr);
        filestream.Close();
        return baser64;
    }

    public string BasicStrReaderFromJson(string json,string key)
    {
        JsonReader Jreader = new JsonTextReader(new StringReader(json));
        while (Jreader.Read())
        {
            var val = (Jreader.Value as string) ?? string.Empty;
            if (val == key)
            {
                return Jreader.ReadAsString();
            }
        }
        return string.Empty;
    }

}

//解析json结果字符串，得到最终文字
[Serializable]
public class TokenData
{
    public string refresh_token;
    public string expires_in;
    public string scope;
    public string session_key;
    public string access_token;
    public string session_secret;
    public static TokenData CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<TokenData>(jsonString);
    }
}


//鉴权

public static class AccessToken

    {

        public static String getAccessToken(string clientId,string clientSecret)
        {
            String authHost = "https://aip.baidubce.com/oauth/2.0/token";
            HttpClient client = new HttpClient();
            List<KeyValuePair<String, String>> paraList = new List<KeyValuePair<string, string>>();
            paraList.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
            paraList.Add(new KeyValuePair<string, string>("client_id", clientId));
            paraList.Add(new KeyValuePair<string, string>("client_secret", clientSecret));

            HttpResponseMessage response = client.PostAsync(authHost, new FormUrlEncodedContent(paraList)).Result;
            String result = response.Content.ReadAsStringAsync().Result;
            return result;
        }
    }
