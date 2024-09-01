$("#txtEmailId").css('text-transform', 'none');
$("#ddlDesignation").select2({ width: 'resolve', theme: "classic" });
$(".alpha").keypress(function (e) { return alpha(e); });
$(".OnlyAlpha").keypress(function (e) { return OnlyAlpha(e); });
$(".checkContactNo").keypress(function (e) { return checkContactNo(e); });
$(".allowCharForEmailId").keypress(function (e) { return allowCharForEmailId(e); });
$("#btnSearch").on("click", Search);
$("#btnAdd").on("click", Add);
$("#btnSave").on("click", Save);
function ClearData() {
    $('#divPopupBody input').val('');
    $('#divPopupBody select').val(DefaultSetting.DefaultValEnc);
    $("#txtUserType").text("");
    $("#txtGroupName").text("");
    $("#txtUserTypeName").text("");
}
function ClearViewData() {
    $("#tblViewData").empty();
}
function GetSalutationList(d) {
    $("#ddlSalutation").empty();
    $("#ddlSalutation").append(new Option("Salutation", DefaultSetting.DefaultValEnc));
    try {
        $.ajax({
            url: "/AdminUser/GetSalutationList",
            type: "GET",
            dataType: "json",
            contentType: "application/json",
            async: true,
            success: function (data) {
                $(data).each(function (index, item) {
                    var opt = new Option(item.SalutationName, item.SalutationId);
                    $("#ddlSalutation").append(opt);
                });
                $("#ddlSalutation").val(d);
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
function GetDesignationList(d, a) {
    $("#ddlDesignation").empty();
    $("#ddlDesignation").append(new Option("Select Designation", DefaultSetting.DefaultValEnc));
    try {
        $.ajax({
            url: "/AdminUser/GetDesignationList",
            type: "GET",
            dataType: "json",
            contentType: "application/json",
            async: true,
            success: function (data) {
                $(data).each(function (index, item) {
                    var opt = new Option(item.Designation, item.DesignationId);
                    $("#ddlDesignation").append(opt);
                });
                $("#ddlDesignation").val(d);
                if (a == DefaultSetting.DefaultValN) {
                    $("#ddlDesignation").prop("disabled", true);
                }
                else {
                    $("#ddlDesignation").prop("disabled", false);
                }
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
function Add() {
    //nothing to do for now (11-02-2020)
}
$('#chkActiveYN').click(function () {
    if ($(this).prop("checked")) {
        $(this).val(DefaultSetting.DefaultValY);
    }
    else {
        $(this).val(DefaultSetting.DefaultValN);
    }
});
function Save() {
    var flag = 0;
    try {
        $("#ddlDesignation").prop("disabled", false);

        if ($("#e").val() != setET(3)) {
            $('#AlertInfoMsg').text("");
            if ($("#ddlSalutation").val() == DefaultSetting.DefaultValEnc) {
                $('#AlertInfo').show();
                $('#AlertInfoMsg').text(User.Required.Salutation);
                $("#ddlSalutation").focus();
                $('#popupModal').scrollTop(0);
                return false;
            }
            if ($.trim($("#txtFirstName").val()) == DefaultSetting.EmptyVal) {
                $('#AlertInfo').show();
                $('#AlertInfoMsg').text(User.Required.FirstName);
                $("#txtFirstName").focus();
                $('#popupModal').scrollTop(0);
                return false;
            }
            if ($.trim($("#txtFirstName").val()) != DefaultSetting.EmptyVal) {
                if (!chkDataFormat(RegexType.OnlyAlpha, $.trim($("#txtFirstName").val()))) {
                    $('#AlertInfo').show();
                    $('#AlertInfoMsg').text(User.Validation.InvalidFirstName);
                    $("#txtFirstName").focus();
                    $('#popupModal').scrollTop(0);
                    return false;
                }
            }
            if ($.trim($("#txtMiddleName").val()) != DefaultSetting.EmptyVal) {
                if (!chkDataFormat(RegexType.OnlyAlpha, $.trim($("#txtMiddleName").val()))) {
                    $('#AlertInfo').show();
                    $('#AlertInfoMsg').text(User.Validation.InvalidMiddleName);
                    $("#txtMiddleName").focus();
                    $('#popupModal').scrollTop(0);
                    return false;
                }
            }
            if ($.trim($("#txtLastName").val()) == DefaultSetting.EmptyVal) {
                $('#AlertInfo').show();
                $('#AlertInfoMsg').text(User.Required.LastName);
                $("#txtLastName").focus();
                $('#popupModal').scrollTop(0);
                return false;
            }
            if ($.trim($("#txtLastName").val()) != DefaultSetting.EmptyVal) {
                if (!chkDataFormat(RegexType.OnlyAlpha, $.trim($("#txtLastName").val()))) {
                    $('#AlertInfo').show();
                    $('#AlertInfoMsg').text(User.Validation.InvalidLastName);
                    $("#txtLastName").focus();
                    $('#popupModal').scrollTop(0);
                    return false;
                }
            }
            if ($("#ddlDesignation").val() == DefaultSetting.DefaultValEnc) {
                $('#AlertInfo').show();
                $('#AlertInfoMsg').text(User.Required.Designation);
                $("#ddlDesignation").focus();
                $('#popupModal').scrollTop(0);
                return false;
            }
            if ($.trim($("#txtEmailId").val()) != DefaultSetting.EmptyVal) {
                if (!chkDataFormat(RegexType.EmailId, $.trim($("#txtEmailId").val()))) {
                    $('#AlertInfo').show();
                    $('#AlertInfoMsg').text(GeneralMsg.Validation.InvalidEmailId);
                    $("#txtEmailId").focus();
                    $('#popupModal').scrollTop(0);
                    return false;
                }
            }

            if ($.trim($("#txtMobileNo").val()) != DefaultSetting.EmptyVal) {
                if (!chkDataFormat(RegexType.MobileNo, $.trim($("#txtMobileNo").val()))) {
                    $('#AlertInfo').show();
                    $('#AlertInfoMsg').text(GeneralMsg.Validation.InvalidMobileNo);
                    $("#txtMobileNo").focus();
                    $('#popupModal').scrollTop(0);
                    return false;
                }
            }
            if ($.trim($("#txtPinCode").val()) != DefaultSetting.EmptyVal) {
                if (!chkDataFormat(RegexType.PinCode, $.trim($("#txtPinCode").val()))) {
                    $('#AlertInfo').show();
                    $('#AlertInfoMsg').text(School.Validation.InvalidPinCode);
                    $("#txtPinCode").focus();
                    $('#popupModal').scrollTop(0);
                    return false;
                }
            }
            //duplicate contact details check
            $.ajax({
                url: "/AdminUser/ChkDuplicateContact",
                type: "GET",
                dataType: "json",
                contentType: "application/json",
                data: { 'e': '' + $.trim($("#txtEmailId").val()) + '', 'm': '' + $.trim($("#txtMobileNo").val()) + '', 'u': '' + $.trim($("#U").val()) + '', 'et': '' + $("#e").val() + '' },
                async: false,
                success: function (data) {
                    if (data.err == DefaultSetting.DefaultErrVal) {
                        $('#AlertInfo').show();
                        $('#AlertInfoMsg').html(data.errDesc);
                        $('#popupModal').scrollTop(0);
                        flag = 1;
                        return false;
                    }
                },
                error: function (error) {
                    console.log(error.responseText);
                    alert(OperationError());
                }
            });

            if (flag == 0) {
                var x = confirm(Msg_Confirm());
                if (x) {
                    $("#popupModal").modal("hide");
                    ShowLoader();
                }
                return x;
            }
            else {
                return false;
            }
        }
        else {
            var x = confirm(Msg_Confirm());
            if (x) {
                $("#popupModal").modal("hide");
                ShowLoader();
            }
            return x;
        }
    }
    catch (err) {
        alert(OperationError());
        console.log(err.message);
        return false;
    }
}
function View(e) {
    ClearViewData();
    try {
        var u = e.data.record.UserId;

        $.ajax({
            url: "/AdminUser/GetUserView",
            type: "GET",
            dataType: "json",
            data: { 'u': '' + u + '' },
            contentType: "application/json",
            async: true,
            success: function (data) {
                $("#tblViewData").append(data.View);
                $("#popupModalView").modal("show");
            },
            error: function (error) {
                alert(OperationError());
                console.log(error.responseText);
            }
        });
    }
    catch (err) {
        alert(OperationError());
        console.log(err.message);
    }
}
function Edit(e) {
    $("#lblModalHeader").text("");
    ClearData();
    try {
        var u = e.data.record.UserId;
        $.ajax({
            url: "/AdminUser/GetUserDetails",
            type: "GET",
            dataType: "json",
            data: { 'u': '' + u + '' },
            contentType: "application/json",
            async: true,
            success: function (data) {
                if (data.Records.length > 0) {
                    $('#AlertInfo').hide();
                    $("#e").val(setET(2));
                    $("#U").val(u);
                    $("#lblModalHeader").text("User Management :: Edit Record");
                    $("#txtFirstName").val(data.Records[0].FirstName);
                    $("#txtMiddleName").val(data.Records[0].MiddleName);
                    $("#txtLastName").val(data.Records[0].LastName);
                    $("#txtEmailId").val(data.Records[0].EmailId);
                    $("#txtMobileNo").val(data.Records[0].MobileNo);
                    $("#txtAddressLine1").val(data.Records[0].txtAddressLine1);
                    $("#txtAddressLine2").val(data.Records[0].txtAddressLine2);
                    $("#txtCity").val(data.Records[0].txtCity);
                    $("#txtPinCode").val(data.Records[0].PinCode);
                    $("#txtUserType").val(data.Records[0].UserType);
                    $("#txtGroupName").val(data.Records[0].GroupName);
                    $("#txtUserTypeName").val(data.Records[0].UserTypeName);
                    if (data.Records[0].ActiveYN == DefaultSetting.DefaultValY) {
                        $("#chkActiveYN").prop("checked", true);
                        $("#chkActiveYN").val(DefaultSetting.DefaultValY);
                    }
                    else {
                        $("#chkActiveYN").prop("checked", false);
                        $("#chkActiveYN").val(DefaultSetting.DefaultValN);
                    }
                    $("#txtUserType").prop("disabled", true);
                    $("#txtGroupName").prop("disabled", true);
                    $("#txtUserTypeName").prop("disabled", true);
                    GetDesignationList(data.Records[0].DesignationId, data.Records[0].DesignEditableYN);
                    GetSalutationList(data.Records[0].SalutationId);
                    $("#popupModal").modal("show");
                }
                else {
                    alert(OperationError());
                }
            },
            error: function (xhr) {
                alert(OperationError());
                console.log(xhr.responseText);
            }
        });
    }
    catch (err) {
        alert(OperationError());
        console.log(err.message);
    }
}
function Remove(e) {
}
var chkMod = function (e) {
    try {
        if (e.target.parentElement.nodeName.toLowerCase() == "td") {
            console.log(e);
            window.location.href = "../../../../Error/Unexpected.html";
        }
    }
    catch (err) {
    }
};
function Search() {
    $("#GridUser").off('DOMSubtreeModified', chkMod);
    grid.reload({ searchString: $.trim($("#txtSearch").val()) });
}
window.onbeforeunload = DisableButtons;
