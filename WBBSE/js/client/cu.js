$(".alpha").keypress(function (e) { return alpha(e); });
$(".OnlyAlpha").keypress(function (e) { return OnlyAlpha(e); });
$(".checkContactNo").keypress(function (e) { return checkContactNo(e); });
$(".allowCharForEmailId").keypress(function (e) { return allowCharForEmailId(e); });
$(".alphaWithoutSpace").keypress(function (e) { return alphaWithoutSpace(e); });
$("#btnSubmit").on("click", Save);
//function toggleInfo() {
//    $("#divS").css("display", "none");
//    $("#divE").css("display", "none");
//}
function Save() {
    $("input").removeClass("custom-border");
    try {
        //$("#AlertInfo").css("display", "none");
        $('#AlertInfoMsg').text("");

        if ($.trim($("#txtName").val()) == DefaultSetting.EmptyVal) {
            $('#AlertInfo').show();
            $('#AlertInfoMsg').text(ContactUs.Required.Name);
            //$("#txtName").focus();
            $("#txtName").addClass("custom-border");
            $('#content').scrollTop(0);
            return false;
        }
        if (!chkDataFormat(RegexType.OnlyAlpha, $.trim($("#txtName").val()))) {
            $('#AlertInfo').show();
            $('#AlertInfoMsg').text(ContactUs.Validation.InvalidName);
            $("#txtName").focus();
            $('#content').scrollTop(0);
            return false;
        }
        if ($.trim($("#txtEmailId").val()) == DefaultSetting.EmptyVal) {
            $('#AlertInfo').show();
            $('#AlertInfoMsg').text(ContactUs.Required.EmailId);
            $("#txtEmailId").focus();
            $('#content').scrollTop(0);
            return false;
        }
        if ($.trim($("#txtEmailId").val()) != DefaultSetting.EmptyVal) {
            if (!chkDataFormat(RegexType.EmailId, $.trim($("#txtEmailId").val()))) {
                $('#AlertInfo').show();
                $('#AlertInfoMsg').text(GeneralMsg.Validation.InvalidEmailId);
                $("#txtEmailId").focus();
                $('#content').scrollTop(0);
                return false;
            }
        }
        if ($.trim($("#txtMobileNo").val()) == DefaultSetting.EmptyVal) {
            $('#AlertInfo').show();
            $('#AlertInfoMsg').text(ContactUs.Required.MobileNo);
            $("#txtMobileNo").focus();
            $('#content').scrollTop(0);
            return false;
        }
        if ($.trim($("#txtMobileNo").val()) != DefaultSetting.EmptyVal) {
            if (!chkDataFormat(RegexType.MobileNo, $.trim($("#txtMobileNo").val()))) {
                $('#AlertInfo').show();
                $('#AlertInfoMsg').text(GeneralMsg.Validation.InvalidMobileNo);
                $("#txtMobileNo").focus();
                $('#content').scrollTop(0);
                return false;
            }
        }
        if ($.trim($("#txtSubject").val()) == DefaultSetting.EmptyVal) {
            $('#AlertInfo').show();
            $('#AlertInfoMsg').text(ContactUs.Required.Subject);
            $("#txtSubject").focus();
            $('#content').scrollTop(0);
            return false;
        }
        console.log($.trim($("#txtSubject").val()));
        if (!chkDataFormat(RegexType.Alpha, $.trim($("#txtSubject").val()))) {
            $('#AlertInfo').show();
            $('#AlertInfoMsg').text(ContactUs.Validation.InvalidSubject);
            $("#txtSubject").focus();
            $('#content').scrollTop(0);
            return false;
        }
        if ($.trim($("#txtBodyText").val()) == DefaultSetting.EmptyVal) {
            $('#AlertInfo').show();
            $('#AlertInfoMsg').text(ContactUs.Required.Message);
            $("#txtBodyText").focus();
            $('#content').scrollTop(0);
            return false;
        }
        if (!chkDataFormat(RegexType.Alpha, $.trim($("#txtBodyText").val()))) {
            $('#AlertInfo').show();
            $('#AlertInfoMsg').text(ContactUs.Validation.InvalidMessage);
            $("#txtBodyText").focus();
            $('#popupModal').scrollTop(0);
            return false;
        }
        if ($("#txtCaptcha").val().trim() == "") {
            $('#AlertInfo').show();
            $("#AlertInfoMsg").text(Login.Validation.CaptchaRequired);
            $("#txtCaptcha").focus();
            $('#content').scrollTop(0);
            return false;
        }

    }
    catch (err) {
        alert(OperationError());
        console.log(err.message);
        return false;
    }
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

//(function () {
//    'use strict';
//    window.addEventListener('load', function () {
//        // Get the forms we want to add validation styles to
//        var forms = document.getElementsByClassName('needs-validation');
//        // Loop over them and prevent submission
//        var validation = Array.prototype.filter.call(forms, function (form) {
//            form.addEventListener('submit', function (event) {
//                if (form.checkValidity() === false) {
//                    event.preventDefault();
//                    event.stopPropagation();
//                }
//                form.classList.add('was-validated');
//            }, false);
//        });
//    }, false);
//})();
