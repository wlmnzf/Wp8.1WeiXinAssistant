using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SQLite;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Collections.ObjectModel;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace WeiXinAssistant
{
    public sealed partial class ShowAllPeople : Page
    {
        public ShowAllPeople()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            LongDataBind();
        }

        public async void LongDataBind()
        {
            //DateTime today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            //var yesterday = today.AddDays(-1);
            //var theDayBeforeYesterday = today.AddDays(-2);
            List<Item> allPeople = new List<Item>();
            try
            {
                SQLiteAsyncConnection conn = new SQLiteAsyncConnection(ApplicationData.Current.LocalFolder.Path + "\\" + LoginInfo.UserName + "\\NewMessage.db");
                var groupQuery = conn.Table<GroupListTable>().Where(x => x.Username == LoginInfo.UserName);
               
                var query = conn.Table<PersonalInfoTable>().Where(x => x.Username == LoginInfo.UserName);
                var result = await query.ToListAsync();
                var items = new ObservableCollection<PersonalInfoTable>(result);
                string url = "";
              //  string content = "";
                string key = "";
                for (int i = 0; i < items.Count; i++)
                {
                    url = LoginInfo.UserName + "\\ICO" + "\\ico" + items[i].Fakeid + ".jpg";
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

                    string groupId=items[i].Groupid;
                    var keyList=groupQuery.Where(x => x.Groupid==groupId);
                    var keyOC = await keyList.ToListAsync();
                    var keys=new ObservableCollection<GroupListTable>(keyOC);
                    key = keys[0].Name;

                    string name=items[i].Nickname;
                    if (!String.IsNullOrEmpty(items[i].Remarkname))
                        name = items[i].Remarkname;
                    allPeople.Add(new Item { Key = key, l_imageSource = newJpg, l_nickname =name, l_fakeid = items[i].Fakeid });
                    // source.Add(new newMessageList(newjpg, messageCol.MessageTables[i].NickName, content, TimeStamp.GetTime(messageCol.MessageTables[i].Time), messageCol.MessageTables[i].has_Reply, messageCol.MessageTables[i].is_star, messageCol.MessageTables[i].FakeId));
                    // if (i == 10) break;
                }
                List<ItemInGroup> Items = (from item in allPeople group item by item.Key into newItems select new ItemInGroup { Key = newItems.Key, ItemContent = newItems.ToList() }).ToList();
                this.itemcollectSource.Source = Items;
                // 分别对两个视图进行绑定 
                outView.ItemsSource = itemcollectSource.View.CollectionGroups;
                inView.ItemsSource = itemcollectSource.View;
            }
            catch
            {
                // DataGet();
            }
        }

        private void inView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Item i = e.ClickedItem as Item;
            Global.PageFakeid = i.l_fakeid;
            Frame.Navigate(typeof(TalkPage));
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SendGroup));
        }


    }
}
