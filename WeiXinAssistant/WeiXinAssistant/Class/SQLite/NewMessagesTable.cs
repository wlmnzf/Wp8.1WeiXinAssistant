using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.ComponentModel;

namespace WeiXinAssistant
{
   public class NewMessagesTable:  INotifyPropertyChanged
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
            get
            {
                return talkid;
            }
            set
            {
                if (talkid!= value)
                {
                    talkid= value;
                    NotifyPropertyChanged("Talkid");
                }
            }
        }


        // ownid
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
                    username= value;
                    NotifyPropertyChanged("Username");
                }
            }
        }


        // 定义员工名字字段
        private string fakeid;
        public string FakeId
        {
            get
            {
                return fakeid;
            }
            set
            {
                if (fakeid != value)
                {
                    fakeid = value;
                    NotifyPropertyChanged("Fakeid");
                }
            }
        }

        private string nickname;
        public string Nickname
        {
            get
            {
                return nickname;
            }
            set
            {
                if (nickname != value)
                {
                    nickname = value;
                    NotifyPropertyChanged("NickName");
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

        private string content;
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

        private string hasreply;
        public string Hasreply
        {
            get
            {
                return hasreply;
            }
            set
            {
                if (hasreply != value)
                {
                    hasreply = value;
                    NotifyPropertyChanged("Hasreply");
                }
            }
        }


        private string isstar;
        public string Isstar
        {
            get
            {
                return isstar;
            }
            set
            {
                if (isstar != value)
                {
                    if (value == null)
                        value = "0";
                    isstar = value;
                    NotifyPropertyChanged("Isstar");
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
