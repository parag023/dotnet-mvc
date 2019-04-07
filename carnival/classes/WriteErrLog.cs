using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace web
{
    public class WriteErrLog
    {
        private string fileName;
        public WriteErrLog()
        {
            fileName = "AllExceptionsLog.txt";
        }
        public WriteErrLog(string fileName)
        {
            this.fileName = fileName;
        }
        public void WriteError_File(string errorText)
        {
            writeLogs(errorText);
        }
        public void WriteError_File(string Err, string PageName,string UserName)
        {
            try
            {
                string Temp_Text = UserName + " : Error : " + Err + " " + DateTime.Now + Environment.NewLine + "On Page : " + PageName + Environment.NewLine + Environment.NewLine;
                writeLogs(Temp_Text);
            }
            catch (Exception ex)
            { 
                
            }
        }
        protected void writeLogs(string logstring)
        {
            try
            {
                System.IO.Directory.CreateDirectory(@"D:\\ExceptionLog\\");
                File.AppendAllText("E:\\ExceptionLog\\" + fileName, logstring);
            }
            catch (Exception exc)
            { }
        }


    }
}
