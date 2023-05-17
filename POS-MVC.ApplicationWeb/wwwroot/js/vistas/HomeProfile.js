$(document).ready(function () {

    $(".container-fluid").LoadingOverlay("show");

    fetch("/Home/GetUser")
        .then(response => {
            $(".container-fluid").LoadingOverlay("hide");
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            if (responseJson.state) {
                const b = responseJson.object

                $("#imgFoto").attr("src", b.photoUrl)
                $("#txtNombre").val(b.name)
                $("#txtCorreo").val(b.email)
                $("#txTelefono").val(b.phone)
                $("#txtRol").val(b.role)

            } else {
                swal("Error", responseJson.message, "error")
            }
        })
})

$("#btnGuardarCambios").click(function () {

    if ($("#txtCorreo").val().trim() == "") {
        toastr.warning("", "Debe completar el campo: Correo")
        $("#txtCorreo").focus()
        return;
    }

    if ($("#txTelefono").val().trim() == "") {
        toastr.warning("", "Debe completar el campo: Telefono")
        $("#txTelefono").focus()
        return;
    }

    swal({
        title: "Desea guardar los cambios?",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-primary",
        confirmButtonText: "Si",
        cancelButtonText: "No",
        closeOnConfirm: false,
        closeOnCancel: true
    },
        function (response) {
            if (response) {
                $(".showSweetAlert").LoadingOverlay("show");

                let model = {
                    email: $("#txtCorreo").val().trim(),
                    phone: $("#txTelefono").val().trim()
                }

                fetch("/Home/SaveProfile", {
                    method: "POST",
                    headers: { "Content-Type": "application/json; chatset=utf-8" },
                    body: JSON.stringify(model)
                })
                    .then(response => {
                        $(".showSweetAlert").LoadingOverlay("hide");
                        return response.ok ? response.json() : Promise.reject(response);
                    })
                    .then(responseJson => {
                        if (responseJson.state) {
                            swal("Correcto", "Se han registrado los cambios", "success")
                        } else {
                            swal("Error", responseJson.message, "error")
                        }
                    })
            }
        }
    )
})

$("#btnCambiarClave").click(function () {

    const inputs = $("input.input-validar").serializeArray();
    const inputsOutValue = inputs.filter((item) => item.value.trim() == "")
    if (inputsOutValue.length > 0) {
        const message = `Debe completar el campo :"${inputsOutValue[0].name}"`;
        toastr.warning("", message)
        $(`input[name="${inputsOutValue[0].name}"]`).focus()
        return;
    }

    if ($("#txtClaveNueva").val().trim() != $("#txtConfirmarClave").val().trim()) {
        toastr.warning("", "Las contrasenas no coinciden")
        $("#txtClaveNueva").focus()
        return;
    }

    let model = {
        currentPassword: $("#txtClaveActual").val().trim(),
        newPassword: $("#txtClaveNueva").val().trim(),
    }

    fetch("/Home/ChangePassword", {
        method: "POST",
        headers: { "Content-Type": "application/json; chatset=utf-8" },
        body: JSON.stringify(model)
    })
        .then(response => {
            $(".showSweetAlert").LoadingOverlay("hide");
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            if (responseJson.state) {
                swal("Correcto", "Su contrasena ha sido actualizada", "success")
                $("input.input-validar").val("");
            } else {
                swal("Error", responseJson.message, "error")
            }
        })

})