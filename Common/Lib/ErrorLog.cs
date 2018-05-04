using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;

namespace Common.Lib
{
    public class ErrorLog
    {
        public static void WriteErrorLogOnFile(string message)
        {
            DirectoryInfo rootPath = new DirectoryInfo(HttpContext.Current.Server.MapPath("/"));// 웹서버 기준으로 볼때
            //DirectoryInfo rootPath = new DirectoryInfo(@"\\c:"); //윈폼 기준
            string aidPath = Path.Combine(rootPath.Parent.FullName, "aid");

            string ErrorLogPath = Path.Combine(aidPath, "ErrorLog\\");

            try
            {
                DirectoryInfo directory = new DirectoryInfo(ErrorLogPath);
                if (!directory.Exists)
                    directory.Create();

                FileInfo file = new FileInfo(ErrorLogPath + DateTime.Now.ToString("yyyyMMdd") + ".txt");

                StreamWriter writer;
                if (file.Exists)
                    writer = file.AppendText();
                else
                    writer = file.CreateText();

                writer.WriteLine(message);
                writer.Close();
            }
            catch
            {
            }
        }

    }
}
