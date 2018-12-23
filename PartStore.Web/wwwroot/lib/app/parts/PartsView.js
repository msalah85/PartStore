var pageManager = function () {
    var ClientID = null, from = null, to = null, $clientID = $('#ClientID'),
        Init = function () {

            $('#btnSearchAll').click(function (e) {
                e.preventDefault();
                ClientID = $clientID.val(), from = commonManger.dateFormat($('#From').val()), to = commonManger.dateFormat($('#To').val());
                filllistItems();
            });

            filllistItems();
        },
        filllistItems = function () {
            var pTable = $('#listItems').DataTable({
                "sDom": "<'row-fluid'<'span6'l><'span6 lft-pane'BT>r>t<'row-fluid'<'span6'i><'span6'p>>",
                buttons: [{ extend: 'csv', text: 'تصدير إكسيل' },
                    { extend: 'copy', text: 'نسـخ', },
                    {
                        text: 'طباعة',
                        action: function (e, dt, node, config) {
                            $('.dataTables_length,.form-horizontal').closest('div.row-fluid').addClass('hidden-print');
                            window.print();
                        }
                    }
                ],
                "bProcessing": true,
                "bServerSide": true,
                "bRetrieve": false,
                "bDestroy": true,
                "sAjaxSource": "PartsView.aspx/LoadData",
                "fnServerParams": function (aoData) {
                    aoData.push({ "name": "ClientID", "value": ClientID }, { "name": "From", "value": from }, { "name": "To", "value": to });
                },
                "fnServerData": function (sSource, aoData, fnCallback) {
                    dataService.callAjax('GET', aoData, sSource, function (data) { fnCallback(data.d); }, commonManger.errorException);
                },
                "iDisplayLength": 50,
                "aaSorting": [],
                "aoColumns": [
                    {
                        "mDataProp": "ID",
                        "bSortable": true
                    },
                    {
                        "mDataProp": "AddDate",
                        "bSortable": true,
                        "mData": function (oObj) {
                            return commonManger.formatJSONDate(oObj['AddDate']);
                        }
                    },
                    {
                        "mDataProp": "full_name",
                        "bSortable": true
                    },
                    {
                        "mDataProp": "InvoiceNo",
                        "bSortable": true
                    },
                    {
                        "mDataProp": "TotalAmount",
                        "bSortable": false
                    },
                    {
                        "mDataProp": "Discount",
                        "bSortable": false
                    },
                    {
                        "mDataProp": "NetAmount",
                        "bSortable": false
                    },
                    {

                        "bSortable": false,
                        "sClass": "hidden-print",
                        "mData": function (d) {
                            var paid = '';
                            if (d.PayAmount <= 0)
                                paid = '<a href="PartsAdd.aspx?id=' + d.ID + '" class="hidden-print btn btn-mini btn-info" data-rel="tooltip" data-placement="top" title="تعديل" data-original-title="تعديل"><i class="icon-edit icon-only"></i></a>\
                                    <button class="btn btn-mini btn-danger remove" data-rel="tooltip" data-placement="top" data-original-title="حــذف" title="حــذف"><i class="icon-remove icon-only"></i></button>';
                            return '<a href="PartsPrint.aspx?id=' + d.ID + '" class="hidden-print btn btn-mini btn-grey" data-rel="tooltip" data-placement="top" title="طبــاعه" data-original-title="طبــاعه"><i class="icon-print icon-only"></i></a> ' + paid;
                        }
                    }
                ]
            });

            //commonManger.searchData(pTable);

            $("#listItems tbody").delegate("tr button", "click", function (e) {
                e.preventDefault();
                var self = $(this), pos = self.closest('tr').index(), aData;
                if (pos != null) {
                    if (self.hasClass('remove')) {
                        DeleteConfirmation(function () {
                            aData = pTable.fnGetData(pos);
                            var _id = aData["ID"];
                            commonManger.deleteData('anyThing', commonManger.successDeleted, commonManger.errorException, 'PartsInvoices', 'ID', _id);
                        })
                    }
                }
            });
        };
    return {
        Init: Init
    };
}();