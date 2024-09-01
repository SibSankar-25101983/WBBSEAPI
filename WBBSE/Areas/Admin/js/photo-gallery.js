$("#btnAdd").on("click", Add);
$("#btnSave").on("click", Save);
function readURL1(input) {
    var mode = $("#e").val();
    if (mode == setET(2)) {
        var images = document.getElementsByName("postedFiles");
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('#imgSlider').attr('src', e.target.result);
            }

            reader.readAsDataURL(input.files[0]);
        }
    }
}
function Add() {
    $('#AlertInfo').hide();
    $('#viewImage').hide();
    $("#e").val(setET(1));
    $("#I").val(DefaultSetting.DefaultValEnc);
    $("#lblModalHeader").text("Photo Gallery :: Add Image(s)");
    $("#fuSlider").prop("multiple", "multiple");
    $("#popupModal").modal("show");
}
function Save() {
    if ($("#e").val() != setET(3)) {
        var postedFiles = document.getElementsByName("postedFiles");
        if (postedFiles == null) {
            $('#AlertInfo').show();
            $('#AlertInfoMsg').text(Image.Required.ImageSelection);
            $('#popupModal').scrollTop(0);
            return false;
        }
        if (postedFiles[0].value == null || postedFiles[0].value == "") {
            $('#AlertInfo').show();
            $('#AlertInfoMsg').text(Image.Required.ImageSelection);
            $('#popupModal').scrollTop(0);
            return false;
        }
        for (i = 0; i < postedFiles[0].files.length; i++) {
            var pExt = /[^.]+$/.exec(postedFiles[0].files[i].name);
            if (postedFiles[0].files[i].type.toLowerCase() != "image/jpg" && postedFiles[0].files[i].type.toLowerCase() != "image/jpeg" && postedFiles[0].files[i].type.toLowerCase() != "image/png") {
                $('#AlertInfo').show();
                $('#AlertInfoMsg').text(Image.Validation.ImageExt);
                $('#popupModal').scrollTop(0);
                return false;
            }
        }
        //var pExt = /[^.]+$/.exec(postedFiles[0].value);
        //for (i = 0; i < postedFiles.length; i++) {
        //    if (pExt[i].toLowerCase() != "jpg" && pExt[i].toLowerCase() != "jpeg" && pExt[i].toLowerCase() != "png") {
        //        $('#AlertInfo').show();
        //        $('#AlertInfoMsg').text(Image.Validation.ImageExt);
        //        $('#popupModal').scrollTop(0);
        //        return false;
        //    }
        //}
        //var pSize = postedFiles[0].files[0].size;
        //if (pSize < 0 || pSize > 512000) {
        //    $("#divIInfo").css('display', '');
        //    $("#InfoI").text(UI_Image_Size());
        //    return false;
        //}
    }

    var x = confirm(Msg_Confirm());
    if (x) {
        $("#popupModal").modal("hide");
        ShowLoader();
    }
    return x;
}
function View(e) {
    try {
        var parentNode = $(e).closest('td');
        var src = parentNode[0].getElementsByTagName("img")[0].src;
        $("#imgSliderView").prop("src", src);
        $("#popupModalView").modal("show");
    }
    catch (err) {
        console.log(err);
    }
}
function Edit(e) {
    try {
        var parentNode = $(e).closest('td');
        var src = parentNode[0].getElementsByTagName("img")[0].src;
        $("#imgSlider").prop("src", src);
        $('#AlertInfo').hide();
        $("#e").val(setET(2));
        $("#I").val(parentNode[0].getElementsByTagName("input")[0].value);
        $("#N").val(parentNode[0].getElementsByTagName("input")[1].value);
        $("#popupModal").modal("show");
    }
    catch (err) {
        console.log(err);
    }
}
function Remove(e) {
    try {
        var parentNode = $(e).closest('td');
        var src = parentNode[0].getElementsByTagName("img")[0].src;
        $("#e").val(setET(3));
        $("#I").val(parentNode[0].getElementsByTagName("input")[0].value);
        $("#N").val(parentNode[0].getElementsByTagName("input")[1].value);
        $("#btnSave").click();

    }
    catch (err) {
        console.log(err);
    }
}
window.onbeforeunload = DisableButtons;
