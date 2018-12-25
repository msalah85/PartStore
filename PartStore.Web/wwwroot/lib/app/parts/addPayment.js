
var pageManager = function () {
    var
        pageElemnts = {
            operationEl: $('#OperationId'),
            banksSelector: 'banks',
            accountSelector: 'Account'
        },


        init = function () {
            bindDate();
            bindSelect2();
            events();
        },
        events = function () {
            $(pageElemnts.operationEl).change(function () {
                var
                    _self = $(this),
                    vl = _self.val(); //{(1,Expense),(2, Deposit),(3,Check)}

                showHideBanks(vl);
            });

            // edit mode
            $(pageElemnts.operationEl).trigger('change');
        },
        showHideBanks = function (operationNo) {
            // reset showing banks.
            $('.' + pageElemnts.banksSelector).addClass('hidden');
            $('.' + pageElemnts.accountSelector).removeClass('hidden');

            if (window.location.href.toLowerCase().indexOf('/create') > 0) { //Reset selected value (Add mode only)
                $('.' + pageElemnts.banksSelector + ' select').val(null).trigger('change');
            }

            if (operationNo < 3) { // expense/deposit
                $('#' + pageElemnts.banksSelector + operationNo).removeClass('hidden'); // activate one only.
            } else if (operationNo === '3') { // transfer                
                $('.' + pageElemnts.banksSelector).removeClass('hidden'); // show both
                $('.' + pageElemnts.accountSelector).addClass('hidden');
                $('#' + pageElemnts.accountSelector + 'Id').val(null).trigger('change');
            } else if (operationNo > 3) { // checks
                $('#' + pageElemnts.banksSelector + (operationNo - 3)).removeClass('hidden'); // only one
            }
        },
        bindSelect2 = function () {
            $(".select2").select2({
                placeholder: "Please choose",
                allowClear: true,
                theme: "bootstrap"
            });
        },
        bindDate = function () {
            $('.today').datepicker('setDate', new Date());
        };

    return {
        Init: init
    };
}();

pageManager.Init();