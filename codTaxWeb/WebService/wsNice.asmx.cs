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
using System.Web.Script.Serialization;
using System.Data;

namespace codTaxWeb.WebService
{
    /// <summary>
    /// wsNice의 요약 설명입니다.........
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class wsNice : System.Web.Services.WebService
    {
        
        #region taxWeb -> nice 세금계산서 등록을 위한 최초 1회 회원가입
        [WebMethod]
        public string wsfnNiceRegist(Models.RegistForm_Client p_obj_NiceUserRegist)
        {
            string retString = "";
            string retVal, errMsg, frnNo, userid, passwd;
            try
            {  
                //나이스 회원 가입 membjoin 호출
                NiceSoapService.DTIServiceService niceMembJoinRQ = new NiceSoapService.DTIServiceService();
                niceMembJoinRQ.membJoin(
                    Common.ConnectionString.Nice_LinkCD,
                    p_obj_NiceUserRegist.LINKID,
                    p_obj_NiceUserRegist.BIZNO,
                    p_obj_NiceUserRegist.CUSTNAME,
                    p_obj_NiceUserRegist.OWNERNAME,
                    p_obj_NiceUserRegist.BIZCOND,
                    p_obj_NiceUserRegist.BIZITEM,
                    p_obj_NiceUserRegist.RSBMNAME,
                    p_obj_NiceUserRegist.EMAIL,
                    p_obj_NiceUserRegist.TELNO,
                    p_obj_NiceUserRegist.HPNO,
                    p_obj_NiceUserRegist.ZIPCODE,
                    p_obj_NiceUserRegist.ADDR1,
                    p_obj_NiceUserRegist.ADDR2,
                    out retVal, out errMsg, out frnNo, out userid, out passwd
                    );
                if (retVal == "0")//성공이면 cod-API 에 데이터를 저장                
                {
                    //cod - API 에 데이타를 저장
                    //p_obj_NiceUserRegist.FRNNO = "0001480192";
                    //p_obj_NiceUserRegist.USERID = "COD_64";
                    //p_obj_NiceUserRegist.PASSWD = "FC3CB6CAF909501EFD165DA02BF1CCD1D2BA484C91551888380DA39BCBAC5B29";

                    p_obj_NiceUserRegist.FRNNO = frnNo;
                    p_obj_NiceUserRegist.USERID = userid;
                    p_obj_NiceUserRegist.PASSWD = passwd;

                    p_obj_NiceUserRegist.DRIVER_IDX = p_obj_NiceUserRegist.LINKID;//같이 쓴다.
                    p_obj_NiceUserRegist.url = Common.ConnectionString.ExtAPI_URL + "/driver/tax/member";

                    //cod  API 에 나이스 회원가입 정보 저장 후 리턴
                    retString = WebProtocols.EXtAPI.niceRegistSave(p_obj_NiceUserRegist);

                    retString = retString.Replace("\n", "");
                    retString = retString.Replace("\\\"", "\"");
                    retString = retString.Replace("\\\"{", "{");
                    retString = retString.Replace("}\\\"", "}");

                    Models.Basic_Json_Format CodAPI_RtnMsg = Common.Lib.cJSON._DeSerialize<Models.Basic_Json_Format>(retString);                    
                    
                    if (CodAPI_RtnMsg.Status_Code != "200")//cod-api 저장 하면서 에러 발생
                    {
                        retString = "{\"Status_Code\": \"" + CodAPI_RtnMsg.Status_Code + "\", \"Status_Msg\":\"" + CodAPI_RtnMsg.Status_Msg + "\"}";
                    }
                    else //cod-api 까지 정상 저장
                    {
                        DataTable tempTable = new DataTable("Result_Data");
                        DataColumn column;
                        DataRow row;

                        column = new DataColumn();
                        column.DataType = System.Type.GetType("System.String");
                        column.ColumnName = "FRNNO";
                        column.ReadOnly = false;
                        column.Unique = false;
                        tempTable.Columns.Add(column);

                        column = new DataColumn();
                        column.DataType = System.Type.GetType("System.String");
                        column.ColumnName = "USERID";
                        column.ReadOnly = false;
                        column.Unique = false;
                        tempTable.Columns.Add(column);

                        column = new DataColumn();
                        column.DataType = System.Type.GetType("System.String");
                        column.ColumnName = "PASSWD";
                        column.ReadOnly = false;
                        column.Unique = false;
                        tempTable.Columns.Add(column);

                        column = new DataColumn();
                        column.DataType = System.Type.GetType("System.String");
                        column.ColumnName = "LINKCD";
                        column.ReadOnly = false;
                        column.Unique = false;
                        tempTable.Columns.Add(column);

                        column = new DataColumn();
                        column.DataType = System.Type.GetType("System.String");
                        column.ColumnName = "CERTIFICATIONURL";
                        column.ReadOnly = false;
                        column.Unique = false;
                        tempTable.Columns.Add(column);

                        column = new DataColumn();
                        column.DataType = System.Type.GetType("System.String");
                        column.ColumnName = "RETURL";
                        column.ReadOnly = false;
                        column.Unique = false;
                        tempTable.Columns.Add(column);


                        row = tempTable.NewRow();
                        row["LINKCD"] = Common.ConnectionString.Nice_LinkCD;
                        row["CERTIFICATIONURL"] = Common.ConnectionString.Nice_Cetification_URL;
                        row["RETURL"] = Common.ConnectionString.Nice_RedirectUri_URL;                        
                        row["FRNNO"] = frnNo;
                        row["USERID"] = userid;
                        row["PASSWD"] = passwd;

                        tempTable.Rows.Add(row);
                        retString = Common.Lib.cJSON.FromCODAPI(tempTable, CodAPI_RtnMsg.Status_Code, CodAPI_RtnMsg.Status_Msg); //table to json
                    }
                }//나이스 회원가입이 정상적으로 되고 cod-API 저장 까지 완료
                else //나이스 회원가입에서 에러가 발생 했다면.
                {
                    //a 리소스(경우에따라 b,c,d)에서 에러를 찾아와라
                    string errKey = "a" + retVal;
                    errMsg = HttpContext.GetGlobalResourceObject("a", errKey).ToString();
                    retString = "{\"Status_Code\": \"" + retVal + "\", \"Status_Msg\":\"" + errMsg + "\"}";
                }                
            }
            catch (Exception exp)
            {
                retString = "{\"Status_Code\": \"1001\", \"Status_Msg\":\"" + exp.Message.ToString() + "\"}";
            }
            return retString;
        }
        #endregion

        #region taxWeb -> nice 인증서 정보가 유효한지 체크
        [WebMethod]
        public string wsfnNiceSelectExpireDt(Models.Nice_SelectExpireDt_Client p_objSelectExpireDt)
        {
            string retString = "";
            string retVal, errMsg, regYn, expireDt;
            try
            {
                //나이스 인증서 유효성 체크 및 만료일자 조회
                NiceSoapService.DTIServiceService niceMembJoinRQ = new NiceSoapService.DTIServiceService();
                niceMembJoinRQ.selectExpireDt(
                    Common.ConnectionString.Nice_LinkCD,                    
                    p_objSelectExpireDt.FRNNO,
                    p_objSelectExpireDt.USERID,
                    p_objSelectExpireDt.PASSWD,
                    out retVal, out errMsg, out regYn, out expireDt
                    );
                //-	고객의 인증서가 등록되어있는 경우 regYn: Y 와 expireDt : YYYYMMDD 형식의 날짜를 리턴(ex:20171128).
                //고객의 인증서가 등록되어있지 않은 경우 regYn: N 리턴
                if (regYn == "Y")//정상
                {
                    DateTime expireDT = DateTime.ParseExact(expireDt + " 00:00", "yyyyMMdd H:mm", null);
                    if (regYn == "Y" && expireDT >= DateTime.Now)//인증서가 등록되어 있고 //유효기한이 현재 일보다 크다면 정상
                    {
                        retString = "{\"Status_Code\": \"200\", \"Status_Msg\":\"Success\"}";                        
                    }
                    else //인증서가 등록이 되어 있지 않거나 유효기간이 지났으면. 등록페이지로 이동시켜라. 이때 나이스와 약속한 정보가 필요하다.
                    {
                        retString = "{\"Status_Code\": \"201\", \"Status_Msg\":\"not Regist\", \"Result_Data\":[" +
                         "{\"LINKCD\": \"" + Common.ConnectionString.Nice_LinkCD + "\"," +
                         "\"CERTIFICATIONURL\": \"" + Common.ConnectionString.Nice_Cetification_URL + "\"," +
                         "\"RETURL\": \"" + Common.ConnectionString.Nice_RedirectUri_URL + "\"," +
                         "\"FRNNO\": \"" + p_objSelectExpireDt.FRNNO + "\"," +
                         "\"USERID\": \"" + p_objSelectExpireDt.USERID + "\"," +
                         "\"PASSWD\": \"" + p_objSelectExpireDt.PASSWD + "\"}" +
                         "]}";                        
                    }
                }
                else//regYn==N
                {
                    retString = "{\"Status_Code\": \"201\", \"Status_Msg\":\"not Regist\", \"Result_Data\":[" +
                         "{\"LINKCD\": \"" + Common.ConnectionString.Nice_LinkCD + "\"," +
                         "\"CERTIFICATIONURL\": \"" + Common.ConnectionString.Nice_Cetification_URL + "\"," +
                         "\"RETURL\": \"" + Common.ConnectionString.Nice_RedirectUri_URL + "\"," +
                         "\"FRNNO\": \"" + p_objSelectExpireDt.FRNNO + "\"," +
                         "\"USERID\": \"" + p_objSelectExpireDt.USERID + "\"," +
                         "\"PASSWD\": \"" + p_objSelectExpireDt.PASSWD + "\"}" +
                         "]}";                    
                }
            }//try
            catch (Exception exp)
            {
                retString = "{\"Status_Code\": \"1001\", \"Status_Msg\":\"" + exp.Message.ToString() + "\"}";                
            }
            return retString;
        }
        #endregion

        //wsfnSelectExpireDt
    }
}
