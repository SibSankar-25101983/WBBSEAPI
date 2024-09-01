/*
    ASCII CODES :-
    8 - BACKSPACE
    9 - HORIZONTAL TAB
    37 - %
    39 - '
    45 : - (minus)
    46 - .
*/

/*SPECIAL CHARACTERS*/
function checkSpecialChar(event) {
    var key = String.fromCharCode(event.which);
    if (key == '`' || key == '~' || key == '!' || key == '@' || key == '$' || key == '%' || key == '^' || key == '*' || key == '_' || key == '=' || key == '+' || key == '\\' || key == '|' || key == '{'
       || key == '}' || key == '[' || key == ']' || key == ':' || key == ';' || key == '"' || key == '\'' || key == '?' || key == '>' || key == '<') {
        return false;
    }
    return true;
}
/********************/
/*FLOAT NUMBERS CHECKING*/
function CheckFloat(e, t) {
    e = (e) ? e : window.event;
    var charCode = (e.which) ? e.which : e.keyCode;
    if (charCode == 9)
        return true;
    else {
        if (charCode > 31 && charCode != 37 && charCode != 39 && charCode != 46 && (charCode < 48 || charCode > 57))
            return false;
        else {
            if (t.value == '' || (t.value == '0' && charCode != 46))
                t.value = '';
            if (t.value == '00' || (t.value == '0' && charCode == 46))
                t.value = '0';
            var string = t.value;
        }
    }
}
/************************/
/*DATE OF BIRTH DATE FORMATION*/
function CalDOB(vAge) {
    var currDate = new Date();
    var currDay = currDate.getDate();
    var currMonth = currDate.getMonth() + 1;
    var currYear = currDate.getFullYear();
    var ModYear = currYear - vAge;
    currDay = (currDay < 10) ? ("0" + currDay) : currDay;
    currMonth = (currMonth < 10) ? ("0" + currMonth) : currMonth;
    return (currDay + "/" + currMonth + "/" + ModYear);
}
/******************************/
/*AGE CALCULATION*/
function CalAge(vDOB) {
    var datearray = vDOB.split("/");
    var DOBDate = datearray[1] + '/' + datearray[0] + '/' + datearray[2];
    var YearDiff = new Date().getFullYear() - new Date(DOBDate).getFullYear();
    return YearDiff;
}
/*****************/
/*ALPHA NUMERIC CHECKING*/
function alpha(event) {
    var regFormat = /[A-Za-z0-9 \/\-()_,.:]/g;
    var key = String.fromCharCode(event.which);
    if (event.keyCode == 8 || event.keyCode == 9 || (event.keyCode == 46) || regFormat.test(key)) {
        return true;
    }
    return false;
}
/************************/
/*ALPHA WITH SPECIAL CHARECTER CHECKING*/
function OnlyAlphaWithSpecialChar(event) {
    var regFormat = /[A-Za-z \-(),&]/g;
    var key = String.fromCharCode(event.which);
    if (event.keyCode == 8 || event.keyCode == 9 || (event.keyCode == 46) || regFormat.test(key)) {
        return true;
    }
    return false;
}
/************************/

/*ALPHA NUMERIC WITHOUT SPACE*/
function alphaWithoutSpace(event) {
    var regFormat = /[A-Za-z0-9]/g;
    var key = String.fromCharCode(event.which);
    if (event.keyCode == 8 || event.keyCode == 37 || event.keyCode == 9 || (event.keyCode == 46) || regFormat.test(key)) {
        return true;
    }
    return false;
}
/*****************************/
/*ONLY ALPHABET CHECKING*/
function OnlyAlpha(event) {
    var regFormat = /[A-Za-z ]/g;
    var key = String.fromCharCode(event.which);
    if (event.keyCode == 8 || event.keyCode == 9 || regFormat.test(key)) {
        return true;
    }
    return false;
}
/************************/
/*EMAIL ID ALLOW CHARACTERS*/
function allowCharForEmailId(event) {
    var allowed = /[A-Za-z0-9._@\-]/g;
    var key = String.fromCharCode(event.which);
    if (event.keyCode == 8 || event.keyCode == 9 || (event.keyCode == 46) || allowed.test(key)) {
        return true;
    }
    return false;
}

/*WEBSITE ALLOW CHARECTER*/
function allowCharForWebsite(event) {
    var regFormat = /[A-Za-z0-9 \/.\:\?=]/g;
    var key = String.fromCharCode(event.which);
    if (event.keyCode == 8 || (event.keyCode == 46) || regFormat.test(key)) {
        return true;
    }
    return false;
}

function allowCharsForPassword(event) {
    var validate = /^[A-Za-z0-9@$#!%*?&\-_]+$/g;
    var key = String.fromCharCode(event.which);
    if (validate.test(key)) {
        return true;
    }
    return false;
}

/***************************/
/*CHECK PASSWORD MINIMUM STRENGTH*/
function checkPassword(password) {
    var reg_Password = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$#!%*?&\-_])[A-Za-z\d@$#!%*?&\-_]{8,}$/g;
    if (reg_Password.test(password)) {
        return true;
    }
    else {
        return false;
    }
}
/*********************************/
/*CHECK FOR INTEGER NUMBERS*/
function checkContactNo(event) {
    var exp = /^[0-9]/g;
    var key = String.fromCharCode(event.which);
    if (event.keyCode == 8 || event.keyCode == 9 || exp.test(key)) {
        return true;
    }
    return false;
}
function checkPrice(event) {
    var exp = /^[0-9.]/g;
    var key = String.fromCharCode(event.which);
    if (event.keyCode == 8 || event.keyCode == 9 || exp.test(key)) {
        return true;
    }
    return false;
}
function checkPhoneNo(event) {
    var exp = /^[0-9\-]/g;
    var key = String.fromCharCode(event.which);
    if (event.keyCode == 8 || event.keyCode == 9 || exp.test(key)) {
        return true;
    }
    return false;
}
/***************************/
/*ADDRESS WITH MULTILINE INPUT*/
function checkAddress(event) {
    if (event.which == 13) {
        return true;
    }
    var validate = /^[A-Za-z0-9,._ \/\-()-?]+$/g;
    var key = String.fromCharCode(event.which);
    if (validate.test(key)) {
        return true;
    }
    return false;
}
/******************************/
/*DATE FORMAT TYPE*/
function checkDateFormat() {
    var exp = /^[0-9\/]/g;
    var key = String.fromCharCode(event.which);
    if (event.keyCode == 8 || event.keyCode == 9 || exp.test(key)) {
        return true;
    }
    return false;
}
/******************/
/*URL FORMAT CHECK*/
function isUrlValid(userInput) {
    var res = userInput.match(/(http(s)?:\/\/.)?(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)/g);
    if (res == null)
        return false;
    else
        return true;
}
/******************/

function ContentEdit(event) {
    if (event.which == 13) {
        return true;
    }
    var validate = /^[A-Za-z0-9,._ \/\-()?<>&;#\']+$/g;
    var key = String.fromCharCode(event.which);
    if (validate.test(key)) {
        return true;
    }
    return false;
}

function isLeapYear(year) {
    return (year % 100 === 0) ? (year % 400 === 0) : (year % 4 === 0);
}

function isDate(data) {
    try {
        var date_split = data.split("-");
        var dd = date_split[0];
        var mm = date_split[1];
        var yy = date_split[2];
        var date_format = yy + "-" + mm + "-" + dd;
        var d = Date.parse(date_format);
        if (d == NaN) {
            return false;
        }
        var dd_num = parseInt(dd);
        switch (parseInt(mm)) {
            case 1:
            case 3:
            case 5:
            case 7:
            case 8:
            case 10:
            case 12:
                if (dd_num > 31) {
                    return false;
                }
                break;
            case 2:
                if (isLeapYear(parseInt(yy))) {
                    if (dd_num > 29) {
                        return false;
                    }
                }
                else {
                    if (dd > 28) {
                        return false;
                    }
                }
                break;
            case 4:
            case 6:
            case 9:
            case 11:
                if (dd_num > 30) {
                    return false;
                }
                break;
            default:
                return false;
        }
        return true;
    }
    catch (err) {
        console.log(err);
        return false;
    }
}

function ToggleInfo() {
    $("#divS").css("display", "none");
    $("#divE").css("display", "none");
}

function ShowLoader() {
    $("#preloader").css('display', '');
}

function HideLoader() {
    $("#preloader").css('display', 'none');
}

function ShowLoaderNew() {
    $.LoadingOverlay("show");
}

function HideLoaderNew() {
    $.LoadingOverlay("hide");
}

function DisableButtons() {
    var id = "NA";
    try {
        id = document.activeElement.id;
    }
    catch (err) {
        //nothing to do. id not found
    }
    if (id != "btnReport") {
        var inputs = document.getElementsByTagName("button");
        for (var i in inputs) {
            inputs[i].disabled = true;
        }
    }
}

function EnableButtons() {
    var inputs = document.getElementsByTagName("button");
    for (var i in inputs) {
        inputs[i].disabled = false;
    }
}

var DefaultSetting = {
    DefaultVal: "-1",
    DefaultValEnc: "LTE=",
    EmptyVal: "",
    DefaultErrVal: "1",
    DefaultValY: "Y",
    DefaultValN: "N",
    DefaultValT: true,
    DefaultValF: false,
    ErrValS: "E"
};

var RegexType = {
    Alpha: "Alpha",
    AlphaWithoutSpace: "AlphaWithoutSpace",
    Numeric: "Numeric",
    EmailId: "EmailId",
    MobileNo: "MobileNo",
    URL: "URL",
    OnlyAlpha: "OnlyAlpha",
    PinCode: "PinCode",
    StdCode: "StdCode",
    PhoneNo: "PhoneNo",
    Date: "Date",
    ContentEdit: "ContentEdit",
    CustomDOB: "CustomDOB",
    RollNo: "RollNo",
    SchoolIndexNo: "SchoolIndexNo",
    Captcha: "Captcha",
    Price: "Price"
};

var ProfileVerificationType = {
    PasswordChange: "P",
    EmailOTPSend: "EG",
    EmailOTPResend: "ER",
    EmailConfirmation: "EC",
    ContactNoOTPSend: "CG",
    ContactNoOTPResend: "CR",
    ContactNoConfirmation: "CC"
};


function chkDataFormat(type, data) {
    var reg_data;
    switch (type) {
        case RegexType.Alpha:
            reg_data = /^[A-Za-z0-9 \/\-()_,.:]+$/g;
            if (!reg_data.test(data)) {
                return false;
            }
            break;
        case RegexType.AlphaWithoutSpace:
            reg_data = /[A-Za-z0-9]/g;
            if (!reg_data.test(data)) {
                return false;
            }
            break;
        case RegexType.Numeric:
            reg_data = /^[0-9]+$/g;
            if (!reg_data.test(data)) {
                return false;
            }
            break;
        case RegexType.EmailId:
            reg_data = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
            if (!reg_data.test(data)) {
                return false;
            }
            break;
        case RegexType.MobileNo:
            reg_data = /^[56789][0-9]{9}$/g;
            if (!reg_data.test(data)) {
                return false;
            }
            break;
        case RegexType.URL:
            reg_data = /^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$/g;
            if (!reg_data.test(data)) {
                return false;
            }
            break;
        case RegexType.OnlyAlpha:
            reg_data = /^[A-Za-z ]+$/g;
            if (!reg_data.test(data)) {
                return false;
            }
            break;
        case RegexType.PinCode:
            reg_data = /^([0-9]{6})$/;
            if (!reg_data.test(data)) {
                return false;
            }
            break;
        case RegexType.StdCode:
            reg_data = /^([0-9]{1,10})$/;
            if (!reg_data.test(data)) {
                return false;
            }
            break;
        case RegexType.PhoneNo:
            reg_data = /^([0-9]{8})$/;
            if (!reg_data.test(data)) {
                return false;
            }
            break;
        case RegexType.Date:
            reg_data = /^([0-9]{2})-([0-9]{2})-([0-9]{4})$/;
            if (!reg_data.test(data)) {
                return false;
            }
            if (!isDate(data)) {
                return false;
            }
            break;
        case RegexType.ContentEdit:
            reg_data = /^[A-Za-z0-9,._ \/\-()?<>&#;\'\n]+$/g;
            if (!reg_data.test(data)) {
                return false;
            }
            break;
        case RegexType.CustomDOB:
            reg_data = /^([0-9]{6})$/;
            if (!reg_data.test(data)) {
                return false;
            }
            break;
        case RegexType.RollNo:
            reg_data = /^([A-Za-z0-9]{11})$/;
            if (!reg_data.test(data)) {
                return false;
            }
            break;
        case RegexType.SchoolIndexNo:
            reg_data = /^([A-Za-z0-9]{5})$/;
            if (!reg_data.test(data)) {
                return false;
            }
            break;
        case RegexType.Captcha:
            reg_data = /^([A-Za-z0-9]{6})$/;
            if (!reg_data.test(data)) {
                return false;
            }
            break;
        case RegexType.Price:
            reg_data = /^[0-9.]+$/g;
            if (!reg_data.test(data)) {
                return false;
            }
            break;
        default:
            return false;
    }
    return true;
}

var MacDownloadType = {
    Token: "T",
    TokenManual: "TM",
    Authorization: "A",
    AuthorizationManual: "AM"
};

function chkFooter() {
    try {
        $("#fWebsite").removeClass("custom-footer");
        var ah = screen.availHeight;
        var fp = $("#fWebsite")[0].offsetTop;
        var fh = $("#fWebsite").height();
        //console.log("available height : " + ah);
        //console.log("footer offset : " + fp);
        //console.log("footer height : " + fh);
        if ((fp - fh + 200) < ah) {
            $("#fWebsite").addClass("custom-footer");
        }
        else {
            $("#fWebsite").removeClass("custom-footer");
        }
    }
    catch (err) {
        console.log(err.message);
    }
}
