using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace codTaxWeb.Controllers.DriverManager
{
    public class DriverManagerController : Controller
    {
        // 로그인 링크
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Check_Login()
        {
            return View();
        }
        // 에러페이지
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Error()
        {
            return View();
        }
        // 회원가입 링크 : Post 방식        
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Nice_Regist(Models.RegistForm_Client registData)
        {            
            return View(registData);
        }

        // 나이스 인증서 등록 완료 후 들어오는 페이지
        [AcceptVerbs(HttpVerbs.Post)]
        public void certification_finish(Models.Nice_Certification_Finish_Data niceData )
        {            
            //나이스 인증서 등록이 완료 된 후 cod-api 에 저장
            string retString = "";
            try
            {
                niceData.url = Common.ConnectionString.ExtAPI_URL + "/driver/tax/cert";

                retString = WebProtocols.EXtAPI.Nice_Certification_Finish(niceData);
                retString = retString.Replace("\n", "");
                retString = retString.Replace("\\\"", "\"");
                retString = retString.Replace("\\\"{", "{");
                retString = retString.Replace("}\\\"", "}");

                Models.Basic_Json_Format CodAPI_RtnMsg = Common.Lib.cJSON._DeSerialize<Models.Basic_Json_Format>(retString);
                if (CodAPI_RtnMsg.Status_Code != "200")//나이스인증서등록완료, api서버저장실패
                { 
                    Common.Lib.ErrorLog.WriteErrorLogOnFile("나이스인증서등록완료, api서버저장실패 : " +
                        CodAPI_RtnMsg.Status_Code + ", " +
                        niceData.frnNo + ", " +
                        niceData.userId + "," +
                        niceData.certPw + "," +
                        niceData.expireDt);
                }
            }
            catch (Exception exp)
            {
                Common.Lib.ErrorLog.WriteErrorLogOnFile(exp.Message.ToString() + " : "+
                        niceData.frnNo + ", " +
                        niceData.userId + "," +
                        niceData.certPw + "," +
                        niceData.expireDt);
            }
        }

        // 나이스 인증서 유효성 정상 - 안내 페이지
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult expireDt_success()
        {
            return View();
        }

    }
}