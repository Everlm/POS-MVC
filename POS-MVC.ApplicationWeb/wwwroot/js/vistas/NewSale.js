
let taxRateValue = 0;
$(document).ready(function () {

    fetch("/Sale/ListSaleDocumentType")
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            if (responseJson.length > 0) {
                responseJson.forEach((item) => {
                    $("#cboTipoDocumentoVenta").append(
                        $("<option>").val(item.salesDocumentTypeId).text(item.description)
                    )
                })
            }
        })

    fetch("/Business/GetBusiness")
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            if (responseJson.state) {
                const b = responseJson.object;
                $("#inputGroupSubTotal").text(`Sub Total -${b.currencySymbol}`)
                $("#inputGroupIGV").text(`IGV(${b.taxRate}%) -${b.currencySymbol}`)
                $("#inputGroupTotal").text(`Total -${b.currencySymbol}`)

                taxRateValue = parseFloat(b.taxRate)
            }
        })


    $("#cboBuscarProducto").select2({
        ajax: {
            url: "/Sale/GetProduct",
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            delay: 250,
            data: function (params) {
                return {
                    search: params.term
                };
            },
            processResults: function (data) {
                return {
                    results: data.map((item) => (
                        {
                            id: item.productId,
                            text: item.description,
                            brand: item.brand,
                            category: item.category,
                            imagenUrl: item.imagenUrl,
                            price: parseFloat(item.price)
                        }
                    ))
                };
            },
        },
        language: "es",
        placeholder: 'Buscar producto',
        minimumInputLength: 1,
        templateResult: formatResults
    });

})

function formatResults(data) {

    if (data.loading) {
        return data.text;
    }

    var container = $(
        `<table width="100%">
        <tr>
            <td style="width:60px">
                <img style="height:60px; width:60px; margin-right:10px" src="${data.imagenUrl}"/>
            </td>
             <td>
                <p style="font-weight:border;margin:2px">${data.brand}</p>
                <p style="margin:2px">${data.text}</p>
                
            </td>
        </tr>
        `

    );

    return container;
}


$(document).on("select2:open", function () {
    document.querySelector(".select2-search__field").focus();
})


let productSelect = [];
$("#cboBuscarProducto").on("select2:select", function (e) {

    const data = e.params.data;
    let productFound = productSelect.filter(p => p.productId == data.id)

    if (productFound.length > 0) {
        $("#cboBuscarProducto").val("").trigger("change")
        toastr.warning("", "El producto fue agregado")
        return false
    }

    swal({
        title: data.brand,
        text: data.text,
        imageUrl: data.imagenUrl,
        type: "input",
        showCancelButton: true,
        closeOnConfirm: false,
        inputPlaceholder: "Ingrese la cantidad"

    },
        function (value) {

            if (value === false) {
                return false;
            }

            if (value === "") {
                toastr.warning("", "Ingrese la cantidad")
                return false;
            }

            if (isNaN(parseInt(value))) {
                toastr.warning("", "Ingrese un valor numerico")
                return false;
            }

            let product = {
                productId: data.id,
                productBrand: data.brand,
                productDescription: data.text,
                productCategory: data.category,
                quantity: parseInt(value),
                price: data.price.toString(),
                total: (parseFloat(value) * data.price).toString()
            }

            productSelect.push(product)

            showProductPrice();
            $("#cboBuscarProducto").val("").trigger("change")
            swal.close()
        }

    )
})

function showProductPrice() {

    let total = 0;
    let igv = 0;
    let subtotal = 0;
    let rate = taxRateValue / 100;

    $("#tbProducto tbody").html("")

    productSelect.forEach((item) => {

        total = total + parseFloat(item.total)

        $("#tbProducto tbody").append(
            $("<tr>").append(
                $("<td>").append(
                    $("<button>").addClass("btn btn-danger btn-eliminar btn-sm").append(
                        $("<i>").addClass("fas fa-trash-alt")
                    ).data("productId", item.productId)
                ),
                $("<td>").text(item.productDescription),
                $("<td>").text(item.quantity),
                $("<td>").text(item.price),
                $("<td>").text(item.total),
            )
        )
    })

    subtotal = total / (1 + rate);
    igv = total - subtotal;

    $("#txtSubTotal").val(subtotal.toFixed(2))
    $("#txtIGV").val(igv.toFixed(2))
    $("#txtTotal").val(total.toFixed(2))


}


$(document).on("click", "button.btn-eliminar", function () {

    const _productId = $(this).data("productId")
    productSelect = productSelect.filter(p => p.productId != _productId);

    showProductPrice();
})

$("#btnTerminarVenta").click(function () {

    if (productSelect.length < 1) {
        toastr.warning("", "Debe registrar productos")
        return;
    }

    const saleDetailsViewModel = productSelect;
    const sale = {
        salesDocumentTypeId: $("#cboTipoDocumentoVenta").val(),
        customerDocument: $("#txtDocumentoCliente").val(),
        customerName: $("#txtNombreCliente").val(),
        subTotal: $("#txtSubTotal").val(),
        totalTax: $("#txtIGV").val(),
        total: $("#txtTotal").val(),
        saleDetails: saleDetailsViewModel
    }

    $("#btnTerminarVenta").LoadingOverlay("show");

    fetch("/Sale/CreateSale", {
        method: "POST",
        headers: { "Content-Type": "application/json; chatset=utf-8" },
        body: JSON.stringify(sale)
    })
        .then(response => {

            $("#btnTerminarVenta").LoadingOverlay("hide");
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            if (responseJson.state) {
                productSelect = [];
                showProductPrice();

                $("#txtDocumentoCliente").val("")
                $("#txtNombreCliente").val("")
                $("#cboTipoDocumentoVenta").val($("#cboTipoDocumentoVenta option:first").val())

                swal("Registrado", `Numero de venta:${responseJson.object.saleNumber}`, "success")

            } else {
                swal("Error", "No se puedo realizar la venta", "error")
            }
        })
})