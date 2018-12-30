// initialize fun
$.fn.afterLoadDatawithdata = function (ArrayData) { }
$.fn.afterLoadData = function () { }
$.fn.afterSave = function (data) { }
$.fn.beforeSave = function (data) { return true; }

// jQuery plugin to prevent double click
jQuery.fn.preventDoubleClick = function () {
    $(this).on('click', function (e) {
        var $el = $(this);
        if ($el.data('clicked')) {
            // Previously clicked, stop actions
            e.preventDefault();
            e.stopPropagation();
        } else {
            // Mark to ignore next click
            $el.data('clicked', true);
            // Unmark after 1 second
            window.setTimeout(function () {
                $el.removeData('clicked');
            }, 1000)
        }
    });
    return this;
};
$(document).ready(function () {
    $("*").dblclick(function (e) {
        e.stopPropagation();
        e.preventDefault();
        console.log("double-clicked but did nothing");
        return false;
    });
});

// common manager class
var
    commonManger = function () {
        var mainServiceUrl = "/api/general.aspx/",
            showPopUpDialog = function (title, operation, dialogModal) {
                $('.modal-header h3').html('<i class="icon-edit"></i> ' + title + '');
                $('#' + dialogModal + '').attr('data-operation', operation);
                //$('#formMain')[0].reset(); //resetForm();
                $('#' + dialogModal + '').modal({
                    show: true
                });
            },
            doWork = function (form, url, dtoObject, success) {
                // if validation complete 
                var isValid = applyValidation(form);
                if (isValid) {
                    dataService.callAjax('Post', JSON.stringify(dtoObject), url, success, errorException);
                }
            },
            disableAllFormElements = function (formId) {
                $('#' + formId + ' .modal-body').find('input, textarea, button, select').attr('disabled', 'disabled');
            },
            enableAllFormElements = function (formId) {
                $('#' + formId + ' .modal-body').find('input, textarea, button, select').removeAttr('disabled');
            },
            showMessage = function (title, msg, type) {
                //console.log(title, msg, type);
                toastr.options = {
                    closeButton: true,
                    debug: false,
                    newestOnTop: true,
                    progressBar: true,
                    positionClass: 'toast-top-right',
                    preventDuplicates: true,
                    onclick: null
                };
                if (type == 'warning') {
                    toastr.warning(msg, title)
                } else if (type == 'danger' || type == 'error') {
                    toastr.error(msg, title)
                } else if (type == 'success') {
                    toastr.success(msg, title)
                } else {
                    toastr.info(msg, title)
                }
            },
            errorException = function (jqXhr, textStatus, errorThrown) {
                //try {
                //    var message = textStatus + ": " + errorThrown;
                //    showMessage('Error!', message, 'danger');
                //    console.log(message);
                //}
                //catch (err) {
                console.log(jqXhr, textStatus, errorThrown);
                //}
            },
            applyValidation = function (formId) {
                var _form = $("#" + (formId ? formId : 'aspnetForm'));
                //_form.validate(); // Validate the form and retain the result.
                var isValid = false;
                isValid = _form.valid();
                return isValid;
            },
            searchData = function (oTable) {
                var searchWait = 0;
                var searchWaitInterval;
                $('.dataTables_filter input')
                    .unbind('keypress keyup')
                    .bind('keypress keyup', function (e) {
                        var item = $(this);
                        searchWait = 0;
                        if (!searchWaitInterval) searchWaitInterval = setInterval(function () {
                            if (searchWait >= 3) {
                                clearInterval(searchWaitInterval);
                                searchWaitInterval = '';
                                var searchTerm = $(item).val();
                                oTable.fnFilter(searchTerm);
                                searchWait = 0;
                            }
                            searchWait++;
                        }, 200);
                    });
            },
            ResetControls = function (formId) {
                $("#" + formId + "").find('input:not(.noreset)')
                    .each(function () {
                        if ($(this).attr('id') && !$(this).hasClass('hasfunction') && !$(this).hasClass('noreset')) {
                            var ElementId = $(this).attr('id');
                            var Ctype = $(this).prop('type');
                            if (Ctype == "text" || Ctype == "number" || Ctype == "email" || Ctype == "date") {
                                if ($(this).hasClass('current-date')) {
                                    $(this).val(moment().format('DD/MM/YYYY'));
                                } else if ($(this).hasClass('select2')) {
                                    $(this).select2('data', null);
                                } else {
                                    $(this).val("");
                                }
                            }
                            else if (Ctype == "checkbox") {
                                $(this).prop("checked", true);
                            }
                            else if (Ctype == "hidden") {
                                $(this).val(value = "0");
                            }
                            else if (Ctype == "textarea") {
                                if ($(this).hasClass('textareaSpecial')) {
                                    CKEDITOR.instances[ElementId].setData('<p></p>');
                                } else { $(this).val(value = ""); }
                            }
                        }
                    });
                $("#" + formId + "").find('select:not(.noreset)')
                    .each(function () {
                        if ($(this).attr('name')) {
                            var ElementId = $(this).attr('id');
                            var Ctype = $(this).prop('type');
                            if (Ctype == "select-one") {
                                $('#' + ElementId).val("");
                                if ($('#' + ElementId).hasClass("chzn-select")) {
                                    $('.chzn-select').val('');
                                    $('.chzn-select').trigger('chosen:updated');
                                }
                            }
                        }
                    });
                $("#" + formId + " textarea:not(.noreset)").val('');
            },
            Filllist = function (data, formId) {
                var selectElementsInForm = [];
                $("#" + formId).find('select:not(.notfill)').each(function () {
                    var _id = $(this).attr('id');
                    if (_id) {
                        selectElementsInForm.push(_id);
                    }
                });
                $.each(data, function (index, Basicdata) {
                    if (data.tbl_name != 150 && data.tbl_name != 160) {
                        for (var i = 0; i < selectElementsInForm.length; i++) {
                            if (Basicdata.tbl_name == i) {
                                $('#' + selectElementsInForm[i] + '').append("<option value='" + Basicdata.ID + "'>" + Basicdata.Name + "</option>");
                                if ($('#' + selectElementsInForm[i] + '').hasClass('chzn-select')) {
                                    $('#' + selectElementsInForm[i] + '').chosen().trigger('chosen:updated').trigger("liszt:updated");
                                }
                            }
                        }
                    }
                });
            },
            //global delete data
            deleteDefaultData = function (modalDialog, form, success, error) {
                var ParamValues = [];
                var ParamNames = [];
                var ActionName = tableName + "_Delete";
                if ($('#' + deleteModalDialog).find('.removeField').text().toLowerCase().indexOf(",") >= 0) {
                    ParamValues = $('#' + deleteModalDialog).find('.removeField').text().split(",");
                }
                else {
                    ParamValues.push($('#' + deleteModalDialog).find('.removeField').text());
                }
                if (pKey.toLowerCase().indexOf(",") >= 0) {
                    ParamNames = pKey.split(",");
                }
                else { ParamNames.push(pKey); }
                DTO = { 'Ids': ParamValues, 'ActionName': ActionName, 'Parm_name': ParamNames };
                modalDialog = $('#' + modalDialog);
                dataService.callAjax('Post', JSON.stringify(DTO), mainServiceUrl + 'Delete_Data',
                    function (data) {
                        $(modalDialog).modal('hide');
                        if (data.d.Status) // show success message if done.
                            success(data);
                        else // show error message
                            showMessage('خطأ بالحذف:', 'خطأ أثناء الحذف ' + data.d.message);
                    }, errorException);
            },
            deleteData = function (modalDialog, success, error, tableName, pKey, value) {
                var paramValues = [], paramNames = [], actionName = tableName + "_Delete";
                if (String(value).indexOf(',') >= 0) {
                    paramValues = value.split(",");
                }
                else {
                    paramValues.push(value);
                }
                if (String(pKey).indexOf(',') >= 0) {
                    paramNames = pKey.split(",");
                }
                else { paramNames.push(pKey); }
                DTO = { 'Ids': paramValues, 'ActionName': actionName, 'Parm_name': paramNames };
                modalDialog = $('#' + modalDialog);
                dataService.callAjax('Post', JSON.stringify(DTO), mainServiceUrl + 'Delete_Data',
                    function (data) {
                        $(modalDialog).modal('hide');
                        if (data.d.Status) // show success message if done.
                            success(data);
                        else // show error message
                            showMessage('لم تتم عملية الحذف', data.d.message);
                    }, errorException);
            },
            deleteMultipleData = function (modalDialog, success, error, tableName, pKey, value) {
                var paramValues = [], paramNames = [], actionName = tableName + "_Delete";
                paramValues.push(value);
                paramNames.push(pKey);
                DTO = { 'Ids': paramValues, 'ActionName': actionName, 'Parm_name': paramNames };
                modalDialog = $('#' + modalDialog);
                dataService.callAjax('Post', JSON.stringify(DTO), mainServiceUrl + 'Delete_Data',
                    function (data) {
                        $(modalDialog).modal('hide');
                        if (data.d.Status) // show success message if done.
                            success(data);
                        else // show error message
                            showMessage('لم تتم عملية الحذف', data.d.message);
                    }, errorException);
            },
            getDataForUpdate = function (ArrayData, controlid) {

                $("#" + controlid + "").find('input,select,textarea,label')
                    .each(function () {
                        if ($(this).attr('id')) {
                            if (!$(this).hasClass("notneed")) {

                                ElementId = $(this).attr('id');
                                var Ctype = $(this).prop('type');

                                if (Ctype != "undefined" || Ctype != '') {
                                    if (Ctype == "text" || Ctype == "hidden" || Ctype == "number" || Ctype == "email" || Ctype == "date" || Ctype == "textarea") {
                                        if ($(this).hasClass('textareaSpecial')) {
                                            CKEDITOR.instances[ElementId].setData(ArrayData[ElementId])
                                        }
                                        else if ($(this).hasClass('date-picker')) {
                                            $(this).val(changeDateFormat(ArrayData[ElementId], $(this).attr('date-current-format'), $(this).attr('date-to-format')));
                                        }
                                        else {
                                            var dta = ArrayData[ElementId] + '';
                                            if (dta.toLowerCase().indexOf('date(') > 0) {
                                                $(this).val(formatJSONDate(ArrayData[ElementId]));
                                            }
                                            else {
                                                $(this).val(value = ArrayData[ElementId]);
                                            }
                                        }
                                    }
                                    else if (Ctype == "select-one" && $('#' + ElementId).hasClass("showvalue")) {
                                        $('#' + ElementId + ' option').filter(function (index) { return $(this).text() === ArrayData[ElementId]; }).attr('selected', 'selected');
                                    }
                                    else if (Ctype == "select-one" && !$('#' + ElementId).hasClass("showvalue")) {
                                        $('#' + ElementId).val(ArrayData[ElementId]);
                                        if ($('#' + ElementId).hasClass("chzn-select")) {
                                            $('#' + ElementId).chosen().trigger('chosen:updated').trigger("liszt:updated");
                                        }
                                    }
                                    else if (Ctype == "checkbox") {
                                        $(this).prop("checked", ArrayData[ElementId]);
                                    }
                                    else if ($(this).is("label")) {
                                        if ($(this).hasClass('date-picker')) {
                                            var _date = changeDateFormat(ArrayData[ElementId], 'M/D/YYYY', 'DD/MM/YYYY');
                                            $(this).text(_date);
                                        }
                                        else {
                                            $('#' + ElementId + '').text(ArrayData[ElementId]);
                                        }
                                    }
                                }
                            }
                        }
                    });

                $.fn.afterLoadData();
                $.fn.afterLoadDatawithdata(ArrayData);
            },
            //fill list controls by data
            fillLists = function () {
                var url = mainServiceUrl + 'load_goalble_list';
                var DTO = { 'funName': tableName + "_Properties" };
                dataService.callAjax('Post', JSON.stringify(DTO), url, function (data) {
                    var selectList = JSON.parse(data.d);
                    // bind form lists (select) with its options.
                    bindListsOnForm(selectList, formName); // lists in master form
                    bindListsOnForm(selectList, detailsForm);// lists in detail form
                },
                    function (jqXhr, textStatus, errorThrown) {
                        var title = textStatus + ": " + errorThrown;
                        var message = JSON.parse(jqXhr.responseText).Message;
                        showMessage('danger', title, message);
                    });
            },
            getCurrentDate = function () {
                today_date = new Date();
                today_Date_Str = ((today_date.getDate() < 10) ? "0" : "") + ((today_date.getMonth() < 9) ? "0" : "") + String(today_date.getMonth() + 1) + "" + String(today_date.getDate() + "" + today_date.getFullYear());
                return today_Date_Str;
            },
            returnFiledsNames = function (targetForm) {
                var valuesids = [];
                $("#" + targetForm).find('input,select,textarea')
                    .each(function () {
                        if ($(this).attr('id')) {
                            if (!$(this).hasClass("notneed")) {
                                if (!$(this).hasClass("notshow")) {
                                    valuesids.push($(this).attr('id'));
                                }
                                else { valuesids.push('hide' + $(this).attr('id')); }
                            }
                        }
                    });
                return valuesids;
            },
            returnFiledsNamesToSave = function (targetForm) {
                var valuesids = [];
                $("#" + targetForm + "").find('input,select,textarea')
                    .each(function () {
                        if ($(this).attr('id')) {
                            if (!$(this).hasClass("notneed") && !$(this).hasClass("notsave")) {
                                valuesids.push($(this).attr('id'));
                            }
                        }
                    });
                return valuesids;
            },
            SaveDataMasterDetails = function (modalDialog, form, success, error, fieldsDetails, valuesDetails, actionName, flage) {
                var ParamValues = [], ParamNames = [], arrayall = Returncontrolsval(form);
                ParamNames = arrayall[0];
                ParamValues = arrayall[1];
                var DTO = { 'values': ParamValues, 'actionName': actionName, 'Parm_names': ParamNames, 'fieldsDetails': fieldsDetails, 'valuesDetails': valuesDetails, 'flage': flage };
                dataService.callAjax('Post', JSON.stringify(DTO), mainServiceUrl + 'SaveDataMasterDetails',
                    function (data) {
                        commonManger.showMessage('تم الحفظ بنجاح:', data.d.message);
                        $.fn.afterSave(ParamValues);
                        success(data);
                    }, errorException);
            },
            Returncontrolsval = function (controlid) {
                var values = [], valuesids = [], valuecollection = [], ElementId = "";
                $("#" + controlid).find('input,select,textarea')
                    .each(function () {
                        if ($(this).attr('id')) {
                            if (!$(this).hasClass("notneed")) {
                                ElementId = $(this).attr('id');
                                var Ctype = $(this).prop('type');
                                if (Ctype != "undefined" || Ctype != '') {
                                    if (Ctype == "text" || Ctype == "hidden" || Ctype == "number" || Ctype == "email" || Ctype == "date" || Ctype == "tel") {
                                        if ($(this).hasClass("date-picker")) {
                                            values.push(changeDateFormat($(this).val()));
                                        }
                                        else {
                                            values.push($(this).val());
                                        }
                                        valuesids.push($(this).attr('id'));
                                    }
                                    else if (Ctype == "hidden") {
                                        if ($(this).val() == "") {
                                            values.push("0");
                                        }
                                        else { values.push($(this).val()); }
                                        valuesids.push($(this).attr('id'));
                                    }
                                    else if (Ctype == "select-one") {
                                        values.push($(this).find('option:selected').val());
                                        valuesids.push($(this).attr('id'));
                                    }
                                    else if (Ctype == "checkbox") {
                                        $(this).prop('checked', function (i, value) {
                                            values.push(value);
                                        });
                                        valuesids.push($(this).attr('id'));
                                    }
                                    else if (Ctype == "radio") {
                                        var nme = $(this).attr('name'), vall = $('input[name="' + nme + '"]:checked').val();
                                        values.push(vall);
                                        valuesids.push(nme);
                                    }
                                    else if (Ctype == "textarea") {
                                        if ($(this).hasClass('textareaSpecial')) {
                                            var ckdata = CKEDITOR.instances.Details.getData();
                                            values.push(ckdata);
                                            valuesids.push($(this).attr('id'));
                                        } else {
                                            values.push($(this).val());
                                            valuesids.push($(this).attr('id'));
                                        }

                                    }
                                }
                            }
                        }

                    });
                valuecollection.push(valuesids, values);
                return valuecollection;
            },
            // global save data
            saveDefaultData = function (modalDialog, form, success, error) {
                if (!$.fn.beforeSave()) {
                    return;
                }
                var ParamValues = [], ParamNames = [], arrayall = Returncontrolsval(formName), actionName = tableName + "_Save";
                ParamNames = arrayall[0];
                ParamValues = arrayall[1];
                var DTO = { 'values': ParamValues, 'actionName': actionName, 'Parm_names': ParamNames };
                modalDialog = $('#' + modalDialog);

                // if validation complete 
                var isvalidatedForm = applyValidation(form);
                if (isvalidatedForm) {
                    dataService.callAjax('Post', JSON.stringify(DTO), mainServiceUrl + 'saveDefaultData',
                        function (data) {
                            //console.log(data);
                            $(modalDialog).modal('hide');
                            if (data.d.Status) // show success message if done.
                                success(data);
                            else // show error message
                                showMessage('خطأ أثناء الحفظ', data.d.message);
                            // reset form after saving done. by m.salah 31-1-2015
                            commonManger.ResetControls(formName);
                        }, errorException);
                }
            },
            saveData = function (modalDialog, form, success, error, actionName, flage, afterSave) {
                var ParamValues = [], ParamNames = [], arrayall = Returncontrolsval(form);

                ParamNames = arrayall[0];
                ParamValues = arrayall[1];
                DTO = { 'values': ParamValues, 'actionName': actionName, 'Parm_names': ParamNames, 'flage': flage };
                modalDialog = $('#' + modalDialog);

                // if validation complete 
                var isvalidatedForm = applyValidation(form);
                if (isvalidatedForm) {
                    dataService.callAjax('Post', JSON.stringify(DTO), mainServiceUrl + 'saveData',
                        function (data) {
                            data = data.d;
                            $(modalDialog).modal('hide');
                            if (data.Status) { // show success message if done.
                                success(data); afterSave(data.serializdata);
                                commonManger.showMessage('تم بنجاح:', 'تم الحفظ بنجاح:');
                            }
                            else {// show error message
                                commonManger.showMessage('خطأ أثناء الحفظ', data.message);
                            }
                            // reset form after saving done. by m.salah 31-1-2015
                            commonManger.ResetControls(form);
                        }, errorException);
                }
                else {
                    $(modalDialog).modal('hide');
                    showMessage('حقول مطلوبة', 'برجاء التأكد من اختيار جميع الحقول الاجبارية.')
                }
            },
            successDeleted = function (data) {
                commonManger.showMessage('تم الحذف بنجاح.', 'تمت عملية الحذف بنجاح.');
                $('#listItems').DataTable().draw();
            },
            successSaved = function (data) {
                data = data.d;
                if (data.Status) {
                    commonManger.showMessage('تم الحفظ بنجاح.', 'تمت عملية الحفظ بنجاح.');
                    $('#listItems').dataTable().fnDraw();
                }
                else
                    commonManger.showMessage('لم يتم الحفظ.', 'خطأ فى الحفظ:' + data.messages);
            },
            getNumbersFromString = function (input) {
                if (input !== undefined && input !== null && input.length > 0) {
                    return input.replace(/[^\d.]/g, ''); //match(/(\d+)/g);
                }
                else
                    return 0;
            },
            getQueryStrs = function () {
                var vars = [], hash;
                var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
                for (var i = 0; i < hashes.length; i++) {
                    hash = hashes[i].split('=');
                    vars.push(hash[0]);
                    vars[hash[0]] = hash[1];
                }
                return vars;
            },
            getUrlVars = function () {
                var vars = [], hash;
                var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
                for (var i = 0; i < hashes.length; i++) {
                    hash = hashes[i].split('=');
                    vars.push(hash[0]);
                    vars[hash[0]] = hash[1];
                }
                return vars;
            },
            getUrlSegment = function () {
                return $(location).attr('href').split("/");
            },
            getStringFromHTML = function (input) {
                return $(input).text();
            },
            formatJSONDate = function (jsonDate, format) {
                if (jsonDate == null)
                    return '';
                var newDate = new Date(parseInt(jsonDate.substr(6))),
                    // default format (MM/dd/yyyy) else (dd/MM/yyyy)
                    dat = (format) ? ((newDate.getMonth() + 1) + "/" + newDate.getDate() + "/" + newDate.getFullYear()) : (newDate.getDate() + "/" + (newDate.getMonth() + 1) + "/" + newDate.getFullYear());
                return dat;
            },
            formatJSONDateCal = function (jsonDate, format) {
                //try {
                if (!jsonDate || jsonDate == '')
                    return jsonDate;

                if (format === undefined) {
                    format = "dd/MM/yyyy"; // default date format
                }

                var newDate = $.format.date(jsonDate, format);
                return newDate;
                //}
                //catch {
                //    return jsonDate;
                //}
            },
            changeDateFormat = function (date, currentFormat, endFormat) {

                if (!date || date == '')
                    return '';

                // default formats
                currentFormat = currentFormat ? currentFormat : 'D/M/YYYY'; // UAE format
                endFormat = endFormat ? endFormat : 'MM/DD/YYYY'; // US format

                var _date = moment(date, currentFormat);
                return _date.format(endFormat);
            },
            formatCurrency = function (n, sep, decimals) {
                sep = sep || "."; // Default to period as decimal separator
                decimals = decimals || 2; // Default to 2 decimals
                return n.toLocaleString().split(sep)[0] + sep + n.toFixed(decimals).split(sep)[1];
            },
            showOptionPrintTitle = function (option) {// show filter option at the header title for print
                $('div.page-header h1 small').remove();
                $('div.page-header h1').append($(' <small>' + option + '</small>'));
            },
            populateDataTable = function (gridData, listId, afterSaveTrigg) {
                var myTable = $('#' + listId).DataTable({
                    "sDom": "<'row'>t<'row'>",
                    bDestroy: true,
                    bLengthChange: false,
                    bFilter: false,
                    searching: false,
                    retrieve: true,
                    paging: false,
                    data: gridData,
                    drawCallback: function (settings) {
                        if ($.isFunction($.fn.editable)) { // editable row
                            var api = this.api();
                            $('a.editable', api.table().body()).editable({
                                validate: function (value) {
                                    if ($.trim(value) === '') {
                                        return 'مطلوب.';
                                    } else if (isNaN(value)) {
                                        return 'رقم فقط!';
                                    }
                                },
                                url: function (params) {
                                    params.table = $(this).data('table'); params.id = $(this).data('id');
                                    return $.ajax({
                                        type: 'POST',
                                        url: sUrl + 'InlineEdit',
                                        data: JSON.stringify(params),
                                        contentType: 'application/json; charset=utf-8',
                                        dataType: 'json',
                                        async: true,
                                        cache: false,
                                        timeout: 10000,
                                        success: function (data) {
                                            $.fn.afterSave(data);
                                            commonManger.showMessage('تم الحفظ:', 'تم حفظ المبلغ بنجاح.');
                                        },
                                        error: function () {
                                            commonManger.showMessage('خطأ:', 'لقد حدث خطأ فى تنفيذ الإجراء.');
                                        }
                                    });
                                }
                            });
                            //    .off('hidden').on('hidden', function (e, reason) {
                            //    if (reason === 'save') {
                            //        $(this).closest('td').attr('data-id', $(this).text());
                            //        myTable.row($(this).closest('tr')).invalidate().draw(false);
                            //    }
                            //});
                        }
                    }
                });

                return myTable;
            },
            seoUrl = function (sourceString) {
                var outString = sourceString.replace(/[`~!@#$%^&*()_|+\-=?;:'",.<>\{\}\[\]\\\/]/gi, '');
                return outString;
            },
            //isNumeric=function (n) {
            //    return !isNaN(parseFloat(n)) && isFinite(n);
            //},
            num2Arabic = function (amount, dollarDirham) {
                if (amount && $.isNumeric(amount)) {
                    var DTO = { 'amount': amount, 'dollarDirham': dollarDirham };
                    dataService.callAjax('Post', JSON.stringify(DTO), sUrl + 'Money2Arabic', function (data) {
                        $.fn.afterSave(data);
                    }, errorException);
                }
            },
            prepareData2Grid = function (dataT, aoDatasEcho, funcCallback) {
                var jsnData = comp2Json(dataT.d),
                    aaData = jsnData.list, jsn1 = jsnData.list1;

                jsn1 = jsn1 ? $.map(jsn1, function (el) { return el }) : [0];

                // create obejct for datatables control
                var objDT = {
                    sEcho: aoDatasEcho ? aoDatasEcho : 0,
                    iTotalRecords: jsn1[0],
                    iTotalDisplayRecords: jsn1[0],
                    aaData: $.isArray(aaData) ? aaData : $.makeArray(aaData)
                }

                // bind DT data
                funcCallback(objDT);
            },

            disableControl = function (controlId, isDisable) { // true = is disabled
                $('#' + controlId).prop('disabled', isDisable);
            },

            editableSave = function (params, successCall, eService) {
                return $.ajax({
                    type: 'POST',
                    url: eService ? eService : sUrl + 'InlineEdit',
                    data: JSON.stringify(params),
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    async: true,
                    cache: false,
                    timeout: 10000,
                    success: function (data) {
                        successCall(data.d); //commonManger.showMessage('تم الحفظ:', 'تم حفظ البيان بنجاح.');
                    },
                    error: errorException
                });
            },

            fullRoles = function () {
                var _level = $('level').attr('content');
                return _level === '1' ? true : false;
            },

            comp2Json = function (compressedData) {
                var cdata = LZString.decompressFromUTF16(compressedData), // decompress data
                    xml = $.parseXML(cdata), // xml format
                    jsn = $.xml2json(xml); // json format
                return jsn;
            },

            //dataObject = {actionName = '', names: [], values: []}
            callGenericData = function (dataObject, successCall, funcName) {
                funcName = funcName ? funcName : 'GetDataList';

                dataService.callAjax('POST', JSON.stringify(dataObject), sUrl + funcName,
                    function (data) {
                        try {
                            var cdata = LZString.decompressFromUTF16(data.d), // decompress data
                                xml = $.parseXML(cdata), // xml format
                                jsn = $.xml2json(xml); // json format

                            successCall(jsn);
                        }
                        catch (err) {
                            successCall(data);
                        }
                    }, errorException);
            };

        return {
            showPopUpDialog: showPopUpDialog,
            doWork: doWork,
            showMessage: showMessage,
            searchData: searchData,
            applyValidation: applyValidation,
            disableAllFormElements: disableAllFormElements,
            enableAllFormElements: enableAllFormElements,
            ResetControls: ResetControls,
            Filllist: Filllist,
            getDataForUpdate: getDataForUpdate,
            getUrlVars: getUrlVars,
            getUrlSegment: getUrlSegment,
            returnFiledsNames: returnFiledsNames,
            returnFiledsNamesToSave: returnFiledsNamesToSave,
            SaveDataMasterDetails: SaveDataMasterDetails,
            Returncontrolsval: Returncontrolsval,
            getCurrentDate: getCurrentDate,
            saveData: saveData,
            saveDefaultData: saveDefaultData,
            deleteDefaultData: deleteDefaultData,
            deleteData: deleteData,
            getNumbersFromString: getNumbersFromString,
            getQueryStrs: getQueryStrs,
            getStringFromHTML: getStringFromHTML,
            errorException: errorException,
            formatJSONDate: formatJSONDate,
            formatCurrency: formatCurrency,
            showOptionPrintTitle: showOptionPrintTitle,
            deleteMultipleData: deleteMultipleData,
            successDeleted: successDeleted,
            formatJSONDateCal: formatJSONDateCal,
            dateFormat: changeDateFormat,
            successSaved: successSaved,
            fillLists: fillLists,
            populateDataTable: populateDataTable,
            seoUrl: seoUrl,
            num2Arabic: num2Arabic,
            disableControl: disableControl,
            setData2Grid: prepareData2Grid,
            saveEditable: editableSave,
            fullRoles: fullRoles,
            comp2json: comp2Json,
            postData: callGenericData
        };
    }();

// inline editing
if ($.isFunction($.fn.editable)) {
    $.fn.editable.defaults.mode = 'inline';
    $('a[data-type=text].editable').editable({
        url: function (params) {
            params.table = $(this).data('table');
            params.id = $(this).data('id');
            return $.ajax({
                type: 'POST',
                url: sUrl + 'InlineEdit',
                data: JSON.stringify(params),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                async: true,
                cache: false,
                timeout: 10000,
                success: function (response) {
                    //console.log(response.d);
                    //alert(response.d);
                },
                error: function (err) {
                    console.log("Error in Ajax!..", err);
                }
            });
        }
    });
}