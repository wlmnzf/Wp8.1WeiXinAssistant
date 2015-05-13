using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using System.Net;
using System.IO;

namespace WeiXinAssistant
{
  public  class HttpImageGet
    {
        CookieContainer cc=new CookieContainer();
        public string Operate;
        public void GetImageOperate(string uri,string refer)
        {
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(uri);
                webRequest.Accept = "image/webp,*/*;q=0.8";
                if (LoginInfo.LoginCookie != null)
                    webRequest.CookieContainer = LoginInfo.LoginCookie;
                else
                    webRequest.CookieContainer = cc;                                      //保存cookie  

                //webRequest.AllowWriteStreamBuffering = true;
                webRequest.AllowReadStreamBuffering = true;

                webRequest.Method = "GET";                                          //请求方式是POST
               // webRequest.AllowAutoRedirect = true;
                webRequest.Headers["Accept-Encoding"] = "gzip,deflate";
                webRequest.Headers["Accept-Language"] = "zh-CN";
                webRequest.Headers["Connection"] = "keep-alive";
                webRequest.Headers["Referer"] = refer;
                webRequest.Headers["Host"] = "mp.weixin.qq.com";
                webRequest.Headers["DNT"] = "1";
                 webRequest.Headers["UserAgent"] = "Mozilla/5.0 (Windows; U; Windows NT 5.1; zh-CN; rv:1.9.1.5) Gecko/20091102 Firefox/3.5.5";
                IAsyncResult CodeResult = (IAsyncResult)webRequest.BeginGetResponse(new AsyncCallback(ResponseReady), webRequest);
            }
            catch (Exception err)
            {
            }
      }



        public void ResponseReady(IAsyncResult CodeResult)
        {
            //BitmapImage newjpg=null;
            try
            {
                string u = ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000).ToString();
                HttpWebRequest request = CodeResult.AsyncState as HttpWebRequest;
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(CodeResult);
                using (Stream stream = response.GetResponseStream())
                {
                    #region 注释
                    // if (!Sto.File.DirectoryExists("VC"))
                   // {
                   //     Sto.File.CreateDirectory("VC");
                   // }
                   //// var isoFileStream = new IsolatedStorageFileStream
                   // var outStream = Sto.File.OpenFile("VC\\VCode" + u + ".jpg", FileMode.Create, FileAccess.Write);
                   // FileStream outStream = new FileStream("VC\\VCode" + u + ".jpg", FileMode.Create);
                    /*Int32 i = 0;
                    //循环inStream，将内容写进outStream
                    while (true)
                    {
                        i = stream.ReadByte();
                        if (i != -1)
                        {
                            outStream.WriteByte((Byte)i);
                        }
                        else
                        {
                            break;
                        }
                    }*/
                    //stream.CopyTo(outStream);
                   
                    //outStream.Close();
                    //var readstream = Sto.File.OpenFile("VC\\VCode" + u + ".jpg", FileMode.Open, FileAccess.Read);
                    //Deployment.Current.Dispatcher.BeginInvoke(() => {  newjpg=new BitmapImage(); newjpg.SetSource(readstream); });
                    //关闭文件
                    #endregion
                    ImageResponseOperate image = new ImageResponseOperate(Operate, stream,response.ResponseUri.ToString());
                    image.OperateResponse();
                }
                // Deployment.Current.Dispatcher.BeginInvoke(() => { t.Text = text2; });
               // Deployment.Current.Dispatcher.BeginInvoke(() => { that.vc.Source = newjpg; that.showVCode(); });
            }
            catch (Exception err)
            {
              //  Deployment.Current.Dispatcher.BeginInvoke(() => { that.state.Text = err.Message+"获取验证码超时，请检查网络"; });
                //that.toast.Show();
            }
      }

    }
}
