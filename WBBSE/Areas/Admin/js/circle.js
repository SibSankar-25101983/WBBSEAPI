$(".alpha").keypress(function (e) { return alpha(e); });
$("#ddlBlock").select2({ width: 'resolve', theme: "classic" });
$("#btnSearch").on("click", search);
$("#btnAdd").on("click", add);
$("#btnSave").on("click", save);

function clearData() {
    $('#divPopupBody input').val(DefaultSetting.EmptyVal);
    $('#divPopupBody select').val(DefaultSetting.DefaultValEnc);
    $("#ddlBlock").prop("disabled", false);
}

function toggleInfo() {
    $("#divS").css("display", "none");
    $("#divE").css("display", "none");
}

//Populate Block in DropDown List
function GetBlockList(d) {
    try {
        $("#divLblBlock").css("display", "block");
        $("#divBlock").css("display", "block");
        $("#ddlBlock").empty();
        $("#ddlBlock").append(new Option("Select Block", DefaultSetting.DefaultValEnc));
        $.ajax({
            url: "/AdminCircle/GetBlockList",
            type: "GET",
            dataType: "json",
            contentType: "application/json",
            async: true,
            success: function (data) {
                $(data).each(function (index, item) {
                    var opt = new Option(item.BlockName, item.BlockId);
                    $("#ddlBlock").append(opt);
                });
                $("#ddlBlock").val(d);
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
    $("#lblModalHeader").text("Circle Master :: Add Record");
    GetBlockList(DefaultSetting.DefaultValEnc);
    $("#popupModal").modal("show");
}

function save() {
    try {
        var flag = 0;
        if ($("#e").val() != setET(3)) {
            $('#AlertInfoMsg').text("");

            if ($("#ddlBlock").val() == DefaultSetting.DefaultValEnc) {
                $('#AlertInfo').show();
                $('#AlertInfoMsg').text(Circle.Required.BlockName);
                $("#ddlBlock").focus();
                $('#popupModal').scrollTop(0);
                return false;
            }
            if ($.trim($("#txtCircleName").val()) == DefaultSetting.EmptyVal) {
                $('#AlertInfo').show();
                $('#AlertInfoMsg').text(Circle.Required.CircleName);
                $("#txtCircleName").focus();
                $('#popupModal').scrollTop(0);
                return false;
            }
        }
        else {
            var i = $("#c").val();
            $.ajax({
                url: "/AdminCircle/ChkCircleDelete",
                type: "GET",
                dataType: "json",
                contentType: "application/json",
                data: { 'i': '' + i + '' },
                async: false,
                success: function (data) {
                    if (parseInt(data) > 0) {
                        alert(Circle.Validation.CircleDeleteNotPermitted);
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
                $("#ddlBlock").prop("disabled", false);
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
        $("#lblModalHeader").text("Circle Master :: Edit Record");
        $("#txtCircleName").val(e.data.record.CircleName);
        $("#c").val(e.data.record.CircleId);
        GetBlockList(e.data.record.BlockId);
        $("#ddlBlock").prop("disabled", true);
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
        $("#c").val(e.data.record.CircleId);
        $("#txtCircleName").val(e.data.record.CircleName);
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
    $("#GridCircle").off('DOMSubtreeModified', chkMod);
    grid.reload({ searchString: $.trim($("#txtSearch").val()) });
}

$("#btnReport").click(function () {
    var r = $('input[name="ReportType"]:checked').val();
    $("#reportModal").modal("hide");
    if (r == ReportFormat.Pdf) {
        window.open('../../../../Reporting/Page/MstCircle.aspx', '_blank', 'noopener');
        return false;
    }
    return true;
});

window.onbeforeunload = DisableButtons;
