using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupWithFTP.Class
{
    public abstract class GlobalWorker : BackgroundWorker
    {
        protected FTP ftp = new FTP(Properties.Settings.Default.FTPHost, Properties.Settings.Default.FTPUsername, Properties.Settings.Default.FTPPassword,Properties.Settings.Default.Versions);
       

        protected GlobalWorker()
        {
            WorkerReportsProgress = true;
            WorkerSupportsCancellation = true;
            this.RunWorkerCompleted += GlobalWorker_RunWorkerCompleted;
            this.ProgressChanged += GlobalWorker_ProgressChanged;

        }


        protected void GlobalWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Progress.Instance.UpdateProgressValue();

        }

        protected abstract void GlobalWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e);


    }
}
