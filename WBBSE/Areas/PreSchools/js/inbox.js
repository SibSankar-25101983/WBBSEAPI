$(".alpha").keypress(function (e) { return alpha(e); });
$("#btnSearch").on("click", Search);
$("#btnSearchNotification").on("click", SearchNotification);
$("#ulTabs a").click(function (e) {
    $("#ulTabs a").removeClass("bg-primary text-light");
    $(this).addClass("bg-primary text-light");
});
$("#ddlCircularType").change(function () {
    $("#txtSearch").val("");
    $("#GridCircular").off('DOMSubtreeModified', chkMod);
    grid.reload({ searchString: "", contentType: $("#ddlCircularType").val() });
    ShowLoader();
});
function Search() {
    $("#GridCircular").off('DOMSubtreeModified', chkMod);
    grid.reload({ searchString: $.trim($("#txtSearch").val()), contentType: $("#ddlCircularType").val() });
    ShowLoader();
}
$("#ddlNotificationType").change(function () {
    $("#txtSearchNotification").val("");
    $("#GridNotification").off('DOMSubtreeModified', chkMod);
    grid1.reload({ searchString: "", contentType: $("#ddlNotificationType").val() });
    ShowLoader();
});
function SearchNotification() {
    $("#GridNotification").off('DOMSubtreeModified', chkMod);
    grid1.reload({ searchString: $.trim($("#txtSearchNotification").val()), contentType: $("#ddlNotificationType").val() });
    ShowLoader();
}
function View(e) {
    try {
        if (e.data.record.LinkType == Content.LinkType.PDF) {
            window.open("../../../Web/PdfViewer?l=" + e.data.record.ContentId, "_blank");
        }
        else {
            window.open(e.data.record.URL, "_blank");
        }

    }
    catch (err) {
        alert(OperationError());
        console.log(err.message);
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
window.onbeforeunload = DisableButtons;
