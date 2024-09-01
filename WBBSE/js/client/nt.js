$(".alpha").keypress(function (e) { return alpha(e); });
$("#btnSearch").on("click", Search);
//Populate Notification Type in DropDown List
function GetNotificationTypeList(d) {
    try {
        $("#ddlNotificationType").empty();
        $("#ddlNotificationType").append(new Option("All", DefaultSetting.DefaultValEnc));
        $.ajax({
            url: "../../Common/GetNotificationTypeList",
            type: "GET",
            dataType: "json",
            contentType: "application/json",
            async: true,
            success: function (data) {
                $(data).each(function (index, item) {
                    var opt = new Option(item.NotificationType, item.MenuCode);
                    $("#ddlNotificationType").append(opt);
                });
                $("#ddlNotificationType").val(d);
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
function showGrid() {
    $("#divGrid").show();
}
function Search() {
    showGrid();
    $("#ddlNotificationType").val(DefaultSetting.DefaultValEnc);
    $("#GridNotification").off('DOMSubtreeModified', chkMod);
    //grid.reload({ searchString: $.trim($("#txtSearch").val()) });
    grid.reload({ searchString: $.trim($("#txtSearch").val()), sd: DefaultSetting.DefaultValEnc });
    ShowLoader();
}
$("#ddlNotificationType").change(function () {
    $("#txtSearch").val("");
    showGrid();
    $("#GridNotification").off('DOMSubtreeModified', chkMod);
    grid.reload({ searchString: "", sd: $("#ddlNotificationType").val() });
    ShowLoader();
});
function View(e) {
    if (e.data.record.LinkType == Content.LinkType.PDF) {
        window.open("/Web/PdfViewer?l=" + e.data.record.ContentId, "_blank", "noopener");
    }
    else {
        window.open(e.data.record.URL, "_blank", "noopener");
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
