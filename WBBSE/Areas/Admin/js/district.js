$(".alpha").keypress(function (e) { return alpha(e); });
$("#ddlZone").select2({ width: 'resolve', theme: "classic" });
$("#ddlIndexInitial").select2({ width: 'resolve', theme: "classic" });
$("#btnSearch").on("click", search);
$("#btnAdd").on("click", add);
$("#btnSave").on("click", save);
function clearData() {
    $('#divPopupBody input').val(DefaultSetting.EmptyVal);
    $('#divPopupBody select').val(DefaultSetting.DefaultValEnc);
    $("#ddlZone").prop("disabled", false);
    $("#ddlIndexInitial").prop("disabled", false);
}
function toggleInfo() {
    $("#divS").css("display", "none");
    $("#divE").css("display", "none");
}
function GetIndexInitialList(d) {
    try {
        $("#divLblIndex").css("display", "block");
        $("#divIndex").css("display", "block");
        $("#ddlIndexInitial").empty();
        $("#ddlIndexInitial").append(new Option("Select Index Initial", DefaultSetting.DefaultValEnc));
        $.ajax({
            url: "/AdminDistrict/GetIndexInitialList",
            type: "GET",
            dataType: "json",
            contentType: "application/json",
            async: true,
            success: function (data) {
                $(data).each(function (index, item) {
                    var opt = new Option(item.IndexInitial, item.IndexInitialEnc);
                    $("#ddlIndexInitial").append(opt);
                });
                $("#ddlIndexInitial").val(d);
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
function GetZoneList(d) {
    try {
        $("#divLblZone").css("display", "block");
        $("#divZone").css("display", "block");
        $("#ddlZone").empty();
        $("#ddlZone").append(new Option("Select Zone", DefaultSetting.DefaultValEnc));
        $.ajax({
            url: "/AdminDistrict/GetZoneList",
            type: "GET",
            dataType: "json",
            contentType: "application/json",
            async: true,
            success: function (data) {
                $(data).each(function (index, item) {
                    var opt = new Option(item.ZoneName, item.ZoneId);
                    $("#ddlZone").append(opt);
                });
                $("#ddlZone").val(d);
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
    $('#divLblIndex').show();
    $('#divIndex').show();
    $('#AlertInfo').hide();
    $("#e").val(setET(1));
    $("#lblModalHeader").text("District Master :: Add Record");
    GetZoneList(DefaultSetting.DefaultValEnc);
    GetIndexInitialList(DefaultSetting.DefaultValEnc);

    $("#popupModal").modal("show");
}
function save() {
    try {
        var flag = 0;
        $('#AlertInfo').hide();
        if ($("#e").val() != setET(3)) {

            if ($("#ddlZone").val() == DefaultSetting.DefaultValEnc) {
                $('#AlertInfo').show();
                $('#AlertInfoMsg').text(District.Required.ZoneName);
                $("#ddlZone").focus();
                $('#popupModal').scrollTop(0);
                return false;
            }
            //Allow checking at Added time other wise pass 
            if ($("#e").val() == setET(1)) {
                if ($("#ddlIndexInitial").val() == DefaultSetting.DefaultValEnc) {
                    $('#AlertInfo').show();
                    $('#AlertInfoMsg').text(District.Required.IndexInitial);
                    $("#ddlIndexInitial").focus();
                    $('#popupModal').scrollTop(0);
                    return false;
                }

                //duplicate index initial check
                $.ajax({
                    url: "/AdminDistrict/ChkDuplicateIndexInitial",
                    type: "GET",
                    dataType: "json",
                    contentType: "application/json",
                    data: { 'i': '' + $.trim($("#ddlIndexInitial").val()) + '', },
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
            }
            if ($.trim($("#txtDistrictName").val()) == DefaultSetting.EmptyVal) {
                $('#AlertInfo').show();
                $('#AlertInfoMsg').text(District.Required.DistrictName);
                $("#txtDistrictName").focus();
                $('#popupModal').scrollTop(0);
                return false;
            }

            if ($("#e").val() == setET(2)) {
                var i = $("#d").val();
                var oz = $("#oz").val();
                $.ajax({
                    url: "/AdminDistrict/ChkTransfer",
                    type: "GET",
                    dataType: "json",
                    contentType: "application/json",
                    data: { 'i': '' + i + '', 'cz': '' + $.trim($("#ddlZone").val()) + '', 'oz': '' + oz + '' },
                    async: false,
                    success: function (data) {
                        if (parseInt(data) > 1) {
                            $('#AlertInfo').show();
                            $('#AlertInfoMsg').text(District.Validation.DistrictTransferNotPermitted);
                            //alert(District.Validation.DistrictTransferNotPermitted);
                            flag = 1;
                            return false;
                        }
                    },
                    error: function (error) {
                        console.log(error);
                    }
                });
            }

        }
        else {
            var i = $("#d").val();
            $.ajax({
                url: "/AdminDistrict/ChkDistrictDelete",
                type: "GET",
                dataType: "json",
                contentType: "application/json",
                data: { 'i': '' + i + '' },
                async: false,
                success: function (data) {
                    if (parseInt(data) > 0) {
                        alert(District.Validation.DistrictDeleteNotPermitted);
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
                $("#ddlZone").prop("disabled", false);
                $("#ddlIndexInitial").prop("disabled", false);
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
        $('#divLblIndex').hide();
        $('#divIndex').hide();
        $("#e").val(setET(2));
        $("#lblModalHeader").text("District Master :: Edit Record");
        $("#txtDistrictName").val(e.data.record.DistrictName);
        $("#d").val(e.data.record.DistrictId);
        $("#oz").val(e.data.record.ZoneId);
        GetZoneList(e.data.record.ZoneId);
        $("#ddlZone").prop("disabled", true);
        $("#ddlIndexInitial").prop("disabled", true);
        //GetIndexInitialList(e.data.record.IndexInitial);
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
        $("#d").val(e.data.record.DistrictId);
        $("#txtDistrictName").val(e.data.record.DistrictName);
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
    $("#GridDistrict").off('DOMSubtreeModified', chkMod);
    grid.reload({ searchString: $.trim($("#txtSearch").val()) });
}
$("#btnReport").click(function () {
    var r = $('input[name="ReportType"]:checked').val();
    $("#reportModal").modal("hide");
    if (r == ReportFormat.Pdf) {
        window.open('../../../../Reporting/Page/MstDistrict.aspx', '_blank', 'noopener');
        return false;
    }
    return true;
});
window.onbeforeunload = DisableButtons;
