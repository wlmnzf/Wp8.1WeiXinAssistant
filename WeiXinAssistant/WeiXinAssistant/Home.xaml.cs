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
using Windows.UI.Xaml.Media.Imaging;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace WeiXinAssistant
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Home : Page
    {
        public Home()
        {
            this.InitializeComponent();
        }

       public void SetInfo(string Type,string NickName)
        {
            this.Type.Text = Type;
            this.NickName.Text = NickName;
        }

       public void SetNum()
       {
           NewMessage.Text = Global.NewMessage.ToString();
           NewPerson.Text = Global.NewPerson.ToString();
           AllPeople.Text = Global.AllPeople.ToString();
       }

       public void SetIcon(ref BitmapImage iconImage)
       {
           Ico.Source = iconImage;
       }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            PageInit.homePage = this;
           // pb.Visibility = Visibility.Visible;
            //Global.homeOK = false;
            string uri = "https://mp.weixin.qq.com" + Global.HomeUrl;
            string refer = "https://mp.weixin.qq.com/";
            string host = "mp.weixin.qq.com";
            HttpGet homeGet = new HttpGet();
            homeGet.Operate = "GetNewInfoOperator";
            homeGet.GetOperater(uri,refer,host);
          //  Weixin getHomeInfo = new Weixin(new Uri(uri), refer, host, this);
          //  Global.task = "GetHome";
          //  getHomeInfo.GetOperater();
         //   base.OnNavigatedTo(e);

        }

       // protected override 
        

        #region  总用户数
        private void Image_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ShowAllPeople));
        }

        private void Image_Tapped_2(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ShowAllPeople));
        }

        private void AllPeople_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ShowAllPeople));
        }

        private void TextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ShowAllPeople));
        }
        #endregion

        #region  新消息
        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(NewMessages));
        }

        private void Image_Tapped_3(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(NewMessages));
        }

        private void NewMessage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(NewMessages));
        }

        private void TextBlock_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(NewMessages));
        }
        #endregion

        #region  新增用户
        private void Image_Tapped_4(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DownloadInfo));
        }

        private void Image_Tapped_5(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DownloadInfo));
        }

        private void NewPerson_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DownloadInfo));
        }

        private void TextBlock_Tapped_2(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DownloadInfo));
        }
        #endregion

    }
}
