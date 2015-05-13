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
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    /// 
    public class ItemInGroup1
    {
        public override string ToString() { return Key; }
        public ItemInGroup1(string key, List<Item1> content)
        {
            Key = key;
            ItemContent = new ObservableCollection<Item1>(content);
        }
        public ItemInGroup1(){}
        public string Key { get; set; }
        public ObservableCollection<Item1> ItemContent { get; set; }
    }

    public class Item1
    {
        public string Key { get; set; }
        public BitmapImage l_imagesource { get; set; }
        public string l_fakeid { get; set; }
        public string l_content { get; set; }
        public DateTime l_time { get; set; }
    }



    public sealed partial class TalkPage : Page
    {
        public DispatcherTimer tmr = new DispatcherTimer();
        public DispatcherTimer tmrTip = new DispatcherTimer();
        //List<ItemInGroup1> Items=new List<ItemInGroup1>();
        public ObservableCollection<ItemInGroup1> Items = new ObservableCollection<ItemInGroup1>();
        ItemInGroup1 itemGroup = new ItemInGroup1();
      //  public ObservableCollection<Item1> Item { get; set; }

        public TalkPage()
        {
            this.InitializeComponent();
            GetRemarkName(Global.PageFakeid);
            LongDataBind();
            tmrTip.Tick += CloseTip;
            tmrTip.Interval = TimeSpan.FromSeconds(1);
            tmr.Tick += refreshPM;
            tmr.Interval = TimeSpan.FromSeconds(2);
          
        }

        void CloseTip(object sender, object e)
        {
            StatePanel.Visibility = Visibility.Collapsed;
            tmrTip.Stop();
        }

   async  void  GetRemarkName(string fakeid)
    {
        try
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(ApplicationData.Current.LocalFolder.Path + "\\" + LoginInfo.UserName + "\\NewMessage.db");
            var query = conn.Table<PersonalInfoTable>().Where(x => x.Username == LoginInfo.UserName).Where(x => x.Fakeid == fakeid);
            var result = await query.ToListAsync();
            var items = new ObservableCollection<PersonalInfoTable>(result);
            Name.Text = items[0].Remarkname == "" ? items[0].Nickname : items[0].Remarkname;
        }
        catch { }
    }
     void DataGet()
      {
                string uri = "https://mp.weixin.qq.com/cgi-bin/singlesendpage?tofakeid=" + Global.PageFakeid + "&t=message/send&action=index&token=" + LoginInfo.Token + "&lang=zh_CN";
                string refer = "https://mp.weixin.qq.com/cgi-bin/message?t=message/list&count=20&day=7&token=" + LoginInfo.Token + "&lang=zh_CN";
                string host = "mp.weixin.qq.com";
               // Weixin personmessage = new Weixin(new Uri(uri), refer, host, this);
                HttpGet getTalkMessage=new HttpGet();
               getTalkMessage.Operate="RefreshPersonMessage";
               getTalkMessage.GetOperater(uri,refer,host);
      }

     void refreshPM(object sender, object e)
     {
         DataGet();
         //string uri = "https://mp.weixin.qq.com/cgi-bin/singlesendpage?tofakeid=" + Global.PageFakeid + "&t=message/send&action=index&token=" + LoginInfo.Token + "&lang=zh_CN";
         //string refer = "https://mp.weixin.qq.com/cgi-bin/message?t=message/list&count=20&day=7&token=" + LoginInfo.Token + "&lang=zh_CN";
         //string host = "mp.weixin.qq.com";
         //HttpPost personMessage = new HttpPost();
         //personMessage.Operate = "RefreshPersonMessage";
         //personMessage.PostOperater(uri, refer, host);
     }

     public async void AppendToLong(string content,string fakeid,string url,DateTime time )
     {
         ItemInGroup1 item = new ItemInGroup1();
         item.Key = DateTime.Now.ToString();

         Item1 tmp = new Item1();
         tmp.Key = item.Key;
         tmp.l_content = content;
         tmp.l_fakeid=fakeid;
         BitmapImage newJpg=new BitmapImage();
                                         try
                                             {
                                                 var Icon = await ApplicationData.Current.LocalFolder.GetFileAsync(url);
                                                 IRandomAccessStream iconStream = await Icon.OpenAsync(FileAccessMode.Read);
                                                 newJpg.SetSource(iconStream);
                                             }
                                             catch
                                             {
                                                 string l = "ms-appx:///Design/getheadimg.png";
                                                 newJpg.UriSource = new Uri(l);
                                             }
         tmp.l_imagesource=newJpg;
         tmp.l_time = time;
         List<Item1> tt = new List<Item1>();
         tt.Add(tmp);
         try
         {
             //item.ItemContent = tt;
             //itemGroup = new ItemInGroup1(DateTime.Now.ToString(), tt);
            // Items.Add(itemGroup);
             if(Items.Count!=0)
                  Items[0].ItemContent.Add(tmp);
             else
             {
                 //item.ItemContent = tt;
                itemGroup = new ItemInGroup1("0", tt);
                Items.Add(itemGroup);
             }
           
            // this.itemcollectSource.Source = Items;
             // 分别对两个视图进行绑定 
             // outView.ItemsSource = itemcollectSource.View.CollectionGroups;
            // inView.ItemsSource = itemcollectSource.View;
         }catch(Exception err)
         {
             int aa = 2;
         }
        
         //inView.Items.Add(item);
        // inView.Items.
     }

     public void ScrollToBottom()
     {
         if (inView.Items.Count - 1>=0)
            inView.ScrollIntoView(inView.Items[inView.Items.Count - 1]);
     }

    public  async  void LongDataBind()
        {
            List<Item1> talkMessage = new List<Item1>();
            try
            {
                SQLiteAsyncConnection conn = new SQLiteAsyncConnection(ApplicationData.Current.LocalFolder.Path + "\\" + LoginInfo.UserName + "\\NewMessage.db");
                var query = conn.Table<TalkMessageTable>().Where(x => x.Username == LoginInfo.UserName).Where(x => x.Tofakeid == Global.PageFakeid || x.Fromfakeid == Global.PageFakeid).OrderBy(t => t.Time);
                var result = await query.ToListAsync();
                var items = new ObservableCollection<TalkMessageTable>(result);
                string url = "";
                //string content = "";
                //string key = "";
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i].Fromfakeid == Global.PageFakeid)
                        url = LoginInfo.UserName + "\\ICO" + "\\ico" + items[i].Fromfakeid + ".jpg";
                    else
                        url = LoginInfo.UserName + "\\PersonIcon.jpg";
                    BitmapImage newJpg = new BitmapImage();
                    try
                    {
                        var infoStorageIcon = await ApplicationData.Current.LocalFolder.GetFileAsync(url);
                        IRandomAccessStream iconStream = await infoStorageIcon.OpenAsync(FileAccessMode.Read);
                        newJpg.SetSource(iconStream);
                    }
                    catch
                    {
                        string l = "ms-appx:///Design/getheadimg.png";
                        newJpg.UriSource = new Uri(l);
                    }
                    string content = items[i].Content;
                    DateTime time = TimeStamp.GetTime(items[i].Time);

                    talkMessage.Add(new Item1 { Key = "0", l_imagesource = newJpg, l_content = content, l_time = time, l_fakeid = items[i].Fromfakeid });

                }
                //Items = (from item in talkMessage group item by item.Key into newItems select new ItemInGroup1 { Key = newItems.Key, ItemContent = newItems.ToList() }).ToList();
                itemGroup = new ItemInGroup1("0",talkMessage);
                Items.Add(itemGroup);

                itemcollectSource.Source = Items;
               // inView.ItemsSource = Items;
                inView.ScrollIntoView(inView.Items[inView.Items.Count - 1]);
                //AppendToLong("Test","dddd","",DateTime.Now); 

                //inView.
            }
            catch(Exception err)
            {
                DataGet();
            }
            tmr.Start();
        }

    private void Item_Loaded(object sender, RoutedEventArgs e)
    {
        Grid item = sender as Grid;
        item.ColumnDefinitions[1].Width = new GridLength(Convert.ToDouble(inView.ActualWidth - 160));
    }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            PageInit.talkPage = this;
            DataGet();
        }

        private void inView_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void ContentPanel_Loaded(object sender, RoutedEventArgs e)
        {
        }

        public void ShowTip(string tip,bool autoClose=true)
        {
            State.Text =tip;
            StatePanel.Visibility = Visibility.Visible;
            if (autoClose)
            {
                tmrTip.Start();
            }
        }
        public void ClearMyMessage()
        {
            MyMessage.Text = "";
        }
        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(MyMessage.Text))
            {
                ShowTip("发送消息不能为空");
                return;
            }
            ShowTip("正在发送",false);
            string message = MyMessage.Text;
                Random rd = new Random();
                string random = string.Format("{0:N17}", rd.NextDouble().ToString());
                string sendUrl = "https://mp.weixin.qq.com/cgi-bin/singlesend?t=ajax-response&f=json&token=" + LoginInfo.Token + "&lang=zh_CN";
                string sendData = "token=" + LoginInfo.Token + "&lang=zh_CN&f=json&ajax=1&random=" + random + "&type=1&content=" + message + "&tofakeid=" +Global.PageFakeid + "&imgcode=";
                string sendRefer = "https://mp.weixin.qq.com/cgi-bin/singlesendpage?tofakeid=" + Global.PageFakeid+ "&t=message/send&action=index&token=" + LoginInfo.Token + "&lang=zh_CN";
                
                HttpPost sendInfo = new HttpPost();
                sendInfo.Operate = "SendMessage";
                sendInfo.PostOperater(sendData, sendUrl, sendRefer);

        }

        private void MyMessage_GotFocus(object sender, RoutedEventArgs e)
        {
            SendBar.ClosedDisplayMode = AppBarClosedDisplayMode.Compact;
        }

        private void MyMessage_LostFocus(object sender, RoutedEventArgs e)
        {
            SendBar.ClosedDisplayMode = AppBarClosedDisplayMode.Minimal;
        }
    }
}
