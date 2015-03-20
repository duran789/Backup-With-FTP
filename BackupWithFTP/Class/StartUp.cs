using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupWithFTP.Class
{
    public class StartUp
    {
        public List<string> files = new List<string>();  // List that will hold the files and subfiles in path
        private List<DirectoryInfo> folders = new List<DirectoryInfo>(); // List that hold direcotries that cannot be accessed
        private UploadWorker upload = new UploadWorker();

        public StartUp()
        {
            
        }
        
        public void GetAllFiles(DirectoryInfo dir)
        {

            try
            {
                foreach (FileInfo f in dir.GetFiles())
                {
                    //Console.WriteLine("File {0}", f.FullName);
                    files.Add(f.FullName.ToString());
                }
            }
            catch
            {
                Console.WriteLine("Directory {0}  \n could not be accessed!!!!", dir.FullName);
                return;  // We alredy got an error trying to access dir so dont try to access it again
            }

            // process each directory
            // If I have been able to see the files in the directory I should also be able 
            // to look at its directories so I dont think I should place this in a try catch block
            foreach (DirectoryInfo d in dir.GetDirectories())
            {
                folders.Add(d);
                GetAllFiles(d);
            }


        }

        public void SyncAllFiles()
        {
            foreach (string filePath in files)
            {
                upload.AddFile(filePath);
            }
        }
    }
}
