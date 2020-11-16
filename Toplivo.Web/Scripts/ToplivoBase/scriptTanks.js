//Работа с данными таблицы Tanks с помощью jqGrid плагина JavaScript библиотеки jQuery
$(function () {
    $("#jqGrid").jqGrid({
        url: "/JQGridTanks/GetTanks",
        datatype: 'json',
        mtype: 'Get',
        colNames: ['TankID', 'Емкость', 'Объем', 'Вес', 'Материал', 'Файл', 'Изображение'],
        colModel: [
            { key: true, hidden: true, name: 'TankID', index: 'TankID', editable: true },
            { key: false, name: 'TankType', index: 'TankType', sortable: true, editable: true, search: true },
            {
                key: false, name: 'TankVolume', index: 'TankVolume', sortable: false,
                formatter: 'number', formatoptions: { decimalSeparator: "." }, unformat: unformatNumber1, editable: true, search: false,
                editrules: { required: true, number: true, decimalSeparator: "." }
            },
            {
                key: false, name: 'TankWeight', index: 'TankWeight', sortable: false,
                formatter: 'number', formatoptions: { decimalSeparator: "." }, unformat: unformatNumber2, editable: true, search: false,
                editrules: { required: true, number: true, decimalSeparator: "." }
            },
            { key: false, name: 'TankMaterial', index: 'TankMaterial', sortable: true, editable: true, search: true },
            {
                key: false, hidden: true, name: 'TankPicture', index: 'TankPicture', editable: true, sortable: false, edittype: 'file',
                formatter: imageFormat, unformat: unformatFile
            },
            {
                key: false, name: 'TankPictureFile', index: 'TankPictureFile',
                formatter: imageFormat, unformat: unformatFile, sortable: false,
                editoptions: { enctype: "multipart/form-data" }, 
                edittype: 'image', editable: true, search: false
            }

           
        ],
        pager: jQuery('#jqControls'),
        rowNum: 25,
        rowList: [15, 25, 35, 45],
        sortname: "TankType",
        sortorder: "desc",
        height: '100%',
        viewrecords: true,
        caption: 'Емкости',
        emptyrecords: 'Нет записей',
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
    }).navGrid('#jqControls', {
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
            url: '/JQGridTanks/Edit',
            closeOnEscape: true,
            closeAfterEdit: true,
            recreateForm: true,
            onInitializeForm: function (formid) {

             $(formid).attr('method', 'POST');

                $(formid).attr('action', '');

                $(formid).attr('enctype', 'multipart/form-data');

            },
            beforeShowForm: function ($form) {
                $form.find("#tr_TankPicture").show();
            },
                beforeInitData: function () {
                var $self = $(this),
                    cm = $self.jqGrid("getColProp", "TankPictureFile"),
                   
                    selRowId = $self.jqGrid("getGridParam", "selrow");

                var rowData = $self.jqGrid('getRowData', selRowId);
                cm.editoptions.src = $self.jqGrid('getRowData', selRowId).TankPicture;
            },
            afterclickPgButtons: function () {
                var $self = $(this),
                    selRowId = $self.jqGrid("getGridParam", "selrow");
                var rowData = $self.jqGrid('getRowData', selRowId);

                var tt = $self.jqGrid('getRowData', selRowId).TankPicture;
                $("#TankPictureFile").attr("src", tt);
            },
 
            onclickSubmit: function (params) {
                var files = document.getElementById('TankPicture').files;

                if (window.FormData !== undefined) {
                    var data = new FormData();
                    data.append("TankID", document.getElementById('TankID').value);
                    data.append("TankType", document.getElementById('TankType').value);
                    data.append("TankVolume", document.getElementById('TankVolume').value);
                    data.append("TankWeight", document.getElementById('TankWeight').value);
                    data.append("TankMaterial", document.getElementById('TankMaterial').value);
                    data.append("TankPicture", document.getElementById('TankPicture').value);
                    if (files.length > 0) {
                        for (var x = 0; x < files.length; x++) {
                            data.append("file" + x, files[x]);
                        }
                    }
                    $.ajax({
                        type: "POST",
                        url: '/JQGridTanks/Upload',
                        contentType: false,
                        processData: false,
                        data: data,
                        success: function (result) {
                        },
                        error: function (xhr, status, p3) {
                            alert(xhr.responseText);
                        }
                    });
                } else {
                    alert("Браузер не поддерживает загрузку файлов HTML5!");
                }

            },
            afterComplete: function (response) {
                if (response.responseText) {
                    alert(response.responseText);
                    
                    $(this).trigger('reloadGrid');
                }
            }
        },
        {
            zIndex: 100,
            url: "/JQGridTanks/Create",
            closeOnEscape: true,
            closeAfterAdd: true,
            beforeShowForm: function ($form) {
                $form.find("#tr_TankPictureFile").hide();
            },
            afterComplete: function (response) {
                if (response.responseText) {
                    alert(response.responseText);
                }
            }
        },
        {
            zIndex: 100,
            url: "/JQGridTanks/Delete",
            closeOnEscape: true,
            closeAfterDelete: true,
            recreateForm: true,
            msg: "Вы уверены, что хотите удалить... ? ",
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

function unformatNumber1(cellvalue, options) {

    return cellvalue.replace(",", ".");
}
function unformatNumber2(cellvalue, options) {

    return cellvalue.replace(",", ".");
}

function unformatFile(cellValue, options, cellObject) {
    var imageHtml2 = $('img', cellObject).attr('src');
    return imageHtml2;
}
function imageFormat(cellvalue, options, rowObject) {
    var imageHtml = '<img src=' + rowObject["TankPicture"] + ' alt="Фотография отсутствует"  width="30" height="30" >' ;
    return imageHtml;
}
//var imageHtml = '<img src=' + rowObject["TankPicture"] + ' alt="Фотография отсутствует" width="25" height="25">';




