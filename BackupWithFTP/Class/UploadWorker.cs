using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupWithFTP.Class
{
    public class UploadWorker:  GlobalWorker
    {
        private HashSet<string> filesToUpload = new HashSet<String>();

        public UploadWorker()
        {
          
        }

        protected override void GlobalWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {       
            //throw new NotImplementedException();
            if (filesToUpload.Count > 0)
            {
                this.RunWorkerAsync();
            }
        }

        public void AddFile(string fileName)
        {
            filesToUpload.Add(fileName);
            if (!this.IsBusy)
            {
                this.RunWorkerAsync(); 
            }
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            if (CancellationPending) { e.Cancel = true; return; }
            string fileName = filesToUpload.First();
            ftp.UploadFile(fileName.Replace(Properties.Settings.Default.Directory, ""), fileName);
            filesToUpload.Remove(fileName);
            this.ReportProgress(1);
        }


    }
}
