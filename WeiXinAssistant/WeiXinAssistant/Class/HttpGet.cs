using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace WeiXinAssistant
{
   public class HttpGet
    {
        CookieContainer cc = new CookieContainer();//接收缓存
       public string Operate;
       public string param;
       public void GetOperater(string url,string refer,string host)
       {

           try
           {
               HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
               webRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";

               if (LoginInfo.LoginCookie != null)
                   webRequest.CookieContainer = LoginInfo.LoginCookie;
               else
                   webRequest.CookieContainer = cc;                                      //保存cookie  

               webRequest.Method = "GET";
               webRequest.Headers["Accept-Language"] = "zh-CN";
               webRequest.Headers["Connection"] = "keep-alive";
               webRequest.Headers["Referer"] = refer;
               webRequest.Headers["Host"] = host;
               webRequest.Headers["DNT"] = "1";
               webRequest.Headers["UserAgent"] = "Mozilla/5.0 (Windows; U; Windows NT 5.1; zh-CN; rv:1.9.1.5) Gecko/20091102 Firefox/3.5.5";
              // webRequest.AllowAutoRedirect = true;
               //webRequest.AllowWriteStreamBuffering = true;
               webRequest.AllowReadStreamBuffering = true;
               IAsyncResult ResponseResult = (IAsyncResult)webRequest.BeginGetResponse(new AsyncCallback(ResponseReady), webRequest);
           }
           catch (Exception err)
           {
               
           }
       }

       public void ResponseReady(IAsyncResult ResponseResult)
       {
           HttpWebRequest request = ResponseResult.AsyncState as HttpWebRequest;
           HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(ResponseResult);
           StreamReader sr2 = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
           string responseInfo = sr2.ReadToEnd();
           Operate thisOperate = new Operate(this.Operate, responseInfo,param);
           thisOperate.OperateResponse();
       }



    }
}
