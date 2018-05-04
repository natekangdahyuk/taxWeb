using System;
using System.Web;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Reflection;
using System.Resources;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace Common.Page
{
    public class Base : System.Web.UI.Page
    {
        public static string CommonPwd = "Q%^&F$VB=yyTYadsN";


        #region 로그인여부 체크 (IsLogin)
        /// <summary>
        /// 로그인이 되어 있는지를 체크한다.......
        /// </summary>
        /// <returns>로그인이 되어 있을시 true, 안되어 있을경우 false</returns>
        public static bool IsLogin()
        {
            bool isok = false;
            //아래는 웹서버가 여러대일 경우 쿠키를 활용할때
            string debugStr = fnGetNomalCookie("MacaSign");

            if (!string.IsNullOrEmpty(fnGetNomalCookie("MacaSign")) && fnGetNomalCookie("MacaSign").Length > 0)
            {
                isok = true;
            }
            return isok;
        }
        #endregion


        #region 클라이언트 유니크 키를 쿠키로 생성한다.
        public static void makeClientUniqueKey(string _userID, string platform, string picture)
        {
            string strUniqueKey = _userID + platform;
            Common.Page.Base.fnSetCookie("MacaSign", strUniqueKey);
            Common.Page.Base.fnSetCookie("loginPlatform", platform);
            Common.Page.Base.fnSetCookie("picture", picture);
        }
        #endregion

        #region 쿠키값 가지고 오기 (fnGetCookie)
        /// <summary>
        /// 암호화되어 저장되어 있는 쿠키값을 복호화해서 가지고 온다.
        /// </summary>
        /// <param name="what">가지고 올 쿠키 이름</param>
        /// <returns>복호화된 쿠키값</returns>
        public static string fnGetCookie(string what)
        {
            if (System.Web.HttpContext.Current.Request.Cookies[what] != null)
            {
                if (what == "ASP.NET_SessionId")
                {
                    return System.Web.HttpContext.Current.Request.Cookies[what].Value;
                }
                else
                {


                    string Cookieval;
                    Cookieval = HttpContext.Current.Server.UrlDecode((DecryptString(System.Web.HttpContext.Current.Request.Cookies[what].Value)));

                    return Cookieval;
                }
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 암호화 쿠키설정 (fnSetCookie)
        /// <summary>
        /// 쿠키를 만들고 해당 쿠키에 값을 암호화해서 저장한다.
        /// </summary>
        /// <param name="cookieNm">쿠키명</param>
        /// <param name="cookieVal">쿠키값</param>
        public static void fnSetCookie(string str, string data)
        {
            data = HttpContext.Current.Server.UrlEncode(data);

            System.Web.HttpCookie MyCookie = new System.Web.HttpCookie(str);

            MyCookie.Value = EncryptString(data);
            MyCookie.Path = "/";

            if (HttpContext.Current.Request.ServerVariables["SERVER_NAME"].ToString() != "localhost")
            {
                string domain = Common.Global.CookieDomain;
                MyCookie.Domain = domain;
            }

            System.Web.HttpContext.Current.Response.Cookies.Add(MyCookie);
        }
        #endregion

        #region 비 암호화 쿠키 설정.(fnSetNomalCookie)
        public static void fnSetNomalCookie(string Name, string Val)
        {
            Val = HttpContext.Current.Server.UrlEncode(Val);
            System.Web.HttpCookie MyCookie = new System.Web.HttpCookie(Name);
            MyCookie.Value = Val;

            if (HttpContext.Current.Request.ServerVariables["SERVER_NAME"].ToString() != "localhost")
            {
                string domain = Common.Global.CookieDomain;
                MyCookie.Domain = domain;
            }

            System.Web.HttpContext.Current.Response.Cookies.Add(MyCookie);
        }
        #endregion

        #region 암호화 하지 않은 일반 쿠키를 가지고 온다.(fnGetNomalCookie)
        public static string fnGetNomalCookie(string CookieName)
        {
            if (System.Web.HttpContext.Current.Request.Cookies[CookieName] != null)
            {
                if (CookieName == "ASP.NET_SessionId")
                {
                    return System.Web.HttpContext.Current.Request.Cookies[CookieName].Value;
                }
                else
                {
                    string Cookieval;
                    Cookieval = HttpContext.Current.Server.UrlDecode(System.Web.HttpContext.Current.Request.Cookies[CookieName].Value.ToString());

                    return Cookieval;
                }
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 쿠키지우기
        public static void DeleteCookie(string str)
        {
            System.Web.HttpCookie MyCookie = new System.Web.HttpCookie(str);

            MyCookie.Expires = DateTime.MinValue;
            MyCookie.Path = "/";

            if (HttpContext.Current.Request.ServerVariables["SERVER_NAME"].ToString() != "localhost")
            {
                string domain = Common.Global.CookieDomain;
                MyCookie.Domain = domain;
            }

            System.Web.HttpContext.Current.Response.Cookies.Add(MyCookie);
        }
        #endregion

        #region 암호화 (EncryptString)
        /// <summary>
        /// 입력받은 문자를 암호화한다.
        /// </summary>
        /// <param name="InputText">암호화 할 문자.</param>
        /// <param name="Password">대칭키로 사용할 암호문자</param>
        /// <returns>암호화된 문자</returns>
        public static string EncryptString(string InputText)
        {

            string Password = CommonPwd;
            // Rihndael class를 선언하고, 초기화
            RijndaelManaged RijndaelCipher = new RijndaelManaged();

            // 입력받은 문자열을 바이트 배열로 변환
            byte[] PlainText = System.Text.Encoding.Unicode.GetBytes(InputText);

            // 딕셔너리 공격을 대비해서 키를 더 풀기 어렵게 만들기 위해서 
            // Salt를 사용한다.
            byte[] Salt = Encoding.ASCII.GetBytes(Password.Length.ToString());

            // PasswordDeriveBytes 클래스를 사용해서 SecretKey를 얻는다.
            PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Password, Salt);

            // Create a encryptor from the existing SecretKey bytes.
            // encryptor 객체를 SecretKey로부터 만든다.
            // Secret Key에는 32바이트
            // (Rijndael의 디폴트인 256bit가 바로 32바이트입니다)를 사용하고, 
            // Initialization Vector로 16바이트
            // (역시 디폴트인 128비트가 바로 16바이트입니다)를 사용한다.
            ICryptoTransform Encryptor = RijndaelCipher.CreateEncryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));

            // 메모리스트림 객체를 선언,초기화 
            MemoryStream memoryStream = new MemoryStream();

            // CryptoStream객체를 암호화된 데이터를 쓰기 위한 용도로 선언
            CryptoStream cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);

            // 암호화 프로세스가 진행된다.
            cryptoStream.Write(PlainText, 0, PlainText.Length);

            // 암호화 종료
            cryptoStream.FlushFinalBlock();

            // 암호화된 데이터를 바이트 배열로 담는다.
            byte[] CipherBytes = memoryStream.ToArray();

            // 스트림 해제
            memoryStream.Close();
            cryptoStream.Close();

            // 암호화된 데이터를 Base64 인코딩된 문자열로 변환한다.
            string EncryptedData = Convert.ToBase64String(CipherBytes);

            // 최종 결과를 리턴
            return EncryptedData;
        }
        #endregion

        #region 복호화 (DecryptString)
        /// <summary>
        /// 암호화된 문자를 복호화 한다.
        /// </summary>
        /// <param name="InputText">복호화할 문자</param>
        /// <param name="Password">대칭키로 사용된 암호문자</param>
        /// <returns>복호화 된 문자</returns>
        public static string DecryptString(string InputText)
        {
            string DecryptedData = string.Empty;
            string Password = CommonPwd;
            if (InputText != "")
            {
                RijndaelManaged RijndaelCipher = new RijndaelManaged();


                byte[] EncryptedData = Convert.FromBase64String(InputText.Replace(" ", "+"));
                byte[] Salt = Encoding.ASCII.GetBytes(Password.Length.ToString());

                PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Password, Salt);

                // Decryptor 객체를 만든다.
                ICryptoTransform Decryptor = RijndaelCipher.CreateDecryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));

                MemoryStream memoryStream = new MemoryStream(EncryptedData);

                // 데이터 읽기(복호화이므로) 용도로 cryptoStream객체를 선언, 초기화
                CryptoStream cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);

                // 복호화된 데이터를 담을 바이트 배열을 선언한다.
                // 길이는 알 수 없지만, 일단 복호화되기 전의 데이터의 길이보다는
                // 길지 않을 것이기 때문에 그 길이로 선언한다.
                byte[] PlainText = new byte[EncryptedData.Length];

                // 복호화 시작
                int DecryptedCount = cryptoStream.Read(PlainText, 0, PlainText.Length);

                memoryStream.Close();
                cryptoStream.Close();

                // 복호화된 데이터를 문자열로 바꾼다.
                DecryptedData = Encoding.Unicode.GetString(PlainText, 0, DecryptedCount);
            }
            // 최종 결과 리턴
            return DecryptedData;
        }
        #endregion

        #region 이런방법 저런방법 - 성공 Global Resource를 반환한다.
        public static string GetResource(string classKey, string resourceKey)
        {
            return HttpContext.GetGlobalResourceObject(classKey, resourceKey).ToString();
        }
        #endregion

        /// <param name="message">전달 내용</param>
        /// <param name="page">경고를 띄울 페이지 개체</param>
        public void Alert(string message)
        {
            string script = "<script type='text/javascript'>";
            script += "alert('" + message.Replace("'", "") + "');";
            script += "</script>";

            this.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
        }

        /// <param name="message">전달 내용</param>
        /// <param name="page">경고를 띄울 페이지 개체</param>
        public void Alert(string classKey, string resourceKey)
        {
            string resource = GetResource(classKey, resourceKey).ToString();

            string script = "<script type='text/javascript'>";
            script += "alert('";
            script += resource.Replace("'", "");
            script += "');";
            script += "</script>";

            this.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
        }

        /// <param name="message">전달 내용</param>
        /// <param name="page">경고를 띄울 페이지 개체</param>
        public void Alert(string classKey, string resourceKey, params string[] list)
        {
            string resource = GetResource(classKey, resourceKey).ToString();
            resource = string.Format(resource, list);

            string script = "<script type='text/javascript'>";
            script += "alert('";
            script += resource.Replace("'", "");
            script += "');";
            script += "</script>";

            this.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
        }

        /// <param name="message">전달 내용</param>
        /// <param name="page">경고를 띄울 페이지 개체</param>
        public void Alert(string classKey, string resourceKey, string list)
        {
            string resource = string.Format(GetResource(classKey, resourceKey).ToString(), list);

            string script = "<script type='text/javascript'>";
            script += "alert('";
            script += resource.Replace("'", "");
            script += "');";
            script += "</script>";

            this.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
        }

        public void Alert_GoUrl(string message, string Url)
        {
            string script = "<script type='text/javascript'>";
            script += "alert('" + message.Replace("'", "") + "');";
            script += "location.href='" + Url + "';";
            script += "</script>";

            this.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
        }

        public void Alert_Refresh(string message)
        {
            message = message.Replace("'", "");

            string script = "<script type='text/javascript'>";
            script += "alert('" + message + "');";
            script += "location.href=document.location;";
            script += "</script>";

            this.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
        }

        public void Alert_selfClose(string message)
        {
            string script = "<script type='text/javascript'>";
            script += "alert('" + message.Replace("'", "") + "');";
            script += "self.close();";
            script += "</script>";

            this.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
        }

        public void Alert_Parent(string message)
        {
            string script = "<script type='text/javascript'>";
            script += "alert('" + message + "');";
            //   script += "location.href=document.location;";
            script += "opener.__doPostBack('LinkButton1','');";
            script += "self.close();";
            script += "</script>";

            this.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
        }

        public void Alert_Parent(string classKey, string resourceKey)
        {
            string resource = GetResource(classKey, resourceKey).ToString();

            string script = "<script type='text/javascript'>";
            script += "alert('";
            script += resource.Replace("'", "");
            script += "');";
            script += "opener.__doPostBack('LinkButton1','');";
            script += "self.close();";
            script += "</script>";

            this.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
        }

        public void Alert_HistoryBack(string classKey, string resourceKey)
        {
            string resource = GetResource(classKey, resourceKey).ToString();

            StringBuilder strScript = new StringBuilder();
            strScript.Append("<script type=\"text/javascript\"> \n");
            strScript.Append("        alert(\'" + resource.Replace("'", "\'") + "\'); \n");
            strScript.Append("        history.back(); \n");
            strScript.Append("</script>");
            Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", strScript.ToString());

        }

        public void selfClose()
        {
            string script = "<script type='text/javascript'>";
            script += "self.close();";
            script += "</script>";

            this.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
        }
    }
}
