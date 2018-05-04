//for upper jquery version 1.9($.browser method has been removed as of jQuery 1.9.)
//트리 플러그인 때문에 넣었다.. 지금은 안쓴다.
jQuery.browser = {};
(function () {
    jQuery.browser.msie = false;
    jQuery.browser.version = 0;
    if (navigator.userAgent.match(/MSIE ([0-9]+)\./)) {
        jQuery.browser.msie = true;
        jQuery.browser.version = RegExp.$1;
    }
})();

$(function () {
    //snb 접고 펼치기
    $('li > a').click("#snb", function () {
        var menu = $(this).parent('li');
        if (menu.hasClass('collapsible')) {
            menu.children('ul').slideUp('fast');
            menu.removeClass('collapsible').addClass('expandable');
        } else if (menu.hasClass('expandable')) {
            menu.children('ul').slideDown('fast');
            menu.removeClass('expandable').addClass('collapsible');
        }
    });

    //키보드 체크
    $('.fChk').on('keydown', function (e) {
        if (window.event) {
            myKeyCode = event.keyCode;
        } else if (e.which) {
            myKeyCode = e.which;
        }

        if (myKeyCode === 32) {
            var checked = !$(this).children('input').prop('checked');
            $(this).children('input').prop('checked', checked);
            return false;
        }
        return true;
    });

    // 로그인 버튼 엔터키 누를 시 클릭이벤트
    $("#btnLogin").keyup(function (event) {
        if (event.keyCode === 13) {
            $("#btnLogin").click();
        }
    });
});

// layer close
function mLayer_close(target) {
    var findParent = target.parents('.layer:first');
    var findDimmed = $('#dimmed_' + findParent.attr('id'));
    findParent.hide();
}

//layer open
function layerOpen(targetLayer, currentButton) {
    var findLayer = targetLayer,
        propBtnTopPosition = currentButton.offset().top,
        propBtnLeftPosition = currentButton.offset().left;

    findLayer.css({ 'left': propBtnLeftPosition, 'top': propBtnTopPosition + 20 });

    $(targetLayer).show();
}
