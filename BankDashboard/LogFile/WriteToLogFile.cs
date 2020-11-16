using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace BankDashboard.LogFile
{
    public class WriteToLogFile
    {
        public static string path { get; set; }
        public static void writeMessage(string logMessage)
        {
            if (string.IsNullOrEmpty(path))
            {

                //path =  +  + ;DateTime.Now.ToString("yyyyMMddhhmmss_")+
                //   string logpath = string.Format(ConfigurationManager.AppSettings["LogFilesPath1"].ToString());
                // path = Path.Combine(logpath, "logDetailsBankDashboard.txt");
                
                path = ConfigurationManager.AppSettings["LogFilesPath1"].ToString().Trim().Replace(" ", "");
               
            }
            //path = Path.Combine(@"C:\Users\Madhur_Tyagi_RPA\Desktop\logfile", "abc.txt");
           // File.AppendAllText(path, "");
           if(!(string.IsNullOrEmpty(path)))
            File.AppendAllText(path.Trim(), "[" + DateTime.Now.ToString() + "] " + logMessage + System.Environment.NewLine, System.Text.Encoding.UTF8);

            
        }
    }
}