$.fn.select2.defaults.set("theme", "bootstrap");
$.fn.select2.defaults.set("ajax--cache", true);


///////////////////////////////

var partsManager = function () {
    var pageElements = {
        billBody: $("#listItems tbody"),
        billFooter: $("#listItems tfoot"),
        TotalAmount: $('#TotalAmount'),
        Discount: $('#Discount'),
        NetAmount: $('#NetAmount'),
        newLine: $('.newLine'),
        rowClone: '<tr><td class="itemdiv text-center"><span class="glyphicon glyphicon-resize-vertical movable"></span><span class="num">1</span><input type="hidden" name="childID" value="0" /><div class="tools"><a href="#delete" class="btn btn-xs btn-danger"><span class="glyphicon glyphicon-trash" aria-hidden="true"></span></a></div></td><td class="edit"><select name="ItemID" id="car_1" required class="input-block-level form-control select2 new cars" data-placeholder="' + lang.ChooseaCar + '"><option></option></select></td><td class="edit"><input name="PartName" required class="input-block-level form-control new" type="text"></td><td class="edit"><input name="Quantity" required class="input-block-level form-control number new" type="number" value="1"></td><td class="edit"><input name="Price" required class="input-block-level form-control money new" type="text" value="0"></td><td>0</td></tr>',
        //saveAll: $('#SaveAll'),
        invoiceID: $('#Id'),
        accountId: $('#AccountId'),
        invoiceTypeId: $('#InvoiceTypeId'),
        invoiceN: $('#InvoiceNo'),
        addDate: $('#AddDate'),
        notes: $('#Notes'),
        cSave: $('#btnSave'),
        form: 'aspnetForm'
    },

        getInvoiceDetails = function () {
            var sUrl = "/Invoices/Edit/" + pageElements.invoiceID.val(),
                bindControls = function (data) {

                    var jsn = data.invoice,
                        jsn1 = data.invoice.invoiceDetails;

                    if (jsn) {
                        $('#Discount').val(numeral(jsn.discount).format('0,0.00'));
                        $('#InvoiceNo').val(jsn.invoiceNo);
                        $('#AddDate').val(moment(jsn.addDate).format("MM/DD/YYYY"));
                        $('#Notes').val(jsn.notes);
                        var selectedOption = new Option(jsn.account.title, jsn.accountId, false, false)
                        $(pageElements.accountId).append(selectedOption).trigger('change');
                        //$(pageElements.accountId).val(jsn.accountId).trigger('change');
                    }
                    if (jsn1) {
                        pageElements.billBody.empty();// reset form.
                        $(jsn1).each(function (i, item) {
                            var rowIndex = i + 1,
                                _row = $(pageElements.rowClone.split('car_1').join('car_' + rowIndex)).clone(true),
                                carSelectId = '#car_' + rowIndex;

                            $(".num", _row).text(rowIndex);
                            $("[name='childID']", _row).val(item.id || 0);
                            var selectedItem = {
                                id: item.itemId,
                                text: item.itemId + ' - ' + item.item.make.makeName + ' ' + item.item.model.modelName + ' ' + item.item.yearId
                            },
                                selectedOption = new Option(selectedItem.text, selectedItem.id, false, false);
                            $("[name='ItemID']", _row).append(selectedOption).trigger('change'); // select2 
                            //$("[name='ItemID']", _row).val(item.itemId).trigger('change'); // select2 
                            $("[name='PartName']", _row).val(item.partName);
                            $("[name='Quantity']", _row).val(item.quantity);
                            $("[name='Price']", _row).val(numeral(item.price).format('0,0.00'));
                            $("td:eq(5)", _row).text(numeral(item.subTotal).format('0,0.00'));
                            pageElements.billBody.append($(_row));

                            bindCarsSelect2(carSelectId, item.itemId);

                        }).promise().done(function () {
                            invoiceTotalCalc();
                            //bindSelect2();
                            //reArrangBillIndexs();
                        });
                    }
                };


            dataService.callAjax('GET', {}, sUrl, bindControls, commonManger.errorException);
        },
        init = function () {
            $(document).delegate('input[type="text"],input[type="number"]', "click", function () {
                $(this).select();
            });

            //$('.datepicker').datepicker({ autoclose: true }); // set default today 
            $('.today').datepicker('setDate', new Date()); // 

            // get data to edit
            var sg = commonManger.getUrlSegment();
            if (sg && sg[5]) {
                pageElements.invoiceID.val(sg[5]);
                getInvoiceDetails();
            }

            //bindSelect2(); // enable select2 for select search.
            $('.money').autoNumeric('init'); // 

            // table sortable
            pageElements.billBody.sortable({
                opacity: 0.8,
                revert: true,
                forceHelperSize: true,
                placeholder: 'draggable-placeholder',
                forcePlaceholderSize: true,
                tolerance: 'pointer',
                stop: function (e, ui) { //just for Chrome!!!! so that dropdowns on items don't appear below other items after being moved
                    $(ui.item).css('z-index', 'auto');
                    reArrangBillIndexs();
                }
            });

            // calculate total
            pageElements.billBody.delegate('input[name="Quantity"],input[name="Price"]', "keyup", function () {
                var tr = $(this).closest('tr'),
                    subTotal = numeral(tr.find('input[name="Quantity"]').val()).value() * numeral(tr.find('input[name="Price"]').val()).value();
                tr.find('td:eq(5)').text(numeral(subTotal).format('0,0.00'));
                invoiceTotalCalc();
            });

            // set discount value
            pageElements.billFooter.delegate(pageElements.Discount, "keyup", function () {
                calculateNetPrice();
            });

            // remove part from bill
            pageElements.billBody.delegate('tr td.itemdiv a', "click", function (e) {
                e.preventDefault(); var $this = $(this).closest('tr'); removeElement($this);
            });

            // add new line
            pageElements.newLine.on('click', function (e) {
                e.preventDefault();
                // add new row.
                var rowIndex = (pageElements.billBody.children('tr').length + 1),
                    _row = pageElements.rowClone.split('car_1').join('car_' + rowIndex),
                    carSelectId = '#car_' + rowIndex;

                $(".num", _row).text(rowIndex);
                pageElements.billBody.append($(_row));
                $(carSelectId).focus();
                bindCarsSelect2(carSelectId); // fire choices in cars select
                reArrangBillIndexs();

                // add validate rule.
                $('input.new').each(function () { $(this).rules("add", { required: true }); });
                pageElements.billBody.find('select[name="ItemID"]:last').focus();
                $('.money').autoNumeric('init'); // apply format money.

                invoiceTotalCalc();

                // apply validation on new controls.
                var valid = commonManger.applyValidation(pageElements.form);
                if (!valid)
                    commonManger.showMessage(lang.Required, lang.RequiredMsg, 'warning');

                bindInvoiceItemRowEvents();
            });

            // set new line
            pageElements.billBody.delegate('input[name="Price"]:last', "blur", function () {
                pageElements.newLine.trigger('click');
            });

            // save new client
            pageElements.cSave.click(function (e) {
                e.preventDefault();
                saveAll();
            });

            // bind first car select2
            bindCarsSelect2('#car_1');

            bindInvoiceItemRowEvents();

            // get clients/suppliers based on invoice type selection.
            //pageElements.invoiceTypeId.change(function () {                
            //    var _vl = $(this).val();
            //    pageElements.accountId.html(''); // reset childs controls
            //    if (_vl) {
            //        _vl = parseInt(_vl * 1) < 3 ? 1 : 2; // Clients/Suppliers types only                    
            //        var url = "/Accounts/AccountByType/" + _vl;
            //        commonManger.getSelect(url, 'AccountId', 'accountId', 'title');
            //    }
            //});

            // account
            pageElements.accountId.select2({
                placeholder: lang.PleaseChoose,
                dir: "rtl",
                language: "ar",
                allowClear: true,
                //data: avialableCars
                ajax: {
                    //How long the user has to pause their typing before sending the next request
                    delay: 150,
                    cacheDataSource: [],
                    //The url of the json service
                    url: '/Accounts/AccountByType',
                    dataType: 'json',
                    async: true,
                    //Our search term and what page we are on
                    data: function (params) {
                        var invType = pageElements.invoiceTypeId.val(),
                            accountType = parseInt(invType * 1) < 3 ? 1 : 2;

                        return {
                            pageSize: 10,
                            pageNum: params.page || 1,
                            searchTerm: params.term,
                            // more param.
                            invoiceType: accountType
                        };
                    },
                    processResults: function (data, params) {
                        params.page = params.page || 1;
                        return {
                            //results: $.map(data.Results, function (obj) {
                            //    return { id: obj.Id, text: obj.FirstName };
                            //}),
                            results: data.results,
                            pagination: {
                                more: (params.page * 10) <= data.total
                            }
                        };
                    }
                }
            });
        },
        saveAll = function () {
            var
                _data = {
                    "id": pageElements.invoiceID.val(),
                    "accountId": pageElements.accountId.val(),
                    "invoiceTypeId": pageElements.invoiceTypeId.val(),
                    "invoiceNo": pageElements.invoiceN.val(),
                    "notes": pageElements.notes.val(),
                    "totalAmount": numeral(pageElements.TotalAmount.text()).value(),
                    "discount": numeral(pageElements.Discount.val()).value(),
                    "tax": 0,
                    "netAmount": numeral(pageElements.NetAmount.text()).value(),
                    "userId": 1,
                    //"ip": "",
                    "isCache": true,
                    "addDate": pageElements.addDate.val(),
                    "addTime": "00:00:00",
                    "invoiceDetails": []
                },
                url = '/Invoices/Save',
                successSaveClient = function (d) {
                    if (d.saved === true) {
                        commonManger.showMessage(lang.Success, lang.SuccessMessage, 'success');
                        if (d.id > 0)
                            window.location.href = '/Invoices/Details/' + d.id;
                        else
                            window.location.href = '/Invoices';
                    } else {
                        commonManger.showMessage(lang.Error, lang.SystemError + d.id, 'error');
                    }
                };

            pageElements.billBody.children('tr').each(function (i, item) {
                var
                    _self = $(item),
                    itm = { // child invoice row 
                        id: _self.find('input:eq(0)').val(),
                        invoiceId: pageElements.invoiceID.val(),   // invoice id
                        itemId: $('#car_' + (i + 1)).val(),        // car id
                        partName: _self.find('input:eq(1)').val(),
                        quantity: numeral(_self.find('input:eq(2)').val()).value(),
                        price: numeral(_self.find('input:eq(3)').val()).value(),
                        subTotal: numeral(_self.find('td:last').text()).value()
                    };

                if (itm.itemId !== "" && itm.partName !== "" && itm.subTotal > 0) { // add to save array
                    _data.invoiceDetails.push(itm);
                }
            });

            if (_data.accountId !== '' && _data.invoiceDetails.length > 0) {
                commonManger.doWork(pageElements.form, url, _data, successSaveClient);
            } else {
                commonManger.showMessage(lang.Required, lang.RequiredMsg, 'warning');
            }
        },        
        bindCarsSelect2 = function (selector, selectedVal) {
            var el = $(selector);

            if (el.hasClass('cars')) {
                // bind items list 
                $(selector).select2({
                    placeholder: lang.ChooseACar,
                    dir: "rtl",
                    language: "ar",
                    allowClear: true,
                    //data: avialableCars
                    ajax: {
                        //How long the user has to pause their typing before sending the next request
                        delay: 150,
                        cacheDataSource: [],
                        //The url of the json service
                        url: '/Invoices/GetCars',
                        dataType: 'json',
                        async: true,
                        //Our search term and what page we are on
                        data: function (params) {
                            return {
                                pageSize: 10,
                                pageNum: params.page || 1,
                                searchTerm: params.term,
                                //Value from client side.
                                //countyId: 'vv'
                            };
                        },
                        processResults: function (data, params) {
                            params.page = params.page || 1;
                            return {
                                //results: $.map(data.Results, function (obj) {
                                //    return { id: obj.Id, text: obj.FirstName };
                                //}),
                                results: data.results,
                                pagination: {
                                    more: (params.page * 10) <= data.total
                                }
                            };
                        }
                    }
                });

                // show the selected item
                if (selectedVal)
                    $(selector).val(selectedVal).trigger('change');
            } else {
                $(selector).select2({
                    //placeholder: _placeholder,
                    language: "ar",
                    allowClear: true,
                    dir: "rtl"
                });
            }
        },
        jumbToNext = function (el) {
            var _this = $(el),
                _thisVal = _this.val(),
                _thisTd = _this.closest('td');
            if (_thisVal !== '') {
                _thisTd.next('td').find('input').select();
            }
        },
        bindInvoiceItemRowEvents = function () {
            $('select.cars.select2').on('select2:close', function (e) {
                jumbToNext(this); return false;
            });
            jQuery('input[name="PartName"]').bind('keydown.return', function (evt) {
                jumbToNext(this); return false;
            });
            jQuery('input[name="Quantity"]').bind('keydown.return', function (evt) {
                jumbToNext(this); return false;
            });
            jQuery('input[name="Price"]').bind('keydown.return', function (evt) {
                pageElements.newLine.trigger('click'); return false;
            });
        },
        reArrangBillIndexs = function () {
            pageElements.billBody.find('tr').each(function (i, n) {
                $(this).find('span.num').text(i + 1);
            });
        },
        removeElement = function (el) {
            el.css({ 'transition': 'background-color 1s', 'background-color': 'red' }).fadeOut('slow').promise().done(function () { el.remove(); reArrangBillIndexs(); });
        },
        calculateNetPrice = function () {
            var _net = numeral(pageElements.TotalAmount.text()).value() - numeral(pageElements.Discount.val()).value();
            pageElements.NetAmount.text(numeral(_net).format('0,0.00'));
        },
        invoiceTotalCalc = function () {
            var billTot = 0;
            pageElements.billBody.children('tr').each(function () { billTot += numeral($(this).find('td:eq(5)').text()).value(); })
                .promise().done(function () {
                    pageElements.TotalAmount.text(numeral(billTot).format('0,0.00'));
                    calculateNetPrice();
                });
        };

    return {
        Init: init
    };

}();
partsManager.Init();