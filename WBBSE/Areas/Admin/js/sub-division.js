$(".alpha").keypress(function (e) { return alpha(e); });
$("#ddlDistrict").select2({ width: 'resolve', theme: "classic" });
$("#btnSearch").on("click", search);
$("#btnAdd").on("click", add);
$("#btnSave").on("click", save);
function clearData() {
    $('#divPopupBody input').val(DefaultSetting.EmptyVal);
    $('#divPopupBody select').val(DefaultSetting.DefaultValEnc);
    $("#ddlDistrict").prop("disabled", false);
}
function toggleInfo() {
    $("#divS").css("display", "none");
    $("#divE").css("display", "none");
}
//Populate District in DropDown List 
function GetDistrictList(d) {
    try {
        $("#divLblDistrict").css("display", "block");
        $("#divDistrict").css("display", "block");
        $("#ddlDistrict").empty();
        $("#ddlDistrict").append(new Option("Select District", DefaultSetting.DefaultValEnc));
        $.ajax({
            url: "/AdminSubDivision/GetDistrictList",
            type: "GET",
            dataType: "json",
            contentType: "application/json",
            async: true,
            success: function (data) {
                $(data).each(function (index, item) {
                    var opt = new Option(item.DistrictName, item.DistrictId);
                    $("#ddlDistrict").append(opt);
                });
                $("#ddlDistrict").val(d);
            },
            error: function (error) {
                console.log(error.responseText);
            }
        });
    }
    catch (err) {
        console.log(err.responseText);
    }
}
function add() {
    clearData();
    $('#AlertInfo').hide();
    $("#e").val(setET(1));
    $("#lblModalHeader").text("Sub-Division Master :: Add Record");
    GetDistrictList(DefaultSetting.DefaultValEnc);
    $("#popupModal").modal("show");
}
function save() {
    try {
        var flag = 0;
        $('#AlertInfo').hide();
        if ($("#e").val() != setET(3)) {
            //$('#AlertInfoMsg').text("");

            if ($("#ddlDistrict").val() == DefaultSetting.DefaultValEnc) {
                $('#AlertInfo').show();
                $('#AlertInfoMsg').text(SubDivision.Required.DistrictName);
                $("#ddlDistrict").focus();
                $('#popupModal').scrollTop(0);
                return false;
            }

            if ($.trim($("#txtSubDivisionName").val()) == DefaultSetting.EmptyVal) {
                $('#AlertInfo').show();
                $('#AlertInfoMsg').text(SubDivision.Required.SubDivisionName);
                $("#txtSubDivisionName").focus();
                $('#popupModal').scrollTop(0);
                return false;
            }

            //if ($("#e").val() == setET(2)) {
            //    var i = $("#s").val();
            //    var od = $("#od").val();
            //    $.ajax({
            //        url: "/AdminSubDivision/ChkTransfer",
            //        type: "GET",
            //        dataType: "json",
            //        contentType: "application/json",
            //        data: { 'i': '' + i + '', 'cd': '' + $.trim($("#ddlDistrict").val()) + '', 'od': '' + od + '' },
            //        async: false,
            //        success: function (data) {
            //            if (parseInt(data) > 1) {
            //                $('#AlertInfo').show();
            //                $('#AlertInfoMsg').text(SubDivision.Validation.SubDivisionTransferNotPermitted);
            //                flag = 1;
            //                return false;
            //            }
            //        },
            //        error: function (error) {
            //            console.log(error);
            //        }
            //    });
            //}
        }
        else {
            var i = $("#s").val();
            $.ajax({
                url: "/AdminSubDivision/ChkSubDivisionDelete",
                type: "GET",
                dataType: "json",
                contentType: "application/json",
                data: { 'i': '' + i + '' },
                async: false,
                success: function (data) {
                    if (parseInt(data) > 0) {
                        alert(SubDivision.Validation.SubDivisionDeleteNotPermitted);
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
                $("#ddlDistrict").prop("disabled", false);
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
        $("#lblModalHeader").text("Sub-Division Master :: Edit Record");
        $("#txtSubDivisionName").val(e.data.record.SubDivisionName);
        $("#s").val(e.data.record.SubDivisionId);
        $("#od").val(e.data.record.DistrictId);
        GetDistrictList(e.data.record.DistrictId);
        $("#ddlDistrict").prop("disabled", true);
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
        $("#s").val(e.data.record.SubDivisionId);
        $("#txtSubDivisionName").val(e.data.record.SubDivisionName);
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
    $("#GridSubDivision").off('DOMSubtreeModified', chkMod);
    grid.reload({ searchString: $.trim($("#txtSearch").val()) });
}
$("#btnReport").click(function () {
    var r = $('input[name="ReportType"]:checked').val();
    $("#reportModal").modal("hide");
    if (r == ReportFormat.Pdf) {
        window.open('../../../../Reporting/Page/MstSubDivision.aspx', '_blank', 'noopener');
        return false;
    }
    return true;
});
window.onbeforeunload = DisableButtons;
