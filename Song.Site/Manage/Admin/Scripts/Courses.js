$(function () {
    var form = "<form method=\"get\" action=\"../Course/Courses_Edit.aspx\" target=\"_blank\" id=\"formadd\" ></form>";
    $("body").append(form);
    $("input[name$=btnAdd]").click(function () {
        $("#formadd").submit();
        return false;
    });
});