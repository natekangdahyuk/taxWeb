using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Web.Script.Services;
using System.Collections.Specialized;

namespace codTaxWeb.WebService
{
    /// <summary>
    /// wsMember의 요약 설명입니다...........,,,,,,,,,,,,,,,,,,;;;;;;;;;;;;;;;;;;,,,,,,,,,,,,,,,,,,,,
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class wsMember : System.Web.Services.WebService
    {
        #region taxWeb -> codAPI 차주 로그인 (로그인).
        [WebMethod]
        public string wsfnDriverLogin(Models.memberCheck p_objUserLogin)
        {
            string retString = "";
            try
            {
                p_objUserLogin.url = Common.ConnectionString.ExtAPI_URL + "/driver/tax/login";

                retString = WebProtocols.EXtAPI.memberCheck(p_objUserLogin);
                retString = retString.Replace("\n", "");
                retString = retString.Replace("\\\"", "\"");
                retString = retString.Replace("\\\"{", "{");
                retString = retString.Replace("}\\\"", "}");
            }
            catch (Exception exp)
            {
                retString = "{\"Status_Code\": \"1001\", \"Status_Msg\":\" " + exp.Message.ToString() + "\"}";
            }
            return retString;
        }
        #endregion

        #region taxWeb -> codAPI 나이스 회원가입 되었는지 확인 하는 웹 서비스
        [WebMethod]
        public string wsfnNiceRegistCheck(Models.memberCheck p_objUserLogin)
        {
            string retString = "";
            try
            {
                p_objUserLogin.url = Common.ConnectionString.ExtAPI_URL + "/driver/tax/member_check";

                retString = WebProtocols.EXtAPI.memberCheck(p_objUserLogin);
                retString = retString.Replace("\n", "");
                retString = retString.Replace("\\\"", "\"");
                retString = retString.Replace("\\\"{", "{");
                retString = retString.Replace("}\\\"", "}");
            }
            catch (Exception exp)
            {
                retString = "{\"Status_Code\": \"1001\", \"Status_Msg\":\" " + exp.Message.ToString() + "\"}";
            }
            return retString;
        }
        #endregion

    }
}
