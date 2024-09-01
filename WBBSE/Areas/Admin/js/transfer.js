var ViewMsgDefault = "<div class='alert alert-danger'>Select a school to view transfer history.</div>";
$(".alpha").keypress(function (e) { return alpha(e); });
$("#ddlSubDivision").select2({ width: 'resolve', theme: "classic" });
$("#divTransferData").html(ViewMsgDefault);
$("#divTransferSaveMsg").html(School.Info.SchoolTransferSave);
$("#btnSearch").on("click", Search);
$("#btnAdd").on("click", Add);
$("#btnSave").on("click", Save);
//$("#btnDelete").on("click", Remove);
function ClearData() {
    $('#divPopupBody input').val('');
    $('#divPopupBody select').val(DefaultSetting.DefaultValEnc);
    $("#txtSearch").val(DefaultSetting.EmptyVal);
    $("#divTransferData").empty();
    $("#divTransferData").html(ViewMsgDefault);
    $("#S").val(DefaultSetting.DefaultValEnc);
    $("#SS").val(DefaultSetting.DefaultValEnc);
    $("#ddlSubDivision").empty();
}
$("#txtSearch").autocomplete({
    minLength: 1,
    disabled: false,
    delay: 100,
    source: function (request, response) {
        $.ajax({
            url: "/AdminSchoolTransfer/GetSchoolList",
            dataType: "json",
            data: { s: request.term },
            success: function (data) {
                response($.map(data, function (item) {
                    return { label: item.SchoolName, value: item.SchoolName, SI: item.SchoolId }
                }))
            },
            error: function (err) {
                console.log(err.responseText);
            }
        });
    },
    select: function (e, i) {
        $("#txtSearch").val(i.item.label);
        $("#SS").val(i.item.SI);
    }
});
function Search() {
    if ($("#SS").val() == DefaultSetting.EmptyVal || $("#SS").val() == DefaultSetting.DefaultValEnc) {
        alert(School.Required.SchoolSelectionTransferView);
        return false;
    }
    $("#divTransferData").empty();
    $("#divTransferData").LoadingOverlay("show");
    try {
        $.ajax({
            url: "/AdminSchoolTransfer/GetSchoolTransferView",
            type: "GET",
            dataType: "json",
            contentType: "application/json",
            data: { 's': '' + $("#SS").val() + '' },
            async: true,
            success: function (data) {
                $("#divTransferData").append(data.View);
                $("#divTransferData").LoadingOverlay("hide");
            },
            error: function (error) {
                console.log(error.responseText);
                alert(OperationError());
                $("#divTransferData").html(ViewMsgDefault);
            }
        });
    }
    catch (err) {
        console.log(err.message);
        alert(OperationError());
        $("#divTransferData").html(ViewMsgDefault);
    }
}
function Remove() {
    try {
        if ($("#SS").val() == DefaultSetting.EmptyVal || $("#SS").val() == DefaultSetting.DefaultValEnc) {
            alert(School.Required.SchoolSelectionTransferDelete);
            return false;
        }
        $("#S").val($("#SS").val());
        $("#e").val(setET(3));
        $("#btnSave").click();
    }
    catch (err) {
        console.log(err.message);
        alert(OperationError());
        $("#txtSearch").val(DefaultSetting.EmptyVal);
        $("#divTransferData").html(ViewMsgDefault);
    }
}
function Add() {
    ClearData();
    $('#AlertInfo').hide();
    $("#lblSubDivision").css("display", "none");
    $("#e").val(setET(1));
    $("#lblModalHeader").text("School Transfer :: Add Record");
    $("#popupModal").modal("show");
}
$("#txtSchoolName").autocomplete({
    minLength: 1,
    disabled: false,
    delay: 100,
    source: function (request, response) {
        $.ajax({
            url: "/AdminSchoolTransfer/GetSchoolList",
            dataType: "json",
            data: { s: request.term },
            success: function (data) {
                response($.map(data, function (item) {
                    return { label: item.SchoolName, value: item.SchoolName, SI: item.SchoolId }
                }))
            },
            error: function (err) {
                console.log(err.responseText);
            }
        });
    },
    select: function (e, i) {
        $("#txtSchoolName").val(i.item.label);
        $("#S").val(i.item.SI);
        $("#divSubDivision").LoadingOverlay("show");
        $("#divSubDivisionDropDown").LoadingOverlay("show");
        GetCurrentSubDivisionName($("#S").val());
        GetSubDivisionList($("#S").val());
    }
});
function GetCurrentSubDivisionName(s) {
    try {
        $.ajax({
            url: "/AdminSchoolTransfer/GetCurrentSubDivisionName",
            type: "GET",
            dataType: "json",
            contentType: "application/json",
            data: { 's': '' + s + '' },
            async: true,
            success: function (data) {
                $("#lblSubDivision").css("display", "");
                $("#lblSubDivision").text(data.SubDivisionName);
                $("#divSubDivision").LoadingOverlay("hide");
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
function GetSubDivisionList(s) {
    $("#ddlSubDivision").empty();
    $("#ddlSubDivision").append(new Option("Select Sub-Division", DefaultSetting.DefaultValEnc));
    try {
        $.ajax({
            url: "/AdminSchoolTransfer/GetSubDivisionList",
            type: "GET",
            dataType: "json",
            contentType: "application/json",
            data: { 's': '' + s + '' },
            async: true,
            success: function (data) {
                $(data).each(function (index, item) {
                    var opt = new Option(item.SubDivisionName, item.SubDivisionId);
                    $("#ddlSubDivision").append(opt);
                });
                $("#divSubDivisionDropDown").LoadingOverlay("hide");
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
    if ($("#e").val() != setET(3)) {
        if ($("#S").val() == DefaultSetting.EmptyVal || $("#S").val() == DefaultSetting.DefaultValEnc) {
            $('#AlertInfo').show();
            $('#AlertInfoMsg').text(School.Required.SchoolSelectionTransferSave);
            $("#txtSchoolName").val("");
            $("#txtSchoolName").focus();
            $('#popupModal').scrollTop(0);
            return false;
        }
        if ($("#ddlSubDivision").val() == null || $("#ddlSubDivision").val() == DefaultSetting.DefaultValEnc) {
            $('#AlertInfo').show();
            $('#AlertInfoMsg').text(School.Required.SubDivisionSelectionTransferSave);
            $("#ddlSubDivision").focus();
            $('#popupModal').scrollTop(0);
            return false;
        }
    }
    try {
        var msg;
        if ($("#e").val() != setET(3)) {
            msg = $("#lblSubDivision").text() + ". " + School.Info.SchoolTransferSaveWarning.replace("#", $("#txtSchoolName").val()).replace("$", $("#ddlSubDivision option:selected").text());
        }
        if ($("#e").val() == setET(3)) {
            msg = School.Info.SchoolTransferDeleteWarning.replace("#", $("#txtSearch").val());
        }
        var x = confirm(msg);
        if (x) {
            $("#popupModal").modal("hide");
            ShowLoader();
        }
        return x;
    }
    catch (err) {
        console.log(err);
        alert(OperationError());
        return false;
    }
}
window.onbeforeunload = DisableButtons;
