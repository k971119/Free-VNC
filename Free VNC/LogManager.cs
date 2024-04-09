using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Free_VNC
{
    class LogManager
    {
        private string Log = string.Empty;
        readonly string _sFilePath;

        public LogManager(String path)
        {
            _sFilePath = path;
        }

        public LogManager()
        {
            _sFilePath = Assembly.GetExecutingAssembly().Location + @"\Log\";
        }

        public void ErrorLog(string position, string errMessage)
        {
            try
            {
                DateTime now = DateTime.Now;
                String date = now.Year + "-" + now.Month + "-" + now.Day;
                FileStream fs = new FileStream(_sFilePath + "LogFile" + date + ".txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                StreamWriter sw = new StreamWriter(fs, Encoding.Default);

                fs.Seek(0, SeekOrigin.End);
                sw.WriteLine(string.Format("-------------------------------{0}-------------------------------", DateTime.Now));
                sw.WriteLine(string.Format("{0} : {1}", position, errMessage));
                sw.WriteLine("-------------------------------------------------------------------------------------");
                sw.Close();
                fs.Close();
            }
            catch (DirectoryNotFoundException de)
            {
                DirectoryInfo di = new DirectoryInfo(_sFilePath);
                if (di.Exists == false)
                {
                    di.Create();
                    ErrorLog(errMessage);
                }
            }
        }

        public void ErrorLog(Exception ex)
        {
            try
            {
                DateTime now = DateTime.Now;
                String date = now.Year + "-" + now.Month + "-" + now.Day;
                FileStream fs = new FileStream(_sFilePath + "LogFile" + date + ".txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                StreamWriter sw = new StreamWriter(fs, Encoding.Default);

                fs.Seek(0, SeekOrigin.End);
                sw.WriteLine(string.Format("-------------------------------{0}-------------------------------", DateTime.Now));
                sw.WriteLine(string.Format("{0} : {1}", ex.Message, ex.StackTrace));
                sw.WriteLine("-------------------------------------------------------------------------------------");
                sw.Close();
                fs.Close();
            }
            catch (DirectoryNotFoundException de)
            {
                DirectoryInfo di = new DirectoryInfo(_sFilePath);
                if (di.Exists == false)
                {
                    di.Create();
                    ErrorLog(ex);
                }
            }
        }

        public void log(String msg)
        {
            try
            {
                DateTime now = DateTime.Now;
                String date = now.Year + "-" + now.Month + "-" + now.Day;
                FileStream fs = new FileStream(_sFilePath + "LogFile" + date + ".txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                StreamWriter sw = new StreamWriter(fs, Encoding.Default);

                fs.Seek(0, SeekOrigin.End);
                sw.WriteLine(string.Format("{0} : {1}", DateTime.Now, msg));
                sw.Close();
                fs.Close();
            }
            catch (DirectoryNotFoundException de)
            {
                DirectoryInfo di = new DirectoryInfo(_sFilePath);
                if (di.Exists == false)
                {
                    di.Create();
                    log(msg);
                }
            }
        }

        public void ErrorLog(string errMessage)
        {
            try
            {
                DateTime now = DateTime.Now;
                String date = now.Year + "-" + now.Month + "-" + now.Day;
                FileStream fs = new FileStream(_sFilePath + "LogFile" + date + ".txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                StreamWriter sw = new StreamWriter(fs, Encoding.Default);

                fs.Seek(0, SeekOrigin.End);
                sw.WriteLine(string.Format("-------------------------------{0}-------------------------------", DateTime.Now));
                sw.WriteLine(string.Format("{0}", errMessage));
                sw.WriteLine("-------------------------------------------------------------------------------------");
                sw.Close();
                fs.Close();
            }
            catch (DirectoryNotFoundException de)
            {
                DirectoryInfo di = new DirectoryInfo(_sFilePath);
                if (di.Exists == false)
                {
                    di.Create();
                    ErrorLog(errMessage);
                }
            }
        }

        public void TimeRecv(string sDateTime)
        {
            DateTime now = DateTime.Now;
            String date = now.Year + "-" + now.Month + "-" + now.Day;
            FileStream fs = new FileStream(_sFilePath + "TimerCheck.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            StreamWriter sw = new StreamWriter(fs, Encoding.Default);

            fs.Seek(0, SeekOrigin.End);
            sw.WriteLine(sDateTime);

            sw.Close();
            fs.Close();
        }
    }
}
