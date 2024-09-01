$(".alpha").keypress(function (e) { return alpha(e); });
$("#ddlState").select2({ width: 'resolve', theme: "classic" });
$("#btnSearch").on("click", search);
$("#btnAdd").on("click", add);
$("#btnSave").on("click", save);

function clearData() {
    $('#divPopupBody input').val(DefaultSetting.EmptyVal);
    $('#divPopupBody select').val(DefaultSetting.DefaultValEnc);
    $("#ddlState").prop("disabled", false);
}

function toggleInfo() {
    $("#divS").css("display", "none");
    $("#divE").css("display", "none");
}

function GetStateList(d) {
    try {
        $("#divLblState").css("display", "block");
        $("#divState").css("display", "block");
        $("#ddlState").empty();
        $("#ddlState").append(new Option("Select State", DefaultSetting.DefaultValEnc));
        $.ajax({
            url: "/AdminZone/GetStateList",
            type: "GET",
            dataType: "json",
            contentType: "application/json",
            async: true,
            success: function (data) {
                $(data).each(function (index, item) {
                    var opt = new Option(item.StateName, item.StateId);
                    $("#ddlState").append(opt);
                });
                $("#ddlState").val(d);
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
    $("#lblModalHeader").text("Zone Master :: Add Record");
    GetStateList(DefaultSetting.DefaultValEnc);
    $("#popupModal").modal("show");
}

function save() {
    try {
        var flag = 0;
        $('#AlertInfo').hide();
        if ($("#e").val() != setET(3)) {
            //$('#AlertInfoMsg').text("");

            if ($("#ddlState").val() == DefaultSetting.DefaultValEnc) {
                $('#AlertInfo').show();
                $('#AlertInfoMsg').text(Zone.Required.StateName);
                $("#ddlState").focus();
                $('#popupModal').scrollTop(0);
                return false;
            }
            if ($.trim($("#txtZoneName").val()) == DefaultSetting.EmptyVal) {
                $('#AlertInfo').show();
                $('#AlertInfoMsg').text(Zone.Required.ZoneName);
                $("#txtZoneName").focus();
                $('#popupModal').scrollTop(0);
                return false;
            }
            if ($("#e").val() == setET(2)) {
                var i = $("#z").val();
                var os = $("#os").val();
                $.ajax({
                    url: "/AdminZone/ChkTransfer",
                    type: "GET",
                    dataType: "json",
                    contentType: "application/json",
                    data: { 'i': '' + i + '', 'cs': '' + $.trim($("#ddlState").val()) + '', 'os': '' + os + '' },
                    async: false,
                    success: function (data) {
                        if (parseInt(data) > 1) {
                            $('#AlertInfo').show();
                            $('#AlertInfoMsg').text(Zone.Validation.ZoneTransferNotPermitted);
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
            var i = $("#z").val();
            $.ajax({
                url: "/AdminZone/ChkZoneDelete",
                type: "GET",
                dataType: "json",
                contentType: "application/json",
                data: { 'i': '' + i + '' },
                async: false,
                success: function (data) {
                    if (parseInt(data) > 0) {
                        alert(Zone.Validation.ZoneDeleteNotPermitted);
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
                $("#ddlState").prop("disabled", false);
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
        $("#lblModalHeader").text("Zone Master :: Edit Record");
        $("#txtZoneName").val(e.data.record.ZoneName);
        $("#z").val(e.data.record.ZoneId);
        $("#os").val(e.data.record.StateId);
        GetStateList(e.data.record.StateId);
        $("#ddlState").prop("disabled", true);
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
        $("#z").val(e.data.record.ZoneId);
        $("#txtZoneName").val(e.data.record.ZoneName);
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
    $("#GridZone").off('DOMSubtreeModified', chkMod);
    grid.reload({ searchString: $.trim($("#txtSearch").val()) });
}

$("#btnReport").click(function () {
    var r = $('input[name="ReportType"]:checked').val();
    $("#reportModal").modal("hide");
    if (r == ReportFormat.Pdf) {
        window.open('../../../../Reporting/Page/MstZone.aspx', '_blank', 'noopener');
        return false;
    }
    return true;
});

window.onbeforeunload = DisableButtons;
