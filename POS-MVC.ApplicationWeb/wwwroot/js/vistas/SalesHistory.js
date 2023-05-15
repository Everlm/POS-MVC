const ViewSearch = {
    SearchDate: () => {
        $("#txtFechaInicio").val("")
        $("#txtFechaFin").val("")
        $("#txtNumeroVenta").val("")

        $(".busqueda-fecha").show()
        $(".busqueda-venta").hide()
    },
    SearchSale: () => {
        $("#txtFechaInicio").val("")
        $("#txtFechaFin").val("")
        $("#txtNumeroVenta").val("")

        $(".busqueda-fecha").hide()
        $(".busqueda-venta").show()
    }
}

$(document).ready(function () {
    ViewSearch["SearchDate"]()

    $.datepicker.setDefaults($.datepicker.regional["es"])
    $("#txtFechaInicio").datepicker({ dateFormat: "dd/mm/yy" })
    $("#txtFechaFin").datepicker({ dateFormat: "dd/mm/yy" })
})

$("#cboBuscarPor").change(function () {
    if ($("#cboBuscarPor").val() == "fecha") {
        ViewSearch["SearchDate"]()

    } else {
        ViewSearch["SearchSale"]()
    }
})

$("#btnBuscar").click(function () {
    if ($("#cboBuscarPor").val() == "fecha") {
        if ($("#txtFechaInicio").val().trim() == "" || $("#txtFechaFin").val().trim() == "") {
            toastr.warning("", "Ingrese una fecha de inicio y una de fin")
            return;
        }
    } else {
        if ($("#txtNumeroVenta").val().trim() == "") {
            toastr.warning("", "Ingrese un numero de venta")
            return;
        }
    }

    let saleNumber = $("#txtNumeroVenta").val()
    let startDate = $("#txtFechaInicio").val()
    let endDate = $("#txtFechaFin").val()

    $(".card-body").find("div.row").LoadingOverlay("show");

    fetch(`/Sale/RecordSales?saleNumber=${saleNumber}&startDate=${startDate}&endDate=${endDate}`)
        .then(response => {
            $(".card-body").find("div.row").LoadingOverlay("hide");
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            $("#tbventa tbody").html("");

            if (responseJson.length > 0) {
                responseJson.forEach((sale) => {
                    $("#tbventa tbody").append(
                        $("<tr>").append(
                            $("<td>").text(sale.creationDate),
                            $("<td>").text(sale.saleNumber),
                            $("<td>").text(sale.salesDocumentType),
                            $("<td>").text(sale.customerDocument),
                            $("<td>").text(sale.customerName),
                            $("<td>").text(sale.total),
                            $("<td>").append(
                                $("<button>").addClass("btn btn-info btn-sm").append(
                                    $("<i>").addClass("fas fa-eye")
                                ).data("sale", sale)
                            )
                        )
                    )
                })
            }
        })
})

$("#tbventa tbody").on("click", ".btn-info", function () {
    let b = $(this).data("sale")

    $("#txtFechaRegistro").val(b.creationDate)
    $("#txtNumVenta").val(b.saleNumber)
    $("#txtUsuarioRegistro").val(b.user)
    $("#txtTipoDocumento").val(b.salesDocumentType)
    $("#txtDocumentoCliente").val(b.customerDocument)
    $("#txtNombreCliente").val(b.customerName)
    $("#txtSubTotal").val(b.subTotal)
    $("#txtIGV").val(b.totalTax)
    $("#txtTotal").val(b.total)

    $("#tbProductos tbody").html("");

    b.saleDetails.forEach((item) => {
        $("#tbProductos tbody").append(
            $("<tr>").append(
                $("<td>").text(item.productDescription),
                $("<td>").text(item.quantity),
                $("<td>").text(item.price),
                $("<td>").text(item.total)

            )
        ) 
    })

    $("#linkImprimir").attr("href",`/Sale/ShowPDF?saleNumber=${b.saleNumber}`)
    $("#modalData").modal("show");
})