using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Windows.Storage;

namespace WeiXinAssistant
{
    public static class LoginInfo
    {
       public  static CookieContainer LoginCookie;
       public static string UserName;
       public static string Password;
       public static string Err;
       //public static ApplicationDataContainer UserSettings;
       public static string Token;
       public static string NickName;
       public static string Type;
       public static string FakeId;
       public static string Seq;
       public static string Ticket;
       public static string UniformUserName;
    }
}
