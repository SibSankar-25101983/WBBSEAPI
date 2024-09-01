$("#txtRollNo").focusin(function () {
    $("#RollNoMsg").fadeIn(500);
});
$("#txtRollNo").focusout(function () {
    $("#RollNoMsg").fadeOut(100);
});
$("#txtSchoolIndexNo").focusin(function () {
    $("#SchoolIndexNoMsg").fadeIn(500);
});
$("#txtSchoolIndexNo").focusout(function () {
    $("#SchoolIndexNoMsg").fadeOut(100);
});
$("#txtStudentName").focusin(function () {
    $("#StudentNameMsg").fadeIn(500);
});
$("#txtStudentName").focusout(function () {
    $("#StudentNameMsg").fadeOut(100);
});
$("#txtDOB").focusin(function () {
    $("#DOBMsg").fadeIn(500);
});
$("#txtDOB").focusout(function () {
    $("#DOBMsg").fadeOut(100);
});
$("#txtCaptcha").focusin(function () {
    $("#CaptchaMsg").fadeIn(500);
});
$("#txtCaptcha").focusout(function () {
    $("#CaptchaMsg").fadeOut(100);
});
function refreshCaptcha() {
    $.ajax({
        type: "POST",
        url: '../../Captcha/CaptchaImage',
        success: function (data) {
            $('#imgCaptcha').attr('src', data);
        },
    });
}
function Save() {
    try {
        $("input").removeClass("custom-border");
        $("#AlertInfo").css("display", "none");
        $('#AlertInfoMsg').text("");
        if ($.trim($("#txtRollNo").val()) == DefaultSetting.EmptyVal) {
            $('#AlertInfo').show();
            $('#AlertInfoMsg').text(CandidateLogin.Required.RollNo);
            $("#txtRollNo").addClass("custom-border");
            $(window).scrollTop($("#content").offset().top);
            return false;
        }
        if (!chkDataFormat(RegexType.RollNo, $.trim($("#txtRollNo").val()))) {
            $('#AlertInfo').show();
            $('#AlertInfoMsg').text(CandidateLogin.Validation.InvalidRollNo);
            $("#txtRollNo").addClass("custom-border");
            $(window).scrollTop($("#content").offset().top);
            return false;
        }
        if ($.trim($("#txtSchoolIndexNo").val()) == DefaultSetting.EmptyVal) {
            $('#AlertInfo').show();
            $('#AlertInfoMsg').text(CandidateLogin.Required.SchoolIndexNo);
            $("#txtSchoolIndexNo").addClass("custom-border");
            $(window).scrollTop($("#content").offset().top);
            return false;
        }
        if (!chkDataFormat(RegexType.SchoolIndexNo, $.trim($("#txtSchoolIndexNo").val()))) {
            $('#AlertInfo').show();
            $('#AlertInfoMsg').text(CandidateLogin.Validation.InvalidSchoolIndexNo);
            $("#txtSchoolIndexNo").addClass("custom-border");
            $(window).scrollTop($("#content").offset().top);
            return false;
        }
        if ($.trim($("#txtStudentName").val()) == DefaultSetting.EmptyVal) {
            $('#AlertInfo').show();
            $('#AlertInfoMsg').text(CandidateLogin.Required.Name);
            $("#txtStudentName").addClass("custom-border");
            $(window).scrollTop($("#content").offset().top);
            return false;
        }
        if (!chkDataFormat(RegexType.OnlyAlpha, $.trim($("#txtStudentName").val()))) {
            $('#AlertInfo').show();
            $('#AlertInfoMsg').text(CandidateLogin.Validation.InvalidName);
            $("#txtStudentName").addClass("custom-border");
            $(window).scrollTop($("#content").offset().top);
            return false;
        }
        if ($.trim($("#txtDOB").val()) == DefaultSetting.EmptyVal) {
            $('#AlertInfo').show();
            $('#AlertInfoMsg').text(CandidateLogin.Required.DOB);
            $("#txtDOB").addClass("custom-border");
            $(window).scrollTop($("#content").offset().top);
            return false;
        }
        if (!chkDataFormat(RegexType.CustomDOB, $.trim($("#txtDOB").val()))) {
            $('#AlertInfo').show();
            $('#AlertInfoMsg').text(CandidateLogin.Validation.InvalidDOB);
            $("#txtDOB").addClass("custom-border");
            $(window).scrollTop($("#content").offset().top);
            return false;
        }
        if ($.trim($("#txtCaptcha").val()) == DefaultSetting.EmptyVal) {
            $('#AlertInfo').show();
            $('#AlertInfoMsg').text(CandidateLogin.Required.Captcha);
            $("#txtCaptcha").addClass("custom-border");
            $(window).scrollTop($("#content").offset().top);
            return false;
        }
        if (!chkDataFormat(RegexType.Captcha, $.trim($("#txtCaptcha").val()))) {
            $('#AlertInfo').show();
            $('#AlertInfoMsg').text(CandidateLogin.Validation.InvalidCaptcha);
            $("#txtCaptcha").addClass("custom-border");
            $(window).scrollTop($("#content").offset().top);
            return false;
        }
        var x = confirm(Msg_Confirm());
        if (x) {
            ShowLoaderNew();
        }
        return x;
    }
    catch (err) {
        console.log(err);
        return false;
    }
}
$(document).ready(function () {
    $("#RollNoMsg").text(CandidateLogin.Info.RollNoMsg);
    $("#RollNoMsg").css("display", "none");
    $("#SchoolIndexNoMsg").text(CandidateLogin.Info.IndexNoMsg);
    $("#SchoolIndexNoMsg").css("display", "none");
    $("#StudentNameMsg").text(CandidateLogin.Info.NameMsg);
    $("#StudentNameMsg").css("display", "none");
    $("#DOBMsg").text(CandidateLogin.Info.DateOfBirthMsg);
    $("#DOBMsg").css("display", "none");
    $("#txtDOB").datepicker({ dateFormat: "ddmmy", changeMonth: true, changeYear: true, yearRange: "-30:+00" });
    $("#CaptchaMsg").text(CandidateLogin.Info.CaptchaMsg);
    $("#CaptchaMsg").css("display", "none");
    $(".OnlyAlpha").keypress(function (e) { return OnlyAlpha(e); });
    $(".checkContactNo").keypress(function (e) { return checkContactNo(e); });
    $(".alphaWithoutSpace").keypress(function (e) { return alphaWithoutSpace(e); });
    $("#btnSubmit").on("click", Save);
    $("#AlertInfo").css("display", "none");
    $(window).keydown(function (e) {
        var evt = e || window.event;
        if (evt.keyCode === 13) {
            $("#btnSubmit").click();
        }
    });
    refreshCaptcha();
});
