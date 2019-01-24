$(window).on("load", function () {
    $('input#PartName').focus();

    function getPrice(amount) {
        return amount.replace(/[^0-9\.]/g, '');
    }

    // Get Item costs
    var _totalSalePrice = getPrice($('.total-sale-price').text()) * 1,
        _buyPrice = getPrice($('.buy-price').text()) * 1,
        _totalCosts = 0;

    $('.table tbody tr').each(function (i, el) {
        var tr = $(this),
            carSalePrice = getPrice(tr.find('td:eq(3)').text()) * 1,
            rateMargin = (carSalePrice / _totalSalePrice),
            partCost = (rateMargin * _buyPrice),
            part_Costs = parseFloat(partCost).toFixed(2) * 1;

        _totalCosts = (_totalCosts + part_Costs);
        tr.find('td:eq(4) span.money').text(part_Costs);
    }).promise().done(function () {
        $('.total-costs').text(_totalCosts.toFixed(2));
        $('.money').autoNumeric();
        });
});


$('.select2').select2({
    placeholder: lang.PleaseChoose,
    language: "ar",
    allowClear: true,
    theme: "bootstrap",
    dir: "rtl",
    "ajax--cache": true
});

function showColumns(e) {
    $('.col').toggleClass('hidden');
    var _span = $('#toggleColumns').find('span');

    if (_span.hasClass('glyphicon-minus-sign')) {
        _span.removeClass('glyphicon-minus-sign').addClass('glyphicon-plus-sign');
    } else {
        _span.removeClass('glyphicon-plus-sign').addClass('glyphicon-minus-sign');
    }
}