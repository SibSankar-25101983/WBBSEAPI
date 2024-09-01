$(".alpha").keypress(function (e) { return alpha(e); });
$("#btnSearch").on("click", search);
$("#btnAdd").on("click", add);
$("#btnSave").on("click", save);
function clearData() {
    $('#divPopupBody input').val(DefaultSetting.EmptyVal);
    $('#divPopupBody select').val(DefaultSetting.DefaultValEnc);
}
function toggleInfo() {
    $("#divS").css("display", "none");
    $("#divE").css("display", "none");
}
function add() {
    clearData();
    $('#AlertInfo').hide();
    $("#e").val(setET(1));
    $("#lblModalHeader").text("School Management Master :: Add Record");
    $("#popupModal").modal("show");
}
function save() {
    try {
        var flag = 0;
        $('#AlertInfo').hide();
        if ($("#e").val() != setET(3)) {

            if ($.trim($("#txtSchoolManagement").val()) == DefaultSetting.EmptyVal) {
                $('#AlertInfo').show();
                $('#AlertInfoMsg').text(SchoolParameter.Required.SchoolManagement);
                $("#txtSchoolManagement").focus();
                $('#popupModal').scrollTop(0);
                return false;
            }
        }
        else {
            var i = $("#s").val();
            $.ajax({
                url: "/AdminSchoolManagement/ChkSchoolManagementDelete",
                type: "GET",
                dataType: "json",
                contentType: "application/json",
                data: { 'i': '' + i + '' },
                async: false,
                success: function (data) {
                    if (parseInt(data) > 0) {
                        alert(SchoolParameter.Validation.SchoolManagementDeleteNotPermitted);
                        flag = 1;
                        return false;
                    }
                },
                error: function (error) {
                    console.log(error);
                }
            });

        }
        if (flag == 1) {
            return false;
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
        console.log(err);
        return false;
    }
}
function Edit(e) {
    try {
        $('#AlertInfo').hide();
        $("#e").val(setET(2));
        $("#lblModalHeader").text("School Management Master :: Edit Record");
        $("#txtSchoolManagement").val(e.data.record.SchoolManagement);
        $("#s").val(e.data.record.SchoolManagementId);
    }
    catch (err) {
        alert(OperationError());
        return false;
    }
    $("#popupModal").modal("show");
}
function Remove(e) {
    try {
        $("#e").val(setET(3));
        $("#b").val(e.data.record.BlockId);
        $("#m").val(e.data.record.MigYN);
        if ($("#m").val() == DefaultSetting.DefaultValY) {
            alert(GeneralMsg.Validation.MigDataDelete);
            return false;
        }
        $("#s").val(e.data.record.SchoolManagementId);
        $("#txtSchoolManagement").val(e.data.record.SchoolManagement);
        $("#btnSave").click();
    }
    catch (err) {
        alert(OperationError());
        return false;
    }
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
function search() {
    $("#GridSchoolManagement").off('DOMSubtreeModified', chkMod);
    grid.reload({ searchString: $.trim($("#txtSearch").val()) });
}
$("#btnReport").click(function () {
    var r = $('input[name="ReportType"]:checked').val();
    $("#reportModal").modal("hide");
    if (r == ReportFormat.Pdf) {
        window.open('../../../../Reporting/Page/MstSchoolManagement.aspx', '_blank', 'noopener');
        return false;
    }
    return true;
});
window.onbeforeunload = DisableButtons;
