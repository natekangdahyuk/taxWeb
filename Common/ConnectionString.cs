using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class ConnectionString
    {
        public static string gameServerURL { get; set; }
        public static string fb_callBackUrl { get; set; }
        public static string Login_HomeUrl { get; set; }
        public static string fb_appID { get; set; }
        public static string fb_AppSecret { get; set; }
        public static string google_ClientId { get; set; }
        public static string google_ClientSecret { get; set; }
        public static string google_RedirectUri { get; set; }


        public static string Driver_Regist { get; set; }

        //COD API 정보.....
        public static string ExtAPI_URL { get; set; }

        public static string Sftp_URL { get; set; }
        public static string Sftp_UserName { get; set; }
        public static string Sftp_PWD { get; set; }
        //COD API 정보 종료

        //나이스 API 정보
        public static string Nice_Soap_URL { get; set; }
        public static string Nice_LinkCD { get; set; }
        public static string Nice_Cetification_URL { get; set; }
        public static string Nice_RedirectUri_URL { get; set; }
        
        //나이스 API 정보 종료

        static ConnectionString()
        {
            switch (Global.Domain)
            {
                case Global.DomainEnum.Local:
                    fb_appID = "272363866519314";
                    fb_AppSecret = "210acfd544a4e507ac26225bd7feb0c4";
                    fb_callBackUrl = "http://localhost:3004/pc/login/fb_login_ok.aspx";
                    google_ClientId = "196520409851-lfh4ksvvb7kngifccicit3v3nqq0fmdn.apps.googleusercontent.com";
                    google_ClientSecret = "9HXRsaHnbBovSzDFc5bHwCW9";
                    google_RedirectUri = "http://localhost:3004/pc/login/google_login_ok.aspx";
                    ExtAPI_URL = "http://112.170.43.206:8010";

                    Nice_LinkCD = "COD";
                    Nice_Soap_URL = "https://t-ws.nicedata.co.kr/services/DTIService?wsdl";
                    Nice_Cetification_URL = "http://t-renewal.nicedata.co.kr/ti/TI_80101.do";
                    Nice_RedirectUri_URL = "http://localhost:60201/DriverManager/certification_finish";

                    Sftp_URL = "112.170.43.206";
                    Sftp_UserName = "codlabs";
                    Sftp_PWD = "cod_dev!2017&*()";
                    break;
                case Global.DomainEnum.Dev:
                    fb_appID = "2201956466696946";
                    fb_AppSecret = "7b17262af8af0e669468a6fe7ec3b4d9";
                    fb_callBackUrl = "http://dev.game.macamoa.co.kr:99/pc/login/fb_login_ok.aspx";
                    google_ClientId = "196520409851-lfh4ksvvb7kngifccicit3v3nqq0fmdn.apps.googleusercontent.com";
                    google_ClientSecret = "9HXRsaHnbBovSzDFc5bHwCW9";
                    google_RedirectUri = "http://dev.game.macamoa.co.kr:99/pc/login/google_login_ok.aspx";
                    ExtAPI_URL = "http://112.170.43.206:8010";

                    Nice_LinkCD = "COD";
                    Nice_Soap_URL = "https://t-ws.nicedata.co.kr/services/DTIService?wsdl";
                    Nice_Cetification_URL = "http://t-renewal.nicedata.co.kr/ti/TI_80101.do";
                    Nice_RedirectUri_URL = "http://dev.tax.codlabs.com:8020/DriverManager/certification_finish";

                    Sftp_URL = "112.170.43.206";
                    Sftp_UserName = "codlabs";
                    Sftp_PWD = "cod_dev!2017&*()";
                    break;
                default: //real
                    fb_appID = "2201956466696946";
                    fb_AppSecret = "7b17262af8af0e669468a6fe7ec3b4d9";
                    fb_callBackUrl = "http://game.macamoa.co.kr/pc/login/fb_login_ok.aspx";
                    google_ClientId = "196520409851-lfh4ksvvb7kngifccicit3v3nqq0fmdn.apps.googleusercontent.com";
                    google_ClientSecret = "9HXRsaHnbBovSzDFc5bHwCW9";
                    google_RedirectUri = "http://game.macamoa.co.kr:99/pc/login/google_login_ok.aspx";
                    ExtAPI_URL = "http://xxx.xxx.xxx.xxx:8010";

                    Nice_LinkCD = "COD";
                    Nice_Soap_URL = "https://xxx.xxx.xxx.xxx/services/DTIService?wsdl";
                    Nice_Cetification_URL = "http://t-renewal.nicedata.co.kr/ti/TI_80101.do";
                    Nice_RedirectUri_URL = "http://xxx.xxx.xxx.xxx/DriverManager/certification_finish";

                    Sftp_URL = "xxx.xxx.xxx.xxx";
                    Sftp_UserName = "xxxxx";
                    Sftp_PWD = "xxxxxxx";
                    break;
            }
        }
    }
}
