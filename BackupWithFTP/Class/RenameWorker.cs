using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupWithFTP.Class
{
    public class RenameWorker : GlobalWorker
    {
        private HashSet<KeyValuePair<string, string>> filesToRename = new HashSet<KeyValuePair<string, string>>();
        protected override void GlobalWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (filesToRename.Count > 0)
            {
                this.RunWorkerAsync();
            }
        }

        public void AddFile(string oldFileName, string newFileName)
        {
            filesToRename.Add(new KeyValuePair<string, string>(oldFileName, newFileName));
            if (!this.IsBusy)
            {
                this.RunWorkerAsync();
            }
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            if (CancellationPending) { e.Cancel = true; return; }
            KeyValuePair<string, string> fileNames = filesToRename.First();
            ftp.Rename(fileNames.Key.Replace(Properties.Settings.Default.Directory, ""), fileNames.Value);
            filesToRename.Remove(fileNames);
        }
    }
}
