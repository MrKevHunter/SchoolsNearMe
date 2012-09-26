$().ready(function () {
    $(":button").button();

    $(function () {
        var path = location.pathname.substring(1);
        if (path)
            $('.nav li a[href$="' + path + '"]').parent().addClass('active');
        else
            $('.nav li').first().addClass('active');
    });
});