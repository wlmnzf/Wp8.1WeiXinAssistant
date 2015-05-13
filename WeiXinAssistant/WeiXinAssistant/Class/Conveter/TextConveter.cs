using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml;

namespace WeiXinAssistant
{
    public class TextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string fakeid = value.ToString();
            if (fakeid == Global.PageFakeid)
                return HorizontalAlignment.Left;
            else
                return HorizontalAlignment.Right;
        }

        public object ConvertBack(object value, Type targetType,object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
