$(".alpha").keypress(function (e) { return alpha(e); });
$("#btnSearch").on("click", Search);
$("#btnAdd").on("click", add);
$("#btnSave").on("click", save);
setTimeout(toggleInfo, 5000);
setTimeout(toggleInfo, 5000);
function clearData() {
    $('#divPopupBody input').val('');
}
function getCheckYN() {
    var checkYN = "N";
    $.ajax({
        url: "../../../../Common/CheckYN",
        type: "GET",
        dataType: "json",
        contentType: "application/json",
        async: false,
        success: function (data) {
            checkYN = data;
        },
        error: function (error) {
            checkYN = "N";
            console.log(error.responseText);
        }
    });
    return checkYN;
}
function getRDData(p, g) {
    $.ajax({
        url: "/AdminUserGroup/GetRoleList",
        type: "GET",
        dataType: "json",
        contentType: "application/json",
        async: true,
        data: { 'p': '' + p + '', 'm': '' + $("#e").val() + '', 'g': '' + g + '' },
        success: function (data) {
            $("#gridRoleDetails").append(data);
        },
        error: function (error) {
            console.log(error.responseText);
        }
    });
}
function add() {
    try {
        clearData();
        $('#AlertInfo').hide();
        $("#e").val(setET(1));
        var checkYN = getCheckYN();
        if (checkYN == "Y") {
            $("#divUserType").css("display", "block");
            $("#ddlGroupType").empty();
            $("#ddlGroupType").append(new Option("Select Group Type", "-1"));
            $.ajax({
                url: "/AdminUserGroup/GetUserTypeList",
                type: "GET",
                dataType: "json",
                contentType: "application/json",
                async: true,
                success: function (data) {
                    $(data).each(function (index, item) {
                        var opt = new Option(item.UserType, item.UserTypeId);
                        $("#ddlGroupType").append(opt);
                    });
                    $("#ddlGroupType").prop('disabled', false);
                },
                error: function (error) {
                    console.log(error.responseText);
                }
            });
        }
        else {
            $("#divUserType").css("display", "none");
        }

        $("#ddlRoleDetailsFilter").empty();
        $.ajax({
            url: "/AdminUserGroup/GetParentRoleList",
            type: "GET",
            dataType: "json",
            contentType: "application/json",
            async: true,
            success: function (data) {
                $(data).each(function (index, item) {
                    var opt = new Option(item.RoleName, item.RoleId);
                    $("#ddlRoleDetailsFilter").append(opt);
                });
            },
            error: function (error) {
                console.log(error.responseText);
            }
        });

        //GET ROLE DETAILS DATA
        $("#gridRoleDetails").empty();
        getRDData('', '');

        $("#userGroupPermissionModal").modal("show");
    }
    catch (err) {
        alert(OperationError());
    }
}
$("#ddlRoleDetailsFilter").change(function () {
    var rows = document.getElementById("gridRoleDetails").getElementsByTagName("tr");
    var p = $("#ddlRoleDetailsFilter").val();
    if (p == "c0") {
        for (var i = 1; i < rows.length; i++) {
            rows[i].style.display = "";
        }
    }
    else {
        //var rem = rows.filter('.' + p).show();
        //rows.not(rem).hide();
        //var head = rows.filter('.chead').show();
        //head.show();

        for (var i = 1; i < rows.length; i++) {
            if (p == rows[i].className) {
                rows[i].style.display = "";
            }
            else {
                rows[i].style.display = "none";
            }
        }
    }
});
$('#chkActiveYN').click(function () {
    if ($(this).prop("checked")) {
        $(this).val("Y");
    }
    else {
        $(this).val("N");
    }
});
$('#gridRoleDetails').click(function (e) {
    if (e.target.type == "checkbox") {
        var ref = e.target;
        var name = $(ref).prop("id");
        if (name != "chkViewAll" && name != "chkAddAll" && name != "chkEditAll" && name != "chkDeleteAll" && name != "chkReportAll" && name != "chkSystemAll") {
            if ($(ref).prop("checked")) {
                $(ref).val("Y");
            }
            else {
                $(ref).val("N");
            }
        }
        else {
            var rows = document.getElementById("gridRoleDetails").getElementsByTagName("tr");
            var p = $("#ddlRoleDetailsFilter").val();
            var pos = ref.parentNode.parentNode.cellIndex;

            if (p == "c0") {
                for (var i = 1; i < rows.length; i++) {
                    if ($(ref).prop("checked")) {
                        rows[i].cells[pos].getElementsByTagName("input")[0].checked = true;
                        rows[i].cells[pos].getElementsByTagName("input")[0].value = "Y";
                    }
                    else {
                        rows[i].cells[pos].getElementsByTagName("input")[0].checked = false;
                        rows[i].cells[pos].getElementsByTagName("input")[0].value = "N";
                    }
                }
            }
            else {
                for (var i = 1; i < rows.length; i++) {
                    if (p == rows[i].className) {
                        if ($(ref).prop("checked")) {
                            rows[i].cells[pos].getElementsByTagName("input")[0].checked = true;
                            rows[i].cells[pos].getElementsByTagName("input")[0].value = "Y";
                        }
                        else {
                            rows[i].cells[pos].getElementsByTagName("input")[0].checked = false;
                            rows[i].cells[pos].getElementsByTagName("input")[0].value = "N";
                        }
                    }
                }
            }
        }
    }
});
function save() {
    try {
        $('#AlertInfoUL').empty();
        var flag = 0;
        var checkYN = getCheckYN();
        if (checkYN == "E") {
            $('#AlertInfo').show();
            $('#AlertInfoUL').append("<li><span class='tab'>" + "Operation Error. Error Code : " + gSessionError() + "</span></li>");
            $('#userGroupPermissionModal').scrollTop(0);
            return false;
        }
        if ($("#e").val() != setET(3)) {
            if ($.trim($("#txtGroupName").val()) == DefaultSetting.EmptyVal) {
                $('#AlertInfo').show();
                $('#AlertInfoUL').append("<li><span class='tab'>" + AUG_GroupName_Required() + "</span></li>");
                $("#txtGroupName").focus();
                $('#userGroupPermissionModal').scrollTop(0);
                return false;
            }
            if (!chkDataFormat(RegexType.Alpha, $.trim($("#txtGroupName").val()))) {
                $('#AlertInfo').show();
                $('#AlertInfoUL').append("<li><span class='tab'>" + AUG_Invalid_GroupName() + "</span></li>");
                $("#txtGroupName").focus();
                $('#userGroupPermissionModal').scrollTop(0);
                return false;
            }
            if ($.trim($("#txtGroupName").val()).length > 100) {
                $('#AlertInfo').show();
                $('#AlertInfoUL').append("<li><span class='tab'>" + AUG_GroupName_Max_Length() + "</span></li>");
                $("#txtGroupName").focus();
                $('#userGroupPermissionModal').scrollTop(0);
                return false;
            }
            if (checkYN == "Y") {
                if ($("#ddlGroupType").val() == "-1") {
                    $('#AlertInfo').show();
                    $('#AlertInfoUL').append("<li><span class='tab'>" + UserType_Sel_Required() + "</span></li>");
                    $("#ddlGroupType").focus();
                    $('#userGroupPermissionModal').scrollTop(0);
                    return false;
                }
            }
        }
        if ($("#e").val() == setET(3)) {
            var g = $("#g").val();
            $.ajax({
                url: "/AdminUserGroup/ChkUserGroupDelete",
                type: "GET",
                dataType: "json",
                contentType: "application/json",
                data: { 'g': '' + g + '' },
                async: false,
                success: function (data) {
                    if (parseInt(data) > 0) {
                        alert(AUG_DeleteNotPermitted());
                        flag = 1;
                        return false;
                    }
                },
                error: function (error) {
                    console.log(error.responseText);
                }
            });
        }
        if (flag == 1) {
            return false;
        }
        else {
            var x = confirm(Msg_Confirm());
            if (x) {
                $("#ddlGroupType").prop('disabled', false);
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
        $("#txtGroupName").val(e.data.record.GroupName);
        $("#g").val(e.data.record.GroupId);
        if (e.data.record.ActiveYN == "Y") {
            $('#chkActiveYN').prop("checked", true);
            $('#chkActiveYN').val("Y");
        }
        else {
            $('#chkActiveYN').val("N");
        }
        var checkYN = getCheckYN();
        if (checkYN == "Y") {
            $("#divUserType").css("display", "block");
            $("#ddlGroupType").empty();
            $("#ddlGroupType").append(new Option("Select Group Type", "-1"));
            $.ajax({
                url: "/AdminUserGroup/GetUserTypeList",
                type: "GET",
                dataType: "json",
                contentType: "application/json",
                async: true,
                success: function (data) {
                    $(data).each(function (index, item) {
                        var opt = new Option(item.UserType, item.UserTypeId);
                        $("#ddlGroupType").append(opt);
                    });
                    $("#ddlGroupType").val(e.data.record.UserTypeId);
                    $("#ddlGroupType").prop('disabled', true);
                },
                error: function (error) {
                    console.log(error.responseText);
                }
            });
        }
        else {
            $("#divUserType").css("display", "none");
        }
        $("#ddlRoleDetailsFilter").empty();
        $.ajax({
            url: "/AdminUserGroup/GetParentRoleList",
            type: "GET",
            dataType: "json",
            contentType: "application/json",
            async: true,
            success: function (data) {
                $(data).each(function (index, item) {
                    var opt = new Option(item.RoleName, item.RoleId);
                    $("#ddlRoleDetailsFilter").append(opt);
                });
            },
            error: function (error) {
                console.log(error.responseText);
            }
        });
        $("#gridRoleDetails").empty();
        getRDData('', e.data.record.GroupId);
        $("#userGroupPermissionModal").modal("show");
    }
    catch (err) {
        alert(OperationError());
    }
}
function Remove(e) {
    try {
        $("#e").val(setET(3));
        $("#g").val(e.data.record.GroupId);
        $("#txtGroupName").val("dss237OknYMinxmsXxwqfg==");
        $("#ddlGroupType").append(new Option(e.data.record.UserTypeId, e.data.record.UserTypeId));
        $("#ddlGroupType").val(e.data.record.UserTypeId);
        $("#btnSave").click();
    }
    catch (err) {
        alert(OperationError());
    }
}
function toggleInfo() {
    $("#divS").css("display", "none");
    $("#divE").css("display", "none");
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
function Search() {
    $("#gridUserGroup").off('DOMSubtreeModified', chkMod);
    grid.reload({ searchString: $.trim($("#txtSearch").val()) });
}
window.onbeforeunload = DisableButtons;
