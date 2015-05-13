using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace WeiXinAssistant
{
  public class HttpPost
    {
         byte[] byteArray;
         public string Operate;
         public string param;
         CookieContainer cc=new CookieContainer();//接收缓存

            public void PostOperater(string Data,string uri,string refer)
        {     
            try
            {
                byteArray = Encoding.UTF8.GetBytes(Data); // 转化
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(uri);  //新建一个WebRequest对象用来请求或者响应url
                webRequest.Accept = "application/json, text/javascript, */*; q=0.01";
                if(LoginInfo.LoginCookie!=null)
                    webRequest.CookieContainer = LoginInfo.LoginCookie;  
                else
                    webRequest.CookieContainer = cc;                                      //保存cookie  

                webRequest.Method = "POST";
                webRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";       //请求的内容格式为application/x-www-form-urlencoded
                webRequest.Headers["ContentLength"] = byteArray.Length.ToString();
                webRequest.Headers["Referer"] =refer;
                webRequest.AllowReadStreamBuffering = true;
                webRequest.Headers["UserAgent"] = "Mozilla/5.0 (Windows; U; Windows NT 5.1; zh-CN; rv:1.9.1.5) Gecko/20091102 Firefox/3.5.5";
                IAsyncResult asyncResult = (IAsyncResult)webRequest.BeginGetRequestStream(new AsyncCallback(RequestReady), webRequest);
            }
            catch (Exception err)
            {
            }
        }



        public  void RequestReady(IAsyncResult asyncResult)
        {
            try
            {
                HttpWebRequest webRequest = asyncResult.AsyncState as HttpWebRequest;
                using (Stream stream = webRequest.EndGetRequestStream(asyncResult))
                {
                    stream.Write(byteArray, 0, byteArray.Length);
                    stream.Dispose();
                }
                IAsyncResult ResponseResult = (IAsyncResult)webRequest.BeginGetResponse(new AsyncCallback(ResponseReady), webRequest);
                //int a = 100;
            }
            catch (Exception err)
            {

            }
        }




        public void ResponseReady(IAsyncResult ResponseResult)
        {
            try
            {
                HttpWebRequest request = ResponseResult.AsyncState as HttpWebRequest;
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(ResponseResult);
                StreamReader sr2 = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                string responseInfo= sr2.ReadToEnd();
                Operate thisOperate = new Operate(this.Operate,responseInfo,param);
                thisOperate.OperateResponse();
            }
            catch(Exception err)
            {
            
            }
        }
    }
}
