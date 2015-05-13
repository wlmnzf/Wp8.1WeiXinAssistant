using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace WeiXinAssistant
{
   public class StorageOperate
    {
      public async Task<bool> LocalStorage(string Path,string fileName,  byte[] bytes)
       {
           try
           {
               StorageFolder localFolderStorage = ApplicationData.Current.LocalFolder;
               var localFolder = await localFolderStorage.CreateFolderAsync(Path, CreationCollisionOption.OpenIfExists);
               var localFile = await localFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

                   using (StorageStreamTransaction transaction = await localFile.OpenTransactedWriteAsync())
                   {
                       //Stream targetStream = await localFile.OpenStreamForWriteAsync(); //localFolderStorage.OpenStreamForWriteAsync(Path+"\\"+fileName,  CreationCollisionOption.OpenIfExists))
                       DataWriter dataWriter = new DataWriter(transaction.Stream);
                       dataWriter.WriteBytes(bytes);
                       await dataWriter.StoreAsync();
                       //stream.CopyTo(targetStream);
                       // stream.Dispose();
                       //targetStream.Close();
                   }

               return true;
           }
           // Use catch blocks to handle errors
           catch (Exception err)
           {
               return false;
           }
       }



      public async Task<bool> TempStorage(string fileName,Stream stream)
      {
          try
          {
              StorageFolder localTempStorage = ApplicationData.Current.TemporaryFolder;
              //var localFolder = await localTempStorage.CreateFolderAsync(Path, CreationCollisionOption.OpenIfExists);
              var localFile = await localTempStorage.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
              if (localFile != null)
              {
                  using (Stream targetStream = await localTempStorage.OpenStreamForWriteAsync( fileName, CreationCollisionOption.OpenIfExists))
                  {
                      stream.CopyTo(targetStream);
                      stream.Dispose();
                      //targetStream.Close();
                  }
              }
              return true;
          }
          // Use catch blocks to handle errors
          catch (FileNotFoundException)
          {
              return false;
          }
      }

      public  void SettingStorage(string key,string value)
      {
          var setting = ApplicationData.Current.LocalSettings;
          setting.Values[key] = value;
      }
      public string SettingStorage(string key)
      { 
         var setting = ApplicationData.Current.LocalSettings;
          //setting.Values[key] = value;
         if (setting.Values.ContainsKey(key))
         {
             return setting.Values[key].ToString();
         }
         else
             return "";
      }
     

    }
}
