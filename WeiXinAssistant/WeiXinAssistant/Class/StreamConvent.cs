using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Windows.Storage.Streams;

namespace WeiXinAssistant
{
    class StreamConvent
    {
        public byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始 
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        public async Task<IRandomAccessStream>  Stream2IRandomAccessStream(Stream stream)
        {
            byte[] bytes = StreamToBytes(stream);
            InMemoryRandomAccessStream memoryStream = new InMemoryRandomAccessStream();
            DataWriter datawriter = new DataWriter(memoryStream.GetOutputStreamAt(0));
            datawriter.WriteBytes(bytes);
            await datawriter.StoreAsync();
            return memoryStream;
        }

        public async Task<IRandomAccessStream> Stream2IRandomAccessStream(byte[] bytes)
        {
            InMemoryRandomAccessStream memoryStream = new InMemoryRandomAccessStream();
            DataWriter datawriter = new DataWriter(memoryStream.GetOutputStreamAt(0));
            datawriter.WriteBytes(bytes);
            await datawriter.StoreAsync();
            return memoryStream;
        }
    }
}
