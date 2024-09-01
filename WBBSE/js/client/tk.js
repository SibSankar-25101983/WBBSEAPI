$(document).ready(function () {
    HideLoader();
});
$("#btnRequest").click(function () {
    var flag = 0;
    $.ajax({
        url: "/Web/ChkRequestPermision",
        type: "GET",
        dataType: "json",
        contentType: "application/json",
        async: false,
        success: function (data) {
            if (data.permission == DefaultSetting.DefaultValN) {
                flag = 1;
                alert(MAC.Validation.DuplicateAuthorizationRequest);
            }
        },
        error: function (error) {
            console.log(error.responseText);
        }
    });
    if (flag == 0) {
        var x = confirm(Msg_Confirm());
        if (x) {
            ShowLoader();
        }
        return x;
    }
    else {
        return false;
    }
});
