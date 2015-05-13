using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Windows.UI.Popups;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Windows.Storage;
using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using SQLite;




namespace WeiXinAssistant
{
    class Operate
    {
        string operateContent;
        string responseInfo;
        string param;
        public Operate()
        { }

        public Operate(string Operate,string responseInfo,string param="")
        {
            this.operateContent = Operate;
            this.responseInfo = responseInfo;
            this.param = param ;
        }

        public async void OperateResponse()
        {
            SQLiteAsyncConnection con;
            switch(this.operateContent)
            {
                case "LoginOperate":
                     int  code= await  Login();
                     if (code == 0) LoginOK();
                    else LoginErr(code.ToString());
                 break;

                case "GetNewInfoOperator":
                            string pattern;
                            StorageFolder localFolderStorage = ApplicationData.Current.LocalFolder;

                            StorageFile infoStorageIcon;
                            try
                            {
                                infoStorageIcon = await localFolderStorage.GetFileAsync(LoginInfo.UserName + "\\" + "PersonIcon.jpg");

                                IRandomAccessStream iconStream = await infoStorageIcon.OpenAsync(FileAccessMode.Read);
                                await PageInit.homePage.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                                {
                                    BitmapImage newImage = new BitmapImage();
                                    newImage.SetSource(iconStream);
                                    PageInit.homePage.SetIcon(ref newImage);
                                });
                            }
                           catch(Exception err)
                            {
                                infoStorageIcon = null;
                            }

                            if (infoStorageIcon == null)
                            {
                                pattern = "fakeid=(\\d*)";
                                var m = Regex.Match(responseInfo, pattern);
                                LoginInfo.FakeId = m.Groups[1].Value.ToString();
                                //无法检测到fakeid
                                if (string.IsNullOrEmpty(LoginInfo.FakeId))
                                {
                                   // home.toast.Message = "无法查找到到您的Fakeid，可能是登陆超时"; home.toast.Show(); 

                                    //此处添加通知
                                }
                                else
                                {
                                    string responseInfoUri = "https://mp.weixin.qq.com/misc/getheadimg?fakeid=" + LoginInfo.FakeId + "&token=" + LoginInfo.Token + "&lang=zh_CN";
                                    string responseInfoRefer = "https://mp.weixin.qq.com/cgi-bin/home?t=home/index&lang=zh_CN&token=" + LoginInfo.Token;
                                    HttpImageGet getIcon = new HttpImageGet();
                                    getIcon.Operate = "GetPersonalIcon";
                                    getIcon.GetImageOperate(responseInfoUri, responseInfoRefer);
                                }
                            }




                            StorageFile infoStorageText;
                            try
                            {
                                infoStorageText = await localFolderStorage.GetFileAsync(LoginInfo.UserName + "\\" + "PersonalInfo.txt");

                                using (IRandomAccessStream readStream = await infoStorageText.OpenAsync(FileAccessMode.Read))
                                {
                                    using (DataReader dataReader = new DataReader(readStream))
                                    {
                                        UInt64 size = readStream.Size;
                                        if (size <= UInt32.MaxValue)
                                        {
                                            await dataReader.LoadAsync(sizeof(Int32));
                                            Int32 stringSize = dataReader.ReadInt32();
                                            await dataReader.LoadAsync((UInt32)stringSize);
                                            string fileContent = dataReader.ReadString((uint)stringSize);
                                            string[] splitString = fileContent.Split('\n');
                                            LoginInfo.Type = splitString[0].Split(':')[0] == "type" ? splitString[0].Split(':')[1] : splitString[1].Split(':')[1];
                                            LoginInfo.NickName = splitString[1].Split(':')[0] == "nickname" ? splitString[1].Split(':')[1] : splitString[0].Split(':')[1];

                                            await PageInit.homePage.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                                            {
                                                PageInit.homePage.SetInfo(LoginInfo.Type, LoginInfo.NickName);
                                            });
                                        }
                                        else
                                        {
                                           // OutputTextBlock.Text = "文件 " + file.Name + " 太大，不能再单个数据块中读取";
                                        }
                                    }
                                }



                            }
                            catch (Exception err)
                            {
                                infoStorageText = null;
                            }

                            if (infoStorageText == null)
                            {
                                pattern = "nickname\">(\\S+)</a>";
                                var m = Regex.Match(responseInfo, pattern);
                                LoginInfo.NickName = m.Groups[1].Value;

                                pattern = "type icon_subscribe_label\">(\\S+)</a>";
                                m = Regex.Match(responseInfo, pattern);
                                LoginInfo.Type = m.Groups[1].Value;

                                await PageInit.homePage.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                                  {
                                      PageInit.homePage.SetInfo(LoginInfo.Type,LoginInfo.NickName);
                                  });
       

                                string dataContent = "type:" + LoginInfo.Type + '\n' + "nickname:" + LoginInfo.NickName;
                                StorageFolder accountInfoStorage = ApplicationData.Current.LocalFolder;
                                var localFolder = await localFolderStorage.CreateFolderAsync(LoginInfo.UserName, CreationCollisionOption.OpenIfExists);
                                var localFile = await localFolder.CreateFileAsync("PersonalInfo.txt", CreationCollisionOption.ReplaceExisting);

                                using (StorageStreamTransaction transaction = await localFile.OpenTransactedWriteAsync())
                                { 
                                    using(DataWriter dataWriter= new DataWriter(transaction.Stream))
                                    {
                                        dataWriter.WriteInt32(Encoding.UTF8.GetByteCount(dataContent));
                                        dataWriter.WriteString(dataContent);
                                        transaction.Stream.Size = await dataWriter.StoreAsync();
                                        await transaction.CommitAsync();
                                    }
                                }

                            }



                                pattern = "<em class=\"number\">(\\d+)</em>";
                                var ms = Regex.Matches(responseInfo, pattern);
                                int i = 1;
                                foreach (Match match in ms)
                                {
                                    if (i == 1)
                                    {
                                        Global.NewMessage = int.Parse(match.Groups[1].Value.ToString());
                                        if (Global.NewMessage > 0) Global.NewMessagesCnt = Global.NewMessage;
                                        else Global.NewMessagesCnt = 0;
                                    }
                                    if (i == 2)
                                    {
                                        Global.NewPerson=int.Parse( match.Groups[1].Value.ToString());
                                        if (Global.NewPerson > 0)
                                        {
                                            Global.HasNewPeople = true;
                                        }
                                        else
                                        {
                                            Global.HasNewPeople = false; 
                                        }
                                            //home.NavigationService.Navigate(new Uri("AllPeopleInfo.xaml",UriKind.Relative));
                                    }
                                    if (i == 3)
                                        Global.AllPeople= int.Parse(match.Groups[1].Value.ToString());

                                    i++;
                                }

                                await PageInit.homePage.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                                {
                                    PageInit.homePage.SetNum();
                                });

#region
                           // StorageFile infoStorageText = await localFolderStorage.GetFileAsync(LoginInfo.UserName + "\\" + "PersonalInfo.txt");
                            ////if (infoStorageText!=null&& Sto.File.FileExists(loginInfo.UserName + "\\ico.jpg"))
                            ////{
                            ////    try
                            ////    {
                            ////        //读取本地资料
                            ////        var aFile = new IsolatedStorageFileStream(loginInfo.UserName + "\\Info.txt", FileMode.Open, Sto.File);
                            ////        StreamReader sr = new StreamReader(aFile);
                            ////        string strLine = sr.ReadLine();
                            ////        while (strLine != null)
                            ////        {
                            ////            if (strLine.Split(':')[0] == "type")
                            ////                loginInfo.Type = strLine.Split(':')[1];
                            ////            else
                            ////                loginInfo.NickName = strLine.Split(':')[1];
                            ////            strLine = sr.ReadLine();
                            ////        }
                            ////        sr.Close();
                            ////    }
                            ////    catch (Exception err)
                            ////    {
                            ////        Global.StoErr("ExsistsFile", err);
                            ////        home.toast.Message = "读取文本资料出错";
                            ////        home.toast.Show();
                            ////    }

                            ////    //读取本地图片
                            ////    try
                            ////    {
                            ////        var readstream = Sto.File.OpenFile(loginInfo.UserName + "\\ico.jpg", FileMode.Open, FileAccess.Read);
                            ////        BitmapImage jpg = new BitmapImage();
                            ////        jpg.SetSource(readstream);
                            ////        home.ico.Source = jpg;
                            ////        readstream.Close();
                            ////    }
                            ////    catch (Exception err)
                            ////    {
                            ////        Global.StoErr("ExsistsFile", err);
                            ////        home.toast.Message = "读取头像出错";
                            ////        home.toast.Show();
                            ////    }

                            ////    home.nickname.Text = loginInfo.NickName;
                            ////    home.type.Text = loginInfo.Type;

                            ////    if (Sto.Info.Contains(loginInfo.UserName + "LaunchTimes"))
                            ////        Sto.Info[loginInfo.UserName + "LaunchTimes"] = Convert.ToInt32(Sto.Info[loginInfo.UserName + "LaunchTimes"]) + 1;
                            ////    else
                            ////        Sto.Info[loginInfo.UserName + "LaunchTimes"] = 1;
                            ////    Sto.Info.Save();
                            ////}






                            ////else
                            ////{
                            ////    if (!Sto.File.DirectoryExists(loginInfo.UserName))
                            ////        Sto.File.CreateDirectory(loginInfo.UserName);
                            ////    Match m;

                            ////    try
                            ////    {
                            ////        pattern = "fakeid=(\\d*)";
                            ////        m = Regex.Match(responseInfo, pattern);
                            ////        loginInfo.Fakeid = m.Groups[1].Value.ToString();
                            ////        //无法检测到fakeid
                            ////        if (string.IsNullOrEmpty(loginInfo.Fakeid))
                            ////        { home.toast.Message = "无法查找到到您的Fakeid，可能是登陆超时"; home.toast.Show(); return; }
                            ////        //获取头像
                            ////        string responseInfoUri = "https://mp.weixin.qq.com/misc/getheadimg?fakeid=" + loginInfo.Fakeid + "&token=" + loginInfo.Token + "&lang=zh_CN";
                            ////        string responseInfoRefer = "https://mp.weixin.qq.com/cgi-bin/home?t=home/index&lang=zh_CN&token=" + loginInfo.Token;
                            ////        GetIco getIco = new GetIco();
                            ////        getIco.getImage(responseInfoUri, responseInfoRefer, home);
                            ////    }
                            ////    catch (Exception err)
                            ////    {
                            ////        Global.StoErr("CreateIco", err);
                            ////        if (Sto.File.FileExists(loginInfo.UserName + "\\ico.jpg"))
                            ////            Sto.File.DeleteFile(loginInfo.UserName + "\\ico.jpg");
                            ////        home.toast.Message = "创建头像出错,请刷新";
                            ////        home.toast.Show();
                            ////    }

                            ////    pattern = "nickname\">(\\S+)</a>";
                            ////    m = Regex.Match(responseInfo, pattern);
                            ////    home.nickname.Text = loginInfo.NickName = m.Groups[1].Value;

                            ////    pattern = "type icon_subscribe_label\">(\\S+)</a>";
                            ////    m = Regex.Match(responseInfo, pattern);
                            ////    home.type.Text = loginInfo.Type = m.Groups[1].Value;

                            ////    try
                            ////    {
                            ////        var aFile = new IsolatedStorageFileStream(loginInfo.UserName + "\\Info.txt", FileMode.OpenOrCreate, Sto.File);
                            ////        StreamWriter sw = new StreamWriter(aFile);
                            ////        sw.WriteLine("type:" + loginInfo.Type);
                            ////        sw.WriteLine("nickname:" + loginInfo.NickName);
                            ////        sw.Close();
                            ////    }
                            ////    catch (Exception err)
                            ////    {
                            ////        Global.StoErr("CreateInf", err);
                            ////        if (Sto.File.FileExists(loginInfo.UserName + "\\Info.txt"))
                            ////            Sto.File.DeleteFile(loginInfo.UserName + "\\Info.txt");
                            ////        if (home != null)
                            ////        {
                            ////            //home.t.Text = err.Message;
                            ////            home.toast.Message = "写入资料出错";
                            ////            home.toast.Show();
                            ////        }
                            ////        if (that != null)
                            ////        {
                            ////            that.state.Text = "写入资料出错";
                            ////        }
                            ////        if (newMessage != null)
                            ////        {
                            ////            newMessage.toast.Message = "写入资料出错";
                            ////            newMessage.toast.Show();
                            ////        }
                            ////    }
                            ////}

                            ////try
                            ////{
                            ////    pattern = "<em class=\"number\">(\\d+)</em>";
                            ////    var ms = Regex.Matches(responseInfo, pattern);
                            ////    int i = 1;
                            ////    foreach (Match match in ms)
                            ////    {
                            ////        if (i == 1)
                            ////            Global.newAddMessage = home.talk.Text = match.Groups[1].Value.ToString();
                            ////        if (i == 2)
                            ////        {
                            ////            home.newperson.Text = match.Groups[1].Value.ToString();
                            ////            if (int.Parse(home.newperson.Text) > 0)
                            ////                Global.hasNewPeople = true;
                            ////                //home.NavigationService.Navigate(new Uri("AllPeopleInfo.xaml",UriKind.Relative));
                            ////        }
                            ////        if (i == 3)
                            ////            home.allpeople.Text = match.Groups[1].Value.ToString();
                            ////        i++;
                            ////    }
                            ////    home.pb.Visibility = Visibility.Collapsed;
                            ////    Global.homeOK = true;
                            ////    Global.isFirstLoad = false;
                            ////}
                            ////catch (Exception err)
                            ////{
                            ////    Global.StoErr("RefreshPeopleNum", err);
                            ////    if (home != null)
                            ////    {
                            ////        //home.t.Text = err.Message;
                            ////        home.toast.Message = "写入资料出错";
                            ////        home.toast.Show();
                            ////    }
                                ////}
#endregion
                                break;

                case "GetMessages":
                                //SQLiteAsyncConnection(ApplicationData.Current.LocalFolder.Path + "\\note.db");
                            pattern = "\"msg_item\":((\\S|\\s)*).msg_item,";
                            var mm = Regex.Match(responseInfo, pattern);
                            string  temp = mm.Groups[1].Value.ToString();

                            pattern = "\"id\":(?<id>[\\d]*),\"type\":[\\d]*,\"fakeid\":\"(?<fakeid>[\\d]*)\",\"nick_name\":\"(?<nickname>[^\"]*)\",\"date_time\":(?<time>[\\d]*),\"content\":\"(?<content>[^\"]*)\",\"source\":\"[^\"]*\",(|\"is_starred_msg\":(?<isstar>[\\d]*),)\"msg_status\":[\\d]*,(|\"remark_name\":\"(?<remarkname>[^\"]*)\",)\"has_reply\":(?<hasreply>[\\d]*),\"refuse_reason\":\"[^\"]*\",";
                            //pattern = @""id":(?<id>[\S]*),"type":\S*,"fakeid":"(?<fakeid>[\S]*)","nick_name":"(?<nickname>[\S]*)","date_time":(?<time>[\S]*),"content":"(?<content>[\S]*)","source":"\S*","msg_status":\S*,"has_reply":(?<hasreply>[\S]*),"refuse_reason":"\S*","multi_item":\[\S*\],"to_uin":\S*";
                            ms = Regex.Matches(temp, pattern);

                            if (ms.Count > 0)
                            {
                                con = new SQLiteAsyncConnection(ApplicationData.Current.LocalFolder.Path + "\\"+LoginInfo.UserName + "\\NewMessage.db");
                               // if (con.Table<NewMessagesTable>() == null)
                                    await con.CreateTableAsync<NewMessagesTable>();
                                foreach (Match match in ms)
                                {
                                        Global.NewMessagesCnt--;
                                        string showName = match.Groups["nickname"].Value;
                                        if (!String.IsNullOrEmpty(match.Groups["remarkname"].Value.ToString()))
                                            showName = match.Groups["remarkname"].Value.ToString();
                                        //创建一条表的数据
                                        //MessageTable newmessage = new MessageTable { Num = loginInfo.UserName + ":" + match.Groups["id"].Value, TalkId = match.Groups["id"].Value, ownId = loginInfo.UserName, FakeId = match.Groups["fakeid"].Value, NickName = showName, Time = match.Groups["time"].Value, Content = match.Groups["content"].Value, has_Reply = match.Groups["hasreply"].Value, is_star = match.Groups["isstar"].Value };
                                        await con.InsertAsync(new NewMessagesTable { Talkid = match.Groups["id"].Value, Username = LoginInfo.UserName, FakeId = match.Groups["fakeid"].Value, Nickname = showName, Time = match.Groups["time"].Value, Content = match.Groups["content"].Value, Hasreply = match.Groups["hasreply"].Value, Isstar = match.Groups["isstar"].Value});
                                       
                                    if (Global.NewMessagesCnt <= 0) break;
                                }//做好截至

                                await PageInit.newMessagesPage.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                                {
                                    PageInit.newMessagesPage.LongDataBind();
                                });
                         
                            }
                        

                 break;

                case "GetAllPeopleInfo":
                               List<string> group = new List<string>();
                               List<string> cnt = new List<string>();
                               List<string> name = new List<string>();

                               pattern = "\"id\":(?<id>[\\d]*),\"name\":\"(?<name>[^\"]*)\",\"cnt\":(?<cnt>[\\d]*)}";
                               ms = Regex.Matches(responseInfo, pattern);
                               foreach (Match match in ms)
                               {
                                   if (int.Parse(match.Groups["cnt"].Value.ToString()) != 0)
                                   {
                                       group.Add(match.Groups["id"].Value);
                                       name.Add(match.Groups["name"].Value);
                                       cnt.Add(match.Groups["cnt"].Value);
                                   }
                               }

                               string[] groupidList = group.ToArray();
                               string[] nameList = name.ToArray();
                               string[] cntList = cnt.ToArray();

                               int peopleNum = 0;
                               for (int j = 0; j < groupidList.Length; j++)
                               {
                                   peopleNum += int.Parse(cnt[j]);
                               }
                               Global.PeopleSum = peopleNum;
                               Global.PerPersonProgress = 90.000 / peopleNum;
                               await PageInit.downloadInfoPage.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                               {
                                   PageInit.downloadInfoPage.SetProgressValue(10);
                                   PageInit.downloadInfoPage.FindInfo(groupidList,nameList,cntList);
                               });
                              
                 break;

                case "GetInfoWithIcon":
                       con = new SQLiteAsyncConnection(ApplicationData.Current.LocalFolder.Path + "\\"+LoginInfo.UserName + "\\NewMessage.db");
                       await con.CreateTableAsync<PersonalInfoTable>();
                       // string pattern;
                        pattern = "\"id\":(?<id>[\\d]*),\"nick_name\":\"(?<nickname>[^\"]*)\",\"remark_name\":\"(?<remarkname>[^\"]*)\",\"group_id\":(?<groupid>[\\d]*)";
                        ms = Regex.Matches(responseInfo, pattern);
                        foreach (Match match in ms)
                        {
                            StorageFile infoStorageImage;
                            try
                            {
                                infoStorageImage = await ApplicationData.Current.LocalFolder.GetFileAsync(LoginInfo.UserName + "\\ICO" + "\\ico" + match.Groups["id"].Value + ".jpg");
                                await PageInit.downloadInfoPage.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                                {
                                    PageInit.downloadInfoPage.AddProgressValue((int)Global.PerPersonProgress);
                                });
                            }
                            catch 
                            {
                                infoStorageImage = null;
                            }

                            if (infoStorageImage == null)
                            {
                                //sw.WriteLine("fakeid:" + match.Groups["id"].Value + ":nickname:" + match.Groups["nickname"] + ":remarkname:" + match.Groups["remarkname"].Value + ":grupid:" + match.Groups["groupid"].Value);
                                await con.InsertAsync(new PersonalInfoTable { Username=LoginInfo.UserName,Fakeid = match.Groups["id"].Value, Nickname = match.Groups["nickname"].Value, Remarkname = match.Groups["remarkname"].Value, Groupid = match.Groups["groupid"].Value });
                                string tempUri = "https://mp.weixin.qq.com/misc/getheadimg?fakeid=" + match.Groups["id"].Value + "&token=" + LoginInfo.Token + "&lang=zh_CN";
                                string tempRefer = "https://mp.weixin.qq.com/cgi-bin/contactmanage?t=user/index&pagesize="+Global.PeopleSum+"&pageidx=0&type=0&token="+LoginInfo.Token+"&lang=zh_CN";
                               // Global.jpgName = "allico";
                                //getImage(tempUri, tempRefer);
                                HttpImageGet getInfoWithIcon = new HttpImageGet();
                                getInfoWithIcon.Operate = "GetInfoWithIcon";
                                getInfoWithIcon.GetImageOperate(tempUri,tempRefer);
                            }
                           
                        }

                        await PageInit.downloadInfoPage.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            PageInit.downloadInfoPage.SetProgressValue(100);
                            PageInit.downloadInfoPage.SetState("完成");
                            PageInit.downloadInfoPage.SetButtonContent("继续");
                            //begin.Content = "继续";
                            //82,376,0,0
                            //begin.Margin = new Thickness(82, 376, 0, 0);
                            //begin.Visibility = Visibility.Visible;
                            PageInit.downloadInfoPage.SetButtonVisibility(Visibility.Visible);
                        });

                 break;

                case "RefreshPersonMessage":
                                 ItemInGroup1 tmp =new ItemInGroup1();
                                  tmp.Key = DateTime.Now.ToString(); 
                                  
                                 pattern = "\"id\":(?<id>[\\d]*),\"type\":(?<type>[\\d]*),\"fakeid\":\"(?<fakeid>[\\d]*)\",\"nick_name\":\"(?<nickname>[^\"]*)\",\"date_time\":(?<time>[\\d]*),(\"content\":\"(?<content>[^\"]*)\"|)";
                                //pattern="\"id\":(?<id>[\\d]*),\"type\":(?<type>[\\d]*),\"fakeid\":\"(?<fakeid>[\\d]*)\",\"nick_name\":\"(?<nickname>[^\"]*)\",\"date_time\":(?<time>[\\d]*),(\"content\":\"(?<content>[^\"]*)\"|),\"source\":\"[^\"]*\",\"msg_status\":[\\d]*,\"has_reply\":[\\d]*,\"refuse_reason\":\"[^\"]*\",\"multi_item\":\\[[^\"]*\\],\"to_uin\":(?<tofakeid>[\\d]*),";
                                 ms = Regex.Matches(responseInfo, pattern);

                                 if (ms.Count != 0)
                                 {
                                     bool isRun = false;
                                     int index = 0; long tempMax = -1;
                                     con = new SQLiteAsyncConnection(ApplicationData.Current.LocalFolder.Path + "\\" + LoginInfo.UserName + "\\NewMessage.db");
                                     await con.CreateTableAsync<TalkMessageTable>();
                                   string   vv = new StorageOperate().SettingStorage(LoginInfo.UserName + "MaxTalkId" + Global.PageFakeid).ToString();
                                     if (new StorageOperate().SettingStorage(LoginInfo.UserName + "MaxTalkId" + Global.PageFakeid).ToString() == "")
                                         new StorageOperate().SettingStorage(LoginInfo.UserName + "MaxTalkId" + Global.PageFakeid, "0");
                                     try
                                     {
                                         long fff= long.Parse(new StorageOperate().SettingStorage(LoginInfo.UserName + "MaxTalkId" + Global.PageFakeid).ToString());
                                         while (long.Parse(ms[index].Groups["id"].Value.ToString()) >fff)
                                         {
                                             isRun = true;
                                             if (index == 0) tempMax = long.Parse(ms[index].Groups["id"].Value.ToString());
                                             await con.InsertAsync(new TalkMessageTable { Talkid = ms[index].Groups["id"].Value, Username = LoginInfo.UserName, Fromfakeid = ms[index].Groups["fakeid"].Value, Tofakeid = ms[index].Groups["fakeid"].Value==Global.PageFakeid?LoginInfo.UserName:Global.PageFakeid, Time = ms[index].Groups["time"].Value, Content = ms[index].Groups["content"].Value });
                                           
                                             //Item1 item = new Item1();
                                             //item.Key = tmp.Key;
                                             string content= ms[index].Groups["content"].Value;
                                             string fakeid= Global.PageFakeid;
                                             DateTime time = TimeStamp.GetTime(ms[index].Groups["time"].Value);
                                             string url = "";
                                             if (ms[index].Groups["fakeid"].Value.ToString() == Global.PageFakeid)
                                                 url = LoginInfo.UserName + "\\ICO" + "\\ico" + ms[index].Groups["fakeid"].Value .ToString()+ ".jpg";
                                             else
                                                 url = LoginInfo.UserName + "\\PersonIcon.jpg";

                                             await PageInit.talkPage.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                                             {
                                                 //  if(tmp.ItemContent!=null)
                                                 if(!String.IsNullOrEmpty(content))
                                                     PageInit.talkPage.AppendToLong(content, ms[index].Groups["fakeid"].Value, url, time);
                                                 // Item1 tmp = new Item1();
                                                 //PageInit.talkPage.LongDataBind();
                                             });
                                            //BitmapImage newJpg = new BitmapImage();
                                             //try
                                             //{
                                             //    var Icon = await ApplicationData.Current.LocalFolder.GetFileAsync(url);
                                             //    IRandomAccessStream iconStream = await Icon.OpenAsync(FileAccessMode.Read);
                                             //    newJpg.SetSource(iconStream);
                                             //}
                                             //catch
                                             //{
                                             //    string l = "ms-appx:///Design/getheadimg.png";
                                             //    newJpg.UriSource = new Uri(l);
                                             //}
                                             //item.l_imagesource = newJpg;
                                             //item.l_imagesource = null;
                                            // tmp.ItemContent.Add(item);
                                             index++;
                                             if (index == ms.Count)
                                                 break;
                                         }
              
                                         if (tempMax > long.Parse(new StorageOperate().SettingStorage(LoginInfo.UserName + "MaxTalkId" + Global.PageFakeid).ToString()))
                                             new StorageOperate().SettingStorage(LoginInfo.UserName + "MaxTalkId" + Global.PageFakeid, tempMax.ToString());
                                         if (isRun)
                                         {
                                             await PageInit.talkPage.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                                             {
                                                 PageInit.talkPage.ScrollToBottom();
                                             });
                                         }
                             
                                     }
                                     catch (Exception err)
                                     {
                                        // int ddd;
                                     }
                                 }

                       
                             //await PageInit.talkPage.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                              // {
                                 //  if(tmp.ItemContent!=null)
                                  // PageInit.talkPage.AppendToLong(tmp);
                                  // Item1 tmp = new Item1();
                                   //PageInit.talkPage.LongDataBind();
                             //  });

                break;
                case "RefreshSpan":
                      
                break;

                case "SendGroup":
                        pattern = " data:{(?<data>[^}]*)},";
                        Match m1 = Regex.Match(responseInfo,pattern);
                        temp = m1.Groups["data"].Value.ToString();

                        pattern = "ticket:\"(?<ticket>[^\"]*)\"";
                         m1 = Regex.Match(temp,pattern);
                        LoginInfo.Ticket = m1.Groups["ticket"].Value.ToString();

                        pattern = " user_name:\"(?<username>[^\"]*)\"";
                        m1 = Regex.Match(temp,pattern);
                        LoginInfo.UniformUserName = m1.Groups["username"].Value.ToString();

                        pattern = "wx.cgiData = {(?<info>[\\S\\s]*)seajs.use";   //可以匹配更多信息
                         m1 = Regex.Match(responseInfo, pattern);
                        temp = m1.Groups["info"].Value.ToString();

                        pattern = "operation_seq: \"(?<seq>[\\d]*)\"";
                         m1 = Regex.Match(temp, pattern);
                        LoginInfo.Seq = m1.Groups["seq"].Value.ToString();

                        pattern = "\"id\":(?<id>[\\d]*),\"name\":\"(?<name>[^\"]*)\",\"cnt\":(?<cnt>[\\d]*)";
                         ms = Regex.Matches(temp,pattern);
                        Global.groupsInfo = new Dictionary<string, string>();
                        Global.groupsInfo.Add("全部","-1");
                        foreach(Match a in ms)
                        {
                            Global.groupsInfo.Add(a.Groups["name"].Value.ToString() + " " + a.Groups["cnt"].Value.ToString()+"人",a.Groups["id"].Value.ToString());
                        }
                        
                              await PageInit.sendGroup.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                               {
                                   PageInit.sendGroup.SetListPicker();
                                   PageInit.sendGroup.SetState(1);
                               });


                
                break;
                case "send":
                JArray ja = new JArray(JsonConvert.DeserializeObject(responseInfo));
                string ret = ja[0]["base_resp"]["ret"].ToString();
                string msg = ja[0]["base_resp"]["err_msg"].ToString();
                await PageInit.sendGroup.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    PageInit.sendGroup.SendResult(ret,msg);
                });

                break;

                case "SendMessage":
                try
                {
                    ja = new JArray(JsonConvert.DeserializeObject(responseInfo));
                    string returnValue= ja[0]["base_resp"]["ret"].ToString();
                    string Result = ja[0]["base_resp"]["err_msg"].ToString();
                    if (returnValue=="0"||Result == "ok")
                    {
                        await PageInit.talkPage.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                          {
                              PageInit.talkPage.ShowTip("发送成功");
                              PageInit.talkPage.ClearMyMessage();
                          });
                       // tmr.Stop();
                      //  tmr.Start();
                    }
                    else if (returnValue=="10706"||Result == "customer block")
                    {
                        await PageInit.talkPage.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            PageInit.talkPage.ShowTip("48小时未联系");
                        });
                       // tmr.Stop();
                      //  tmr.Start();
                    }
                    else if (returnValue=="-1"||Result == "system error")
                    {
                        await PageInit.talkPage.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            PageInit.talkPage.ShowTip("系统错误");
                        });
                        //tmr.Stop();
                       // tmr.Start();
                    }
                    else
                    {
                        await PageInit.talkPage.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            PageInit.talkPage.ShowTip("发送失败"+Result);
                        });
                       // tmr.Stop();
                       // tmr.Start();
                    }
                }
                catch (Exception err)
                {
                }
                break;
            }                                         
        }


       async Task<int> Login()
        {
            try
            {
                JArray ja = new JArray(JsonConvert.DeserializeObject(responseInfo));
                LoginInfo.Err = ja[0]["base_resp"]["err_msg"].ToString();
                string n = ja[0]["base_resp"]["ret"].ToString();
                if (n == "0")
                {
                    Global.HomeUrl = ja[0]["redirect_url"].ToString();
                    //string aa = ja[0]["redirect_url"].ToString();
                    string partten = "token=(?<token>[\\d]*)";
                    var m = Regex.Match(Global.HomeUrl, partten);
                    string token = m.Groups["token"].Value.ToString();
                    LoginInfo.Token = token;
                }
                return int.Parse(n);
            }
            catch (Exception err)
            {

            }
            return 0;
        }


       async void LoginOK()
        {
           ApplicationDataCompositeValue loginCertificate=new ApplicationDataCompositeValue();
           loginCertificate["Account"]=LoginInfo.UserName;
           loginCertificate["Password"]=LoginInfo.Password;
          // LoginInfo.UserSettings.Values["LoginCertificate"] = loginCertificate;
           ApplicationDataContainer UserSettings = ApplicationData.Current.RoamingSettings;
           UserSettings.Values["LoginCertificate"] = loginCertificate;
           if (string.IsNullOrEmpty(LoginInfo.Token))
           {
               await PageInit.mainPage.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    PageInit.mainPage.SetLoginStateText("检测不到Token,可能您开通了微信保护功能");
               });
               return;
           }

           #region
           //if (!Sto.Info.Contains(that.acc.Text + "LaunchTimes"))
           //    Sto.Info[that.acc.Text + "LaunchTimes"] = 1;

           //if (Sto.File.DirectoryExists(that.acc.Text) && (Convert.ToInt32(Sto.Info[that.acc.Text + "LaunchTimes"]) == 15 || Sto.Info["hasErr"].ToString() == "1" || Global.isFirstSetupOrUpdate == 1))
           //{
           //    Sto.Info[that.acc.Text + "LaunchTimes"] = 1;
           //    Sto.deleteFile(that.acc.Text);
           //    that.toggle.IsChecked = false;
           //}


           //if (loginInfo.LoginCookie == null)
           //    loginInfo.LoginCookie = cc;

           //loginInfo.CreateDate = DateTime.Now;
           //loginInfo.Token = token;
           //loginInfo.Err = null;

           //that.pb.Visibility = Visibility.Collapsed;
           //that.state.Visibility = Visibility.Collapsed;

           //Global.isFirstLoad = true;
           //if (Sto.File.DirectoryExists(loginInfo.UserName + "AllInfo"))
           //    that.NavigationService.Navigate(new Uri("/HomePage.xaml", UriKind.Relative));
           //else
           //    that.NavigationService.Navigate(new Uri("/AllPeopleInfo.xaml", UriKind.Relative));
           #endregion

              StorageFolder Fold;
              try
              {
                  Fold = await ApplicationData.Current.LocalFolder.GetFolderAsync(LoginInfo.UserName + "\\ICO");
              }
              catch
              {
                  Fold = null;
              }
           await PageInit.mainPage.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    PageInit.mainPage.SetLoginStateText("登陆成功");
                    PageInit.mainPage.ShowVeriCode(0);
                    PageInit.mainPage.SetLoginProgressVisibility(0);
                    //如果没有用户信息则导航到导入信息的页面
                    if (Fold == null)
                        PageInit.mainPage.Frame.Navigate(typeof(DownloadInfo));
                    else
                        PageInit.mainPage.Frame.Navigate(typeof(Home));
                });
        }

     public  async  void LoginErr(string n)
        {
            string i = "";
            switch (n)
            {
                case "-100":
                    i = "正在刷新验证码";
                    break;
                case "-1":
                    i = "系统错误，请稍候再试。";
                    break;
                case "-2":
                    i = "帐号或密码错误。";
                    break;
                case "-23":
                    i = "您输入的帐号或者密码不正确，请重新输入。";
                    break;
                case "-21":
                    i = "不存在该帐户。";
                    break;
                case "-7":
                    i = "您目前处于访问受限状态。";
                    break;
                case "-8":
                    { i = "请输入图中的验证码"; }
                    break;
                case "-27":
                    { i = "您输入的验证码不正确，请重新输入"; }
                    break;
                case "-26":
                    i = "该公众会议号已经过期，无法再登录使用。";
                    break;
                case "0":
                    { i = "成功登录，正在跳转..."; }
                    break;
                case "-25":
                    i = "海外帐号请在公众平台海外版登录";
                    break;
                default:
                    i = "未知的返回。";
                    break;
            }
            await PageInit.mainPage.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    PageInit.mainPage.SetLoginProgressVisibility(0);
                    PageInit.mainPage.SetLoginStateText(i);
                });

            if (n == "-8" || n == "-27"||n=="-100")
            {
                await PageInit.mainPage.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        string code_url = "https://mp.weixin.qq.com/cgi-bin/verifycode?username=" + PageInit.mainPage.GetAccountText() + "&r=" + (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;
                        HttpImageGet getVeriCode = new HttpImageGet();
                        getVeriCode.Operate = "VeriCodeOperate";
                        getVeriCode.GetImageOperate(code_url, "https://mp.weixin.qq.com/");
                        PageInit.mainPage.ShowVeriCode(1);
                    });
            }
        }

    }
}
