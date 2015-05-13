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
using SQLite;
using Windows.Storage;
using Windows.System.Display;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace WeiXinAssistant
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class DownloadInfo : Page
    {
        SQLiteAsyncConnection con;
        public DownloadInfo()
        {
            this.InitializeComponent();
            var displayRequest = new DisplayRequest();
            displayRequest.RequestActive();
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            PageInit.downloadInfoPage = this;
        }

        public void SetProgressValue(int value)
        {
            DownloadProgress.Value = value;
        }

        public void AddProgressValue(int value)
        {
            DownloadProgress.Value += value;
        }
        public void SetState(string state)
        {
           this.state.Text = state;
        }
        public void SetButtonContent(string state)
        {
            Begin.Content = state;
        }
        public void SetButtonVisibility(Visibility state)
        {
            Begin.Visibility= state;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if ((string)Begin.Content != "继续")
            {
                string uri = "https://mp.weixin.qq.com/cgi-bin/contactmanage?t=user/index&pagesize=10&pageidx=0&type=0&groupid=0&token=" + LoginInfo.Token + "&lang=zh_CN";
                string refer = "https://mp.weixin.qq.com/cgi-bin/home?t=home/index&lang=zh_CN&token=" + LoginInfo.Token;
                string host = "mp.weixin.qq.com";
                HttpGet getAllInfo = new HttpGet();
                getAllInfo.Operate = "GetAllPeopleInfo";
                SetProgressValue(5);
                getAllInfo.GetOperater(uri, refer, host);
                Begin.Visibility = Visibility.Collapsed;
                // while (!Global.test) ;
                // findInfo();
            }
            else
            {
                this.Frame.Navigate(typeof(Home));
            }
        }
        public async void FindInfo(string []groupidList,string []nameList,string []cntList)
        {
            try
            {
                string[] groupid = groupidList;
                string[] name = nameList;
                string[] cnt = cntList;
               // SQLiteAsyncConnection con = new SQLiteAsyncConnection(ApplicationData.Current.LocalFolder.Path + "\\" + LoginInfo.UserName + "\\NewMessage.db");
               // await con.CreateTableAsync<GroupListTable>();
                //不存在文件夹
                StorageFolder Fold;
                try
                {
                    Fold = await ApplicationData.Current.LocalFolder.GetFolderAsync(LoginInfo.UserName + "\\ICO");
                }
                catch
                {
                    Fold = null;
                }
                //var  = await ApplicationData.GetFileAsync(LoginInfo.UserName + "\\" + "PersonIcon.jpg");
                if (Fold == null)
                {
                  await  ApplicationData.Current.LocalFolder.CreateFolderAsync(LoginInfo.UserName, CreationCollisionOption.OpenIfExists);
                }

                con = new SQLiteAsyncConnection(ApplicationData.Current.LocalFolder.Path + "\\" + LoginInfo.UserName + "\\NewMessage.db");
                // if (con.Table<NewMessagesTable>() == null)
                await con.CreateTableAsync<GroupListTable>();

                for (int i = 0; i < groupid.Length; i++)
                {
                    await con.InsertAsync(new GroupListTable {Username=LoginInfo.UserName,Groupid=groupid[i],Name=name[i],Cnt=cnt[i]});
                }

                    // for (int i = 0; i < groupid.Length; i++)
                    //{
                    state.Text = "正在处理: 共" + Global.PeopleSum + "人";
                    string tempuri = "https://mp.weixin.qq.com/cgi-bin/contactmanage?t=user/index&pagesize=" + Global.PeopleSum + "&pageidx=0&type=0&token="+LoginInfo.Token+"&lang=zh_CN";
                    string temprefer = "https://mp.weixin.qq.com/cgi-bin/contactmanage?t=user/index&pagesize=" + Global.PeopleSum + "&pageidx=0&type=0&token=" + LoginInfo.Token + "&lang=zh_CN";
                    string temphost = "mp.weixin.qq.com";
                    HttpGet getAllInfo = new HttpGet();
                    getAllInfo.Operate = "GetInfoWithIcon";
                    getAllInfo.GetOperater(tempuri,temprefer,temphost);
                    #region
                    // Global.task = "GetPage";
                   // Global.cnt = 0;
                    //Sto.stoInfo<string>("groupid", groupid[i]);
                    //smallHttp tempget = new smallHttp(tempuri, temprefer, temphost, this);
                    //tempget.GetOperater();
                    //while (Global.cnt < int.Parse(cnt[i])) ;
                   // Deployment.Current.Dispatcher.BeginInvoke(() =>
                   // {
                    //    pb.Value = Global.pb;
                  //  });
                //}
                //if (Global.failedFakeId.Count != 0)
                //{
                //    state.Text = "正在尝试重新获得失败的图片，请稍候";
                //    foreach (string fakeid in Global.failedFakeId)
                //    {
                //        try
                //        {
                //            int i = Global.failedFakeId.IndexOf(fakeid);
                //            string tempUri = "https://mp.weixin.qq.com/misc/getheadimg?fakeid=" + fakeid + "&token=" + loginInfo.Token + "&lang=zh_CN";
                //            string tempRefer = Global.failedReferer[i];
                //            smallHttp getFailedIco = new smallHttp();
                //            getFailedIco.getImage(tempUri, tempRefer);
                //        }
                //        catch
                //        {
                //            state.Text = "请求" + fakeid + "失败";
                //        }
                //    }
                    //}
                    #endregion
                //    Deployment.Current.Dispatcher.BeginInvoke(() =>
                //{
                //    pb.Value = 100; state.Text = "完成";
                //    begin.Content = "继续";
                //    //82,376,0,0
                //    begin.Margin = new Thickness(82, 376, 0, 0);
                //    begin.Visibility = Visibility.Visible;
                //});
            }
            catch (Exception err)
            {
                
            }
        }

    }
}
