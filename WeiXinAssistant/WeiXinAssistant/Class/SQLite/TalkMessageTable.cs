using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.ComponentModel;

namespace WeiXinAssistant
{
    class TalkMessageTable: INotifyPropertyChanged
    {

        private int id;
        [AutoIncrement, PrimaryKey]
        public int ID
        {
            get { return id; }
            set
            {
                if (value != id)
                {
                    id = value;
                    NotifyPropertyChanged("ID");

                }
            }
        }

        private string talkid;
        public string Talkid
        {
            get { return talkid; }
            set
            {
                if (value != talkid)
                {
                    talkid = value;
                    NotifyPropertyChanged("Talkid");

                }
            }
        }

        private string username;
        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                if (username != value)
                {
                    username = value;
                    NotifyPropertyChanged("Username");
                }
            }
        }

        private string fromfakeid;
        public string Fromfakeid
        {
            get
            {
                return fromfakeid;
            }
            set
            {
                if (fromfakeid != value)
                {
                    fromfakeid = value;
                    NotifyPropertyChanged("Fromfakeid");
                }
            }
        }

        private string  content;
        public string Content
        {
            get
            {
                return content;
            }
            set
            {
                if (content != value)
                {
                    content = value;
                    NotifyPropertyChanged("Content");
                }
            }
        }

        private string time;
        public string Time
        {
            get
            {
                return time;
            }
            set
            {
                if (time != value)
                {
                    time = value;
                    NotifyPropertyChanged("Time");
                }
            }
        }

        private string tofakeid;
        public string Tofakeid
        {
            get
            {
                return tofakeid;
            }
            set
            {
                if (tofakeid != value)
                {
                    tofakeid = value;
                    NotifyPropertyChanged("Tofakeid");
                }
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
