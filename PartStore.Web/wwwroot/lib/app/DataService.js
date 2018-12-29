var dataService = function () {
        var
            callAjax = function (reqestType, postedData, url, successCallback, errorCallback) {
                jqueryAjax(reqestType, postedData, url, successCallback, errorCallback);
            },
            jqueryAjax = function (reqestType, postedData, url, successCallback, errorCallback) {                
                $.ajax({
                    type: reqestType,
                    data: postedData,
                    processData: false,
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    url: url,
                    success: function (data) {
                        successCallback(data);
                    },
                    headers: {
                        "__RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val()
                    },
                    beforeSend: function (xhr, settings) {
                        // xhr.setRequestHeader("Content-Encoding", "gzip");
                        $(".sinpper").html("<i class='icon-spinner icon-spin orange bigger-125 fa fa-spinner'></i>");
                        $('div[id$=UpdateProgress1]').css('display', 'block');
                    },
                    complete: function () {
                        $(".sinpper").html(""); $('div[id$=UpdateProgress1]').css('display', 'none');
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        errorCallback(jqXHR, textStatus, errorThrown);
                    }
                });
            },
            inlineAjax = function (postedData, successCallback, errorCallback) {
                return $.ajax({
                    type: 'POST',
                    url: sUrl + 'InlineEdit',
                    data: JSON.stringify(postedData),
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    async: true,
                    cache: false,
                    timeout: 10000,
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader("Content-Encoding", "gzip");
                        $(".sinpper").html("<i class='icon-spinner icon-spin orange bigger-125'></i>"); $('div[id$=UpdateProgress1]').css('display', 'block');
                    },
                    complete: function () {
                        $(".sinpper").html(""); $('div[id$=UpdateProgress1]').css('display', 'none');
                    },
                    success: function (data) {
                        successCallback(data);
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        errorCallback(jqXHR, textStatus, errorThrown);
                    }
                });
            };
        return {
            callAjax: callAjax,
            inlineAjax: inlineAjax
        };
    }();


