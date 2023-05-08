const UserModel = {
    userId: 0,
    name: "",
    email: "",
    phone: "",
    roleId: 0,
    isActive: 1,
    photoUrl: ""
}

let tableData;

//List Users
$(document).ready(function () {

    fetch("/User/ListRoles")
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            if (responseJson.length > 0) {
                responseJson.forEach((item) => {
                    $("#cboRol").append(
                        $("<option>").val(item.roleId).text(item.description)
                    )
                })
            }
        })

    tableData = $('#tbdata').DataTable({
        responsive: true,
        "ajax": {
            "url": '/User/ListUsers',
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "userId", "visible": false, "searchable": false },
            {
                "data": "photoUrl", render: function (data) {
                    return `<img style = "height:60px" src=${data} class="rounded mx-auto d-block"/>`
                }
            },
            { "data": "name" },
            { "data": "email" },
            { "data": "phone" },
            { "data": "role" },
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
                filename: 'Reporte Usuarios',
                exportOptions: {
                    columns: [2, 3, 4, 5, 6]
                }
            }, 'pageLength'
        ],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        },
    });
})

//Modal User
function showModal(model = UserModel) {
    $("#txtId").val(model.userId)
    $("#txtNombre").val(model.name)
    $("#txtCorreo").val(model.email)
    $("#txtTelefono").val(model.phone)
    $("#txtRol").val(model.roleId == 0 ? $("#cboRol option:first").val() : model.roleId)
    $("#txtEstado").val(model.isActive)
    $("#txtFoto").val("")
    $("#imgUsuario").attr("src", model.photoUrl)

    $("#modalData").modal("show")

}

$("#btnNuevo").click(function () {
    showModal()
})

//Save User
$("#btnGuardar").click(function () {
    const inputs = $("input.input-validar").serializeArray();
    const inputsOutValue = inputs.filter((item) => item.value.trim() == "")
    if (inputsOutValue.length > 0) {
        const message = `Debe completar el campo :"${inputsOutValue[0].name}"`;
        toastr.warning("", message)
        $(`input[name="${inputsOutValue[0].name}"]`).focus()
        return;
    }

    const model = structuredClone(UserModel);
    model["userId"] = parseInt($("#txtId").val())
    model["name"] = $("#txtNombre").val()
    model["email"] = $("#txtCorreo").val()
    model["phone"] = $("#txtTelefono").val()
    model["roleId"] = $("#cboRol").val()
    model["isActive"] = $("#cboEstado").val()

    const inputPhoto = document.getElementById("txtFoto")
    const formData = new FormData();

    formData.append("photo", inputPhoto.files[0])
    formData.append("model", JSON.stringify(model))

    $("#modalData").find("div.modal-content").LoadingOverlay("show");

    if (model.userId == 0) {
        fetch("/User/CreateUser", {
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
                    swal("Correcto", "El usuario fue creado", "success")
                } else {
                    swal("Error", responseJson.message, "error")
                }
            })

    } else {
        fetch("/User/UpdateUser", {
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
                    swal("Correcto", "El usuario fue modificado", "success")
                } else {
                    swal("Error", responseJson.message, "error")
                }
            })
    }

})


//Edit User
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


//Delete User
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
        text: `Eliminar al usuario "${data.name}"`,
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

                fetch(`/User/DeleteUser?id=${data.userId}`, {
                    method: "DELETE"
                })
                    .then(response => {
                        $(".showSweetAlert").LoadingOverlay("hide");
                        return response.ok ? response.json() : Promise.reject(response);
                    })
                    .then(responseJson => {
                        if (responseJson.state) {
                            tableData.row(fill).remove().draw()
                            swal("Correcto", "El usuario fue Eliminado", "success")
                        } else {
                            swal("Error", responseJson.message, "error")
                        }
                    })
            }
        }
    )

})
