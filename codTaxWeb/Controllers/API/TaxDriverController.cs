using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Web.UI;
using Newtonsoft.Json;

namespace codTaxWeb.Controllers.API
{
    [RoutePrefix("TaxDriver")]
    public class TaxDriverController : ApiController
    {
        [Route("NiceCertificationSubmit")] //차주 앱으로부터 세금계산서 발행 호출을 받았다.
        [HttpPost]
        public object NiceCertificationSubmit(Models.NiceCertification_Try_Send niceCertificationSubmitClient)
        {            
            string retString = "";
            int tb_idx = 0;
            try
            {
                Models.memberCheck memCheck = new Models.memberCheck();
                memCheck.phone_no = niceCertificationSubmitClient.phone_no;
                memCheck.birth_day = niceCertificationSubmitClient.birth_day;

                memCheck.url = Common.ConnectionString.ExtAPI_URL + "/driver/tax/member_check";
                //Cod-API에 물어봐라.... 나이스데이터 가입 된 유저인가?
                retString = WebProtocols.EXtAPI.memberCheck(memCheck);
                retString = retString.Replace("\n", "");
                retString = retString.Replace("\\\"", "\"");
                retString = retString.Replace("\\\"{", "{");
                retString = retString.Replace("}\\\"", "}");
                
                Models.NiceCertificationSubmit_Result CodAPI_RtnMsg = Common.Lib.cJSON._DeSerialize<Models.NiceCertificationSubmit_Result>(retString);
                //1-1.  나이스 회원 가입 여부 체크 "통신 확인 완료 : 200"
                if (CodAPI_RtnMsg.Status_Code == "200") 
                {                    
                    //2-1.  //가입이 되어 있다면.
                    if (CodAPI_RtnMsg.Result_Data[0].REG_YN == "Y")
                    {  
                        string retVal, errMsg, regYn, expireDt, billNo;
                        NiceSoapService.DTIServiceService niceMembJoinRQ = new NiceSoapService.DTIServiceService();
                        niceMembJoinRQ.selectExpireDt(CodAPI_RtnMsg.Result_Data[0].LINKCD
                            , CodAPI_RtnMsg.Result_Data[0].FRNNO
                            , CodAPI_RtnMsg.Result_Data[0].USERID
                            , CodAPI_RtnMsg.Result_Data[0].PASSWD,
                            out retVal, out errMsg, out regYn, out expireDt);
                        //3-1.  공인인증서 유효한지 nice에서 체크했더니 "Y"
                        if (regYn == "Y") 
                        {
                            DateTime expireDT = DateTime.ParseExact(expireDt + " 00:00", "yyyyMMdd H:mm", null);
                            //4-1. 인증서가 등록되어 있고 //유효기한이 현재 일보다 크다면 정상
                            if (expireDT >= DateTime.Now)//
                            {
                                //5. codAPI 에 세금계산서 발급한다고 API 호출 (통보 성공)  

                                niceCertificationSubmitClient.url= Common.ConnectionString.ExtAPI_URL + "/driver/tax/xml";
                                retString = WebProtocols.EXtAPI.bill_try_send(niceCertificationSubmitClient);
                                retString = retString.Replace("\n", "");
                                retString = retString.Replace("\\\"", "\"");
                                retString = retString.Replace("\\\"{", "{");
                                retString = retString.Replace("}\\\"", "}");
                                
                                Models.XmlandTBidx_Result CodAPI_XmlMsg = Common.Lib.cJSON._DeSerialize<Models.XmlandTBidx_Result>(retString);
                                //5-1.codAPI 에 세금계산서 발급한다고 API 호출 (통보 성공)  
                                if (CodAPI_XmlMsg.Status_Code == "200")
                                {
                                    tb_idx = CodAPI_XmlMsg.Result_Data[0].TB_IDX;//나중에 발급 완료 후 재 사용
                                    string certPw = CodAPI_RtnMsg.Result_Data[0].CERTPW;
                                    //string DTIXml = "<?xml version='1.0' encoding='UTF-8'?><TaxInvoice xmlns='urn:kr:or:kec:standard:Tax:ReusableAggregateBusinessInformationEntitySchemaModule:1:0' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:schemaLocation='urn:kr:or:kec:standard:Tax:ReusableAggregateBusinessInformationEntitySchemaModule:1:0http://www.kec.or.kr/standard/Tax/TaxInvoiceSchemaModule_1.0.xsd'><TaxInvoiceDocument><TypeCode>0101</TypeCode><DescriptionText>123456789</DescriptionText><IssueDateTime>20180325</IssueDateTime><AmendmentStatusCode></AmendmentStatusCode><PurposeCode>02</PurposeCode><OriginalIssueID></OriginalIssueID></TaxInvoiceDocument><TaxInvoiceTradeSettlement><InvoicerParty><ID>1188600533</ID><TypeCode>업태</TypeCode><NameText>마카모아</NameText><ClassificationCode>종목</ClassificationCode><SpecifiedOrganization><TaxRegistrationID></TaxRegistrationID></SpecifiedOrganization><SpecifiedPerson><NameText>강다혁</NameText></SpecifiedPerson><DefinedContact><URICommunication>kangdahyuk@codlabs.com</URICommunication></DefinedContact><SpecifiedAddress><LineOneText>경기도 용인시 수지구 정평로 73 극동 임광아파트 301동 901호</LineOneText></SpecifiedAddress></InvoicerParty><InvoiceeParty><ID>1968100843</ID><TypeCode>서비스</TypeCode><NameText>씨오디랩스</NameText><ClassificationCode>소프트웨어 개발 및 공급업</ClassificationCode><SpecifiedOrganization><TaxRegistrationID></TaxRegistrationID><BusinessTypeCode>01</BusinessTypeCode></SpecifiedOrganization><SpecifiedPerson><NameText>박진홍</NameText></SpecifiedPerson><PrimaryDefinedContact><PersonNameText>김문식</PersonNameText><DepartmentNameText>경영지원실</DepartmentNameText><TelephoneCommunication>010-7167-2868</TelephoneCommunication><URICommunication>mskim@codlabs.com</URICommunication></PrimaryDefinedContact><SpecifiedAddress><LineOneText>공급받는자 사업장주소</LineOneText></SpecifiedAddress></InvoiceeParty><SpecifiedPaymentMeans><TypeCode>10</TypeCode><PaidAmount>660000</PaidAmount></SpecifiedPaymentMeans><SpecifiedMonetarySummation><ChargeTotalAmount>600000</ChargeTotalAmount><TaxTotalAmount>60000</TaxTotalAmount><GrandTotalAmount>660000</GrandTotalAmount></SpecifiedMonetarySummation></TaxInvoiceTradeSettlement><TaxInvoiceTradeLineItem><SequenceNumeric>572</SequenceNumeric><InvoiceAmount>300000</InvoiceAmount><ChargeableUnitQuantity>1</ChargeableUnitQuantity><InformationText>1</InformationText><NameText>화물</NameText><PurchaseExpiryDateTime>20180325</PurchaseExpiryDateTime><TotalTax><CalculatedAmount>30000</CalculatedAmount></TotalTax><UnitPrice><UnitAmount>300000</UnitAmount></UnitPrice><DescriptionText></DescriptionText></TaxInvoiceTradeLineItem><TaxInvoiceTradeLineItem><SequenceNumeric>574</SequenceNumeric><InvoiceAmount>300000</InvoiceAmount><ChargeableUnitQuantity>1</ChargeableUnitQuantity><InformationText>1</InformationText><NameText>화물</NameText><PurchaseExpiryDateTime>20180325</PurchaseExpiryDateTime><TotalTax><CalculatedAmount>30000</CalculatedAmount></TotalTax><UnitPrice><UnitAmount>300000</UnitAmount></UnitPrice><DescriptionText></DescriptionText></TaxInvoiceTradeLineItem></TaxInvoice>";
                                    //string certPw = "1B165906311101374234";

                                    string DTIXml = CodAPI_XmlMsg.Result_Data[0].DTIXML;                                                                        
                                    string sendMailYN = CodAPI_XmlMsg.Result_Data[0].SENDMAILYN;
                                    string sendSmsYN = CodAPI_XmlMsg.Result_Data[0].SENDSMSYN;
                                    string sendSmsMsg = CodAPI_XmlMsg.Result_Data[0].SENDSMSMSG;

                                    //nice 세금계산서 발급
                                    niceMembJoinRQ.makeAndPublishSign(
                                    CodAPI_RtnMsg.Result_Data[0].LINKCD,
                                    CodAPI_RtnMsg.Result_Data[0].FRNNO,
                                    CodAPI_RtnMsg.Result_Data[0].USERID,
                                    CodAPI_RtnMsg.Result_Data[0].PASSWD,
                                    certPw,
                                    DTIXml,
                                    sendMailYN,
                                    sendSmsYN,
                                    sendSmsMsg,
                                    out retVal, out errMsg, out billNo);

                                    //6-1. nice 세금계산서 발급 했더니 성공
                                    if (retVal == "0")
                                    {  
                                        Models.NiceCertification_Success_Send bill_success_send = new Models.NiceCertification_Success_Send();
                                        bill_success_send.phone_no = niceCertificationSubmitClient.phone_no;
                                        bill_success_send.birth_day = niceCertificationSubmitClient.birth_day;
                                        bill_success_send.tb_idx = tb_idx;
                                        bill_success_send.billno = billNo;

                                        //codAPI 에 리턴값 billNo 저장
                                        bill_success_send.url = Common.ConnectionString.ExtAPI_URL + "/driver/tax/update/billno";
                                        retString = WebProtocols.EXtAPI.bill_success_send(bill_success_send);
                                        retString = retString.Replace("\n", "");
                                        retString = retString.Replace("\\\"", "\"");
                                        retString = retString.Replace("\\\"{", "{");
                                        retString = retString.Replace("}\\\"", "}");
                                        
                                        Models.Basic_Json_Format success_send = Common.Lib.cJSON._DeSerialize<Models.Basic_Json_Format>(retString);
                                        //7-1. codAPI 에 리턴값 billNo 저장(성공)
                                        if (success_send.Status_Code == "200")
                                        {
                                            string errKey = "a" + "200";
                                            errMsg = HttpContext.GetGlobalResourceObject("taxWebErr", errKey).ToString();
                                            retString = "{\"Status_Code\": \"200\", \"Status_Msg\":\"" + errMsg + "\", \"Result_Data\":[]}";
                                            return new HttpResponseMessage()
                                            {
                                                Content = new StringContent(retString, System.Text.Encoding.UTF8, "application/json")
                                            };
                                        }
                                        //7-2. codAPI 에 리턴값 billNo 저장(실패)
                                        else{
                                            string errKey = "a" + "204";
                                            errMsg = HttpContext.GetGlobalResourceObject("taxWebErr", errKey).ToString();
                                            retString = "{\"Status_Code\": \"204\", \"Status_Msg\":\"" + errMsg + "\", \"Result_Data\":[]}";
                                            return new HttpResponseMessage()
                                            {
                                                Content = new StringContent(retString, System.Text.Encoding.UTF8, "application/json")
                                            };
                                        }
                                    }
                                    //6-2. nice 세금계산서 발급 했더니 실패
                                    else
                                    {
                                        //API 에도 에러 메시지를 던져 주어야 한다. 
                                        Models.NiceCertification_Error_Send bill_error_send = new Models.NiceCertification_Error_Send();
                                        bill_error_send.phone_no = niceCertificationSubmitClient.phone_no;
                                        bill_error_send.birth_day = niceCertificationSubmitClient.birth_day;
                                        bill_error_send.tb_idx = tb_idx;
                                        bill_error_send.nice_code = retVal;

                                        //codAPI 에 에러 코드를 저장
                                        bill_error_send.url = Common.ConnectionString.ExtAPI_URL + "/driver/tax/update/no_billno";
                                        retString = WebProtocols.EXtAPI.bill_error_send(bill_error_send);
                                        retString = retString.Replace("\n", "");
                                        retString = retString.Replace("\\\"", "\"");
                                        retString = retString.Replace("\\\"{", "{");
                                        retString = retString.Replace("}\\\"", "}");
                                        //API 에도 에러 메시지를 던져 주어야 한다. -- 끝
                                        Models.Basic_Json_Format error_send = Common.Lib.cJSON._DeSerialize<Models.Basic_Json_Format>(retString);

                                        //7-11. codAPI 에 에러를 저장하고 리턴값(성공)
                                        if (error_send.Status_Code == "200") //nice로 부터 받은 에러 값을 그대로 클라이언트에 전달 한다.
                                        {
                                            string errKey = "a" + retVal;
                                            errMsg = HttpContext.GetGlobalResourceObject("a", errKey).ToString();
                                            retString = "{\"Status_Code\": \"" + retVal + "\", \"Status_Msg\":\"" + errMsg + "\", \"Result_Data\":[]}";

                                            return new HttpResponseMessage()
                                            {
                                                Content = new StringContent(retString, System.Text.Encoding.UTF8, "application/json")
                                            };
                                        }
                                        //7-12. codAPI 에 에러를 저장하고 리턴값(실패)
                                        else
                                        {
                                            string errKey = "a" + error_send.Status_Code;
                                            errMsg = HttpContext.GetGlobalResourceObject("taxWebErr", errKey).ToString();
                                            retString = "{\"Status_Code\": \"" + error_send.Status_Code + "\", \"Status_Msg\":\"" + errMsg + "\", \"Result_Data\":[]}";
                                            return new HttpResponseMessage()
                                            {
                                                Content = new StringContent(retString, System.Text.Encoding.UTF8, "application/json")
                                            };
                                        }                                        
                                    }//세금계산서 발급 실패5-1
                                    //
                                }
                                //5-2.codAPI 에 세금계산서 발급한다고 API 호출 (통보 실패)
                                else{                                    
                                    return new HttpResponseMessage()
                                    {
                                        Content = new StringContent(retString, System.Text.Encoding.UTF8, "application/json")
                                    };
                                }
                            }
                            //4-2. 인증서가 등록되어 있고 //유효기한이 현재 일보다 작다면 인증서 유효기간이 지났습니다.
                            else
                            {  
                                string errKey = "a" + "206";
                                errMsg = HttpContext.GetGlobalResourceObject("taxWebErr", errKey).ToString();
                                retString = "{\"Status_Code\": \"206\", \"Status_Msg\":\"" + errMsg + "\", \"Result_Data\":[]}";
                                return new HttpResponseMessage()
                                {
                                    Content = new StringContent(retString, System.Text.Encoding.UTF8, "application/json")
                                };
                            }
                        }
                        //3-2.  공인인증서 유효한지 nice에서 체크했더니 "N"
                        else //인증서가 등록되어 있지 않습니다. 리턴 끝.
                        {  
                            string errKey = "a" + "202";
                            errMsg = HttpContext.GetGlobalResourceObject("taxWebErr", errKey).ToString();
                            retString = "{\"Status_Code\": \"202\", \"Status_Msg\":\"" + errMsg + "\", \"Result_Data\":[]}";
                            return new HttpResponseMessage()
                            {
                                Content = new StringContent(retString, System.Text.Encoding.UTF8, "application/json")
                            };
                        }
                    }
                    //2-2.  //가입이 안되 되어 있다면.
                    else //아직 나이스 회원가입이 되지 않았다.
                    {                        
                        string errKey = "a" + "201";
                        string errMsg = HttpContext.GetGlobalResourceObject("taxWebErr", errKey).ToString();
                        retString = "{\"Status_Code\": \"201\", \"Status_Msg\":\"" + errMsg + "\", \"Result_Data\":[]}";
                        return new HttpResponseMessage()
                        {
                            Content = new StringContent(retString, System.Text.Encoding.UTF8, "application/json")
                        };
                    }
                }
                //1-1.  나이스 회원 가입 여부 체크 "통신 확인 안됨 : 200 이외의 것들"
                else //http://wiki.codlabs.com:8090/pages/resumedraft.action?draftId=26017921&draftShareId=3c0ba3c3-98eb-4a4c-a8b8-4e68b09f6cd0 에 정의
                {                    
                    return new HttpResponseMessage()
                    {
                        Content = new StringContent(retString, System.Text.Encoding.UTF8, "application/json")
                    };
                }
            }
            catch (Exception exp)
            {
                retString = "{\"Status_Code\": \"1001\", \"Status_Msg\":\" " + exp.Message.ToString() + "\", \"Result_Data\":[]}";
                return new HttpResponseMessage()
                {
                    Content = new StringContent(retString, System.Text.Encoding.UTF8, "application/json")
                };
            }            
        }
    }
}
