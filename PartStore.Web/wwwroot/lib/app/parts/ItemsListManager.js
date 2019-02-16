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
    gridColumns = [{ "mDataProp": "itemsPk" },
    {
        "orderable": false,
        "render": function (data, type, row, meta) {
            return '<a href="/Photos/Index/' + row.itemId + '" title="' + lang.Photos + '">\
                                    <img class="thumb" height="35px" width="35px" src="/public/'+ (row.photos.length > 0 ? row.photos[0].url : '') + '" /></a>';
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
        "sClass": 'noprint',
        "orderable": false,
        "render": function (data, type, row, meta) {
            return '<div class="dropdown">\
                        <button class="btn btn-default dropdown-toggle" type="button" id="dropdownMenu1" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">\
                            ' + lang.Options + ' <span class="caret"></span>\
                        </button>\
                        <ul class="dropdown-menu pull-right" aria-labelledby="dropdownMenu1">\
                            <li><a href="photos/index/' + row.itemId + '"><span class="glyphicon glyphicon-picture text-info"></span> ' + lang.Photos + '</a></li>\
                            <li><a href="/Items/Car/' + row.itemId + '"><span class="glyphicon glyphicon-list-alt text-info"></span> المبيعات</a></li>\
                            <li role="separator" class="divider"></li>\
                            <li><a href="items/edit/' + row.itemId + '"><span class="glyphicon glyphicon-edit text-info"></span> ' + lang.Edit + '</a></li>\
                            <li><a href="items/delete/' + row.itemId + '"><span class="glyphicon glyphicon-trash text-danger"></span> ' + lang.Delete + '</a></li>\
                <li><a class="btnSold" href="javascript:void(0);"><span class="glyphicon glyphicon-ok-sign text-success"></span> ' + lang.Sale + '</a></li>\
                <li role="separator" class="divider"></li>\
                            <li><a href="ItemParts/Index/' + row.itemId + '"><span class="glyphicon glyphicon-wrench text-info"></span> ' + lang.CarParts + ' الافتراضية</a></li>\
                        </ul></div>';
        }
    }];



defaultGridManager.Init();


$('.btnTopSearch').click(function (e) {
    e.preventDefault();
    defaultGridManager.Refresh();
});

// sold car event
$("#" + gridId + " tbody").delegate("tr a.btnSold", "click", function (event) {
    event.preventDefault();

    var self = $(this), pos = self.closest('tr'), aData,
        oTable = $('#' + gridId).DataTable();

    if (pos !== null) {
        aData = oTable.row(pos).data();

        var _car = '#' + aData.itemId + ' - ' + aData.make.makeName + ' ' + aData.model.modelName + ' ' + aData.yearId;
        $('strong.car').text(_car);
        $('#sold').prop('checked', aData.sold).val(aData.itemId);
        $('#soldModal').modal('show');
    }
});

$('button.btnSaveSold').click(function (e) {
    e.preventDefault();

    var
        $sold = $('#sold'),
        _dta = {
            itemId: $sold.val() || 0,
            sold: $sold.is(':checked') ? true : false
        },
        url = '/Items/EditSale',
        successSaveFn = function (d) {
            if (d.saved === true) {
                commonManger.showMessage(lang.Success, lang.SuccessMessage, 'success');
                defaultGridManager.Refresh();
                $('.modal').modal('hide');
            } else {
                commonManger.showMessage(lang.Error, lang.SystemError + d.error, 'error');
            }
        };

    commonManger.doWork('#form', url, _dta, successSaveFn);
});