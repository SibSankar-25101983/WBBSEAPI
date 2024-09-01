$(".alpha").keypress(function (e) { return alpha(e); });
$("#btnSearch").on("click", Search);
$("#ddlNoticeType").change(function () {
    $("#txtSearch").val("");
    $("#GridNotice").off('DOMSubtreeModified', chkMod);
    grid.reload({ searchString: "", contentType: $("#ddlNoticeType").val() });
    ShowLoader();
});
function Search() {
    $("#GridNotice").off('DOMSubtreeModified', chkMod);
    grid.reload({ searchString: $.trim($("#txtSearch").val()), contentType: $("#ddlNoticeType").val() });
    ShowLoader();
}
function View(e) {
    try {
        if (e.data.record.LinkType == Content.LinkType.PDF) {
            window.open("../../../Web/NoticeViewer?l=" + e.data.record.InboxId, "_blank", "noopener");
        }
        else if (e.data.record.LinkType == Content.LinkType.URL) {
            window.open(e.data.record.URL, "_blank", "noopener");
        }
        else {
            return;
        }
    }
    catch (err) {
        alert(OperationError());
        console.log(err.message);
    }
}
function UnreadNoticeListStatus() {
    try {
        $.ajax({
            url: "/PreSchoolNotice/UnreadNoticeListStatus",
            type: "GET",
            dataType: "json",
            contentType: "application/json",
            async: true,
            success: function (data) {
                $(".hide-unread").delay(3000).fadeOut(1500);
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
