using System;
using System.Runtime.InteropServices;

namespace send_file_to_ps3
{
    class Program
    {
        [DllImport("user32.Dll")]
        public static extern int MessageBeep(uint n);
        static void Main(string[] args)
        {
            string error = "";
            try
            {   
                error = "host error";
                string host = args[0];
                error = "localPath error";
                string localFilePath = args[1];
                error = "serverPath error";
                string serverFilePath = args[2];
                error = "error connecting..";
                using (FtpConnection ftp = new FtpConnection(host))
                {

                    if (!ftp.IsConnected)
                        Console.WriteLine(error);
                    error = "error moving file..!";
                    string fileName = serverFilePath.Contains("/") ? serverFilePath.Substring(serverFilePath.LastIndexOf('/')).Replace("/", "").Replace("//", "") : serverFilePath;
                    string dirPath = serverFilePath.Contains("/") ? serverFilePath.Substring(0, serverFilePath.LastIndexOf('/')) + '/' : "dev_hdd0/";
                    ftp.SetCurrentDirectory(dirPath);

                    ftp.PutFile(localFilePath, fileName);
                    foreach (var item in ftp.GetFiles())
                    {
                        if (item.FullName == fileName)
                        {
                            Console.WriteLine("success :\nsource( {0} )\ndestination( {1} )", localFilePath, dirPath+ fileName);
                            MessageBeep(0);
                            break;
                        }
                    }
                  
                }
            }
            catch
            {
                Console.WriteLine(error);
                MessageBeep(16);
                Console.Read();
            }
        }
    }
}
