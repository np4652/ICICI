var dialog;
((dialog) => {
    dialog.bankSetting = function (id = 0) {
        $.post('/Bank/EditSetting', { id: id }).done(result => {
            Q.alert({
                title: "Bank Setting",
                body: result,
                width: '900px',
            });
        }).fail(xhr => {
            console.log(xhr.responseText);
            Q.notify(-1, 'An error occurred.');
        }).always(() => { });
    }
})(dialog || (dialog = {}));
var services;
var serviceProperty = {
    Add: {},
    Delete: {},
    Change: {},
    Detail: {},
    Dropdown: {}
};

((services) => {
    services.scheduleJob = function ({ startDate = '', endDate = '' }) {
        $.post('/Task/ScheduleFetchStatement', { startDate: startDate, endDate: endDate })
            .done(result => { Q.notify(1, 'Job Scheduled successfully.'); }).fail(xhr => {
                console.log(xhr.responseText);
                Q.notify(-1, 'An error occurred.');
            }).always(() => { });
    };
    services.scheduleTestJob = function () {
        $.post('/Task/ScheduleTestTask')
            .done(result => { Q.notify(1, 'Job Scheduled successfully.'); }).fail(xhr => {
                console.log(xhr.responseText);
                Q.notify(-1, 'An error occurred.');
            }).always(() => { });
    };

    services.pauseTask = function () {
        $.post('/Task/PauseTask')
            .done(result => { Q.notify(1, 'Job paused successfully.'); }).fail(xhr => {
                console.log(xhr.responseText);
                Q.notify(-1, 'An error occurred.');
            }).always(() => { });
    };

    services.resumeTask = function () {
        $.post('/Task/ResumeTask')
            .done(result => { Q.notify(1, 'Job resumed successfully.'); }).fail(xhr => {
                console.log(xhr.responseText);
                Q.notify(-1, 'An error occurred.');
            }).always(() => { });
    };
    services.Dropdown = {
        Category: param => new Promise((resolve, reject) => {
            if (!param) {
                param = {
                    CategoryID: 0,
                    CategoryName: '',
                }
            }
            $.post('/Master/GetCategoryDrop', param).done(result => resolve(result)).fail(xhr => reject(new Error(xhr)));
        }),
        Unit: param => new Promise((resolve, reject) => {
            if (!param) {
                param = {
                    UnitID: 0,
                    UnitName: '',
                }
            }
            $.post('/Master/Unit', param).done(result => resolve(result)).fail(xhr => reject(new Error(xhr)));
        }),
        Products: param => new Promise((resolve, reject) => {
            $.post('/Master/GetProductDrop', { categoryid: param }).done(result => resolve(result)).fail(xhr => reject(new Error(xhr)));
        }),
        Frequency: param => new Promise((resolve, reject) => {
            $.post('/Master/GetFrequency', param).done(result => resolve(result)).fail(xhr => reject(new Error(xhr)));
        }),
        Users: param => new Promise((resolve, reject) => {
            $.post('/Account/UsersDropdown', param).done(result => resolve(result)).fail(xhr => reject(new Error(xhr)));
        }),
    };


})(services || (services = serviceProperty));

var s = services;
var Dropdown = services.Dropdown;