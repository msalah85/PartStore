$.fn.select2.defaults.set("theme", "bootstrap");
var actionTypes = function () {
    return {
        'Makes': 'نوع السيارة',
        'Models': 'موديل السيارة',
        'Years': 'سنة الصنع'
    }
}();

var modelsManager = function () {
    var
        _makeId = '#MakeId',
        _modelId = '#ModelId',


        init = function () {
            events();
        },
        events = function () {
            select2WithNew();

            //make change
            $(_makeId).change(function () {
                var makeId = $(this).val();
                $(_modelId).html(''); // reset childs controls

                if (makeId) {
                    var url = "/Models/ModelByMake/" + makeId;
                    getSelect(url, _modelId, 'modelId', 'modelName');
                }

                $.fn.triggerSelect2 = function () {                    
                    select2WithNew(_modelId);
                };
            });

            $(".select2").on("select2:select", function (e) {
                if (e.params.data.isNew) {
                    // store the new tag:
                    var _this = $(this),
                        _actionType = _this.attr('data-type'),
                        _data = {
                            name: e.params.data.text,
                            makeId: $(_makeId).val()
                        },
                        _url = '/' + _actionType + '/Save',  // makes, models, years, accounts
                        successSave = function (res) {
                            _this.find('[value="' + e.params.data.id + '"]').replaceWith('<option selected value="' + res + '">' + e.params.data.text + '</option>');
                            commonManger.showMessage(lang.Success, lang.SuccessAddMessage + actionTypes[_actionType], 'success');
                        };


                    if (_actionType.toLowerCase() === "models") {
                        if (_data.makeId !== "") {
                            _data.modelName = _data.name;
                        }
                        else { // show alert
                            _data = undefined;
                            alert('يرجي اختيار نوع السيارة أولا');
                        }
                    } else {
                        _data = _data.name;
                    }

                    if (_data !== undefined) {
                        commonManger.postData(_data, successSave, _url);
                    }
                }
            });
        },
        select2WithNew = function (el) {
            var selector = '.select2';
            if (el) {
                selector = el;
                //if ($(selector).data('select2'))
                $(selector).select2('destroy');

                //return;
            }

            $(selector).select2({
                placeholder: lang.PleaseChoose,
                allowClear: true,
                language: "ar",
                dir: "rtl",
                tags: true,
                multiple: false,
                createTag: function (newTag) {
                    return {
                        id: newTag.term,
                        text: newTag.term,
                        isNew: true // add indicator:
                    };
                },
                templateResult: function (data) {
                    if (data.isNew)
                        return 'جديد اختر للإضافة- ' + data.text;
                    else
                        return data.text;
                }
            });
        },
        getSelect = function (url, childCTRL, value, text) {
            var bindData = function (response) {
                commonManger.bindSelect(childCTRL.substring(1, childCTRL.length), value, text, response);
            };

            commonManger.getData(null, null, url, bindData);
        };

    return {
        Init: init
    };

}();


modelsManager.Init();