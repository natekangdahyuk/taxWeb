using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System.Configuration;

using System.Globalization;
using System.Resources;

namespace Common
{
    public class Global : Common.Page.Base
    {
        // 사이트 운영 환경을 url로 판단한다.
        public enum DomainEnum { Local, Dev, Alpha, QA, Real };

        public static DomainEnum Domain
        {
            get
            {
                string domain = HttpContext.Current.Request.ServerVariables["SERVER_NAME"].ToLower();

                if (domain.StartsWith("local"))
                    return DomainEnum.Local;

                if (domain.StartsWith("dev."))
                    return DomainEnum.Dev;

                if (domain.StartsWith("alpha."))
                    return DomainEnum.Alpha;

                if (domain.StartsWith("qa."))
                    return DomainEnum.QA;

                return DomainEnum.Real;
            }
        }

        //프로젝트 이름 설정
        public static string ProjectName = "codOPTool";

        //쿠키 설정
        public static string CookieDomain = "optool.codlabs.com";

        

        //사이트 파일 업로드 폴더
        public static string FileUploadPath = "D:\\wwwRoot\\uploadFiles\\codOPTooL\\";// 진짜 파일이 업로드 되는곳
        public static string FileUploadPathTemp = "D:\\wwwRoot\\uploadFiles\\codOPTooL\\Temp\\";// 임시로 저장하고 수정하는 곳

        //사용자 정의 파일 업로드 폴더 만들기
        public static void CustumFileUploadFolder(string fileUploadPath)
        {
            DirectoryInfo directory = new DirectoryInfo(fileUploadPath);
            if (!directory.Exists)
                directory.Create();
        }
        
    }
}
