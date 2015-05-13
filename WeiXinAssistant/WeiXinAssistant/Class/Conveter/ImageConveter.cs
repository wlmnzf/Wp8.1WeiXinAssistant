using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml;

namespace WeiXinAssistant
{
    public class LeftImageConveter : IValueConverter
    {
        public object Convert(object value, Type targetType,object parameter, string language)
        {
            string fakeid = value.ToString();
            if (fakeid == Global.PageFakeid)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }


    public class RightImageConveter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string fakeid = value.ToString();
            if (fakeid == Global.PageFakeid)
                return Visibility.Collapsed;
            else
                return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
