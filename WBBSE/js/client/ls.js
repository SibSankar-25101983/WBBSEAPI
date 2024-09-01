$("#divInfo").css("display", "none");
$("#lblInfo").text("");
$(".alpha").keypress(function (e) { return alpha(e); });
$(".alphaWithoutSpace").keypress(function (e) { return alphaWithoutSpace(e); });
$(window).keydown(function (e) {
    var evt = e || window.event;
    if (evt.keyCode === 13) {
        $("#btnSubmit").click();
    }
});
setTimeout(function () { $("#txtUserName").val(""); $("#txtPassword").val(""); }, 1000);
try {
    var ah = screen.availHeight;
    var fp = $("#fWebsite")[0].offsetTop;
    var fh = $("#fWebsite").height();

    if ((fp - fh) < ah) {
        $("#fWebsite").addClass("custom-footer");
    }
    else {
        $("#fWebsite").removeClass("custom-footer");
    }
}
catch (err) {
    console.log(err.message);
}
function validate() {
    $("#divInfo").css("display", "none");
    $("#lblInfo").text("");
    if ($("#txtUserName").val().trim() == "") {
        $("#divInfo").css("display", "");
        $("#lblInfo").text(Login.Validation.UserNameRequired);
        $("#txtUserName").focus();
        return false;
    }
    if ($("#txtPassword").val().trim() == "") {
        $("#divInfo").css("display", "");
        $("#lblInfo").text(Login.Validation.PasswordRequired);
        $("#txtPassword").focus();
        return false;
    }
    if ($("#txtCaptcha").val().trim() == "") {
        $("#divInfo").css("display", "");
        $("#lblInfo").text(Login.Validation.CaptchaRequired);
        $("#txtCaptcha").focus();
        return false;
    }
    return true;
}
function refreshCaptcha() {
    $.ajax({
        type: "POST",
        url: '../../Captcha/CaptchaImage',
        success: function (data) {
            $('#imgCaptcha').attr('src', data);
        },
    });
}
