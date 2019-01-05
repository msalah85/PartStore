$('.btnSearch').click(function (e) {
    e.preventDefault();
    var type = $('select[name="searchType"]').val(),
        vl = $('input[name="searchVl"]').val();
    if (vl !== '')
        window.location.href = '/Items/CarInfo?' + type + '=' + vl;
});