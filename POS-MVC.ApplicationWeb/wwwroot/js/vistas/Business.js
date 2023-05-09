
$(document).ready(function () {

    $(".card-body").LoadingOverlay("show");

    fetch("/Business/GetBusiness")
        .then(response => {
            $(".card-body").LoadingOverlay("hide");
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            if (responseJson.state) {
                const b = responseJson.object

                $("txtNumeroDocumento").val(b.documentNumber)
                $("txtRazonSocial").val(b.name)
                $("txtCorreo").val(b.email)
                $("txtDireccion").val(b.address)
                $("txTelefono").val(b.phone)
                $("txtImpuesto").val(b.taxRate)
                $("txtSimboloMoneda").val(b.currencySymbol)
                $("txtLogo").attr("src", b.logoUrl)

            } else {
                swal("Error", responseJson.message, "error")
            }
        })
})

//Save Business
$("#btnGuardarCambios").click(function () {

    const inputs = $("input.input-validar").serializeArray();
    const inputsOutValue = inputs.filter((item) => item.value.trim() == "")

    if (inputsOutValue.length > 0) {
        const message = `Debe completar el campo :"${inputsOutValue[0].name}"`;
        toastr.warning("", message)
        $(`input[name="${inputsOutValue[0].name}"]`).focus()
        return;
    }

    const model = {
        documentNumber: $("txtNumeroDocumento").val(),
        name: $("txtRazonSocial").val(),
        email: $("txtCorreo").val(),
        address: $("txtDireccion").val(),
        phone: $("txTelefono").val(),
        taxRate: $("txtImpuesto").val(),
        currencySymbol: $("txtSimboloMoneda").val()

    }

    const inputLogo = document.getElementById("txtLogo")
    const formData = new FormData()

    formData.append("logo", inputLogo.files[0])
    formData.append("model", JSON.stringify(model))

    $(".card-body").LoadingOverlay("show");

    fetch("/Business/SaveBusiness", {
        method: "POST",
        body: formData
    })
        .then(response => {
            $(".card-body").LoadingOverlay("hide");
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            if (responseJson.state) {
                const b = responseJson.object

                $("#imgLogo").attr("src", b.logoUrl)

            } else {
                swal("Error", responseJson.message, "error")
            }
        })
})
