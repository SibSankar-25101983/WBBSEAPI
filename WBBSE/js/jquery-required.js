$(document).on('focus', ':input', function () { $(this).attr('autocomplete', 'off'); });
$("body").bind('cut copy paste', function (e) {e.preventDefault();});
$("body").on("contextmenu", function (e) {return false;});
$("body").keydown(function (e) {if (e.ctrlKey) { return false; }});
$("a").click(function (e) { if (e.ctrlKey) { return false; } });
if (window.IsDuplicate()) { window.location.href = "../../../../Error/MultiTab.html"; };
