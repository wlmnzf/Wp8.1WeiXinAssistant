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
using SQLite;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Collections.ObjectModel;
// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace WeiXinAssistant
{
    public class ItemInGroup
    {
        public string Key { get; set; }

        public List<Item> ItemContent { get; set; }
    }

    public class Item
    {
        public string Key { get; set; }
        public BitmapImage l_imageSource
        {
            get;
            set;
        }
        public string l_nickname
        {
            get;
            set;
        }
        public string l_content
        {
            get;
            set;
        }
        public DateTime l_time
        {
            get;
            set;
        }
        public string l_hasreply
        {
            get;
            set;
        }
        public string l_isstar
        {
            get;
            set;
        }
        public string l_fakeid
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    /// 
    public sealed partial class NewMessages : Page
    {
        public NewMessages()
        {
            this.InitializeComponent();
            if (Global.NewMessagesCnt > 0)
            {
                DataGet();
            }
          //  LongDataBind();
        }

        void DataGet()
        {
            string uri = "https://mp.weixin.qq.com/cgi-bin/message?t=message/list&count=10000&day=7&token=" + LoginInfo.Token + "&lang=zh_CN&filterivrmsg=1";
            string refer = "https://mp.weixin.qq.com/cgi-bin/message?t=message/list&count=10000&day=7&token=" + LoginInfo.Token + "&lang=zh_CN";
            string host = "mp.weixin.qq.com";
            HttpGet getMessages = new HttpGet();
            getMessages.Operate = "GetMessages";
            getMessages.GetOperater(uri, refer, host);
        }

     public  async  void LongDataBind()
        {
           DateTime  today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
           var  yesterday = today.AddDays(-1);
           var theDayBeforeYesterday = today.AddDays(-2);
            List<Item> newMessage = new List<Item>();
            try
            {
                SQLiteAsyncConnection conn = new SQLiteAsyncConnection(ApplicationData.Current.LocalFolder.Path + "\\"+LoginInfo.UserName + "\\NewMessage.db");
                var query = conn.Table<NewMessagesTable>().Where(x => x.Username == LoginInfo.UserName).OrderByDescending(t => t.Time);
                var result = await query.ToListAsync();
                var items = new ObservableCollection<NewMessagesTable>(result);
                string url = "";
                string content = "";
                string key = "";
                for (int i = 0; i < items.Count; i++)
                {
                    url = LoginInfo.UserName + "\\ICO" + "\\ico" + items[i].FakeId + ".jpg";
                    BitmapImage newJpg = new BitmapImage();
                    try
                    {
                        var infoStorageIcon = await ApplicationData.Current.LocalFolder.GetFileAsync(url);
                        IRandomAccessStream iconStream = await infoStorageIcon.OpenAsync(FileAccessMode.Read);
                        newJpg.SetSource(iconStream);
                        //throw(new Exception("www"));
                    }
                    catch
                    {
                        //StorageFolder localFile = Windows.ApplicationModel.Package.Current.InstalledLocation;
                        string l = "ms-appx:///Design/getheadimg.png";
                        newJpg.UriSource = new Uri(l);
                        //toast.Message="载入头像出错";
                        //toast.Show();
                    }
                    content = items[i].Content;

                    DateTime time = TimeStamp.GetTime(items[i].Time);
                    if (time > today)
                    {
                        key = "今天";
                    }
                    else if (time > yesterday)
                    {
                        key = "昨天";
                    }
                    else if (time > theDayBeforeYesterday)
                    {
                        key = "前天";
                    }
                    else
                    {
                        key = "更早";
                    }
                    newMessage.Add(new Item { Key = key, l_imageSource = newJpg, l_nickname = items[i].Nickname, l_content = content, l_time = time, l_hasreply = items[i].Hasreply, l_isstar = items[i].Isstar, l_fakeid = items[i].FakeId });
                    // source.Add(new newMessageList(newjpg, messageCol.MessageTables[i].NickName, content, TimeStamp.GetTime(messageCol.MessageTables[i].Time), messageCol.MessageTables[i].has_Reply, messageCol.MessageTables[i].is_star, messageCol.MessageTables[i].FakeId));
                   // if (i == 10) break;
                }
                List<ItemInGroup> Items = (from item in newMessage group item by item.Key into newItems select new ItemInGroup { Key = newItems.Key, ItemContent = newItems.ToList() }).ToList();
                this.itemcollectSource.Source = Items;
                // 分别对两个视图进行绑定 
                outView.ItemsSource = itemcollectSource.View.CollectionGroups;
                inView.ItemsSource = itemcollectSource.View;
            }
            catch 
            {
                DataGet();
            }

        }
        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //检测数据库是否有数据，如果没有或者有新数据则运行刷新，否则只显示旧数据
            LongDataBind();
            PageInit.newMessagesPage = this;
        }

        private void inView_ItemClick(object sender, ItemClickEventArgs e)
        {
             Item i= e.ClickedItem as Item;
             Global.PageFakeid = i.l_fakeid;
             Frame.Navigate(typeof(TalkPage));
           // sender as
        }

        private void Item_Loaded(object sender, RoutedEventArgs e)
        {
            Grid item = sender as Grid;
            item.ColumnDefinitions[1].Width = new GridLength(Convert.ToDouble(inView.ActualWidth - 245));
        }


    }


}
