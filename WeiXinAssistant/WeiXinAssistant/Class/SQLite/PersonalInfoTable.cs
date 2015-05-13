using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.ComponentModel;

namespace WeiXinAssistant
{
   public  class PersonalInfoTable  : INotifyPropertyChanged
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

        private string username;
        public string Username
        {
            get { return username; }
            set
            {
                if (value != username)
                {
                    username = value;
                    NotifyPropertyChanged("Username");

                }
            }
        }


        private string fakeid;
        public string Fakeid
        {
            get { return fakeid; }
            set
            {
                if (value != fakeid)
                {
                    fakeid = value;
                    NotifyPropertyChanged("Fakeid");

                }
            }
        }

        private string nickname;
        public string Nickname
        {
            get { return nickname; }
            set
            {
                if (value != nickname)
                {
                    nickname = value;
                    NotifyPropertyChanged("Nickname");

                }
            }
        }

        private string remarkname;
        public string Remarkname
        {
            get { return remarkname; }
            set
            {
                if (value != remarkname)
                {
                    remarkname = value;
                    NotifyPropertyChanged("Remarkname");

                }
            }
        }

        private string groupid;
        public string Groupid
        {
            get { return groupid; }
            set
            {
                if (value != groupid)
                {
                    groupid = value;
                    NotifyPropertyChanged("Groupid");

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
