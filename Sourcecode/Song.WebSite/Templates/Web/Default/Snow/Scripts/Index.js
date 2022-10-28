$ready(function () {
    console.log(window.location);
    return;
    var count = 2024;
    for (let i = 1; i <= count; i++) {
        document.write('<span>'+i + ' . ');

        var s = String.fromCharCode(i);
        if (s == '') s = '不可显示的字符';
        document.write(s + ' </span><br/> ');
    }
});