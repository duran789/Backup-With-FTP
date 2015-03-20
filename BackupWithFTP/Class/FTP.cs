using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BackupWithFTP.Class
{
    public class FTP
    {
        private string host = null;
        private string user = null;
        private string pass = null;
        private FtpWebRequest ftpRequest = null;
        private FtpWebResponse ftpResponse = null;
        private Stream ftpStream = null;
        private int bufferSize = 2048;
        private int versions = 0;

        public FTP(string hostIP, string userName, string password, int versions)
        {
            host = hostIP;
            user = userName;
            pass = password;
            this.versions = versions;
        }


        public string UploadFile(string remoteFileName, string localFile)
        {
            try
            {
                remoteFileName = remoteFileName.Replace("\\", "/");
                MakeFTPDir(remoteFileName);
                if (CheckFileExists(remoteFileName))
                {
                    string fileName=localFile.Substring(localFile.LastIndexOf('\\')+1);
                    string name = fileName.Substring(0,fileName.LastIndexOf('.') );
                    string ext=fileName.Substring(fileName.LastIndexOf('.') + 1);
                    for (int i = versions; i > 0; i--)
                    {
                        fileName = name + "-" + i.ToString() + "-." + ext;
                        if (CheckFileExists(fileName))
                        {
                            
                                int j = i + 1;
                                string newFilename = name + "-" + j.ToString() + "-." + ext;
                                Rename(fileName, newFilename);                     
                        }
                    }                   
                    Rename(remoteFileName, fileName);
                }

                Upload(remoteFileName, localFile);

                return "";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        private string Upload(string remoteFileName, string localFile)
        {
            try
            {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + remoteFileName);
                ftpRequest.ConnectionGroupName = "BackUpWithFTP";
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
                /* Establish Return Communication with the FTP Server */
                ftpStream = ftpRequest.GetRequestStream();
                /* Open a File Stream to Read the File for Upload */
                FileStream localFileStream = new FileStream(localFile, FileMode.Open);
                /* Buffer for the Downloaded Data */
                byte[] byteBuffer = new byte[bufferSize];
                int bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
                /* Upload the File by Sending the Buffered Data Until the Transfer is Complete */
                try
                {
                    while (bytesSent != 0)
                    {
                        ftpStream.Write(byteBuffer, 0, bytesSent);
                        bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
                    }
                }
                catch (Exception ex)
                {
                    return ex.ToString();
                }
                finally
                {
                    /* Resource Cleanup */
                    localFileStream.Close();
                    ftpStream.Close();
                    // ftpRequest = null;
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return "";
        }

        public void Rename(string remoteFileName, string localFile)
        {
            try
            {
                remoteFileName = remoteFileName.Replace("\\", "/");
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)WebRequest.Create(host + "/" + remoteFileName);
                ftpRequest.ConnectionGroupName = "BackUpWithFTP";
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.Rename;
                /* Rename the File */
                ftpRequest.RenameTo = localFile;
                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Resource Cleanup */
                ftpResponse.Close();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                ftpRequest = null;
            }
            return;
        }

        private void MakeFTPDir(string pathToCreate)
        {
            string[] subDirs = pathToCreate.Split('/');
            string address = host;

            for (int i = 0; i < subDirs.Count() - 1; i++)
            {

                //}
                //foreach (string subDir in subDirs)
                //{
                try
                {
                    address = address + "/" + subDirs[i];
                    ftpRequest = (FtpWebRequest)FtpWebRequest.Create(address);
                    ftpRequest.ConnectionGroupName = "BackUpWithFTP";
                    ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
                    ftpRequest.UseBinary = true;
                    ftpRequest.UsePassive = true;
                    ftpRequest.KeepAlive = true;
                    ftpRequest.Credentials = new NetworkCredential(user, pass);
                    FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();
                    ftpStream = response.GetResponseStream();
                    ftpStream.Close();
                    response.Close();
                }
                catch (Exception ex)
                {
                    //directory already exist I know that is weak but there is no way to check if a folder exist on ftp...
                }
            }
        }

        private bool CheckFileExists(string remoteFileName)
        {
            try
            {
                ftpRequest = (FtpWebRequest)WebRequest.Create(host + "/" + remoteFileName);
                ftpRequest.ConnectionGroupName = "BackUpWithFTP";
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                ftpRequest.Method = WebRequestMethods.Ftp.GetDateTimestamp;

                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Resource Cleanup */
                ftpResponse.Close();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
