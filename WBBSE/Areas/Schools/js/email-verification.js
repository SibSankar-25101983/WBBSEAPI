$(".allowCharForEmailId").keypress(function (e) { return allowCharForEmailId(e); });
$(".checkContactNo").keypress(function (e) { return checkContactNo(e); });
$("#txtEmailId").css('text-transform', 'none');
$("#btnEClose").click(function () { $("#divEInfo").hide(); });
$("#btnGetOTPEmailId").on("click", getOTPE);
function getOTPE() {
    if ($.trim($("#txtEmailId").val()) == DefaultSetting.EmptyVal) {
        $("#divEInfo").css('display', '');
        $("#InfoE").text(EmailId_Required());
        return false;
    }
    if (!chkDataFormat(RegexType.EmailId, $.trim($("#txtEmailId").val()))) {
        $("#divEInfo").css('display', '');
        $("#InfoE").text(GeneralMsg.Validation.InvalidEmailId);
        return false;
    }
    if (!chkDuplicateContact($.trim($("#txtEmailId").val()), "", "", "divEInfo", "InfoE")) {
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
    if ($.trim($("#txtEmailId").val()) == DefaultSetting.EmptyVal) {
        $("#divEInfo").css('display', '');
        $("#InfoE").text(EmailId_Required());
        return false;
    }
    if (!chkDataFormat(RegexType.EmailId, $.trim($("#txtEmailId").val()))) {
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
function chkDuplicateContact(e, p, m, md, sp) {
    var flag = 0;
    $.ajax({
        url: "/SchoolProfileVerification/ChkDuplicateContact",
        type: "GET",
        dataType: "json",
        contentType: "application/json",
        data: { 'e': '' + e + '', 'p': '' + p + '', 'm': '' + m + '' },
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
