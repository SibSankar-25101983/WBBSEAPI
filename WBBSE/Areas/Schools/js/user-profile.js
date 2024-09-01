﻿function GetSalutationList() {
    $("#ddlSalutation").empty();
    $("#ddlSalutation").append(new Option("Salutation", DefaultSetting.DefaultValEnc));
    try {
        $.ajax({
            url: "/SchoolUserProfile/GetSalutationList",
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
        document.getElementById("OldPassword").value = (Sha256.hash(document.getElementById("OldPassword").value)).toUpperCase();
        document.getElementById("NewPassword").value = (Sha256.hash(document.getElementById("NewPassword").value)).toUpperCase();
        document.getElementById("ConfirmPassword").value = (Sha256.hash(document.getElementById("ConfirmPassword").value)).toUpperCase();
        $("#divPassword").modal("hide");
        ShowLoader();
        return true;
    }
    else {
        return false;
    }
}
function saveEditProfile() {
    $("#divEditProfileInfo").css('display', 'none');
    if ($.trim($("#txtFirstName").val()) == "") {
        $("#divEditProfileInfo").css('display', '');
        $("#InfoEditProfile").text(FirstName_Required());
        return false;
    }
    if ($.trim($("#txtLastName").val()) == "") {
        $("#divEditProfileInfo").css('display', '');
        $("#InfoEditProfile").text(LastName_Required());
        return false;
    }
    if ($.trim($("#txtMobileNo").val()) == "") {
        $("#divEditProfileInfo").css('display', '');
        $("#InfoEditProfile").text(ContactNo_Required());
        return false;
    }
    if ($.trim($("#txtEmailId").val()) == "") {
        $("#divEditProfileInfo").css('display', '');
        $("#InfoEditProfile").text(EmailId_Required());
        return false;
    }
    if ($.trim($("#txtMobileNo").val()) != DefaultSetting.EmptyVal) {
        if (!chkDataFormat(RegexType.MobileNo, $.trim($("#txtMobileNo").val()))) {
            $("#divEditProfileInfo").css('display', '');
            $("#InfoEditProfile").text(GeneralMsg.Validation.InvalidMobileNo);
            $("#txtMobileNo").focus();
            $('#popupModal').scrollTop(0);
            return false;
        }
    }
    if ($.trim($("#txtEmailId").val()) != DefaultSetting.EmptyVal) {
        if (!chkDataFormat(RegexType.EmailId, $.trim($("#txtEmailId").val()))) {
            $("#divEditProfileInfo").css('display', '');
            $("#InfoEditProfile").text(GeneralMsg.Validation.InvalidEmailId);
            $("#txtEmailId").focus();
            $('#popupModal').scrollTop(0);
            return false;
        }
    }
    var x = confirm(Msg_Confirm());
    if (x) {
        $("#divEditProfile").modal("hide");
        ShowLoader();
    }
    return x;
}
$(document).ready(function () {
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
    $("#btnPClose").click(function () { $("#divPInfo").hide(); });
    $("#btnEditProfileClose").click(function () { $("#divEditProfileInfo").hide(); });
    $("#btnSavePassword").on("click", saveP);
    $("#btnSaveEditProfile").on("click", saveEditProfile);
    $("#txtEmailId").css("text-transform", "none");
    $(".OnlyAlpha").keypress(function (e) { return OnlyAlpha(e); });
    $(".checkContactNo").keypress(function (e) { return checkContactNo(e); });
    $(".allowCharForEmailId").keypress(function (e) { return allowCharForEmailId(e); });
    GetSalutationList();
    HideLoader();
});
window.onbeforeunload = DisableButtons;