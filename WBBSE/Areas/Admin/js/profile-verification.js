$('[data-toggle="popover"]').popover({
    title: 'Password Policy'
    , content: Password_Policy()
    , html: true
    , placement: "right"
});
$("#OldPassword").keypress(function (e) {
    return allowCharsForPassword(e);
});
$("#NewPassword").keypress(function (e) {
    return allowCharsForPassword(e);
});
$("#btnSavePassword").on("click", saveP);
$("#btnGetOTPEmailId").on("click", getOTPE);
$("#btnGetOTPContactNo").on("click", getOTPC);
$("#EMailId").css('text-transform', 'none');
$(".allowCharForEmailId").keypress(function (e) { return allowCharForEmailId(e); });
$(".checkContactNo").keypress(function (e) { return checkContactNo(e); });
$("#btnEIClose").click(function () { $("#divEIClose").hide(); });
$("#btnSIClose").click(function () { $("#divSIClose").hide(); });
$("#btnPClose").click(function () { $("#divPInfo").hide(); });
$("#btnEClose").click(function () { $("#divEInfo").hide(); });
$("#btnCClose").click(function () { $("#divCInfo").hide(); });
//set body height
var height = screen.availHeight;
//var navTopHeight = $("#navTop").outerHeight();
$("#divMainBody").css("min-height", height - height / 4);
function getTime() {
    try {
        $.ajax({
            type: "GET",
            url: "../../../../Services/WSCommon.asmx/GetTime",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            success: function (response) {
                data = response.d;
                initializeClock();
            },
            error: function (msg) {
                console.log(msg.responseText);
            }
        });
    }
    catch (err) {
        console.log(err);
    }
}
function initializeClock() {
    var timeAry = data.split(':');
    hours = timeAry[0];
    minutes = timeAry[1];
    seconds = timeAry[2];
    tick();
    setInterval(tick, 1000);
}
function tick() {
    seconds++;
    if (seconds > 59) {
        minutes++;
        seconds = 0;
    }
    if (minutes > 59) {
        hours++;
        minutes = 0;
    }
    if (hours > 23) {
        hours = 0;
    }
    $("#time").html(" <strong><u>" + ("0" + hours).slice(-2) + ":" + ("0" + minutes).slice(-2) + ":" + ("0" + seconds).slice(-2) + "</u></strong>");
}
function chkDuplicateContact(e, m, md, sp) {
    var flag = 0;
    $.ajax({
        url: "/AdminProfileVerification/ChkDuplicateContact",
        type: "GET",
        dataType: "json",
        contentType: "application/json",
        data: { 'e': '' + e + '', 'm': '' + m + '' },
        async: false,
        success: function (data) {
            if (data.err == DefaultSetting.DefaultErrVal) {
                $("#" + md).css('display', '');
                $("#" + sp).text(data.errDesc.replace("<br/>", ""));
                flag = 1;
                return false;
            }
        },
        error: function (error) {
            console.log(error.responseText);
            alert(OperationError());
            return false;
        }
    });
    if (flag == 0) {
        return true;
    }
    else {
        return false;
    }
}
function saveP() {
    $("#divPInfo").css('display', 'none');
    if ($.trim($("#OldPassword").val()) == "") {
        $("#divPInfo").css('display', '');
        $("#InfoP").text(Old_Password_Required());
        return false;
    }
    if ($.trim($("#NewPassword").val()) == "") {
        $("#divPInfo").css('display', '');
        $("#InfoP").text(New_Password_Required());
        return false;
    }
    if ($.trim($("#ConfirmPassword").val()) == "") {
        $("#divPInfo").css('display', '');
        $("#InfoP").text(Confirm_Password_Required());
        return false;
    }
    if ($.trim($("#OldPassword").val()) == $.trim($("#NewPassword").val())) {
        $("#divPInfo").css('display', '');
        $("#InfoP").text(Old_New_Password());
        return false;
    }
    if ($.trim($("#NewPassword").val()) != $.trim($("#ConfirmPassword").val())) {
        $("#divPInfo").css('display', '');
        $("#InfoP").text(Password_Mismatch());
        return false;
    }
    if (!checkPassword($.trim($("#NewPassword").val()))) {
        $("#divPInfo").css('display', '');
        $("#InfoP").text(Password_Policy_Required());
        return false;
    }
    var x = confirm(Msg_Confirm());
    if (x) {
        $("#hp").val(ProfileVerificationType.PasswordChange);
        document.getElementById("OldPassword").value = (Sha256.hash(document.getElementById("OldPassword").value)).toUpperCase();
        document.getElementById("NewPassword").value = (Sha256.hash(document.getElementById("NewPassword").value)).toUpperCase();
        document.getElementById("ConfirmPassword").value = (Sha256.hash(document.getElementById("ConfirmPassword").value)).toUpperCase();
        ShowLoader();
        return true;
    }
    else {
        return false;
    }
}
function getOTPE() {
    if ($.trim($("#EMailId").val()) == DefaultSetting.EmptyVal) {
        $("#divEInfo").css('display', '');
        $("#InfoE").text(EmailId_Required());
        return false;
    }
    if (!chkDataFormat(RegexType.EmailId, $.trim($("#EMailId").val()))) {
        $("#divEInfo").css('display', '');
        $("#InfoE").text(GeneralMsg.Validation.InvalidEmailId);
        return false;
    }
    if (!chkDuplicateContact($.trim($("#EMailId").val()), "", "divEInfo", "InfoE")) {
        return false;
    }
    var x = confirm(Msg_Confirm());
    if (x) {
        $("#he").val(ProfileVerificationType.EmailOTPSend);
        ShowLoader();
        return true;
    }
    else {
        return false;
    }
}
function resendOTPE() {
    $("#divEInfo").css('display', 'none');
    if ($.trim($("#EMailId").val()) == DefaultSetting.EmptyVal) {
        $("#divEInfo").css('display', '');
        $("#InfoE").text(EmailId_Required());
        return false;
    }
    if (!chkDataFormat(RegexType.EmailId, $.trim($("#EMailId").val()))) {
        $("#divEInfo").css('display', '');
        $("#InfoE").text(GeneralMsg.Validation.InvalidEmailId);
        return false;
    }
    var x = confirm(Msg_Confirm());
    if (x) {
        $("#he").val(ProfileVerificationType.EmailOTPResend);
        ShowLoader();
        return true;
    }
    else {
        return false;
    }
}
function saveOTPE() {
    $("#divEInfo").css('display', 'none');
    if ($.trim($("#txtOTPEmailId").val()) == DefaultSetting.EmptyVal) {
        $("#divEInfo").css('display', '');
        $("#InfoE").text(OTP_Required());
        return false;
    }
    if (!chkDataFormat(RegexType.Numeric, $.trim($("#txtOTPEmailId").val()))) {
        $("#divEInfo").css('display', '');
        $("#InfoE").text(GeneralMsg.Validation.InvalidOTP);
        return false;
    }
    if ($.trim($("#txtOTPEmailId").val()).length != 6) {
        $("#divEInfo").css('display', '');
        $("#InfoE").text(GeneralMsg.Validation.InvalidOTP);
        return false;
    }
    var x = confirm(Msg_Confirm());
    if (x) {
        $("#he").val(ProfileVerificationType.EmailConfirmation);
        ShowLoader();
        return true;
    }
    else {
        return false;
    }
}
function getOTPC() {
    $("#divPInfo").css('display', 'none');
    if ($.trim($("#ContactNo").val()) == DefaultSetting.EmptyVal) {
        $("#divPInfo").css('display', '');
        $("#InfoP").text(ContactNo_Required());
        return false;
    }
    if (!chkDataFormat(RegexType.MobileNo, $.trim($("#ContactNo").val()))) {
        $("#divPInfo").css('display', '');
        $("#InfoP").text(GeneralMsg.Validation.InvalidMobileNo);
        return false;
    }
    if (!chkDuplicateContact("", $.trim($("#ContactNo").val()), "divPInfo", "InfoP")) {
        return false;
    }
    var x = confirm(Msg_Confirm());
    if (x) {
        $("#hc").val(ProfileVerificationType.ContactNoOTPSend);
        return true;
    }
    else {
        return false;
    }
}
function resendOTPC() {
    $("#divPInfo").css('display', 'none');
    if ($.trim($("#ContactNo").val()) == "") {
        $("#divPInfo").css('display', '');
        $("#InfoP").text(ContactNo_Required());
        return false;
    }
    if (!chkDataFormat(RegexType.MobileNo, $.trim($("#ContactNo").val()))) {
        $("#divPInfo").css('display', '');
        $("#InfoP").text(GeneralMsg.Validation.InvalidMobileNo);
        return false;
    }
    var x = confirm(Msg_Confirm());
    if (x) {
        $("#hc").val(ProfileVerificationType.ContactNoOTPResend);
        return true;
    }
    else {
        return false;
    }
}
function saveOTPC() {
    $("#divCInfo").css('display', 'none');
    if ($.trim($("#txtOTPContactNo").val()) == DefaultSetting.EmptyVal) {
        $("#divCInfo").css('display', '');
        $("#InfoC").text(OTP_Required());
        return false;
    }
    if (!chkDataFormat(RegexType.Numeric, $.trim($("#txtOTPContactNo").val()))) {
        $("#divCInfo").css('display', '');
        $("#InfoC").text(GeneralMsg.Validation.InvalidOTP);
        return false;
    }
    if ($.trim($("#txtOTPContactNo").val()).length != 6) {
        $("#divCInfo").css('display', '');
        $("#InfoC").text(GeneralMsg.Validation.InvalidOTP);
        return false;
    }
    var x = confirm(Msg_Confirm());
    if (x) {
        $("#hc").val(ProfileVerificationType.ContactNoConfirmation);
        return true;
    }
    else {
        return false;
    }
}
window.onbeforeunload = DisableButtons;
