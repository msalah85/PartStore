$.fn.addMoreFilters = function (data) {
    return;
};
$.fn.gvFooterCall = function (tfoot, data, start, end, display) {
    
};

var defaultGridManager = function () {

    var
        Init = function () {
            // initialize default properties   
            fillDataTable();

            // fire delete and reset delegates
            workPerform();
        },
        workPerform = function () {
            $('#btnAddNew').on('click', function () {
                var title = "Add " + TitlePage, operation = "save";
                commonManger.ResetControls(formName);
                commonManger.showPopUpDialog(title, operation, modalDialog);
            });

            $('#' + modalDialog + ' .btn-primary').on('click', function (e) {
                e.preventDefault();
                commonManger.saveDefaultData(modalDialog, formName, successCallback, commonManger.errorException);
            });

            // reset form after closing modal from x close button.
            $('#' + modalDialog + ' button.clse, .modal-header .close').on('click', function (e) {
                commonManger.ResetControls(formName);
                e.preventDefault();
            });

            $('#' + deleteModalDialog + ' .btn-delete').on('click', function (e) {
                e.preventDefault();
                commonManger.deleteDefaultData(deleteModalDialog, formName, successCallback, commonManger.errorException);
            });
        },
        successCallback = function (data) {
            $('#' + modalDialog).modal('hide');
            commonManger.showMessage(lang.saveDefaultData, data.message, 'success');

            if (data.Status) {
                //.fnDraw({stateSave: true});
                refresh();
            }
        },
        refresh = function () {
            $('#' + gridId).DataTable().draw(true);
        },
        fillDataTable = function () {
            var oTable = $('#' + gridId).DataTable({
                dom: //"<'row'<'col-sm-12 col-md-6'l><'col-sm-12 col-md-6'f>>" +
                    "<'row'<'col-sm-12'tr>>" +
                    "<'row'<'col-sm-12 col-md-5'li><'col-sm-12 col-md-7'p>>",
                language: {
                    "url": "/lib/DataTables/Arabic.json"
                },
                bAutoWidth: false,
                bServerSide: true,
                ajax: {
                    url: sUrl + "/LoadDT",
                    data: function (d) {
                        // example
                        //return $.extend({}, d, {
                        //    "extra_search": $('#extra').val()
                        //});  

                        try {
                            var extra = $.fn.addMoreFilters(d);
                            return extra;
                        }
                        catch (err) {
                            console.log(err);
                            return d;
                        }

                        return {};
                    }
                },
                iDisplayLength: 50,
                aaSorting: [], // default none sorting none.
                order: [[0, "desc"]],
                aoColumns: gridColumns,
                footerCallback: function (tfoot, data, start, end, display) {
                    //var api = this.api();
                    //$(api.column(5).footer()).html(
                    //    api.column(5).data().reduce(function (a, b) {
                    //        return a + b;
                    //    }, 0)
                    //);
                    $.fn.gvFooterCall(tfoot, data, start, end, display);
                }
            });

            // top buttons
            //$.fn.dataTable.Buttons.swfPath = "/Content/sys/assets/js/dataTables/extensions/Buttons/swf/flashExport.swf"; //in Ace demo ../assets will be replaced by correct assets path
            //$.fn.dataTable.Buttons.defaults.dom.container.className = 'dt-buttons btn-overlap btn-group btn-overlap';

            //new $.fn.dataTable.Buttons(oTable, {
            //    buttons: [
            //        {
            //            "extend": "colvis",
            //            "text": "<i class='fa fa-search bigger-110 blue'></i> <span class='hidden'>Show/hide columns</span>",
            //            "className": "btn btn-white btn-primary btn-bold",
            //            columns: ':not(:first):not(:last)'
            //        },
            //        {
            //            "extend": "copy",
            //            "text": "<i class='fa fa-copy bigger-110 pink'></i> <span class='hidden'>Copy to clipboard</span>",
            //            "className": "btn btn-white btn-primary btn-bold"
            //        },
            //        {
            //            "extend": "csv",
            //            "text": "<i class='fa fa-database bigger-110 orange'></i> <span class='hidden'>Export to CSV</span>",
            //            "className": "btn btn-white btn-primary btn-bold"
            //        },
            //        {
            //            "extend": "excel",
            //            "text": "<i class='fa fa-file-excel-o bigger-110 green'></i> <span class='hidden'>Export to Excel</span>",
            //            "className": "btn btn-white btn-primary btn-bold"
            //        },
            //        {
            //            "extend": "print",
            //            "text": "<i class='fa fa-print bigger-110 grey'></i> <span class='hidden'>Print</span>",
            //            "className": "btn btn-white btn-primary btn-bold",
            //            autoPrint: true,
            //            message: 'Country Sea'
            //        }
            //    ]
            //});

            //oTable.buttons().container().appendTo($('.tableTools-container'));

            //style the message box
            //var defaultCopyAction = oTable.button(1).action();
            //oTable.button(1).action(function (e, dt, button, config) {
            //    defaultCopyAction(e, dt, button, config);
            //    $('.dt-button-info').addClass('gritter-item-wrapper gritter-info gritter-center white');
            //});


            //var defaultColvisAction = oTable.button(0).action();
            //oTable.button(0).action(function (e, dt, button, config) {
            //    defaultColvisAction(e, dt, button, config);
            //    if ($('.dt-button-collection > .dropdown-menu').length == 0) {
            //        $('.dt-button-collection')
            //            .wrapInner('<ul class="dropdown-menu dropdown-light dropdown-caret dropdown-caret" />')
            //            .find('a').attr('href', '#').wrap("<li />")
            //    }
            //    $('.dt-button-collection').appendTo('.tableTools-container .dt-buttons')
            //});


            //commonManger.searchData(oTable);

            $("#" + gridId + " tbody").delegate("tr button", "click", function (event) {
                event.preventDefault();
                var self = $(this), pos = self.closest('tr'), aData;
                if (pos !== null) {
                    if (self.hasClass('edit')) {
                        var titleedit = "Edit " + TitlePage, operation = 'save';
                        aData = oTable.row(pos).data(); // get data of the clicked row
                        commonManger.ResetControls(formName);
                        commonManger.getDataForUpdate(aData, formName);
                        commonManger.showPopUpDialog(titleedit, operation, modalDialog);
                    }
                    else if (self.hasClass('remove')) {
                        aData = oTable.row(pos).data();
                        var title = " Delete " + TitlePage, peration = 'delete', ParamNames = [], _id = "";
                        if (pKey.toLowerCase().indexOf(",") >= 0) {
                            ParamNames = pKey.split(",");
                            for (var i = 0; i < ParamNames.length - 1; i++) {
                                _id += aData[ParamNames[i]] + ",";
                            }
                            _id += aData[ParamNames[ParamNames.length - 1]];
                        }
                        else { _id = aData[pKey]; }
                        $('#' + deleteModalDialog).find('.removeField').text(_id);
                        commonManger.showPopUpDialog(title, operation, deleteModalDialog);
                    }
                }
            });
        };

    return {
        Init: Init,
        Refresh: refresh,
        successCallback: successCallback
    };

}();