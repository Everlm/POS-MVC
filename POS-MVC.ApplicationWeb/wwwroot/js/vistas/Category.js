const CategoryModel = {
    categoryId: 0,
    description: "",
    isActive: 1
}

let tableData;

$(document).ready(function () {

    tableData = $('#tbdata').DataTable({
        responsive: true,
        "ajax": {
            "url": '/Category/ListCategories',
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "categoryId", "visible": false, "searchable": false },
            { "data": "description" },
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
                filename: 'Reporte Categorias',
                exportOptions: {
                    columns: [1, 2]
                }
            }, 'pageLength'
        ],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        },
    });
})


function showModal(model = CategoryModel) {
    $("#txtId").val(model.categoryId)
    $("#txtDescripcion").val(model.description)
    $("#txtEstado").val(model.isActive)

    $("#modalData").modal("show")
}

$("#btnNuevo").click(function () {
    showModal()
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



$("#btnGuardar").click(function () {

    if ($("#txtDescripcion").val().trim() == "") {
        toastr.warning("", "Debe completar el campo: Descripcion")
        $("#txtDescripcion").focus()
        return;
    }

    const model = structuredClone(CategoryModel);
    model["categoryId"] = parseInt($("#txtId").val())
    model["description"] = $("#txtDescripcion").val()
    model["isActive"] = $("#cboEstado").val()

    $("#modalData").find("div.modal-content").LoadingOverlay("show");

    if (model.categoryId == 0) {
        fetch("/Category/CreateCategory", {
            method: "POST",
            headers: { "Content-Type": "application/json; chatset=utf-8" },
            body: JSON.stringify(model)
        })
            .then(response => {
                $("#modalData").find("div.modal-content").LoadingOverlay("hide");
                return response.ok ? response.json() : Promise.reject(response);
            })
            .then(responseJson => {
                if (responseJson.state) {
                    tableData.row.add(responseJson.object).draw(false)
                    $("#modalData").modal("hide")
                    swal("Correcto", "La categoria fue creada", "success")
                } else {
                    swal("Error", responseJson.message, "error")
                }
            })

    } else {
        fetch("/Category/UpdateCategory", {
            method: "PUT",
            headers: { "Content-Type": "application/json; chatset=utf-8" },
            body: JSON.stringify(model)
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
                    swal("Correcto", "La categoria ha sido modificada", "success")
                } else {
                    swal("Error", responseJson.message, "error")
                }
            })
    }

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
        text: `Eliminar la categoria "${data.description}"`,
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

                fetch(`/Category/DeleteCategory?categoryId=${data.categoryId}`, {
                    method: "DELETE"
                })
                    .then(response => {
                        $(".showSweetAlert").LoadingOverlay("hide");
                        return response.ok ? response.json() : Promise.reject(response);
                    })
                    .then(responseJson => {
                        if (responseJson.state) {
                            tableData.row(fill).remove().draw()
                            swal("Correcto", "La categoria ha sido eliminada", "success")
                        } else {
                            swal("Error", responseJson.message, "error")
                        }
                    })
            }
        }
    )

})
