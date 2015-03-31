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
        private double step = 0;

        private Progress() {
            this.ProgressValue = 0;
            this.MaxValue = 100;
        }

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

        private double progressValue;

        public double ProgressValue
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


        public void CalcStep(int howMuch)
        {
            step = 100.00/howMuch;
        }

        public void UpdateProgressValue()
        {
            this.ProgressValue = this.ProgressValue + step;
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
