using BackupWithFTP.Class;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BackupWithFTP.Controller
{
    class MainController
    {

        public Progress prog { get { return Progress.Instance; } }
        public MainController()
        {
            CreateSelectCommand();
            CreateSaveCommand();
            //  Watcher watch = new Watcher(Properties.Settings.Default.Directory);
        }

        public ICommand SelectCommand
        {
            get;
            internal set;
        }

        private void CreateSelectCommand()
        {
            SelectCommand = new RelayCommand(SelectExecute);
        }

        public void SelectExecute()
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if (dialog.SelectedPath != null)
            {
                Properties.Settings.Default.Directory = dialog.SelectedPath;
            }
        }

        public ICommand SaveCommand
        {
            get;
            internal set;
        }

        private void CreateSaveCommand()
        {
            SaveCommand = new RelayCommand(SaveExecute);
        }

        public void SaveExecute()
        {
            Properties.Settings.Default.Save();
            DirectoryInfo dir = new DirectoryInfo(Properties.Settings.Default.Directory);
            StartUp start = new StartUp();
            start.GetAllFiles(dir);
            Progress.Instance.MaxValue = start.files.Count();
            start.SyncAllFiles();
        }


    }
}
