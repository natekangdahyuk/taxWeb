//로그인 안 했으면.. 못 들어오게
var local_session_id = "";
function Local_OnemoreAuthCheck() { //잘되면 common 으로 이동
    local_session_id = Cookies.get("session_id");
    if (typeof local_session_id === "undefined") {
        //location.href = "/UserManager/Check_Login"; //이거 작동 안되기에 setTimOut 으로 변경
        setTimeout(function () { document.location.href = "/UserManager/Check_Login" }, 500);
    }
}

// String.format 함수 시작
String.prototype.format = function (format) {
    var args = arguments;
    return this.replace(/\{(\d+)\}/g, function (m, i) { return args[i]; });
}

String.format = function (str) {
    var args = arguments;
    var args2 = [];
    for (var i = 1; i < args.length; i++) {
        args2.push(args[i]);
    }

    return str.replace(/\{(\d+)\}/g, function (m, i) { return args2[i]; });
}

// HTML 컨버젼
String.prototype.cHtml = function () {
    return this.replace(/(\&)/g, "&amp;").replace(/\</g, "&lt;").replace(/\>/g, "&gt;");
}

// Date 형식으로 변환
String.prototype.toDate = function () {
    var str = String(this);

    if (str.indexOf("/") == 0) {
        return eval("new " + str.gsub("/", ""));
    }

    else if (str.length == 8) {
        return new Date(str.substr(0, 4), Number(str.substr(4, 2)) - 1, str.substr(6, 2));
    }
    else if (str.length == 10) {
        return new Date(str.substr(0, 4), Number(str.substr(5, 2)) - 1, str.substr(8, 2));
    }
    else if (str.indexOf("T") >= 0) {
        return new Date(str.substr(0, 4), Number(str.substr(5, 2)) - 1, Number(str.substr(8, 2)),
            str.substr(11, 2), str.substr(14, 2), str.substr(17, 2));
    }
    else if (str.length == 14) {
        return new Date(str.substr(0, 4), Number(str.substr(4, 2)) - 1, str.substr(6, 2),
            str.substr(8, 2), str.substr(10, 2), str.substr(12, 2));
    }
    else if (str.length > 15) {
        return new Date(str.substr(0, 4), str.substr(5, 2), str.substr(8, 2),
            str.substr(11, 2), str.substr(14, 2), str.substr(17, 2));
    }
}
// 공백 제거
String.prototype.trim = function () { var re = /^\s+|\s+$/g; return function () { return this.replace(re, ""); }; }();

// String.format 함수 종료

/************************************************
 * 입력폼 숫자 
*************************************************/
//[] <--문자 범위 [^] <--부정 [0-9] <-- 숫자  
//[0-9] => \d , [^0-9] => \D
var rgx1 = /\D/g;  // /[^0-9]/g 와 같은 표현
var rgx2 = /(\d+)(\d{3})/;

//textBox 에 입력된 값이 숫자인지 판단하여 숫자만 리턴
function getOnlyNumber(obj) {
    var num01;
    var num02;
    num01 = obj.value;
    num02 = num01.replace(rgx1, "");
    num01 = num02;
    obj.value = num01;

}

//textBox 에 입력된 값이 숫자인지 판단하여 숫자면 콤마찍어서 넘겨주고 아니면. 빈칸 리턴
function getCommaNumber(obj) {
    var num01;
    var num02;
    num01 = obj.value;
    num01 = num01.replace(/(^0+)/, ""); // 앞에 숫자 0 이 붙었다면.. 지운다.
    num02 = num01.replace(rgx1, "");

    num01 = setComma(num02);
    obj.value = num01;

}

//콤마 찍어주기
function setComma(inNum) {
    var outNum;
    outNum = inNum;
    while (rgx2.test(outNum)) {
        outNum = outNum.replace(rgx2, '$1' + ',' + '$2');
    }
    return outNum;
}

//콤마 풀기
function uncomma(str) {
    str = String(str);
    return str.replace(/[^\d]+/g, '');
}

//C#에서 만든 쿠키를 js 에서 불러 사용하기
function getSystemCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

/************************************************
 * 리스트 페이징 만들기 -- 상,하차지 주소록 리스트
 * - totCnt     : 총 게시물(리스트) 건수
 * - psize      : 페이징 사이즈
 * - objCPage   : 현재페이지 오브젝트(HiddenText)
 * - objDiv     : 페이징 오브젝트(Div)
 * - selectfunc : 리스트 조회 함수명(Function)
*************************************************/
function Gf_SetListPaging_AddrList(totCnt, psize, objCPage, objDiv, selectfunc) {
    var viewPage = 10; // 페이징 목록 수 (기본셋팅)
    var nowPage = parseInt($("#" + objCPage).val());
    var pageSize = parseInt(psize);
    var totPage = parseInt(totCnt / pageSize) + ((totCnt % pageSize > 0) ? 1 : 0); // 총 페이지 수
    var _pagingHtml;

    $("#" + objDiv).html("");

    if (totCnt == 0 || totPage == 1) return; // 리스트 데이터가 없을 경우 or 페이지가 하나(1)일 경우

    _pagingHtml = "<div class='inner'>";
    var $trPaging = $("#" + objDiv);

    if (nowPage > 1) {
        _pagingHtml += "<a class='prev' href='#'>&lt;&lt; 이전 페이지</a>";
    }
    else {
        _pagingHtml += "<span class='prevNoLink'>&lt;&lt; 이전 페이지</span>";
    }


    var sNum = 0;
    var eNum = (totPage > viewPage) ? viewPage : totPage;

    _pagingHtml += "<ol>";
    if (nowPage > (viewPage / 2)) {
        if (nowPage + (viewPage / 2) <= totPage) {
            sNum = nowPage - (viewPage / 2);
            eNum = nowPage + (viewPage / 2);
        }
        else {
            sNum = (totPage > viewPage) ? totPage - viewPage : sNum;
            eNum = totPage;
        }
    }
    for (var i = sNum; i < eNum; i++) {
        if (i == nowPage - 1) {
            _pagingHtml += "<li><a href='#' class='active'>" + (i + 1) + "</a></li>";
        }
        else {
            _pagingHtml += "<li><a href='#' class='off'>" + (i + 1) + "</a></li>";
        }
    }
    _pagingHtml += "</ol>";

    if (totPage != 1 && nowPage != totPage) {
        _pagingHtml += "<a class='next' href='#'>다음 페이지  &gt;&gt;</a>";
    }
    else {
        _pagingHtml += "<span class='nextNoLink'>다음 페이지  &gt;&gt;</span>";
    }



    _pagingHtml += "</div>";
    $("#" + objDiv).html(_pagingHtml);

    // 페이징 클릭 이벤트 설정
    $("#" + objDiv).find(".first").click(function () {
        var currPage = parseInt($("#" + objCPage).val());
        var movePage = 1;

        $("#" + objCPage).val(movePage);
        eval(selectfunc)();
    });
    $("#" + objDiv).find(".prev").click(function () {
        var currPage = parseInt($("#" + objCPage).val());
        var movePage = ((currPage > 1) ? currPage - 1 : 1);

        $("#" + objCPage).val(movePage);
        eval(selectfunc)();
    });
    $("#" + objDiv).find(".off").click(function () {
        //alert($(this).text());
        $("#" + objCPage).val($(this).text());
        eval(selectfunc)();
    });
    $("#" + objDiv).find(".next").click(function () {
        var currPage = parseInt($("#" + objCPage).val());
        var movePage = ((currPage < totPage) ? currPage + 1 : totPage);

        $("#" + objCPage).val(movePage);
        eval(selectfunc)();
    });
    $("#" + objDiv).find(".last").click(function () {
        var currPage = parseInt($("#" + objCPage).val());
        var movePage = totPage;

        $("#" + objCPage).val(movePage);
        eval(selectfunc)();
    });
}

/************************************************
 * 리스트 페이징 만들기 -- 화물 리스트
 * - totCnt     : 총 게시물(리스트) 건수
 * - psize      : 페이징 사이즈
 * - objCPage   : 현재페이지 오브젝트(HiddenText)
 * - objDiv     : 페이징 오브젝트(Div)
 * - selectfunc : 리스트 조회 함수명(Function)
*************************************************/
function Gf_SetListPaging_Cargo(totCnt, psize, objCPage, objDiv, selectfunc) {
    var viewPage = 10; // 페이징 목록 수 (기본셋팅)
    var nowPage = parseInt($("#" + objCPage).val());
    var pageSize = parseInt(psize);
    var totPage = parseInt(totCnt / pageSize) + ((totCnt % pageSize > 0) ? 1 : 0); // 총 페이지 수
    var _pagingHtml;

    $("#" + objDiv).html("");

    if (totCnt == 0 || totPage == 1) return; // 리스트 데이터가 없을 경우 or 페이지가 하나(1)일 경우

    _pagingHtml = "<div class='inner'>";
    var $trPaging = $("#" + objDiv);
        
    if (nowPage > 1) {
        _pagingHtml += "<a class='pgPrev' href='#'>&lt;&lt; 이전 페이지</a>";
    }
    else {
        _pagingHtml += "<span class='prevNoLink'>&lt;&lt; 이전 페이지</span>";
    }
    

    var sNum = 0;
    var eNum = (totPage > viewPage) ? viewPage : totPage;

    _pagingHtml += "<span>";
    if (nowPage > (viewPage / 2)) {
        if (nowPage + (viewPage / 2) <= totPage) {
            sNum = nowPage - (viewPage / 2);
            eNum = nowPage + (viewPage / 2);
        }
        else {
            sNum = (totPage > viewPage) ? totPage - viewPage : sNum;
            eNum = totPage;
        }
    }
    for (var i = sNum; i < eNum; i++) {
        if (i == nowPage - 1) {
            _pagingHtml += "<a href='#' class='active'>" + (i + 1) + "</a>";
        }
        else {
            _pagingHtml += "<a href='#' class='off'>" + (i + 1) + "</a>";
        }
    }
    _pagingHtml += "</span>";

    if (totPage !=1 && nowPage != totPage) {
        _pagingHtml += "<a class='pgNext' href='#'>다음 페이지  &gt;&gt;</a>";
    }
    else {
        _pagingHtml += "<span class='nextNoLink'>다음 페이지  &gt;&gt;</span>";
    }
    
    

    _pagingHtml += "</div>";
    $("#" + objDiv).html(_pagingHtml);

    // 페이징 클릭 이벤트 설정
    $("#" + objDiv).find(".first").click(function () {
        var currPage = parseInt($("#" + objCPage).val());
        var movePage = 1;

        $("#" + objCPage).val(movePage);
        eval(selectfunc)();
    });
    $("#" + objDiv).find(".pgPrev").click(function () {
        var currPage = parseInt($("#" + objCPage).val());
        var movePage = ((currPage > 1) ? currPage - 1 : 1);

        $("#" + objCPage).val(movePage);
        eval(selectfunc)();
    });
    $("#" + objDiv).find(".off").click(function () {
        $("#" + objCPage).val($(this).text());
        eval(selectfunc)();
    });
    $("#" + objDiv).find(".pgNext").click(function () {
        var currPage = parseInt($("#" + objCPage).val());
        var movePage = ((currPage < totPage) ? currPage + 1 : totPage);

        $("#" + objCPage).val(movePage);
        eval(selectfunc)();
    });
    $("#" + objDiv).find(".last").click(function () {
        var currPage = parseInt($("#" + objCPage).val());
        var movePage = totPage;

        $("#" + objCPage).val(movePage);
        eval(selectfunc)();
    });
}

/************************************************
 * 리스트 페이징 만들기 -- 관리자 툴
 * - totCnt     : 총 게시물(리스트) 건수
 * - psize      : 페이징 사이즈
 * - objCPage   : 현재페이지 오브젝트(HiddenText)
 * - objDiv     : 페이징 오브젝트(Div)
 * - selectfunc : 리스트 조회 함수명(Function)
*************************************************/
function Gf_SetListPaging(totCnt, psize, objCPage, objDiv, selectfunc) {
    var viewPage = 10; // 페이징 목록 수 (기본셋팅)
    var nowPage = parseInt($("#" + objCPage).val());
    var pageSize = parseInt(psize);
    var totPage = parseInt(totCnt / pageSize) + ((totCnt % pageSize > 0) ? 1 : 0); // 총 페이지 수
    var _pagingHtml;

    $("#" + objDiv).html("");

    if (totCnt == 0 || totPage == 1) return; // 리스트 데이터가 없을 경우 or 페이지가 하나(1)일 경우

    _pagingHtml = "<div class='paging_dev'>";
    var $trPaging = $("#" + objDiv);

    _pagingHtml += "<a class='first' href='#'>처음 페이지 이동</a>";
    _pagingHtml += "<a class='prev' href='#'>이전 페이지 이동</a>";

    //if (nowPage > 1) {
    //    _pagingHtml += "<a class='prev' href='#'>이전 페이지 이동</a>";
    //}

    var sNum = 0;
    var eNum = (totPage > viewPage) ? viewPage : totPage;

    _pagingHtml += "<span>";
    if (nowPage > (viewPage / 2)) {
        if (nowPage + (viewPage / 2) <= totPage) {
            sNum = nowPage - (viewPage / 2);
            eNum = nowPage + (viewPage / 2);
        }
        else {
            sNum = (totPage > viewPage) ? totPage - viewPage : sNum;
            eNum = totPage;
        }
    }
    for (var i = sNum; i < eNum; i++) {
        if (i == nowPage - 1) {
            _pagingHtml += "<a href='#' class='on'>" + (i + 1) + "</a>";
        }
        else {
            _pagingHtml += "<a href='#' class='off'>" + (i + 1) + "</a>";
        }
    }
    _pagingHtml += "</span>";

    
    _pagingHtml += "<a class='next' href='#'>다음 페이지 이동</a>";
    _pagingHtml += "<a class='last' href='#'>마지막 페이지 이동</a>";

    _pagingHtml += "</div>";
    $("#" + objDiv).html(_pagingHtml);

    // 페이징 클릭 이벤트 설정
    $("#" + objDiv).find(".first").click(function () {
        var currPage = parseInt($("#" + objCPage).val());
        var movePage = 1;

        $("#" + objCPage).val(movePage);
        eval(selectfunc)();
    });
    $("#" + objDiv).find(".prev").click(function () {
        var currPage = parseInt($("#" + objCPage).val());
        var movePage = ((currPage > 1) ? currPage - 1 : 1);

        $("#" + objCPage).val(movePage);
        eval(selectfunc)();
    });
    $("#" + objDiv).find(".off").click(function () {
        $("#" + objCPage).val($(this).text());
        eval(selectfunc)();
    });
    $("#" + objDiv).find(".next").click(function () {
        var currPage = parseInt($("#" + objCPage).val());
        var movePage = ((currPage < totPage) ? currPage + 1 : totPage);

        $("#" + objCPage).val(movePage);
        eval(selectfunc)();
    });
    $("#" + objDiv).find(".last").click(function () {
        var currPage = parseInt($("#" + objCPage).val());
        var movePage = totPage;

        $("#" + objCPage).val(movePage);
        eval(selectfunc)();
    });
}

// 자바스크립트 콤마 찍기(ex: 금액)
function commaStr(n) {
    var reg = /(^[+-]?\d+)(\d{3})/;
    n += "";

    while (reg.test(n))
        n = n.replace(reg, "$1" + "," + "$2");
    return n;
}


//공지사항등 날짜에 사용하는  Json 날짜 수정
function makeDateFormat_YYYY_MM_DD_HH_mm_DD(wDate) {             
    var regex = /-?\d+/;
    var matches = regex.exec(wDate);
    var dt = new Date(parseInt(matches[0]));
    var year = dt.getFullYear();
    var month = dt.getMonth() + 1;
    var monthString = month > 9 ? month : '0' + month;
    var day = dt.getDate();
    var dayString = day > 9 ? day : '0' + day;
    var hour = dt.getHours();
    var hourString = hour > 9 ? hour : '0' + hour;
    var min = dt.getMinutes();
    var minString = min > 9 ? min : '0' + min;
    var sec = dt.getSeconds();
    var secString = sec > 9 ? sec : '0' + sec;

    var wDate = year + '/' + monthString + '/' + dayString + ' ' + hourString + ':' + minString + '-' + secString;    
    return wDate;
}

//패스워드 유효성 체크 -- 숫자, 영문, 특수문자만 가능 8~32자만
function isValidFormPassword(pw) {
    var check = /^(?=.*[a-zA-Z])(?=.*[!@#$%^*+=-])(?=.*[0-9]).{6,16}$/;

    if (!check.test(pw)) {
        return false;
    }

    if (pw.length < 8 || pw.length > 32) {
        return false;
    }

    return true;
}

//이메일 유효성 체크
function Gf_EmailCheck(val) {
    var regex = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

    var email = val;
    if (!regex.test(email)) {
        //alert("올바른 이메일 주소를 입력하세요")
        return false;
    }
    else {
        return true;
    }
}

//우편번호 주소찾기 팝업 - 주소록 등록
function OpenZip() {
    popZipCode_pop = window.open("/Assets/Popup/popAddrZipCode.html", "popAddrZipCode", "scrollbars=yes,toolbar=no,location=no,directories=no,width=772, height=700,resizable=no,menubar=no");
    popZipCode_pop.opener = self;
    popZipCode_pop.focus();
}

//우편번호 주소찾기 팝업 - 화물등록 -상차지, 하차지 주소 등록
function OpenAddrList(up_or_down) {
    if (up_or_down == 'up') {
        //OpenAddrList_pop = window.open("/CargoManager/Cargo_popAddr_list?code=up", "OpenAddrList_pop", "scrollbars=yes,toolbar=no,location=no,directories=no,width=900, height=700,resizable=no,menubar=no");
        OpenAddrList_pop = window.open("/CargoManager/Cargo_popAddr_list?code=up", "OpenAddrList_pop", "scrollbars=yes,toolbar=no,location=no,directories=no,width=1050, height=750,resizable=no,menubar=no");
    }
    else {
        //OpenAddrList_pop = window.open("/CargoManager/Cargo_popAddr_list?code=down", "OpenAddrList_pop", "scrollbars=yes,toolbar=no,location=no,directories=no,width=900, height=700,resizable=no,menubar=no");
        OpenAddrList_pop = window.open("/CargoManager/Cargo_popAddr_list?code=down", "OpenAddrList_pop", "scrollbars=yes,toolbar=no,location=no,directories=no,width=1050, height=750,resizable=no,menubar=no");
    }
    OpenAddrList_pop.opener = self;
    OpenAddrList_pop.focus();
}

//현재 시간 가져오기 yyyy-mm-dd hh:mm:sss 형태
function makeDateFormat_YYYY_MM_DD_HH_mm_ss(wDate) {
    var dt = new Date(wDate);
    var year = dt.getFullYear();
    var month = dt.getMonth() + 1;
    var monthString = month > 9 ? month : '0' + month;
    var day = dt.getDate();
    var dayString = day > 9 ? day : '0' + day;
    var hour = dt.getHours();
    var hourString = hour > 9 ? hour : '0' + hour;
    var min = dt.getMinutes();
    var minString = min > 9 ? min : '0' + min;
    var sec = dt.getSeconds();
    var secString = sec > 9 ? sec : '0' + sec;

    var wDate = year + '-' + monthString + '-' + dayString + ' ' + hourString + ':' + minString+':'+secString;
    return wDate;
}

//화물 관리에서 년/월/일 시:분 까지 보여 줄때 사용 날짜 수정  2017-09-19T03:15:17 "T"가 들어간 날짜 형태
function makeDateFormat_YYYY_MM_DD_HH_mm(wDate) {    
    var dt = new Date(wDate);
    var year = dt.getFullYear();
    var month = dt.getMonth() + 1;
    var monthString = month > 9 ? month : '0' + month;
    var day = dt.getDate();
    var dayString = day > 9 ? day : '0' + day;
    var hour = dt.getHours();
    var hourString = hour > 9 ? hour : '0' + hour;
    var min = dt.getMinutes();
    var minString = min > 9 ? min : '0' + min;
    var sec = dt.getSeconds();
    var secString = sec > 9 ? sec : '0' + sec;

    var wDate = year + '/' + monthString + '/' + dayString + ' ' + hourString + ':' + minString;
    return wDate;
}

//화물 관리에서 년-월-일만 보여 줄때 사용 날짜 수정  2017-09-19T03:15:17 "T"가 들어간 날짜 형태
function makeDateFormat_YYYY_MM_DD(wDate) {
    var dt = new Date(wDate);
    var year = dt.getFullYear();
    var month = dt.getMonth() + 1;
    var monthString = month > 9 ? month : '0' + month;
    var day = dt.getDate();
    var dayString = day > 9 ? day : '0' + day;
    
    var wDate = year + '-' + monthString + '-' + dayString;
    return wDate;
}

//화물 관리에서 "시" 만 보여 줄때 사용 날짜 수정  2017-09-19T03:15:17 "T"가 들어간 날짜 형태
function makeDateFormat_hh(wDate) {
    var dt = new Date(wDate);
    var year = dt.getFullYear();
    var month = dt.getMonth() + 1;
    var monthString = month > 9 ? month : '0' + month;
    var day = dt.getDate();
    var dayString = day > 9 ? day : '0' + day;
    var hour = dt.getHours();
    var hourString = hour > 9 ? hour : '0' + hour;
    var min = dt.getMinutes();
    var minString = min > 9 ? min : '0' + min;
    var sec = dt.getSeconds();
    var secString = sec > 9 ? sec : '0' + sec;

    var wDate = hourString;
    return wDate;
}

//화물 관리에서 "분" 만 보여 줄때 사용 날짜 수정  2017-09-19T03:15:17 "T"가 들어간 날짜 형태
function makeDateFormat_mm(wDate) {
    var dt = new Date(wDate);
    var year = dt.getFullYear();
    var month = dt.getMonth() + 1;
    var monthString = month > 9 ? month : '0' + month;
    var day = dt.getDate();
    var dayString = day > 9 ? day : '0' + day;
    var hour = dt.getHours();
    var hourString = hour > 9 ? hour : '0' + hour;
    var min = dt.getMinutes();
    var minString = min > 9 ? min : '0' + min;
    var sec = dt.getSeconds();
    var secString = sec > 9 ? sec : '0' + sec;

    var wDate = minString;
    return wDate;
}

//화물 리스트에서 남은 날짜와 시간을 표시하기 위해 만든 함수
function showRemaining(now, end) {
    var _second = 1000;
    var _minute = _second * 60;
    var _hour = _minute * 60;
    var _day = _hour * 24;

    var retString =""

    var distance = end - now;
    if (distance < 0) {
        return retString;
    }

    var days = Math.floor(distance / _day);
    var hours = Math.floor((distance % _day) / _hour);
    var minutes = Math.floor((distance % _hour) / _minute);
    var seconds = Math.floor((distance % _minute) / _second);

    return retString = days + "," + hours + "," + minutes + "," + seconds;    
}


// JSON으로 Serialize 되어 있는 datetime을 String으로 변환
function parseDatetime(pDatetime) {
    var vDate = eval("new " + pDatetime.replace(/\//g, "") + ";");
    var vConDate = new Date(vDate);

    var oStr = String.format(
        "{0}/{1}/{2} {3}:{4}"
        , vConDate.getFullYear()
        , LPAD((vConDate.getMonth() + 1).toString(), '0', 2)
        , LPAD(vConDate.getDate().toString(), '0', 2)
        , LPAD(vConDate.getHours().toString(), '0', 2)
        , LPAD(vConDate.getMinutes().toString(), '0', 2)
    );
    return oStr;
}

function sqlToJsDate(sqlDate) {
    //sqlDate in SQL DATETIME format ("yyyy-mm-dd hh:mm:ss.ms")    
    var sqlDateArr1 = sqlDate.split("-");    
    //format of sqlDateArr1[] = ['yyyy','mm','dd hh:mm:ms']    
    var sYear = sqlDateArr1[0];    
    var sMonth = (Number(sqlDateArr1[1]) - 1).toString();    
    var sqlDateArr2 = sqlDateArr1[2].split(" ");    
    //format of sqlDateArr2[] = ['dd', 'hh:mm:ss.ms']    
    var sDay = sqlDateArr2[0];    
    var sqlDateArr3 = sqlDateArr2[1].split(":");    
    //format of sqlDateArr3[] = ['hh','mm','ss.ms']    
    var sHour = sqlDateArr3[0];    
    var sMinute = sqlDateArr3[1];    
    var sqlDateArr4 = sqlDateArr3[2].split(".");    
    //format of sqlDateArr4[] = ['ss','ms']    
    var sSecond = sqlDateArr4[0];    
    var sMillisecond = sqlDateArr4[1];
        
    return new Date(sYear, sMonth, sDay, sHour, sMinute, sSecond, sMillisecond);    
}

function sqlToJsDateYYYY_MM_DD(sqlDate) {
    //sqlDate in SQL DATETIME format ("yyyy-mm-dd hh:mm:ss.ms")    
    var sqlDateArr1 = sqlDate.split(" ");    
    return sqlDateArr1[0];
}

function sqlToJsDateOnlyHour(sqlDate) {
    //sqlDate in SQL DATETIME format ("yyyy-mm-dd hh:mm:ss.ms")    
    var sqlDateArr1 = sqlDate.split("-");
    //format of sqlDateArr1[] = ['yyyy','mm','dd hh:mm:ms']    
    var sYear = sqlDateArr1[0];
    var sMonth = (Number(sqlDateArr1[1]) - 1).toString();
    var sqlDateArr2 = sqlDateArr1[2].split(" ");
    //format of sqlDateArr2[] = ['dd', 'hh:mm:ss.ms']    
    var sDay = sqlDateArr2[0];
    var sqlDateArr3 = sqlDateArr2[1].split(":");
    //format of sqlDateArr3[] = ['hh','mm','ss.ms']    
    var sHour = sqlDateArr3[0];
    var sMinute = sqlDateArr3[1];
    var sqlDateArr4 = sqlDateArr3[2].split(".");
    //format of sqlDateArr4[] = ['ss','ms']    
    var sSecond = sqlDateArr4[0];
    var sMillisecond = sqlDateArr4[1];

    return sHour;
}

function sqlToJsDateOnlyMin(sqlDate) {
    //sqlDate in SQL DATETIME format ("yyyy-mm-dd hh:mm:ss.ms")    
    var sqlDateArr1 = sqlDate.split("-");
    //format of sqlDateArr1[] = ['yyyy','mm','dd hh:mm:ms']    
    var sYear = sqlDateArr1[0];
    var sMonth = (Number(sqlDateArr1[1]) - 1).toString();
    var sqlDateArr2 = sqlDateArr1[2].split(" ");
    //format of sqlDateArr2[] = ['dd', 'hh:mm:ss.ms']    
    var sDay = sqlDateArr2[0];
    var sqlDateArr3 = sqlDateArr2[1].split(":");
    //format of sqlDateArr3[] = ['hh','mm','ss.ms']    
    var sHour = sqlDateArr3[0];
    var sMinute = sqlDateArr3[1];
    var sqlDateArr4 = sqlDateArr3[2].split(".");
    //format of sqlDateArr4[] = ['ss','ms']    
    var sSecond = sqlDateArr4[0];
    var sMillisecond = sqlDateArr4[1];

    return sMinute;
}

//자바스크립트 push 배열의 정렬을 시키기 위해 사용
function comparator(a, b) {
    return a[0] - b[0];
}


var dataset = []; //쿠키에 저장된 서버 코드 값들을 불러와 저장
var arr_list = "";



//상태 코드 시작
var STATE_TYPE_OBJ = new Object();
state_type_list = getSystemCookie("state_type_list");
arr_list = state_type_list.split(',');
for (i = 0; i < arr_list.length; i++) {
    dataset.push([arr_list[i], arr_list[i + 1]]);
    i++;
}
dataset.sort(comparator);//정렬하려고

$.each(dataset, function (i) {
    STATE_TYPE_OBJ[dataset[i][0]] = dataset[i][1]; 
});//each
dataset.length = 0;//초기화
state_type_list = "";//초기화
arr_list = "";//초기화
//상태코드 끝

//화물종류 시작
var CARGO_TYPE_OBJ = new Object();
cargo_type_list = getSystemCookie("cargo_type_list");
arr_list = cargo_type_list.split(',');
for (i = 0; i < arr_list.length; i++) {
    dataset.push([arr_list[i], arr_list[i + 1]]);
    i++;
}
dataset.sort(comparator);//정렬하려고

$.each(dataset, function (i) {
    CARGO_TYPE_OBJ[dataset[i][0]] = dataset[i][1]; 
});//each
dataset.length = 0;//초기화
cargo_type_list = "";//초기화
arr_list = "";//초기화
//화물 종류 끝

//화물차종 시작
var CAR_TYPE_OBJ = new Object();
car_type_list = getSystemCookie("car_type_list");
arr_list = car_type_list.split(',');
for (i = 0; i < arr_list.length; i++) {
    dataset.push([arr_list[i], arr_list[i + 1]]);
    i++;
}
dataset.sort(comparator);//정렬하려고

$.each(dataset, function (i) {
    CAR_TYPE_OBJ[dataset[i][0]] = dataset[i][1];
});//each
dataset.length = 0;//초기화
car_type_list = "";//초기화
arr_list = "";//초기화
//화물차종 끝

//톤에이지 시작
var TONAGE_OBJ = new Object();
tonage_list = getSystemCookie("tonage_list");
arr_list = tonage_list.split(',');
for (i = 0; i < arr_list.length; i++) {
    dataset.push([arr_list[i], arr_list[i + 1]]);
    i++;
}
dataset.sort(comparator);//정렬하려고

$.each(dataset, function (i) {
    TONAGE_OBJ[dataset[i][0]] = dataset[i][1];
});//each
dataset.length = 0;//초기화
tonage_list = "";//초기화
arr_list = "";//초기화
//톤에이지 끝

//차량특징 시작
var CAR_FEATURE_OBJ = new Object();
car_feature_list = getSystemCookie("car_feature_list");
arr_list = car_feature_list.split(',');
for (i = 0; i < arr_list.length; i++) {
    dataset.push([arr_list[i], arr_list[i + 1]]);
    i++;
}
dataset.sort(comparator);//정렬하려고

$.each(dataset, function (i) {
    CAR_FEATURE_OBJ[dataset[i][0]] = dataset[i][1];
});//each
dataset.length = 0;//초기화
car_feature_list = "";//초기화
arr_list = "";//초기화
//차량특징 끝

//운임종류 시작
var PRICE_TYPE_OBJ = new Object();
price_type_list = getSystemCookie("price_type_list");
arr_list = price_type_list.split(',');
for (i = 0; i < arr_list.length; i++) {
    dataset.push([arr_list[i], arr_list[i + 1]]);
    i++;
}
dataset.sort(comparator);//정렬하려고

$.each(dataset, function (i) {
    PRICE_TYPE_OBJ[dataset[i][0]] = dataset[i][1];
});//each
dataset.length = 0;//초기화
price_type_list = "";//초기화
arr_list = "";//초기화
//운임종류 끝

//상차방법 시작
var UP_TYPE_OBJ = new Object();
up_type_list = getSystemCookie("up_type_list");
arr_list = up_type_list.split(',');
for (i = 0; i < arr_list.length; i++) {
    dataset.push([arr_list[i], arr_list[i + 1]]);
    i++;
}
dataset.sort(comparator);//정렬하려고

$.each(dataset, function (i) {
    UP_TYPE_OBJ[dataset[i][0]] = dataset[i][1];
});//each
dataset.length = 0;//초기화
up_type_list = "";//초기화
arr_list = "";//초기화
//상차방법 끝

//하차방법 시작
var DOWN_TYPE_OBJ = new Object();
down_type_list = getSystemCookie("down_type_list");
arr_list = down_type_list.split(',');
for (i = 0; i < arr_list.length; i++) {
    dataset.push([arr_list[i], arr_list[i + 1]]);
    i++;
}
dataset.sort(comparator);//정렬하려고

$.each(dataset, function (i) {
    DOWN_TYPE_OBJ[dataset[i][0]] = dataset[i][1];
});//each
dataset.length = 0;//초기화
down_type_list = "";//초기화
arr_list = "";//초기화
//하차방법 끝



//var STATE_TYPEEnum = ['배차대기', '배차완료', '배차취소', '오더취소', '운행완료', '운행중', '인수승인대기', '인수승인완료', '배차실패'];

//var CARGO_TYPEEnum = ['제조식품', '목재류', '철재류', '화학식품',    '플라스틱류',    '유리제품',    '건축자재류',    '의류',    '생활용품류',    '기계및 장치류',    '기호품', '농수축산물', '가전', '상온기타', '냉장 채소류', '냉장 가공식품류', '냉장 육류', '냉장 어패류', '냉장 유제품', '냉장 육제품',    '냉장 기타', '냉동 농수산 축산물',    '냉동 가공제품',    '냉동기타'];
    
//var CAR_TYPEEnum = ['카고', '탑', '윙바디', '카고/윙바디', '탑/윙바디', '카고축', '호루', '냉동탑', '윙축', '리프트', '윙리프트', '탑리프트', '냉장탑', '초장축', '냉장윙', '지게차', '윙플러스', '플러스', '플러스축'];

//var TONAGEEnum = ['1톤미만', '1톤', '1.25톤', '1.4톤', '2.5톤', '3.5톤', '4.5톤', '5톤', '5톤축', '5톤플러스','5톤플러스축', '8톤', '8.5톤', '9톤', '9.5톤', '10톤', '11톤', '14톤', '14.5톤', '14톤축','15톤', '18톤', '25톤', '25톤초과'];

//var CAR_FEATURE_TYPEEnum = ['냉동', '냉장', '무진동'];

//var PRICE_TYPEEnum = ['인수증', '선불', '착불'];

//var UP_TYPE