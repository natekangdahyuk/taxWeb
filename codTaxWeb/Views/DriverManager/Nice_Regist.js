$(document).ready(function () {
    //정보등록 클릭    
    $("#btn_Submit").click(function () {
        btn_Submit();
    });

});//ready

function btn_Submit() {
    var SESSION_ID = $("#SESSION_ID").val();
    var LINKID = $("#LINKID").val();
    var BIZNO = $("#BIZNO").val();
    var CUSTNAME = $("#CUSTNAME").val();
    var OWNERNAME = $("#OWNERNAME").val();
    var BIZCOND = $("#BIZCOND").val();
    var BIZITEM = $("#BIZITEM").val();
    var RSBMNAME = $("#RSBMNAME").val();
    var EMAIL = $("#EMAIL").val();
    var TELNO = $("#TELNO").val();
    var HPNO = $("#HPNO").val();

    var ZIPCODE = $("#ZIPCODE").val();
    var ADDR1 = $("#ADDR1").val();
    var ADDR2 = $("#ADDR2").val();

    if (SESSION_ID.length < 1) {
        alert("잘못된 접근입니다.");
        return false;
    }

    if (LINKID.length < 1) {
        alert("잘못된 접근입니다.");
        return false;
    }

    if (BIZNO.length < 1) {
        alert("입력이 필요합니다.");
        return false;
    }
    if (CUSTNAME.length < 1) {
        alert("입력이 필요합니다.");
        return false;
    }
    if (OWNERNAME.length < 1) {
        alert("입력이 필요합니다.");
        return false;
    }
    if (BIZCOND.length < 1) {
        alert("입력이 필요합니다.");
        return false;
    }
    if (BIZITEM.length < 1) {
        alert("입력이 필요합니다.");
        return false;
    }
    if (RSBMNAME.length < 1) {
        alert("입력이 필요합니다.");
        return false;
    }
    if (EMAIL.length < 1) {
        alert("입력이 필요합니다.");
        return false;
    }
    if (TELNO.length < 1) {
        alert("입력이 필요합니다.");
        return false;
    }
    if (HPNO.length < 1) {
        alert("입력이 필요합니다.");
        return false;
    }

    //입력이 모두 되었다면.. 나이스로 쏘기 위해.. taxWeb 으로 넘기자.
    var p_obj_NiceUserRegist = {};
        p_obj_NiceUserRegist.SESSION_ID = SESSION_ID,
        p_obj_NiceUserRegist.LINKID = LINKID,
        p_obj_NiceUserRegist.BIZNO = BIZNO,
        p_obj_NiceUserRegist.CUSTNAME = CUSTNAME,
        p_obj_NiceUserRegist.OWNERNAME = OWNERNAME,
        p_obj_NiceUserRegist.BIZCOND = BIZCOND,
        p_obj_NiceUserRegist.BIZITEM = BIZITEM,
        p_obj_NiceUserRegist.RSBMNAME = RSBMNAME,
        p_obj_NiceUserRegist.EMAIL = EMAIL,
        p_obj_NiceUserRegist.TELNO = TELNO,
        p_obj_NiceUserRegist.HPNO = HPNO,
        p_obj_NiceUserRegist.ZIPCODE = ZIPCODE,
        p_obj_NiceUserRegist.ADDR1 = ADDR1,
        p_obj_NiceUserRegist.ADDR2 = ADDR2

    var DTO = { 'p_obj_NiceUserRegist': p_obj_NiceUserRegist };
    fn_nice_regist_AjAx(DTO);
}

function fn_nice_regist_AjAx(DTO) {
    $.ajax({

        url: "/WebService/wsNice.asmx/wsfnNiceRegist",

        type: "POST",
        contentType: "application/json; charset=utf-8",
        cache: false,
        async: false,
        dataType: "json",
        data: JSON.stringify(DTO),
        success: function (data) {
            fn_nice_regist_Success(data);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            var err;
            if (textStatus !== "abort" && errorThrown !== "abort") {
                try {
                    err = jqXHR.statusText;
                    alert(err);
                } catch (e) {
                    alert("ERROR Try Catch:\n" + e.message);
                }
            }// aborted requests should be just ignored and no error message be displayed
        }//error
    }); //ajax

}

function fn_nice_regist_Success(data) {
    var json = JSON.parse(data.d);
    var errMsg = "";
    //if (json.Status_Code === "517") {
    if (json.Status_Code === "200") {
        var p_frnNo = niceEncodingString(json.Result_Data[0]["FRNNO"]);
        var p_userId = niceEncodingString(json.Result_Data[0]["USERID"]);
        var p_passwd = niceEncodingString(json.Result_Data[0]["PASSWD"]);
        var p_linkCd = niceEncodingString(json.Result_Data[0]["LINKCD"]);
        var p_certificationUrl = json.Result_Data[0]["CERTIFICATIONURL"]; //이건 주소
        var p_retUrl = niceEncodingString(json.Result_Data[0]["RETURL"]);

        var p_nice_url = p_certificationUrl+"?" +"frnNo=" + p_frnNo + "&" + "userId=" + p_userId + "&" + "passwd=" + p_passwd + "&" + "linkCd=" + p_linkCd + "&" + "retUrl=" + p_retUrl;
        
        location.href = p_nice_url;
        return true;
    }    
    else if (json.Status_Code === "528") {
        errMsg = "affected row 0";
        alert("fn_nice_regist_Success : " + errMsg);
        return false;
    }
    else if (json.Status_Code === "900") { //세션 종료는 바로 로그아웃
        location.href = "/DriverManager/Check_Login";//로그아웃이 없고 로그인 페이지로 이동
        return false;
    }
    else if (json.Status_Code === "1500") {
        errMsg = "DB error : ";
        addMsg = json.Status_Msg;
        errMsg = errMsg + addMsg;
        alert("fn_nice_regist_Success : " + errMsg);
        return false;
    }
    else if (json.Status_Code === "500") {
        errMsg = "알수 없는 에러 : ";
        addMsg = json.Status_Msg;
        errMsg = errMsg + addMsg;
        alert("fn_nice_regist_Success : " + errMsg);
        return false;
    }
    else {        
        alert("fn_nice_regist_Success : " + json.Status_Msg);
        return false;
    }
    
}

