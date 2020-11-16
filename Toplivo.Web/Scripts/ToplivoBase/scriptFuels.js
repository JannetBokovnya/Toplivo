//Работа с данными таблицы Fuels с помощью jqGrid плагина JavaScript библиотеки jQuery
$(function () {
    $("#jqGrid").jqGrid({
        url: "/JQGridFuels/GetFuels",
        datatype: 'json',
        mtype: 'Get',
        colNames: ['FuelID', 'Вид топлива', 'Плотность'],
        colModel: [
            { key: true, hidden: true, name: 'FuelID', index: 'FuelID', editable: true },
            { key: false, name: 'FuelType', index: 'FuelType', sortable: true, editable: true },
            { key: false, name: 'FuelDensity', index: 'FuelDensity', formatter: 'number',
                formatoptions: { decimalSeparator: "." }, unformat: unformatNumber, editable: true, search: false,
                editrules: { required: true, number: true, decimalSeparator: "." }
            }],
        
        pager: jQuery('#jqControls'),
        rowNum: 25,
        rowList: [25, 35, 45],
        sortname: "FuelType",
        sortorder: "desc", // порядок сортировки,
        height: '100%',
        viewrecords: true,
        caption: 'Виды топлива',
        emptyrecords: 'Нет видов топлива для отображения',
        jsonReader: {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            Id: "0"
        },
        autowidth: true,
        multiselect: false
    }).navGrid('#jqControls',
        {
            edit: true,
            edittext: "Редактировать",
            view: true,
            viewtext: "Смотреть",
            add: true,
            addtext: "Добавить",
            del: true,
            deltext: "Удалить",
            search: true,
            searchtext: "Найти",
            refresh: true,
            refreshtext: "Обновить"
        },
        {
            zIndex: 100,
            url: '/JQGridFuels/Edit',
            closeOnEscape: true,
            closeAfterEdit: true,
            recreateForm: true,
            afterComplete: function (response) {
                if (response.responseText) {
                    alert(response.responseText);
                }
            }
        },
        {
            zIndex: 100,
            url: "/JQGridFuels/Create",
            closeOnEscape: true,
            closeAfterAdd: true,
            afterComplete: function (response) {
                if (response.responseText) {
                    alert(response.responseText);
                }
            }
        },
        {
            zIndex: 100,
            url: "/JQGridFuels/Delete",
            closeOnEscape: true,
            closeAfterDelete: true,
            recreateForm: true,
            msg: "Вы уверены, что хотите удалить запись? ",
            afterComplete: function (response) {
                if (response.responseText) {
                    alert(response.responseText);
                }
            }
        },
        {
            zIndex: 100,
            caption: "Поиск",
            sopt: ['cn']
        }

    );

});
function unformatNumber(cellvalue, options) {

    return cellvalue.replace(",", ".");
}