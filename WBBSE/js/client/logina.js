window.onbeforeunload = DisableButtons;
$(document).ready(function () {
    $(".alpha").keypress(function (e) { return alpha(e); });
    $(".alphaWithoutSpace").keypress(function (e) { return alphaWithoutSpace(e); });
    $('input').attr('autocomplete', "off");
    $(window).keydown(function (e) {
        var evt = e || window.event;
        if (evt.keyCode === 13) {
            $("#btnSubmit").click();
        }
    });
    setTimeout(function () { $("#txtUserName").val(""); $("#txtPassword").val(""); }, 500);
    refreshCaptcha();
    $('.js-tilt').tilt({
        scale: 1.1
    })
    try {
        var ah = screen.availHeight;
        var fp = $("#fAdmin")[0].offsetTop;
        var fh = $("#fAdmin").height();

        if ((fp - fh + 150) < ah) {
            $("#fAdmin").addClass("custom-footer");
        }
        else {
            $("#fAdmin").removeClass("custom-footer");
        }
    }
    catch (err) {
        console.log(err.message);
    }
});
function validate() {
    $("#divUserName").removeClass('alert-validate');
    $("#divUserPwd").removeClass('alert-validate');
    $("#divCaptcha").removeClass('alert-validate');
    if ($("#txtUserName").val().trim() == "") {
        $("#divUserName").addClass('alert-validate');
        $("#txtUserName").focus();
        return false;
    }
    if ($("#txtPassword").val().trim() == "") {
        $("#divUserPwd").addClass('alert-validate');
        $("#txtPassword").focus();
        return false;
    }
    if ($("#txtCaptcha").val().trim() == "") {
        $("#divCaptcha").addClass('alert-validate');
        $("#txtCaptcha").focus();
        return false;
    }
    return true;
}
function refreshCaptcha() {
    $.ajax({
        type: "POST",
        url: '../../../Captcha/CaptchaImage',
        success: function (data) {
            $('#imgCaptcha').attr('src', data);
        },
    });
}
