var data = [];

var defCol = [
    {
        dataField: "modifiedDate",
        caption: "Date Modified",
        alignment: "center",
        dataType: "date",
        format: "dd/MM/yyyy",
        minWidth: "15%",
        visible: false,
        allowHiding: true,
        showInColumnChooser: true,
        allowEditing: false,
    }, {
        dataField: "createdDate",
        caption: "Date Created",
        alignment: "center",
        dataType: "date",
        format: "dd/MM/yyyy",
        minWidth: "15%",
        visible: false,
        allowHiding: true,
        showInColumnChooser: true,
        allowEditing: false,
    },
    {
        dataField: "createdBy",
        dataType: "string",
        caption: "Created By",
        alignment: "left",
        minWidth: "15%",
        visible: false,
        allowHiding: true,
        showInColumnChooser: true,
        allowEditing: false,
    },
    {
        dataField: "modifiedBy",
        dataType: "string",
        caption: "Modified By",
        alignment: "left",
        minWidth: "15%",
        visible: false,
        allowHiding: true,
        showInColumnChooser: true,
        allowEditing: false,
        showInColumnChooser: true,
    }];

columns = columns.concat(defCol);



var store = new DevExpress.data.CustomStore({

    load: function (loadOptions) {
        var d = $.Deferred();
        $.ajax({
            url: "/" + table + "/GetList",
            method: 'get',
            dataType: 'json',
            crossDomain: true,
            headers: {
                "Content-Type": 'application/json',
                "cache-control": "no-cache"
            },
        }).done(function (e) {
            d.resolve(e);
        }).fail(function (jqXhr, status, message) {
            d.reject();
            console.log(status + ": " + message);
        });
        return d.promise();
    }
})

var source = new DevExpress.data.DataSource({
    store: store
});

var dataGrid = $("#grid").dxDataGrid({
    dataSource: source, 
    columnAutoWidth: false,
    showBorders: true,
    hoverStateEnabled: true,
    repaintChangesOnly: true,
    columnHidingEnabled: true,
    showRowLines: true,
    paging: {
        pageSize: 10
    },
    pager: {
        showPageSizeSelector: false,
        allowedPageSizes: [10, 25, 50, 100]
    },
    rowAlternationEnabled: true,
    editing: {
        useIcons: true,
        mode: 'row',
        allowAdding: true,
        allowDeleting: true,
        allowUpdating: true,
    },
    columnFixing: {
        enabled: true
    },
    columnChooser: {
        enabled: true,
        mode: "select"
    },
    filterRow: {
        applyFilter: "auto",
        applyFilterText: "Apply filter",
        betweenEndText: "End",
        betweenStartText: "Start",
        operationDescriptions: {},
        resetOperationText: "Reset",
        showAllText: "",
        showOperationChooser: true,
        visible: true
    },
    onToolbarPreparing: function (e) {
        e.toolbarOptions.visible = false;
    },
    onContextMenuPreparing: function (e) {
        if (e.target == "header") {
            if (!e.items) e.items = [];
            e.items.push({
                text: "Hide",
                onItemClick: function () {
                    $("#grid").dxDataGrid("columnOption", e.column.name, "visible", false);
                }
            });
            e.items.push({
                text: "Unhide",
                onItemClick: function () {
                    dataGrid.showColumnChooser()
                }
            });
        }
    },
    remoteOperations: false,
    onSaving: function (e) {
        e.cancel = true;
        var change = dataGrid.option("editing.changes")[0];        
        if (change) {
            let action;
            let data = [];
            let method = "post";
            if (change.type === "insert") {
                action = "add";
                method = "post";
                data[0] = change.data;
            } else if (change.type === "update") {
                action = "update";
                method = "put";
                data[0] = change.key;
                for (var item in change.data) {
                    data[0][item] = change.data[item];
                }
            } else {                
                method = "delete";
                data.push(change.key[idName]);
                action = "delete";
            }           
            saveAction(data, action, method);
            
        } else {
            DevExpress.ui.notify("No data have been changed", "warning", 2000);
        }
        
    },
    headerFilter: {
        allowSearch: true,
        searchTimeout: 500,
        texts: {},
        visible: true,
    },
    columns: columns,

}).dxDataGrid("instance");

function saveAction(data,action, method) {
    $.ajax({
        url: "/"+ table +"/"+action,
        method: method,
        dataType: "json",
        headers: {
            "Content-Type": "application/json",
            "cache-control": "no-cache",
        },
        data: JSON.stringify(data)
    }).done(function (rs) {
        if (rs.status == 1) {
            dataGrid.refresh(true).done(() => {
                dataGrid.cancelEditData();
            });
            DevExpress.ui.notify("Successful", "success", 2000);
        } else {
            DevExpress.ui.notify("error: " + rs.message, "warning", 2000);
        }
    }).fail(function () {
        DevExpress.ui.notify("Update data failed", "warning", 2000);
    })
}
$("#add-btn").click(function () {
    dataGrid.addRow();
});

$("#refresh-btn").click(function () {
    dataGrid.cancelEditData();
    dataGrid.clearFilter();
});