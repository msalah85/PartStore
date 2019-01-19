$('.btnSearch').click(function (e) {
    e.preventDefault();
    var type = $('select[name="searchType"]').val(),
        vl = $('input[name="searchVl"]').val();
    if (vl !== '')
        window.location.href = '/Items/CarInfo?' + type + '=' + vl;
});

jQuery('input[name="searchVl"]').bind('keydown.return', function (evt) {
    $('.btnSearch').trigger('click'); return false;
});

jQuery('select[name="searchType"]').bind('keydown.return', function (evt) {
    $('input[name="searchVl"]').focus(); return false;
});