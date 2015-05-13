using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.ComponentModel;

namespace WeiXinAssistant
{
    public class GroupListTable : INotifyPropertyChanged
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


        private string groupid;
        public string Groupid
        {
            get
            {
                return groupid;
            }
            set
            {
                if (groupid != value)
                {
                    groupid = value;
                    NotifyPropertyChanged("Groupid");
                }
            }
        }

       private string name;
       public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (name != value)
                {
                    name= value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

       private string cnt;
       public string Cnt
       {
           get
           {
               return cnt;
           }
           set
           {
               if (cnt != value)
               {
                  cnt = value;
                   NotifyPropertyChanged("Cnt");
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
