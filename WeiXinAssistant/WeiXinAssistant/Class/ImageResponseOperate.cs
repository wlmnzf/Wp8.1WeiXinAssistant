using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;
using System.IO;
using Windows.Storage;
using System.Text.RegularExpressions;


namespace WeiXinAssistant
{
   public  class ImageResponseOperate
    {
        string operateContent;
        Stream imageStream;
        string fakeId;
        public ImageResponseOperate(string Operate,Stream responseStream)
        {
            this.operateContent = Operate;
            imageStream = responseStream;
        }
        public ImageResponseOperate(string Operate, Stream responseStream,string url="")
        {
            this.operateContent = Operate;
            imageStream = responseStream;
            if (url == "")
            {
                this.fakeId = "";
            }
            else
            {
                string pattern = "fakeid=(?<fakeid>[^&]+)";
                var m = Regex.Match(url, pattern);
                this.fakeId = m.Groups["fakeid"].Value;
            }
        }
        //public byte[] StreamToBytes(Stream stream)
        //{
        //    byte[] bytes = new byte[stream.Length];
        //    stream.Read(bytes, 0, bytes.Length);

        //    // 设置当前流的位置为流的开始 
        //    stream.Seek(0, SeekOrigin.Begin);
        //    return bytes;
        //}
        public async void OperateResponse()
        {
            StreamConvent convent;
            IRandomAccessStream memoryStream;
            switch (this.operateContent)
            {
                case "VeriCodeOperate":
                    //var randomAccessStream = new InMemoryRandomAccessStream();
                    //var outputStream = randomAccessStream.GetOutputStreamAt(0);
                    //await RandomAccessStream.CopyAsync(imageStream.AsInputStream(), outputStream);

                    //byte[] bytes = StreamToBytes(imageStream);
                    //InMemoryRandomAccessStream memoryStream = new InMemoryRandomAccessStream();
                    //DataWriter datawriter = new DataWriter(memoryStream.GetOutputStreamAt(0));
                    //datawriter.WriteBytes(bytes);
                    //await datawriter.StoreAsync();
                    convent = new StreamConvent();
                    memoryStream = await convent.Stream2IRandomAccessStream(imageStream);
                    await PageInit.mainPage.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        BitmapImage newImage = new BitmapImage();
                        newImage.SetSource(memoryStream);
                        PageInit.mainPage.SetVeriCode(ref newImage);
                    });
                    break;

                case "GetPersonalIcon":
                    bool result = true;
                    convent = new StreamConvent();
                    memoryStream = await convent.Stream2IRandomAccessStream(imageStream);
                    try
                    {
                 
                       // StorageOperate personalIcon = new StorageOperate();
                        StorageFolder localFolderStorage = ApplicationData.Current.LocalFolder;
                        var localFolder = await localFolderStorage.CreateFolderAsync(LoginInfo.UserName, CreationCollisionOption.OpenIfExists);
                        var localFile = await localFolder.CreateFileAsync("PersonIcon.jpg", CreationCollisionOption.ReplaceExisting);
                        IRandomAccessStream stream = await localFile.OpenAsync(FileAccessMode.ReadWrite);
                       await RandomAccessStream.CopyAsync(memoryStream, stream);

                    }
                    catch {
                        result= false;
                    }
                    

                    if (result == false)
                    {
                        //储存失败，删除原有文件，用默认文件代替

                    }
                    else 
                    {
                        convent = new StreamConvent();
                        //memoryStream = await convent.Stream2IRandomAccessStream(bytes);
                        memoryStream.Seek(0);
                        await PageInit.homePage.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            BitmapImage newImage = new BitmapImage();
                            newImage.SetSource(memoryStream);
                            PageInit.homePage.SetIcon(ref newImage);
                        });
                    }
                    break;

                case "GetInfoWithIcon":
                    result = true;
                    convent = new StreamConvent();
                    memoryStream = await convent.Stream2IRandomAccessStream(imageStream);
                    try
                    {
                       // StorageOperate personalIcon = new StorageOperate();
                        StorageFolder localFolderStorage = ApplicationData.Current.LocalFolder;
                        var localFolder = await localFolderStorage.CreateFolderAsync(LoginInfo.UserName, CreationCollisionOption.OpenIfExists);
                        localFolder = await localFolder.CreateFolderAsync("ICO", CreationCollisionOption.OpenIfExists);
                        var localFile = await localFolder.CreateFileAsync("ico"+fakeId+".jpg", CreationCollisionOption.ReplaceExisting);
                        IRandomAccessStream stream = await localFile.OpenAsync(FileAccessMode.ReadWrite);
                       await RandomAccessStream.CopyAsync(memoryStream, stream);

                    }
                    catch {
                        result= false;
                    }
                    

                    if (result == false)
                    {
                        //储存失败，删除原有文件，用默认文件代替

                    }
                    else 
                    {
                        await PageInit.downloadInfoPage.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            PageInit.downloadInfoPage.AddProgressValue((int)Global.PerPersonProgress);
                        });
                    }
                 break;
            }
        }


        //public async Task<bool> SaveImage(string path,string fileName)
        //{
        //    //string Path=
        //    try
        //    {
        //        StorageOperate veriCode = new StorageOperate();
        //        bool Result = await veriCode.LocalStorage(path,fileName, imageStream);
        //        return true;
        //    }
        //    catch(Exception err)
        //    {
        //       return  false;
        //    }
        //}

    }
}
