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
$(document).ready(function () {
    //set body height
    var height = screen.availHeight;
    //var navTopHeight = $("#navTop").outerHeight();
    $("#divMainBody").css("min-height", height - height / 4);
    $("#divProfile input").css('text-transform', 'uppercase');
    $("#divProfile textarea").css('text-transform', 'uppercase');
    $('[data-toggle="popover"]').popover({
        title: 'Password Policy'
        , content: Password_Policy()
        , html: true
        , placement: "right"
    });
    $("#txtEmailId").css("text-transform", "none");
    $("#btnSavePassword").on("click", saveP);
    $("#btnGetOTPEmailId").on("click", getOTPE);
    $("#btnGetOTPContactNo").on("click", getOTPC);
    $("#EMailId").css('text-transform', 'none');
    $("#btnEIClose").click(function () { $("#divEIClose").hide(); });
    $("#btnSIClose").click(function () { $("#divSIClose").hide(); });
    $("#btnPClose").click(function () { $("#divPInfo").hide(); });
    $("#btnEClose").click(function () { $("#divEInfo").hide(); });
    $("#btnCClose").click(function () { $("#divCInfo").hide(); });

    try{
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

    }
    GetSalutationList();
    HideLoader();
});
$("#OldPassword").keypress(function (e) {
    return allowCharsForPassword(e);
});
$("#NewPassword").keypress(function (e) {
    return allowCharsForPassword(e);
});
$("#EMailId").keypress(function (e) {
    return allowCharForEmailId(e);
});
$(".checkPhoneNo").keypress(function (e) {
    return checkPhoneNo(e);
});
$("#ContactNo").keypress(function (e) {
    var txtLen = $("#ContactNo").val().length;
    if (txtLen > 9) {
        return false;
    }
    else {
        return checkContactNo(e);
    }
});
$("#txtOTPEmailId").keypress(function (e) {
    var txtLen = $("#txtOTPEmailId").val().length;
    if (txtLen > 5) {
        return false;
    }
    else {
        return checkContactNo(e);
    }
});
$("#txtOTPContactNo").keypress(function (e) {
    var txtLen = $("#txtOTPContactNo").val().length;
    if (txtLen > 5) {
        return false;
    }
    else {
        return checkContactNo(e);
    }
});
$(".OnlyAlpha").keypress(function (e) { return OnlyAlpha(e); });
function GetSalutationList() {
    $("#ddlSalutation").empty();
    $("#ddlSalutation").append(new Option("Select Salutation", DefaultSetting.DefaultValEnc));
    try {
        $.ajax({
            url: "/PreSchoolProfileVerification/GetSalutationList",
            type: "GET",
            dataType: "json",
            contentType: "application/json",
            async: true,
            success: function (data) {
                $(data).each(function (index, item) {
                    var opt = new Option(item.SalutationName, item.SalutationId);
                    $("#ddlSalutation").append(opt);
                });
                $("#ddlSalutation").val($("#S").val());
            },
            error: function (error) {
                console.log(error.responseText);
            }
        });
    }
    catch (err) {
        console.log(err.message);
    }
}
function chkDuplicateContact(e, p, m, md, sp) {
    var flag = 0;
    $.ajax({
        url: "/PreSchoolProfileVerification/ChkDuplicateContact",
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
function saveP() {
    $("#divPInfo").css('display', 'none');
    if ($("#ddlSalutation").val() == DefaultSetting.DefaultValEnc) {

        $("#divPInfo").css('display', '');
        $("#InfoP").text(User.Required.Salutation);
        return false;
    }
    if ($.trim($("#txtFirstName").val()) == DefaultSetting.EmptyVal) {
        $("#divPInfo").css('display', '');
        $("#InfoP").text(User.Required.FirstName);
        return false;
    }
    if (!chkDataFormat(RegexType.OnlyAlpha, $.trim($("#txtFirstName").val()))) {
        $("#divPInfo").css('display', '');
        $("#InfoP").text(User.Validation.InvalidFirstName);
        return false;
    }
    if ($.trim($("#txtLastName").val()) == DefaultSetting.EmptyVal) {
        $("#divPInfo").css('display', '');
        $("#InfoP").text(User.Required.LastName);
        return false;
    }
    if (!chkDataFormat(RegexType.OnlyAlpha, $.trim($("#txtLastName").val()))) {
        $("#divPInfo").css('display', '');
        $("#InfoP").text(User.Validation.InvalidLastName);
        return false;
    }
    if ($.trim($("#txtMiddleName").val()) != DefaultSetting.EmptyVal) {
        if (!chkDataFormat(RegexType.OnlyAlpha, $.trim($("#txtMiddleName").val()))) {
            $("#divPInfo").css('display', '');
            $("#InfoP").text(User.Validation.InvalidMiddleName);
            return false;
        }
    }
    if ($.trim($("#txtEmailId").val()) == DefaultSetting.EmptyVal) {
        $("#divPInfo").css('display', '');
        $("#InfoP").text(User.Required.EmailId);
        return false;
    }
    if (!chkDataFormat(RegexType.EmailId, $.trim($("#txtEmailId").val()))) {
        $("#divPInfo").css('display', '');
        $("#InfoP").text(GeneralMsg.Validation.InvalidEmailId);
        return false;
    }
    if ($.trim($("#txtContactNo").val()) == DefaultSetting.EmptyVal) {
        $("#divPInfo").css('display', '');
        $("#InfoP").text(User.Required.ContactNo);
        return false;
    }
    if (!chkDataFormat(RegexType.MobileNo, $.trim($("#txtContactNo").val()))) {
        $("#divPInfo").css('display', '');
        $("#InfoP").text(GeneralMsg.Validation.InvalidMobileNo);
        return false;
    }
    if ($.trim($("#txtStdCode").val()) != "") {
        if (!chkDataFormat(RegexType.StdCode, $.trim($("#txtStdCode").val()))) {
            $("#divPInfo").css('display', '');
            $("#InfoP").text(GeneralMsg.Validation.InvalidSTDCode);
            return false;
        }
    }
    if ($.trim($("#txtPhoneNo").val()) != "") {
        if (!chkDataFormat(RegexType.Numeric, $.trim($("#txtPhoneNo").val()))) {
            $("#divPInfo").css('display', '');
            $("#InfoP").text(GeneralMsg.Validation.InvalidPhoneNo);
            return false;
        }
    }
    if ($.trim($("#txtStdCode").val()) != DefaultSetting.EmptyVal && $.trim($("#txtPhoneNo").val()) == DefaultSetting.EmptyVal) {
        $("#divPInfo").css('display', '');
        $("#InfoP").text(School.Required.PhoneNoSTDCode);
        return false;
    }
    if ($.trim($("#txtPhoneNo").val()) != DefaultSetting.EmptyVal && $.trim($("#txtStdCode").val()) == DefaultSetting.EmptyVal) {
        $("#divPInfo").css('display', '');
        $("#InfoP").text(School.Required.STDCodePhoneNo);
        return false;
    }
    if ($.trim($("#OldPassword").val()) == DefaultSetting.EmptyVal) {
        $("#divPInfo").css('display', '');
        $("#InfoP").text(Old_Password_Required());
        return false;
    }
    if ($.trim($("#NewPassword").val()) == DefaultSetting.EmptyVal) {
        $("#divPInfo").css('display', '');
        $("#InfoP").text(New_Password_Required());
        return false;
    }
    if ($.trim($("#ConfirmPassword").val()) == DefaultSetting.EmptyVal) {
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
    if (!chkDuplicateContact($.trim($("#txtEmailId").val()), $.trim($("#txtPhoneNo").val()), $.trim($("#txtContactNo").val()), "divPInfo", "InfoP")) {
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
    $("#divEInfo").css('display', 'none');
    if ($.trim($("#EMailId").val()) == "") {
        $("#divEInfo").css('display', '');
        $("#InfoE").text(EmailId_Required());
        return false;
    }
    var x = confirm(Msg_Confirm());
    if (x) {
        $("#he").val(ProfileVerificationType.EmailOTPSend);
        return true;
    }
    else {
        return false;
    }
}
function resendOTPE() {
    $("#divEInfo").css('display', 'none');
    if ($.trim($("#EMailId").val()) == "") {
        $("#divEInfo").css('display', '');
        $("#InfoE").text(EmailId_Required());
        return false;
    }
    var x = confirm(Msg_Confirm());
    if (x) {
        $("#he").val(ProfileVerificationType.EmailOTPResend);
        return true;
    }
    else {
        return false;
    }
}
function saveOTPE() {
    $("#divEInfo").css('display', 'none');
    if ($.trim($("#txtOTPEmailId").val()) == "") {
        $("#divEInfo").css('display', '');
        $("#InfoE").text(OTP_Required());
        return false;
    }
    if ($.trim($("#txtOTPEmailId").val()).length != 6) {
        $("#divEInfo").css('display', '');
        $("#InfoE").text(Invalid_OTP());
        return false;
    }
    var x = confirm(Msg_Confirm());
    if (x) {
        $("#he").val(ProfileVerificationType.EmailConfirmation);
        return true;
    }
    else {
        return false;
    }
}
function getOTPC() {
    $("#divPInfo").css('display', 'none');
    if ($.trim($("#ContactNo").val()) == "") {
        $("#divPInfo").css('display', '');
        $("#InfoP").text(ContactNo_Required());
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
    if ($.trim($("#txtOTPContactNo").val()) == "") {
        $("#divCInfo").css('display', '');
        $("#InfoC").text(OTP_Required());
        return false;
    }
    if ($.trim($("#txtOTPContactNo").val()).length != 6) {
        $("#divCInfo").css('display', '');
        $("#InfoC").text(Invalid_OTP());
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
