using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace WeiXinAssistant
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SendGroup : Page
    {
        public SendGroup()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            PageInit.sendGroup = this;
            SetState(0);
            string uri = "https://mp.weixin.qq.com/cgi-bin/masssendpage?t=mass/send&token=" + LoginInfo.Token + "&lang=zh_CN";
            string refer = "https://mp.weixin.qq.com/cgi-bin/home?t=home/index&lang=zh_CN&token=" + LoginInfo.Token;
            string host = "mp.weixin.qq.com";
            HttpGet sendGroupPost = new HttpGet();
            sendGroupPost.Operate= "SendGroup";
            sendGroupPost.GetOperater(uri, refer, host);
        }

        public void SetListPicker()
        {
            ListPicker.ItemsSource = Global.groupsInfo.Keys;
            ListPicker.SelectedItem = ListPicker.Items[0];
            //pb.Visibility = Visibility.Collapsed;
            //sendGroup.PreparingText.Visibility = Visibility.Collapsed;
           // sendGroup.SendBox.IsEnabled = true;
            //Global.isFirstPrepare = false;
        
        }

        public void SetState(int result)
        {
            if (result == 1)
            {
                pb.Visibility = Visibility.Collapsed;
                Send.IsEnabled = true;
            }
            else 
            {
                pb.Visibility = Visibility.Visible;
                Send.IsEnabled = false;
            }

        }

        public async void SendResult(string ret,string msg)
        {
            if (ret=="0"||msg == "ok")
            {
               await new MessageDialog("发送成功").ShowAsync();
                SendBox.Text = "";
            }
            else if (ret == "64004" || msg == "not have masssend quota today!")
            {
               await new MessageDialog( "您可群发的消息还剩0条").ShowAsync();
            }
            else if (ret == "-1" || msg == "sys error")
            {
                await new MessageDialog(  "发送失败，您可能未绑定安全助手").ShowAsync();
            }
            else if (ret == "-1" || msg == "system fail")
            {
                await new MessageDialog("发送失败，可能您开启了，群发消息保护").ShowAsync();
            }
            else if (ret == "64004" || msg == "default")
                {
                    await new MessageDialog("您可群发的消息还剩0条").ShowAsync();
                }
            
  
        }

        private async void Send_Click(object sender, RoutedEventArgs e)
        {
            if (SendBox.Text.Length > 600)
            {
                await  new MessageDialog("内容字数超过限制").ShowAsync();
                return;
            }
            if (SendBox.Text.Length == 0)
            {
                await new MessageDialog("内容不能为空").ShowAsync();
                return;
            }
            Random rd = new Random();
            string random = string.Format("{0:N17}", rd.NextDouble().ToString());
            if (String.IsNullOrEmpty(LoginInfo.Seq))
            {
                await new MessageDialog("operation_seq尚未获得，请稍候").ShowAsync();
                return;
            }
            //性别：0（全部），1（男），2（女）
            //groupid
            //国家：(中文)
            string postdata = "token=" + LoginInfo.Token + "&lang=zh_CN&f=json&ajax=1&random=" + random + "&type=1&content=" + SendBox.Text + "&cardlimit=1&sex=0&groupid=" + Global.groupsInfo[ListPicker.SelectedItem.ToString()] + "&synctxweibo=" + 0 + "&country=&province=&city=&imgcode=&operation_seq=" + LoginInfo.Seq;
            string url = "https://mp.weixin.qq.com/cgi-bin/masssend?t=ajax-response&token=" + LoginInfo.Token + "&lang=zh_CN";//请求登录的URL
            string refer = "https://mp.weixin.qq.com/cgi-bin/masssendpage?t=mass/send&token=" + LoginInfo.Token + "&lang=zh_CN";
            HttpPost  sendGroup = new HttpPost();
            sendGroup.Operate= "send";
            sendGroup.PostOperater(postdata, url, refer);
        }

        private async void SendBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SendBox.Text.Length > 600)
            {
                  await  new MessageDialog("内容字数超过限制").ShowAsync();
                return;
            }
            textCount.Text = SendBox.Text.Length + "/600";
        }

        private void textCount_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }
    }
}
