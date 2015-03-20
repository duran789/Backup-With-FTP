using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupWithFTP.Class
{
    public class Progress : INotifyPropertyChanged
    {


        private static Progress instance;

        private Progress() { }

        public static Progress Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Progress();
                }
                return instance;
            }
        }

        private int progressValue;

        public int ProgressValue
        {
            get { return progressValue; }
            set
            {
                progressValue = value;
                NotifyPropertyChanged("ProgressValue");
            }
        }

        private int maxValue;

        public int MaxValue
        {
            get { return maxValue; }
            set
            {
                maxValue = value;
                NotifyPropertyChanged("MaxValue");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }




    }
}
