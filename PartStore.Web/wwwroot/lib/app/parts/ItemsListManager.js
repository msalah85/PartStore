$.fn.addMoreFilters = function (data) {
    var _saleType = $('#car-sale-type').val();

    if (_saleType != '') {
        return $.extend({}, data, {
            sold: _saleType
        });
    }
    else
        return;
};

$.fn.gvFooterCall = function (tfoot, data, start, end, display) {
    var
        _total = 0,
        api = $(data).map(function (i, vl) {
            _total += vl.lastCost;
            return vl.lastCost;
        });

    $(tfoot).find('th').eq(1).html('<span class="money">' + _total + '</span>');
    $('.money').autoNumeric();
};

var gridId = 'tblList',
    sUrl = 'items',
    modalDialog = 'myModal',
    deleteModalDialog = 'myDelModal',
    gridColumns = [{ "mDataProp": "itemId" },
    {
        "render": function (data, type, row, meta) {
            return '<a href="/Photos/Index/' + row.itemId + '" title="' + lang.Photos + '">\
                                    <figure class="deal-thumbnail embed-responsive" data-bg-img="bg"\
                                            style="background-image: url(\'/public/'+ (row.photos.length > 0 ? row.photos[0].url : '') + '");\'>\
                                    </figure></a>';
        }
    },
    {
        "render": function (data, type, row, meta) {
            return (row.make != null ? row.make.makeName : '') + ' ' + (row.model ? row.model.modelName : '') + ' ' + row.yearId;
        }
    },
    { "mDataProp": "vin" },
    { "mDataProp": "lotNo" },
    {
        "mDataProp": "modelId",
        "render": function (data, type, row, meta) {
            return '<span class="money">' + row.lastCost + '</span>';
        }
    },
    {
        "mDataProp": "more",
        "render": function (data, type, row, meta) {
            var saleLnk = '<li><a href="items/sale/' + row.itemId + '"><span class="glyphicon glyphicon-ok-sign text-success"></span> ' + lang.Sale + '</a></li>';

            return '<div class="dropdown">\
                        <button class="btn btn-default dropdown-toggle" type="button" id="dropdownMenu1" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">\
                            ' + lang.Options + '<span class="caret"></span>\
                        </button>\
                        <ul class="dropdown-menu pull-right" aria-labelledby="dropdownMenu1">\
                            <li><a href="photos/index/' + row.itemId + '"><span class="glyphicon glyphicon-picture text-info"></span> ' + lang.Photos + '</a></li>\
                            <li><a href="Car/' + row.itemId + '"><span class="glyphicon glyphicon-list-alt text-info"></span> المبيعات</a></li>\
                            <li role="separator" class="divider"></li>\
                            <li><a href="items/edit/' + row.itemId + '"><span class="glyphicon glyphicon-edit text-info"></span> ' + lang.Edit + '</a></li>\
                            <li><a href="items/delete/' + row.itemId + '"><span class="glyphicon glyphicon-trash text-danger"></span> ' + lang.Delete + '</a></li>' +
                (row.sold !== true ? saleLnk : '') +
                '<li role="separator" class="divider"></li>\
                            <li><a href="ItemParts/Index/' + row.itemId + '"><span class="glyphicon glyphicon-wrench text-info"></span> ' + lang.CarParts + ' الافتراضية</a></li>\
                        </ul>\
                    </div>';
        }
    }];


defaultGridManager.Init();


$('.btnTopSearch').click(function (e) {
    e.preventDefault();
    defaultGridManager.Refresh();
});