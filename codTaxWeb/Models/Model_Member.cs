using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    #region 기본 Json 포맷클래스
    public class Basic_Json_Format
    {
        public string Status_Code { get; set; }
        public string Status_Msg { get; set; }
        public List<ResultData> Result_Data { get; set; }
    }

    public class ResultData
    {
    }
    #endregion

    
    #region 차주앱 -> TaxWeb 세금계산서 발행 요청
    public class memberCheck
    {
        public string url { get; set; }

        public string phone_no { get; set; }
        public string birth_day { get; set; }        
    }

    public class NiceCertificationSubmit_Result
    {
        public string Status_Code { get; set; }
        public string Status_Msg { get; set; }
        public List<NiceCertificationSubmit_ResultData> Result_Data { get; set; }
    }

    public class NiceCertificationSubmit_ResultData
    {
        public string PASSWD { get; set; }
        public string REG_YN { get; set; }
        public string LINKCD { get; set; }
        public string USERID { get; set; }
        public string FRNNO { get; set; }
        public string CERTPW { get; set; }
        public string DTIXML { get; set; }
        public string SENDMAILYN { get; set; }
        public string SENDSMSYN { get; set; }
        public string SENDSMSMSG { get; set; }
    }
    #endregion

    #region taxWeb -> codAPI xml 과 TB_IDX 받은 값 파싱 -- 세금계산서 발급 시도 - 리턴값    
    public class XmlandTBidx_Result
    {
        public string Status_Code { get; set; }
        public string Status_Msg { get; set; }
        public List<XmlandTBidx_Result_ResultData> Result_Data { get; set; }
    }

    public class XmlandTBidx_Result_ResultData
    {
        public string SENDMAILYN { get; set; }
        public string DTIXML { get; set; }
        public string SENDSMSMSG { get; set; }
        public int TB_IDX { get; set; }
        public string SENDSMSYN { get; set; }
    }
    #endregion


    #region taxWeb -> CodAPI 세금계산서 발행 시도 통보
    public class NiceCertification_Try_Send
    {
        public string url { get; set; }

        public string phone_no { get; set; }
        public string birth_day { get; set; }        
        public string calc_dt { get; set; }
    }
    #endregion

    #region taxWeb -> CodAPI 세금계산서 발행 완료 통보
    public class NiceCertification_Success_Send
    {
        public string url { get; set; }

        public string phone_no { get; set; }
        public string birth_day { get; set; }
        public int tb_idx { get; set; }
        public string billno { get; set; }
    }
    #endregion

    #region taxWeb -> CodAPI 세금계산서 발행 에러 통보
    public class NiceCertification_Error_Send
    {
        public string url { get; set; }

        public string phone_no { get; set; }
        public string birth_day { get; set; }
        public int tb_idx { get; set; }
        public string nice_code { get; set; }
    }
    #endregion

    #region TaxWeb -> NiceData 회원가입 폼
    public class RegistForm_Client
    {
        public string url { get; set; }

        public string DRIVER_IDX { get; set; }
        public string LINKID { get; set; }
        public string SESSION_ID { get; set; } //혹시 모를 세션 체크를 위해         

        public string FRNNO { get; set; }
        public string USERID { get; set; }
        public string PASSWD { get; set; }
        
        public string BIZNO { get; set; }
        public string CUSTNAME { get; set; }
        public string OWNERNAME { get; set; }
        public string BIZCOND { get; set; }
        public string BIZITEM { get; set; }

        public string RSBMNAME { get; set; }
        public string EMAIL { get; set; }
        public string TELNO { get; set; }
        public string HPNO { get; set; }

        public string ZIPCODE { get; set; }
        public string ADDR1 { get; set; }
        public string ADDR2 { get; set; }
    }
    #endregion

    #region NiceData -> TaxWeb 인증서 등록(수정) 완료 - Post 방식의 값이 전달 됨
    public class Nice_Certification_Finish_Data
    {
        public string url { get; set; }

        public string frnNo { get; set; }//retUrl 호출시 파라미터.
        public string userId { get; set; }//retUrl 호출시 파라미터.
        public string certPw { get; set; }//retUrl 호출시 파라미터. 비밀번호를 암호화해서 리턴.
        public string expireDt { get; set; }//retUrl 호출시 파라미터. YYYYMMDD 형식
    }
    #endregion

    #region Nice 인증서 유효성 체크
    public class Nice_SelectExpireDt_Client
    {
        public string LINKCD { get; set; }
        public string FRNNO { get; set; }
        public string USERID { get; set; }
        public string PASSWD { get; set; }
        public string CERTPW { get; set; }

    }

    #endregion
}