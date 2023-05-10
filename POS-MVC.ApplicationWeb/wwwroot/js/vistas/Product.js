const ProductModel = {
    productId: 0,
    barCode: "",
    brand: "",
    description: "",
    categoryId: 0,
    stock: "",
    imagenUrl: "",
    price: "",
    isActive: 1,
}

let tableData;

$(document).ready(function () {

    fetch("/Category/ListCategories")
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            if (responseJson.data.length > 0) {
                responseJson.data.forEach((item) => {
                    $("#cboCategoria").append(
                        $("<option>").val(item.categoryId).text(item.description)
                    )
                })
            }
        })

    tableData = $('#tbdata').DataTable({
        responsive: true,
        "ajax": {
            "url": '/Product/ListProducts',
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "productId", "visible": false, "searchable": false },
            {
                "data": "imagenUrl", render: function (data) {
                    return `<img style = "height:60px" src=${data} class="rounded mx-auto d-block"/>`
                }
            },
            { "data": "barCode" },
            { "data": "brand" },
            { "data": "description" },
            { "data": "category" },
            { "data": "stock" },
            { "data": "price" },
            {
                "data": "isActive", render: function (data) {
                    if (data == 1)
                        return '<span class="badge badge-info">Activo</span>';
                    else
                        return '<span class="badge badge-danger">No Activo</span>';
                }
            },
            {
                "defaultContent": '<button class="btn btn-primary btn-editar btn-sm mr-2"><i class="fas fa-pencil-alt"></i></button>' +
                    '<button class="btn btn-danger btn-eliminar btn-sm"><i class="fas fa-trash-alt"></i></button>',
                "orderable": false,
                "searchable": false,
                "width": "80px"
            }
        ],
        order: [[0, "desc"]],
        dom: "Bfrtip",
        buttons: [
            {
                text: 'Exportar Excel',
                extend: 'excelHtml5',
                title: '',
                filename: 'Reporte Productos',
                exportOptions: {
                    columns: [1, 2, 3, 4, 5, 6, 7, 8, 9]
                }
            }, 'pageLength'
        ],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        },
    });
})

function showModal(model = ProductModel) {

    $("#txtId").val(model.productId)
    $("#txtCodigoBarra").val(model.barCode)
    $("#txtMarca").val(model.brand)
    $("#txtDescripcion").val(model.description)
    $("#cboCategoria").val(model.categoryId == 0 ? $("#cboCategoria option:first").val() : model.categoryId)
    $("#txtStock").val(model.stock)
    $("#txtPrecio").val(model.price)
    $("#cboEstado").val(model.isActive)
    $("#txtImagen").val("")
    $("#imgProducto").attr("src", model.imagenUrl)


    $("#modalData").modal("show")

}

$("#btnNuevo").click(function () {
    showModal()
})


$("#btnGuardar").click(function () {

    const inputs = $("input.input-validar").serializeArray();
    const inputsOutValue = inputs.filter((item) => item.value.trim() == "")
    if (inputsOutValue.length > 0) {
        const message = `Debe completar el campo :"${inputsOutValue[0].name}"`;
        toastr.warning("", message)
        $(`input[name="${inputsOutValue[0].name}"]`).focus()
        return;
    }

    const model = structuredClone(ProductModel);
    model["productId"] = parseInt($("#txtId").val())
    model["barCode"] = $("#txtCodigoBarra").val()
    model["brand"] = $("#txtMarca").val()
    model["description"] = $("#txtDescripcion").val()
    model["categoryId"] = $("#cboCategoria").val()
    model["stock"] = $("#txtStock").val()
    model["price"] = $("#txtPrecio").val()
    model["isActive"] = $("#cboEstado").val()

    const inputPhoto = document.getElementById("txtImagen")
    const formData = new FormData();

    formData.append("image", inputPhoto.files[0])
    formData.append("productModel", JSON.stringify(model))

    $("#modalData").find("div.modal-content").LoadingOverlay("show");

    if (model.productId == 0) {
        fetch("/Product/CreateProduct", {
            method: "POST",
            body: formData
        })
            .then(response => {
                $("#modalData").find("div.modal-content").LoadingOverlay("hide");
                return response.ok ? response.json() : Promise.reject(response);
            })
            .then(responseJson => {
                if (responseJson.state) {
                    tableData.row.add(responseJson.object).draw(false)
                    $("#modalData").modal("hide")
                    swal("Correcto", "El producto fue creado", "success")
                } else {
                    swal("Error", responseJson.message, "error")
                }
            })

    } else {
        fetch("/Product/UpdateProduct", {
            method: "PUT",
            body: formData
        })
            .then(response => {
                $("#modalData").find("div.modal-content").LoadingOverlay("hide");
                return response.ok ? response.json() : Promise.reject(response);
            })
            .then(responseJson => {
                if (responseJson.state) {
                    tableData.row(selectedFill).data(responseJson.object).draw(false);
                    selectedFill = null;
                    $("#modalData").modal("hide")
                    swal("Correcto", "El producto fue modificado", "success")
                } else {
                    swal("Error", responseJson.message, "error")
                }
            })
    }

})

let selectedFill;
$("#tbdata tbody").on("click", ".btn-editar", function () {

    if ($(this).closest("tr").hasClass("child")) {
        selectedFill = $(this).closest("tr").prev();
    } else {
        selectedFill = $(this).closest("tr");
    }

    const data = tableData.row(selectedFill).data();

    showModal(data);

})



$("#tbdata tbody").on("click", ".btn-eliminar", function () {

    let fill;
    if ($(this).closest("tr").hasClass("child")) {
        fill = $(this).closest("tr").prev();
    } else {
        fill = $(this).closest("tr");
    }

    const data = tableData.row(selectedFill).data();

    swal({
        title: "Estas seguro?",
        text: `Eliminar el producto "${data.description}"`,
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "Si,Eliminar",
        cancelButtonText: "No, Cancelar",
        closeOnConfirm: false,
        closeOnCancel: true
    },
        function (response) {
            if (response) {
                $(".showSweetAlert").LoadingOverlay("show");

                fetch(`/Product/DeleteProduct?productId=${data.productId}`, {
                    method: "DELETE"
                })
                    .then(response => {
                        $(".showSweetAlert").LoadingOverlay("hide");
                        return response.ok ? response.json() : Promise.reject(response);
                    })
                    .then(responseJson => {
                        if (responseJson.state) {
                            tableData.row(fill).remove().draw()
                            swal("Correcto", "El producto fue Eliminado", "success")
                        } else {
                            swal("Error", responseJson.message, "error")
                        }
                    })
            }
        }
    )

})