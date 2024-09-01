$(".alpha").keypress(function (e) { return alpha(e); });
$("#btnSearch").on("click", Search);
function ClearViewData() {
    $("#tblViewData").empty();
}
$("#ddlSearchType").change(function () {
    if ($(this).val() == "3") {
        $("#txtSearch").autocomplete("option", "disabled", false);
    }
    else {
        $("#txtSearch").autocomplete("option", "disabled", true);
    }
});
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
    }
});
function Search() {
    $("#GridData").off('DOMSubtreeModified', chkMod);
    grid.reload({ searchString: $.trim($("#txtSearch").val()), searchType: $("#ddlSearchType").val() });
}
function View(e) {
    try {
        var r = e.data.record.RollNo;

        $.ajax({
            url: "/AdminPostPublicationApplication/GetApplicationDetails",
            type: "GET",
            dataType: "json",
            data: { 'r': '' + r + '' },
            contentType: "application/json",
            async: true,
            success: function (data) {
                $("#tblViewData").append(data.View);
                $("#popupModalView").modal("show");
            },
            error: function (error) {
                alert(OperationError());
                console.log(error.responseText);
            }
        });
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
