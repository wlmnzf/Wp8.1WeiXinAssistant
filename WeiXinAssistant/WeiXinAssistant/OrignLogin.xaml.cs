using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace WeiXinAssistant
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class OrignLogin : Page
    {
        public OrignLogin()
        {
            this.InitializeComponent();
            MP.NavigationStarting += GetInfo;
            }
        //DOM树加载完成后执行
        private async void WebView_OnDOMContentLoaded(WebView sender, WebViewDOMContentLoadedEventArgs args)
        {
//          string  js = @"
//                            window.external.notify(""ddd"");
//                   }";
//         await sender.InvokeScriptAsync("eval", new string[] { js });
            string[] arguments = { "document.cookie;" };
            string result = await MP.InvokeScriptAsync("eval", arguments);

            await new MessageDialog(result).ShowAsync();
            //await MP.InvokeScriptAsync("alert",new []{"document.cookie"});
            
        }
        private void WebView_OnScriptNotify(object sender, NotifyEventArgs e)
        {
            //这个事件函数可以监听到JS通知的消息，消息类型为文本
            //这里统一消息格式为：JsInvokeModel
            var model = JsonConvert.DeserializeObject<JsInvokeModel>(e.Value);
            switch (model.Type)
            {
                //case "image":
                //    Info.Text = e.Value;
                //    break;
                //case "swiperight":
                //    //右滑
                //    Info.Text = e.Value;
                //    break;
                //case "swipeleft":
                //    //左滑
                //    Info.Text = e.Value;
                //    break;
                //case "text":
                //    Info.Text = e.Value;
                //    break;
            }
        }
       async void  GetInfo(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            int a = 0;
          // await MP.InvokeScriptAsync("eval", new [] { "function(){alert(\"ddd\")}" });
        }
        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
    /// <summary>
    /// JS消息格式
    /// </summary>
    public class JsInvokeModel
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("content1")]
        public string Content1 { get; set; }

        [JsonProperty("content2")]
        public string Content2 { get; set; }

        [JsonProperty("content3")]
        public string Content3 { get; set; }
    }
}
