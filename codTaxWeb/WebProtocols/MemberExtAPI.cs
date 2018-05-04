using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace codTaxWeb.WebProtocols
{
    public partial class EXtAPI
    {
        //차주 로그인,나이스 회원가입 (같이 쓰자 ..귀찮아) 어차피 url 만 다름
        public static string memberCheck(Models.memberCheck formData)
        {
            string url = formData.url;
            var parms = new Dictionary<string, string>();

            parms.Add("phone_no", formData.phone_no.ToString());
            parms.Add("birth_day", formData.birth_day.ToString());

            string postData = DictToString(parms);
            return SendPostData(url, postData);
        }
        //세금계산서 발급 전 시도 정보를 API 에 저장하자.
        public static string bill_try_send(Models.NiceCertification_Try_Send formData)
        {
            string url = formData.url;
            var parms = new Dictionary<string, string>();

            parms.Add("phone_no", formData.phone_no.ToString());
            parms.Add("birth_day", formData.birth_day.ToString());
            parms.Add("calc_dt", formData.calc_dt.ToString());            

            string postData = DictToString(parms);
            return SendPostData(url, postData);
        }
        //세금계산서 발급 후 billno 를 API 에 저장하자.
        public static string bill_success_send(Models.NiceCertification_Success_Send formData)
        {
            string url = formData.url;
            var parms = new Dictionary<string, string>();

            parms.Add("phone_no", formData.phone_no.ToString());
            parms.Add("birth_day", formData.birth_day.ToString());
            parms.Add("tb_idx", formData.tb_idx.ToString());
            parms.Add("billno", formData.billno.ToString());

            string postData = DictToString(parms);
            return SendPostData(url, postData);
        }
        //세금계산서 발급 후 billno 를 API 에 저장하자.
        public static string bill_error_send(Models.NiceCertification_Error_Send formData)
        {
            string url = formData.url;
            var parms = new Dictionary<string, string>();

            parms.Add("phone_no", formData.phone_no.ToString());
            parms.Add("birth_day", formData.birth_day.ToString());
            parms.Add("tb_idx", formData.tb_idx.ToString());
            parms.Add("nice_code", formData.nice_code.ToString());

            string postData = DictToString(parms);
            return SendPostData(url, postData);
        }
        //나이스 회원가입 후 정보 저장
        public static string niceRegistSave(Models.RegistForm_Client formData)
        {
            string url = formData.url;
            var parms = new Dictionary<string, string>();

            parms.Add("driver_idx", formData.DRIVER_IDX.ToString());
            parms.Add("session_id", formData.SESSION_ID.ToString());

            parms.Add("frnno", formData.FRNNO.ToString());
            parms.Add("userid", formData.USERID.ToString());
            parms.Add("passwd", formData.PASSWD.ToString());

            parms.Add("bizno", formData.BIZNO.ToString());
            parms.Add("custname", formData.CUSTNAME.ToString());
            parms.Add("ownername", formData.OWNERNAME.ToString());
            parms.Add("bizcond", formData.BIZCOND.ToString());
            parms.Add("bizitem", formData.BIZITEM.ToString());
            parms.Add("rsbmname", formData.RSBMNAME.ToString());
            parms.Add("email", formData.EMAIL.ToString());
            parms.Add("telno", formData.TELNO.ToString());
            parms.Add("hpno", formData.HPNO.ToString());
            parms.Add("zipcode", formData.ZIPCODE.ToString());
            parms.Add("addr1", formData.ADDR1.ToString());
            parms.Add("addr2", formData.ADDR2.ToString());

            string postData = DictToString(parms);
            return SendPostData(url, postData);
        }

        //나이스 인증서 등록 후  정보 저장
        public static string Nice_Certification_Finish(Models.Nice_Certification_Finish_Data formData)
        {
            string url = formData.url;
            var parms = new Dictionary<string, string>();

            
            parms.Add("userid", formData.userId.ToString());
            parms.Add("expiredt", formData.expireDt.ToString());

            parms.Add("certpw", formData.certPw.ToString());
            string postData = DictToString(parms);
            return SendPostData(url, postData);
        }

    }
}