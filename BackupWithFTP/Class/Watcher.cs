using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupWithFTP.Class
{
    public class Watcher
    {
        private UploadWorker upload = new UploadWorker();
        private RenameWorker rename = new RenameWorker();

        public Watcher(string path)
        {
            try
            {
                FileSystemWatcher watcher = new FileSystemWatcher();
                watcher.Path = path;
                watcher.IncludeSubdirectories = true;
                //watcher.WaitForChanged(WatcherChangeTypes.All);
                watcher.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
                //       // Only watch text files.
                //       watcher.Filter = "*.txt";

                //       // Add event handlers.
                //       watcher.Changed += new FileSystemEventHandler(OnChanged);
                //       watcher.Created += new FileSystemEventHandler(OnChanged);
                //       watcher.Deleted += new FileSystemEventHandler(OnChanged);
                //       watcher.Renamed += new RenamedEventHandler(OnRenamed);
                watcher.Changed += watcher_Changed;
                watcher.Created += watcher_Created;
                watcher.Renamed += watcher_Renamed;
                //       // Begin watching.
                watcher.InternalBufferSize = 32768;
                watcher.EnableRaisingEvents = true;


            }
            catch (Exception exp)
            {
                string p = exp.Message;
            }
        }

        private void watcher_Renamed(object sender, RenamedEventArgs e)
        {
            //throw new NotImplementedException();
           // e.OldFullPath
            if (IsFileReady(e.FullPath))
            {
                rename.AddFile(e.OldFullPath, e.Name.Substring(e.Name.LastIndexOf('\\')+1));
            }
           
        }

        private void watcher_Created(object sender, FileSystemEventArgs e)
        {
            if (IsFileReady(e.FullPath))
            {
                upload.AddFile(e.FullPath);
            }
        }

        private void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (IsFileReady(e.FullPath))
            {
                upload.AddFile(e.FullPath);
            }
        }

        private bool IsFileReady(string path)
        {   //One exception per file rather than several like in the polling pattern       
            try
            {
                //If we can't open the file, it's still copying
                using (var file = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    return true;
                }
            }
            catch (Exception exp)
            {
                return false;
            }
        }
    }
}
