$(".alpha").keypress(function (e) { return alpha(e); });
$("#btnSearch").on("click", Search);
if (screen.availWidth <= 1130.0) {
    $("#GridTender").addClass("table-responsive");
}
else {
    $("#GridTender").removeClass("table-responsive");
}
var chkMod = function (e) {
    try {
        if (e.target.parentElement.nodeName.toLowerCase() == "td") {
            console.log(e);
            window.location.href = "../../../../Error/Unexpected.html";
        }
    }
    catch (err) {
        console.log(err.responseText);
    }
};

$("#ddlTenderType").change(function () {
    $("#txtSearch").val("");
    $("#GridTender").off('DOMSubtreeModified', chkMod);
    grid.reload({ searchString: "", contentType: $("#ddlTenderType").val() });
    ShowLoader();
});

function Search() {
    $("#GridTender").off('DOMSubtreeModified', chkMod);
    grid.reload({ searchString: $.trim($("#txtSearch").val()), contentType: $("#ddlTenderType").val() });
    ShowLoader();
}

function View(e) {
    if (e.data.record.LinkType == Content.LinkType.PDF) {
        window.open("/Web/PdfViewer?l=" + e.data.record.ContentId, "_blank", "noopener");
    }
    else {
        window.open(e.data.record.URL, "_blank", "noopener");
    }
}
