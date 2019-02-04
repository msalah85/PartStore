$.extend(true, $.fn.dataTable.defaults, {
    "searching": false,
    "ordering": false
});



var gridId = 'tblList',
    sUrl = '/Payments',
    modalDialog = 'myModal',
    deleteModalDialog = 'myDelModal',


    gridColumns = [
        { "mDataProp": "id" },
        {
            "render": function (data, type, row, meta) {
                return (row.kind == 1 ? "فاتورة بيع" : "ايصال سداد");
            }
        },
        {
            "mDataProp": "addDate",
            "render": function (data, type, row, meta) {
                return row.addDate ? row.addDate.substring(0, 10) : '';
            }
        },
        {
            "render": function (data, type, row, meta) {
                // parseFloat("1,234,567.89".replace(/,/g,''))
                return '<span class="money pull-left" title="صافى المطلوب حتى هذا الاجراء: ' + row.balance.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + '">' + row.amount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + '</span>';
            }
        },
        {
            "render": function (data, type, row, meta) {
                return '<span class="pull-left"><a href="' + (row.kind == 1 ? "Invoices" : "Payments") + 'Details/' + row.id + '" title="طباعة" class="btn btn-xs btn-default"><span class="glyphicon glyphicon-print"></span></a></span>';
            }
        }];



$.fn.addMoreFilters = function (data) {
    var _clientId = $('#ClientId').val();

    if (_clientId != '') {
        return $.extend({}, data, {
            id: _clientId
        });
    }
    else
        return;
};



defaultGridManager.Init();