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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;


// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=391641 上有介绍

namespace WeiXinAssistant
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
   public  class pass
    { 
    public string Pwd { get; set; }
    }
    public sealed partial class MainPage : Page
    {
         
        public MainPage()
        {
            this.InitializeComponent();
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            this.NavigationCacheMode = NavigationCacheMode.Required;
            ApplicationDataContainer UserSettings = ApplicationData.Current.RoamingSettings;
            ApplicationDataCompositeValue loginCertificate = (ApplicationDataCompositeValue)UserSettings.Values["LoginCertificate"];
            if (loginCertificate != null)
            {
                Account.Text = loginCertificate["Account"].ToString();
                 Password.Password=  loginCertificate["Password"].ToString();
              //  pass nn = new pass { Pwd = loginCertificate["Password"].ToString() };
           //this.DataContext= nn;
                //  Password1.Password = "wlm";
            }
    
        }

        void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e) 
        { 
            if (Frame.CanGoBack) 
            {
                   e.Handled = true; Frame.GoBack();
            }
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            PageInit.mainPage = this;
            string vertion="1.0";
            if (String.IsNullOrEmpty(new StorageOperate().SettingStorage("vertion")) || new StorageOperate().SettingStorage("vertion")!=vertion)
            {
                new StorageOperate().SettingStorage("vertion",vertion);
                await new MessageDialog("趁着假期解决了刷新消息时界面狂闪的问题，虽然偶尔还会闪一下"+'\n'+"另外告诉大家一个好消息最近晚上微软BUILD大会上，微软宣布安卓和ios APP的源代码将能直接编译成Windows App"+'\n'+"到时候大家就可以使用自媒体专家这类更专业的APP").ShowAsync();
            }
            //LoginInfo.UserSettings = ApplicationData.Current.RoamingSettings;
           // Password.Password = "";
        }

        private object MessageDialog()
        {
            throw new NotImplementedException();
        }

        public void SetLoginStateText(string State)
        {
            this.LoginState.Text = State;
        }

        public void SetLoginProgressVisibility(int State)
        {
            if (State == 0) LoginProgress.Visibility = Visibility.Collapsed;
            else LoginProgress.Visibility = Visibility.Visible;
        }
        public string GetAccountText()
        {
            return this.Account.Text;
         }
        public void  SetVeriCode(ref BitmapImage veriCodeImage)
        {
            VeriCodeImage.Source = veriCodeImage;
        }
        public  void ShowVeriCode(int i)
        {
            if (i == 1)
            {
                VeriCode.Visibility = Visibility.Visible;
                LoginButton.VerticalAlignment = VerticalAlignment.Bottom;
               VeriCodeImage.VerticalAlignment = VerticalAlignment.Top;
               VeriCode.Text = "";
            //    var messageDialog = new MessageDialog(LoginButton.VerticalAlignment.ToString() + '\n' + VeriCodeImage.VerticalAlignment.ToString());
             //   await messageDialog.ShowAsync();
              //  LoginButton.Margin = new Thickness(0,40, 0, 0);
               // VeriCodeImage.Margin = new Thickness(0,0,0,50);
            }
            else
            {
                VeriCode.Visibility = Visibility.Collapsed;
                LoginButton.VerticalAlignment = VerticalAlignment.Top;
                VeriCodeImage.VerticalAlignment = VerticalAlignment.Bottom;
                //LoginButton.Margin = new Thickness(0, 0, 0, 0);
                //VeriCodeImage.Margin = new Thickness(0,0,0,0);
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SetLoginProgressVisibility(1);
            LoginInfo.UserName = Account.Text;
            LoginInfo.Password = Password.Password;
            string tmpPassword = Password.Password;
            if (tmpPassword.Length > 16) tmpPassword = Password.Password.Substring(0, 16);
            string password = MD5.GetMd5String(tmpPassword);
            string postdata = "username=" + Account.Text + "&pwd=" + password + "&imgcode=" + VeriCode.Text + "&f=json";
            string url = "https://mp.weixin.qq.com/cgi-bin/login?lang=zh_CN ";//请求登录的URL
            string refer = "https://mp.weixin.qq.com/";
            //HttpPost.PostOperater(postdata,url,refer);
            HttpPost loginPost = new HttpPost();
            loginPost.Operate = "LoginOperate";
            loginPost.PostOperater(postdata, url, refer);
        }

        private void VeriCodeImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Operate Refresh = new Operate();
            Refresh.LoginErr("-100");
        }

        private void Password_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Password.Password = "";
        }

        private void Password_GotFocus(object sender, RoutedEventArgs e)
        {
            this.Password.Password = "";
        }

        private async void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            //var file = await getAttachment();
            //Windows.ApplicationModel.Email.EmailAttachment emailAttachment = new Windows.ApplicationModel.Email.EmailAttachment(file.Name, file);//将文件添加到邮件的附件
            Windows.ApplicationModel.Email.EmailMessage mail = new Windows.ApplicationModel.Email.EmailMessage();
          //  mail.Attachments.Add(emailAttachment);//将附件添加到邮件
            mail.Subject = "建议+反馈";//邮件的主题
             mail.Body = "";//邮件的内容
           mail.To.Add(new Windows.ApplicationModel.Email.EmailRecipient("wlmnzf@hotmail.com", "小新的猫"));//邮件的接受地址和显示名称
           await Windows.ApplicationModel.Email.EmailManager.ShowComposeNewEmailAsync(mail);
        }
        private async Task<StorageFile> getAttachment()//获取邮件的附件
            {
                   var folder = Windows.Storage.ApplicationData.Current.LocalFolder;//获取存储区的根文件夹
                    var subfolder = await folder.CreateFolderAsync("MyFolder", Windows.Storage.CreationCollisionOption.OpenIfExists);//新建一个名为MyFolder的文件夹
                    var file = await subfolder.CreateFileAsync("MyAttachment.txt", Windows.Storage.CreationCollisionOption.ReplaceExisting);//新建一个文件名为MyAttachment.txt
                    await Windows.Storage.FileIO.WriteTextAsync(file, "Hello William！");//向文件中写入“Hello 小梦！”作为文件的内容
                    return file;
            }

        private void Account_Loaded(object sender, RoutedEventArgs e)
        {
            TextBox tb=sender as TextBox;
            tb.Width = Input.ActualWidth - 180;
        }

        private void Password_Loaded(object sender, RoutedEventArgs e)
        {
            PasswordBox pb = sender as PasswordBox;
            pb.Width = Input.ActualWidth - 180;
        }



        private void VeriCode_Loaded(object sender, RoutedEventArgs e)
        {
             TextBox tb = sender as TextBox;
            tb.Width = Input.ActualWidth - 180;
        }

        private void LoginButton_Loaded(object sender, RoutedEventArgs e)
        {
            Button bt = sender as Button;
            bt.Width = Input.ActualWidth - 180;
        }

        //private void HyperlinkButton_Click_1(object sender, RoutedEventArgs e)
        //{
        //    Frame.Navigate(typeof(OrignLogin));
        //}

        private async void HyperlinkButton1_Click(object sender, RoutedEventArgs e)
        {
            //Frame.Navigate(typeof(OrignLogin));
            await Windows.System.Launcher.LaunchUriAsync(new Uri(string.Format("ms-windows-store:navigate?appid={0}", "bcbba613-c8f1-4d2e-b73d-c5ba48ae3e0d")));
        }

    }
}
