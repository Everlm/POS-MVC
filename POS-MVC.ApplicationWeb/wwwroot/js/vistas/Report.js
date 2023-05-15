
let tableData;
$(document).ready(function () {

    $.datepicker.setDefaults($.datepicker.regional["es"])
    $("#txtFechaInicio").datepicker({ dateFormat: "dd/mm/yy" })
    $("#txtFechaFin").datepicker({ dateFormat: "dd/mm/yy" })

    tableData = $('#tbdata').DataTable({
        responsive: true,
        "ajax": {
            "url": '/Report/ReportSale?startDate=10/10/1990&endDate=10/10/1990',
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "creationDate" },
            { "data": "saleNumber" },
            { "data": "documentType" },
            { "data": "documentCustomer" },
            { "data": "nameCustomer" },
            { "data": "subTotalSale" },
            { "data": "totalSale" },
            { "data": "totalTaxSale" },
            { "data": "product" },
            { "data": "quantity" },
            { "data": "price" },
            { "data": "total" },
        ],
        order: [[0, "desc"]],
        dom: "Bfrtip",
        buttons: [
            {
                text: 'Exportar Excel',
                extend: 'excelHtml5',
                title: '',
                filename: 'Reporte de ventas'
            },
            'pageLength'
        ],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        },
    });
})

$("#btnBuscar").click(function () {
    if ($("#txtFechaInicio").val().trim() == "" || $("#txtFechaFin").val().trim() == "") {
        toastr.warning("", "Ingrese una fecha de inicio y una de fin")
        return;
    }

    let startDate = $("#txtFechaInicio").val().trim();
    let endDate = $("#txtFechaFin").val().trim();

    let newUrl = `/Report/ReportSale?startDate=${startDate}&endDate=${endDate}`
    tableData.ajax.url(newUrl).load();
})
