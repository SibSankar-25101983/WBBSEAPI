$("#txtEmailId").css('text-transform', 'none');
$("#txtWebsite").css('text-transform', 'none');
$("#ddlSchoolType").select2({ width: 'resolve', theme: "classic" });
$("#ddlSchoolCategory").select2({ width: 'resolve', theme: "classic" });
$("#ddlSchoolStatus").select2({ width: 'resolve', theme: "classic" });
$("#ddlSchoolMedium").select2({ width: 'resolve', theme: "classic" });
$("#ddlSchoolRecognition").select2({ width: 'resolve', theme: "classic" });
$("#ddlSchoolManagement").select2({ width: 'resolve', theme: "classic" });
$("#ddlDesignation").select2({ width: 'resolve', theme: "classic" });
$(".alpha").keypress(function (e) { return alpha(e); });
$(".OnlyAlpha").keypress(function (e) { return OnlyAlpha(e); });
$(".checkContactNo").keypress(function (e) { return checkContactNo(e); });
$(".allowCharForEmailId").keypress(function (e) { return allowCharForEmailId(e); });
$(".allowCharForWebsite").keypress(function (e) { return allowCharForWebsite(e); });
$("#btnSave").on("click", Save);
function ClearData() {
    $('#divPopupBody input').val('');
    $('#divPopupBody select').val(DefaultSetting.DefaultValEnc);
}
function ClearViewData() {
    $("#tblViewData").empty();
}
function GetSchoolCategoryList(d) {
    $("#ddlSchoolCategory").empty();
    $("#ddlSchoolCategory").append(new Option("Select School Category", DefaultSetting.DefaultValEnc));
    try {
        $.ajax({
            url: "/PreSchoolProfile/GetSchoolCategoryList",
            type: "GET",
            dataType: "json",
            contentType: "application/json",
            async: true,
            success: function (data) {
                $(data).each(function (index, item) {
                    var opt = new Option(item.SchoolCategoryName, item.SchoolCategoryId);
                    $("#ddlSchoolCategory").append(opt);
                });
                $("#ddlSchoolCategory").val(d);
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
function GetSchoolTypeList(d) {
    $("#ddlSchoolType").empty();
    $("#ddlSchoolType").append(new Option("Select School Type", DefaultSetting.DefaultValEnc));
    try {
        $.ajax({
            url: "/PreSchoolProfile/GetSchoolTypeList",
            type: "GET",
            dataType: "json",
            contentType: "application/json",
            async: true,
            success: function (data) {
                $(data).each(function (index, item) {
                    var opt = new Option(item.SchoolTypeName, item.SchoolTypeId);
                    $("#ddlSchoolType").append(opt);
                });
                $("#ddlSchoolType").val(d);
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
function GetSchoolStatusList(d) {
    $("#ddlSchoolStatus").empty();
    $("#ddlSchoolStatus").append(new Option("Select School Status", DefaultSetting.DefaultValEnc));
    try {
        $.ajax({
            url: "/PreSchoolProfile/GetSchoolStatusList",
            type: "GET",
            dataType: "json",
            contentType: "application/json",
            async: true,
            success: function (data) {
                $(data).each(function (index, item) {
                    var opt = new Option(item.SchoolStatusName, item.SchoolStatusId);
                    $("#ddlSchoolStatus").append(opt);
                });
                $("#ddlSchoolStatus").val(d);
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
function GetSchoolMediumList(d) {
    $("#ddlSchoolMedium").empty();
    $("#ddlSchoolMedium").append(new Option("Select School Medium", DefaultSetting.DefaultValEnc));
    try {
        $.ajax({
            url: "/PreSchoolProfile/GetSchoolMediumList",
            type: "GET",
            dataType: "json",
            contentType: "application/json",
            async: true,
            success: function (data) {
                $(data).each(function (index, item) {
                    var opt = new Option(item.SchoolMediumName, item.SchoolMediumId);
                    $("#ddlSchoolMedium").append(opt);
                });
                $("#ddlSchoolMedium").val(d);
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
function GetSchoolRecognitionList(d) {
    $("#ddlSchoolRecognition").empty();
    $("#ddlSchoolRecognition").append(new Option("Select School Recognization", DefaultSetting.DefaultValEnc));
    try {
        $.ajax({
            url: "/PreSchoolProfile/GetSchoolRecognitionList",
            type: "GET",
            dataType: "json",
            contentType: "application/json",
            async: true,
            success: function (data) {
                $(data).each(function (index, item) {
                    var opt = new Option(item.RecognitionStatus, item.SchoolRecognitionId);
                    $("#ddlSchoolRecognition").append(opt);
                });
                $("#ddlSchoolRecognition").val(d);
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
function GetSchoolManagementList(d) {
    $("#ddlSchoolManagement").empty();
    $("#ddlSchoolManagement").append(new Option("Select School Management", DefaultSetting.DefaultValEnc));
    try {
        $.ajax({
            url: "/PreSchoolProfile/GetSchoolManagementList",
            type: "GET",
            dataType: "json",
            contentType: "application/json",
            async: true,
            success: function (data) {
                $(data).each(function (index, item) {
                    var opt = new Option(item.SchoolManagement, item.SchoolManagementId);
                    $("#ddlSchoolManagement").append(opt);
                });
                $("#ddlSchoolManagement").val(d);
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
function GetDesignationList(d) {
    $("#ddlDesignation").empty();
    $("#ddlDesignation").append(new Option("Select Designation", DefaultSetting.DefaultValEnc));
    try {
        $.ajax({
            url: "/PreSchoolProfile/GetDesignationList",
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
function Save() {
    var flag = 0;
    try {
        if ($("#e").val() != setET(3)) {
            $('#AlertInfoMsg').text("");
            //if ($.trim($("#txtSchoolName").val()) == DefaultSetting.EmptyVal) {
            //    $('#AlertInfo').show();
            //    $('#AlertInfoMsg').text(School.Required.SchoolName);
            //    $("#txtSchoolName").focus();
            //    $('#popupModal').scrollTop(0);
            //    return false;
            //}
            //if (!chkDataFormat(RegexType.Alpha, $.trim($("#txtSchoolName").val()))) {
            //    $('#AlertInfo').show();
            //    $('#AlertInfoMsg').text(School.Validation.InvalidSchoolName);
            //    $("#txtSchoolName").focus();
            //    $('#popupModal').scrollTop(0);
            //    return false;
            //}
            if ($.trim($("#txtDISECode").val()) == DefaultSetting.EmptyVal) {
                $('#AlertInfo').show();
                $('#AlertInfoMsg').text(School.Required.DISECode);
                $("#txtDISECode").focus();
                $('#popupModal').scrollTop(0);
                return false;
            }
            var reg_dise = /^([0-9]{11})$/;
            if (!reg_dise.test($.trim($("#txtDISECode").val()))) {
                $('#AlertInfo').show();
                $('#AlertInfoMsg').text(School.Validation.InvalidDISECode);
                $("#txtDISECode").focus();
                $('#txtDISECode').scrollTop(0);
                return false;
            }
            if ($.trim($("#txtAddressLine1").val()) == DefaultSetting.EmptyVal) {
                $('#AlertInfo').show();
                $('#AlertInfoMsg').text(School.Required.StreetName);
                $("#txtAddressLine1").focus();
                $('#popupModal').scrollTop(0);
                return false;
            }
            if ($.trim($("#txtAddressLine1").val()) != DefaultSetting.EmptyVal) {
                if (!chkDataFormat(RegexType.Alpha, $.trim($("#txtAddressLine1").val()))) {
                    $('#AlertInfo').show();
                    $('#AlertInfoMsg').text(School.Validation.InvalidStreetName);
                    $("#txtAddressLine1").focus();
                    $('#popupModal').scrollTop(0);
                    return false;
                }
            }
            if ($.trim($("#txtAddressLine2").val()) == DefaultSetting.EmptyVal) {
                $('#AlertInfo').show();
                $('#AlertInfoMsg').text(School.Required.AreaName);
                $("#txtAddressLine2").focus();
                $('#popupModal').scrollTop(0);
                return false;
            }
            if ($.trim($("#txtAddressLine2").val()) != DefaultSetting.EmptyVal) {
                if (!chkDataFormat(RegexType.Alpha, $.trim($("#txtAddressLine2").val()))) {
                    $('#AlertInfo').show();
                    $('#AlertInfoMsg').text(School.Validation.InvalidAreaName);
                    $("#txtAddressLine2").focus();
                    $('#popupModal').scrollTop(0);
                    return false;
                }
            }
            if ($.trim($("#txtGramPanchayet").val()) != DefaultSetting.EmptyVal) {
                if (!chkDataFormat(RegexType.Alpha, $.trim($("#txtGramPanchayet").val()))) {
                    $('#AlertInfo').show();
                    $('#AlertInfoMsg').text(School.Validation.InvalidGramPanchayetName);
                    $("#txtGramPanchayet").focus();
                    $('#popupModal').scrollTop(0);
                    return false;
                }
            }
            if ($.trim($("#txtPostOffice").val()) != DefaultSetting.EmptyVal) {
                if (!chkDataFormat(RegexType.Alpha, $.trim($("#txtPostOffice").val()))) {
                    $('#AlertInfo').show();
                    $('#AlertInfoMsg').text(School.Validation.InvalidPOName);
                    $("#txtPostOffice").focus();
                    $('#popupModal').scrollTop(0);
                    return false;
                }
            }
            if ($.trim($("#txtPoliceStation").val()) != DefaultSetting.EmptyVal) {
                if (!chkDataFormat(RegexType.Alpha, $.trim($("#txtPoliceStation").val()))) {
                    $('#AlertInfo').show();
                    $('#AlertInfoMsg').text(School.Validation.InvalidPSName);
                    $("#txtPoliceStation").focus();
                    $('#popupModal').scrollTop(0);
                    return false;
                }
            }
            if ($.trim($("#txtCity").val()) == DefaultSetting.EmptyVal) {
                $('#AlertInfo').show();
                $('#AlertInfoMsg').text(School.Required.City);
                $("#txtCity").focus();
                $('#popupModal').scrollTop(0);
                return false;
            }
            if ($.trim($("#txtCity").val()) != DefaultSetting.EmptyVal) {
                if (!chkDataFormat(RegexType.Alpha, $.trim($("#txtCity").val()))) {
                    $('#AlertInfo').show();
                    $('#AlertInfoMsg').text(School.Validation.InvalidCityName);
                    $("#txtCity").focus();
                    $('#popupModal').scrollTop(0);
                    return false;
                }
            }
            if ($.trim($("#txtPinCode").val()) == DefaultSetting.EmptyVal) {
                $('#AlertInfo').show();
                $('#AlertInfoMsg').text(School.Required.PinCode);
                $("#txtPinCode").focus();
                $('#popupModal').scrollTop(0);
                return false;
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
            if ($.trim($("#txtStdCode").val()) != DefaultSetting.EmptyVal) {
                if (!chkDataFormat(RegexType.StdCode, $.trim($("#txtStdCode").val()))) {
                    $('#AlertInfo').show();
                    $('#AlertInfoMsg').text(GeneralMsg.Validation.InvalidSTDCode);
                    $("#txtStdCode").focus();
                    $('#popupModal').scrollTop(0);
                    return false;
                }
            }
            if ($.trim($("#txtPhoneNo").val()) != DefaultSetting.EmptyVal) {
                if (!chkDataFormat(RegexType.Numeric, $.trim($("#txtPhoneNo").val()))) {
                    $('#AlertInfo').show();
                    $('#AlertInfoMsg').text(GeneralMsg.Validation.InvalidPhoneNo);
                    $("#txtPhoneNo").focus();
                    $('#popupModal').scrollTop(0);
                    return false;
                }
            }
            if ($.trim($("#txtMobileNo").val()) == DefaultSetting.EmptyVal) {
                $('#AlertInfo').show();
                $('#AlertInfoMsg').text(School.Required.MobileNo);
                $("#txtMobileNo").focus();
                $('#popupModal').scrollTop(0);
                return false;
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
            if ($.trim($("#txtFaxNo").val()) != DefaultSetting.EmptyVal) {
                if (!chkDataFormat(RegexType.Numeric, $.trim($("#txtFaxNo").val()))) {
                    $('#AlertInfo').show();
                    $('#AlertInfoMsg').text(GeneralMsg.Validation.InvalidFaxNo);
                    $("#txtFaxNo").focus();
                    $('#popupModal').scrollTop(0);
                    return false;
                }
            }
            if ($.trim($("#txtStdCode").val()) != DefaultSetting.EmptyVal) {
                if ($.trim($("#txtPhoneNo").val()) == DefaultSetting.EmptyVal && $.trim($("#txtFaxNo").val()) == DefaultSetting.EmptyVal) {
                    $('#AlertInfo').show();
                    $('#AlertInfoMsg').text(School.Required.PhoneNoOrFaxNo);
                    $('#popupModal').scrollTop(0);
                    return false;
                }
            }
            if ($.trim($("#txtPhoneNo").val()) != DefaultSetting.EmptyVal && $.trim($("#txtStdCode").val()) == DefaultSetting.EmptyVal) {
                $('#AlertInfo').show();
                $('#AlertInfoMsg').text(School.Required.STDCodePhoneNo);
                $('#popupModal').scrollTop(0);
                return false;
            }
            if ($.trim($("#txtFaxNo").val()) != DefaultSetting.EmptyVal && $.trim($("#txtStdCode").val()) == DefaultSetting.EmptyVal) {
                $('#AlertInfo').show();
                $('#AlertInfoMsg').text(School.Required.STDCodeFaxNo);
                $('#popupModal').scrollTop(0);
                return false;
            }
            if ($.trim($("#txtEmailId").val()) == DefaultSetting.EmptyVal) {
                $('#AlertInfo').show();
                $('#AlertInfoMsg').text(School.Required.EmailId);
                $("#txtEmailId").focus();
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
            if ($.trim($("#txtWebsite").val()) != DefaultSetting.EmptyVal) {
                if (!chkDataFormat(RegexType.URL, $.trim($("#txtWebsite").val()))) {
                    $('#AlertInfo').show();
                    $('#AlertInfoMsg').text(GeneralMsg.Validation.InvalidURL);
                    $("#txtWebsite").focus();
                    $('#popupModal').scrollTop(0);
                    return false;
                }
            }
            if ($("#ddlSchoolType").val() == DefaultSetting.DefaultValEnc) {
                $('#AlertInfo').show();
                $('#AlertInfoMsg').text(School.Required.SchoolType);
                $("#ddlSchoolType").focus();
                $('#popupModal').scrollTop(0);
                return false;
            }
            if ($("#ddlSchoolCategory").val() == DefaultSetting.DefaultValEnc) {
                $('#AlertInfo').show();
                $('#AlertInfoMsg').text(School.Required.SchoolCategory);
                $("#ddlSchoolCategory").focus();
                $('#popupModal').scrollTop(0);
                return false;
            }
            if ($("#ddlSchoolStatus").val() == DefaultSetting.DefaultValEnc) {
                $('#AlertInfo').show();
                $('#AlertInfoMsg').text(School.Required.SchoolStatus);
                $("#ddlSchoolStatus").focus();
                $('#popupModal').scrollTop(0);
                return false;
            }
            if ($("#ddlSchoolMedium").val() == DefaultSetting.DefaultValEnc) {
                $('#AlertInfo').show();
                $('#AlertInfoMsg').text(School.Required.SchoolMedium);
                $("#ddlSchoolMedium").focus();
                $('#popupModal').scrollTop(0);
                return false;
            }
            if ($("#ddlSchoolRecognition").val() == DefaultSetting.DefaultValEnc) {
                $('#AlertInfo').show();
                $('#AlertInfoMsg').text(School.Required.SchoolRecognization);
                $("#ddlSchoolRecognition").focus();
                $('#popupModal').scrollTop(0);
                return false;
            }
            if ($("#ddlSchoolManagement").val() == DefaultSetting.DefaultValEnc) {
                $('#AlertInfo').show();
                $('#AlertInfoMsg').text(School.Required.SchoolManagement);
                $("#ddlSchoolManagement").focus();
                $('#popupModal').scrollTop(0);
                return false;
            }
            if ($("#ddlDesignation").val() == DefaultSetting.DefaultValEnc) {
                $('#AlertInfo').show();
                $('#AlertInfoMsg').text(School.Required.Designation);
                $("#ddlDesignation").focus();
                $('#popupModal').scrollTop(0);
                return false;
            }
            //duplicate contact details check
            $.ajax({
                url: "/PreSchoolProfile/ChkDuplicateContact",
                type: "GET",
                dataType: "json",
                contentType: "application/json",
                data: { 'e': '' + $.trim($("#txtEmailId").val()) + '', 'p': '' + $.trim($("#txtPhoneNo").val()) + '', 'm': '' + $.trim($("#txtMobileNo").val()) + '', 'dc': '' + $.trim($("#txtDISECode").val()) + '' },
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
                    return false;
                }
            });
            if (flag == 0) {
                var x = false;
                if ($("#e").val() == setET(4)) {
                    var flag2 = 0;
                    $("input[type=text],select").each(function (e, d) {
                        if (d.value == DefaultSetting.EmptyVal || d.value == DefaultSetting.DefaultValEnc) {
                            flag2++;
                        }
                    });
                    x = confirm((flag2 > 0) ? School.Info.SchoolDataMissing : School.Info.SchoolProfileLock);
                }
                else {
                    x = confirm(Msg_Confirm());
                }
                return x;
                //x = confirm((flag2 > 0) ? School.Info.SchoolDataMissing : Msg_Confirm());
                //var x = confirm(Msg_Confirm());
                //if (x == DefaultSetting.DefaultValT) {
                //    //DisableButtons();
                //    $("#popupModal").modal("hide");
                //    ShowLoader();
                //}
                //return x;
            }
            else {
                return false;
            }
        }
        else {
            return false;
        }
    }
    catch (err) {
        alert(OperationError());
        console.log(err.message);
        return false;
    }
}
$("#btnLock").click(function (e) {
    $("#e").val(setET(4)); /*Lock Operation*/
    return (Save());
});
function View(e) {
    ClearViewData();
    try {
        var s = e.data.record.PreSchoolId;

        $.ajax({
            url: "/PreSchoolProfile/GetPreSchoolView",
            type: "GET",
            dataType: "json",
            data: { 's': '' + s + '' },
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
        var s = e.data.record.PreSchoolId;
        $.ajax({
            url: "/PreSchoolProfile/GetPreSchoolDetails",
            type: "GET",
            dataType: "json",
            data: { 's': '' + s + '' },
            contentType: "application/json",
            async: true,
            //headers: { '__RequestVerificationToken': $("input[name='__RequestVerificationToken']").val() },
            success: function (data) {
                if (data.Records.length > 0) {
                    $('#AlertInfo').hide();
                    $("#e").val(setET(2));
                    $("#P").val(s);
                    $("#lblModalHeader").text("School Profile :: Edit Record [School Code : " + data.Records[0].SchoolCode + "]");
                    $("#txtSchoolName").text(data.Records[0].SchoolName);
                    $("#txtDISECode").val(data.Records[0].DISECode);
                    $("#txtAddressLine1").val(data.Records[0].AddressLine1);
                    $("#txtAddressLine2").val(data.Records[0].AddressLine2);
                    $("#txtGramPanchayet").val(data.Records[0].GramPanchayet);
                    $("#txtPostOffice").val(data.Records[0].PostOffice);
                    $("#txtPoliceStation").val(data.Records[0].PoliceStation);
                    $("#txtCity").val(data.Records[0].City);
                    $("#txtPinCode").val(data.Records[0].PinCode);
                    $("#txtStdCode").val(data.Records[0].StdCode);
                    $("#txtPhoneNo").val(data.Records[0].PhoneNo);
                    $("#txtMobileNo").val(data.Records[0].MobileNo);
                    $("#txtFaxNo").val(data.Records[0].FaxNo);
                    $("#txtEmailId").val(data.Records[0].EmailId);
                    $("#txtWebsite").val(data.Records[0].Website);
                    GetSchoolCategoryList(data.Records[0].SchoolCategoryId);
                    GetSchoolTypeList(data.Records[0].SchoolTypeId);
                    GetSchoolStatusList(data.Records[0].SchoolStatusId);
                    GetSchoolMediumList(data.Records[0].SchoolMediumId);
                    GetSchoolRecognitionList(data.Records[0].SchoolRecognitionId);
                    GetSchoolManagementList(data.Records[0].SchoolManagementId);
                    GetDesignationList(data.Records[0].DesignationId);
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
window.onbeforeunload = DisableButtons;
