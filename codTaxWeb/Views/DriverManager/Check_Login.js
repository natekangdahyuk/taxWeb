$(document).ready(function () {
    //암호화 테스트
    //var p_retUrl = niceEncodingString("http://localhost:60201/DriverManager/certification_finish");
    //var p_retUrl = niceEncodingString("http://dev.tax.codlabs.com:8020/DriverManager/certification_finish");
    //console.log(p_retUrl);

    if (isMobile()) {
        alert("PC 에서 접속해 주세요");
        location.href = "/DriverManager/Error";	//에러페이지
    } 

    //로그인 버튼 클릭
    $("#btnLogin").click(function () {        
        LoginChk();
    });

    // 비밀번호입력에서 엔터키 누를 시 로그인버튼 클릭이벤트
    $("#birth_day").keyup(function (e) {
        if (e.keyCode === 13) {
            LoginChk();
        }
    });
});

function isMobile() {
    var UserAgent = navigator.userAgent;

    if (UserAgent.match(/iPhone|iPod|Android|Windows CE|BlackBerry|Symbian|Windows Phone|webOS|Opera Mini|Opera Mobi|POLARIS|IEMobile|lgtelecom|nokia|SonyEricsson/i) != null || UserAgent.match(/LG|SAMSUNG|Samsung/) != null) {
        return true;
    } else {
        return false;
    }
}


//로그인 체크
function LoginChk() {
    var phone_no = $("#phone_no").val();
    var birth_day = $("#birth_day").val();

    if (phone_no.length < 10) {
        alert("차주 앱 사용 전화번호를 입력해 주세요.");
        return false;
    }

    if (birth_day.length < 6) {
        alert("생일을 입력해 주세요.");
        return false;
    }

    var p_objUserLogin = {};
    p_objUserLogin.phone_no = phone_no;
    p_objUserLogin.birth_day = birth_day;

    var DTO = { 'p_objUserLogin': p_objUserLogin };

    $.ajax({
        url: "/WebService/wsMember.asmx/wsfnDriverLogin",

        type: "POST",
        contentType: "application/json; charset=utf-8",
        cache: false,
        async: false,
        dataType: "json",
        data: JSON.stringify(DTO),

        success: function (data) {
            fn_Login_Success(data);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            //fn_Login_Error(jqXHR, textStatus, errorThrown);
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


}//function callExtAPI

//로그인 성공했다면... 
function fn_Login_Success(data) {
    var json = JSON.parse(data.d);
    var errMsg = "", addMsg = "";
    if (json.Status_Code === "200") {
        //나이스 회원가입을 한 사용자라면 나이스 회원가입 되었는지 확인 해서.. 
        //안되었다면..가입으로.되었다면..인증서 등록으로(/driver/tax / member_check 호출)
        var phone_no = $("#phone_no").val();
        var birth_day = $("#birth_day").val();
        var p_objUserLogin = {};
        p_objUserLogin.phone_no = phone_no;
        p_objUserLogin.birth_day = birth_day;

        var DTO = { 'p_objUserLogin': p_objUserLogin };

        $.ajax({
            url: "/WebService/wsMember.asmx/wsfnNiceRegistCheck",

            type: "POST",
            contentType: "application/json; charset=utf-8",
            cache: false,
            async: false,
            dataType: "json",
            data: JSON.stringify(DTO),

            success: function (data) {
                fn_NiceRegistCheck_Success(data, json);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                //fn_Login_Error(jqXHR, textStatus, errorThrown);
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

    }//아래는 로그인 실패
    else if (json.Status_Code === "500") {
        errMsg = "db ERROR";
        $("#err_area").css("display", "block");
        $("#err_area").text(errMsg);
        return false;
    }
    else if (json.Status_Code === "501") {
        errMsg = "찾을 수 없는 핸드폰 번호입니다";
        $("#err_area").css("display", "block");
        $("#err_area").text(errMsg);
        return false;
    }
    else if (json.Status_Code === "502") {
        errMsg = "패스워드(생년월일)가 일치하지 않습니다";
        $("#err_area").css("display", "block");
        $("#err_area").text(errMsg);
        return false;
    }
    else if (json.Status_Code === "505") {
        errMsg = "회원님은 사용할 수 없는 상태입니다."; //:::: USE_YN이라는 칼럼이 N(COD 관리자가 사용못하게 막는등) ";
        $("#err_area").css("display", "block");
        $("#err_area").text(errMsg);
        return false;
    }
    else if (json.Status_Code === "506") {
        errMsg = "회원님은 가입승인을 받으시기 바랍니다.(관리자에게 문의바랍니다.)";// ::: 그럴경우는 없겠지만 준회원이 이경로로 들어올경우
        $("#err_area").css("display", "block");
        $("#err_area").text(errMsg);
        return false;
    }
    else {
        errMsg = "정말 알수 없는 에러 : ";
        $("#err_area").css("display", "block");
        $("#err_area").text(errMsg);
        return false;
    }
}

function fn_Login_Error(jqXHR, textStatus, errorThrown) {
    var err;
    if (textStatus !== "abort" && errorThrown !== "abort") {
        try {
            //err = textStatus;
            err = jqXHR.statusText;
            alert(err);
        } catch (e) {
            alert("ERROR Try Catch:\n" + e.message);
        }
    }// aborted requests should be just ignored and no error message be displayed
}

function fn_NiceRegistCheck_Success(data, loginOK_json) {
    var json = JSON.parse(data.d);
    var errMsg = "", addMsg = "";
    if (json.Status_Code === "200") {
        if (json.Result_Data[0].REG_YN === "Y") {
            //공인인증서 유효 soap 체크
            var p_objSelectExpireDt = {};
            p_objSelectExpireDt.LINKCD = json.Result_Data[0].LINKCD;
            p_objSelectExpireDt.FRNNO = json.Result_Data[0].FRNNO;
            p_objSelectExpireDt.USERID = json.Result_Data[0].USERID;
            p_objSelectExpireDt.PASSWD = json.Result_Data[0].PASSWD;
            p_objSelectExpireDt.CERTPW = json.Result_Data[0].CERTPW;

            var DTO = { 'p_objSelectExpireDt': p_objSelectExpireDt };

            $.ajax({
                url: "/WebService/wsNice.asmx/wsfnNiceSelectExpireDt",

                type: "POST",
                contentType: "application/json; charset=utf-8",
                cache: false,
                async: false,
                dataType: "json",
                data: JSON.stringify(DTO),

                success: function (data) {
                    fn_ExpireDt_Success(data); // 유효하던 하지 않던 AJAX 는 성공 .. 성공 함수에서 처리 합니다.. 확인 하세요.
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    //fn_Login_Error(jqXHR, textStatus, errorThrown);
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
            //유효하면 팝업(app 에서 세금 계산서 발행 하세요.. 여기서는 할게 없어요.)
            
        }//아직 회원가입이 되지 않았군요..회원가입 페이지로 보내 드릴께요.
        else {//REG_YN == N
            var path = "/DriverManager/Nice_Regist";
            var method = "POST";

            var params = {};
            params.SESSION_ID = loginOK_json.Result_Data[0].SESSION_ID; //서버에서 보내 준 세션아이디 매번 변경
            params.LINKID = loginOK_json.Result_Data[0].LINKID;//LINKID = DRIVERIDX임 //연계사가 사용중인 ID;
            params.BIZNO = loginOK_json.Result_Data[0].BIZNO; //사업자번호;
            params.CUSTNAME = loginOK_json.Result_Data[0].CUSTNAME; //회사명;
            params.OWNERNAME = loginOK_json.Result_Data[0].OWNERNAME;//대표자명
            params.BIZCOND = loginOK_json.Result_Data[0].BIZCOND;//업태;
            params.BIZITEM = loginOK_json.Result_Data[0].BIZITEM;//업종;
            params.RSBMNAME = loginOK_json.Result_Data[0].RSBMNAME;//담당자 명;
            params.EMAIL = loginOK_json.Result_Data[0].EMAIL;//이메일;
            params.TELNO = loginOK_json.Result_Data[0].TELNO;//전화번호;
            params.HPNO = loginOK_json.Result_Data[0].HPNO;//휴대번호;
            params.ZIPCODE = loginOK_json.Result_Data[0].ZIPCODE; //형식적인 빈값
            params.ADDR1 = loginOK_json.Result_Data[0].ADDR1;//형식적인 빈값
            params.ADDR2 = loginOK_json.Result_Data[0].ADDR2;//형식적인 빈값;

            //페이지 이동
            //전송 예)   post_to_url('http://example.com/', { 'q': 'a' });
            post_to_url(path, params, method);
            // 성공하면 아래 페이지로 안간다.
        }
    }//200

    else if (json.Status_Code === "500") {
        errMsg = "db ERROR";
        $("#err_area").css("display", "block");
        $("#err_area").text(errMsg);
        return false;
    }
    else if (json.Status_Code === "501") {
        errMsg = "찾을 수 없는 핸드폰 번호입니다";
        $("#err_area").css("display", "block");
        $("#err_area").text(errMsg);
        return false;
    }
    else if (json.Status_Code === "502") {
        errMsg = "패스워드(생년월일)가 일치하지 않습니다";
        $("#err_area").css("display", "block");
        $("#err_area").text(errMsg);
        return false;
    }
    else if (json.Status_Code === "505") {
        errMsg = "회원님은 사용할 수 없는 상태입니다."; //:::: USE_YN이라는 칼럼이 N(COD 관리자가 사용못하게 막는등) ";
        $("#err_area").css("display", "block");
        $("#err_area").text(errMsg);
        return false;
    }
    else if (json.Status_Code === "506") {
        errMsg = "회원님은 가입승인을 받으시기 바랍니다.(관리자에게 문의바랍니다.)";// ::: 그럴경우는 없겠지만 준회원이 이경로로 들어올경우
        $("#err_area").css("display", "block");
        $("#err_area").text(errMsg);
        return false;
    }
    else {
        errMsg = "정말 알수 없는 에러 : ";
        $("#err_area").css("display", "block");
        $("#err_area").text(errMsg);
        return false;
    }
}

function fn_ExpireDt_Success(data) {
    var json = JSON.parse(data.d);
    var errMsg = "", addMsg = "";
    if (json.Status_Code === "200") {//유효기간이 정상이기에 끝.
        location.href = "/DriverManager/expireDt_success";
        return false;
    }//아래는 인증서 
    else if (json.Status_Code === "201") {//유효기간이 정상이 아닐경우 등록 페이지로 이동 시켜라.
        var p_frnNo = niceEncodingString(json.Result_Data[0]["FRNNO"]);
        var p_userId = niceEncodingString(json.Result_Data[0]["USERID"]);
        var p_passwd = niceEncodingString(json.Result_Data[0]["PASSWD"]);
        var p_linkCd = niceEncodingString(json.Result_Data[0]["LINKCD"]);
        var p_certificationUrl = json.Result_Data[0]["CERTIFICATIONURL"]; //이건 주소
        var p_retUrl = niceEncodingString(json.Result_Data[0]["RETURL"]);

        var p_nice_url = p_certificationUrl + "?" + "frnNo=" + p_frnNo + "&" + "userId=" + p_userId + "&" + "passwd=" + p_passwd + "&" + "linkCd=" + p_linkCd + "&" + "retUrl=" + p_retUrl;

        location.href = p_nice_url;
        return true;
    }
    else {
        errMsg = "정말 알수 없는 에러 : ";
        $("#err_area").css("display", "block");
        $("#err_area").text(errMsg);
        return false;
    }
}

/*
* path : 전송 URL
* params : 전송 데이터 {'q':'a','s':'b','c':'d'...}으로 묶어서 배열 입력
* method : 전송 방식(생략가능)
*/
function post_to_url(path, params, method) {
    method = method || "post"; // Set method to post by default, if not specified.
    // The rest of this code assumes you are not using a library.
    // It can be made less wordy if you use one.
    var form = document.createElement("form");
    form.setAttribute("method", method);
    form.setAttribute("action", path);
    for (var key in params) {
        var hiddenField = document.createElement("input");
        hiddenField.setAttribute("type", "hidden");
        hiddenField.setAttribute("name", key);
        hiddenField.setAttribute("value", params[key]);
        form.appendChild(hiddenField);
    }
    document.body.appendChild(form);
    form.submit();
}