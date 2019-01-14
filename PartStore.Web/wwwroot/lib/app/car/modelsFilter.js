$.fn.select2.defaults.set("theme", "bootstrap");

var modelsManager = function () {
    var
        _makeId = '#MakeId',
        _modelId = '#ModelId',


        init = function () {
            events();
        },
        events = function () {
            //make change
            $(_makeId).change(function () {
                var makeId = $(this).val();
                $(_modelId).html(''); // reset childs controls

                if (makeId) {
                    var url = "/Models/ModelByMake/" + makeId;
                    getSelect(url, _modelId, 'modelId', 'modelName');
                }
            });

            $('.select2').select2({
                placeholder: lang.PleaseChoose,
                allowClear: true,
                language: "ar",
                dir: "rtl"
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