


window.onload = ListadoTipoEjercicios();

function ListadoTipoEjercicios(){
    $.ajax({
        url: '../../TipoEjercFisicos/ListadoTipoEjercicios',
        data: {},
        type: 'POST',
        dataType: 'json',
        success: function (tipoEjercFisicos) {
            $("#ModalTipoEjercFisico").modal("hide");
            LimpiarModal();
            let contenidoTabla = ``;
            $.each(tipoEjercFisicos, function (index, tipoEjercFisico) {  
                contenidoTabla += `
                    <tr>
                        <td>${tipoEjercFisico.nombre}</td>
                        <td class="text-center">
                            <button type="button" onclick="AbrirModalEditar(${tipoEjercFisico.tipoEjercFisicoID})">
                            <i class="fa-solid fa-pen-nib" style="color: #B300FC;"></i></button>
                        </td>
                        <td class="text-center">
                            <button type="button" onclick="EliminarRegistro(${tipoEjercFisico.tipoEjercFisicoID})">
                            <i class="fa-solid fa-poo" style="color: #820d19;"></i>
                            </button>
                        </td>
                    </tr>
                `;
            });
            document.getElementById("tbody-tipoejercicios").innerHTML = contenidoTabla;
        },
        error: function (xhr, status) {
            console.log('Disculpe, existió un problema al cargar el listado');
        }
    });
}

function LimpiarModal(){
    document.getElementById("TipoEjercFisicoID").value = 0;
    document.getElementById("nombre").value = "";
}

function NuevoRegistro(){
    $("#ModalTitulo").text("Nuevo Tipo de Ejercicio");
}

function AbrirModalEditar(TipoEjercFisicoID){
    $.ajax({
        url: '../../TipoEjercFisicos/ListadoTipoEjercicios',
        data: { id: TipoEjercFisicoID},
        type: 'POST',
        dataType: 'json',
        success: function (tipoEjercFisicos) {
            let tipoEjercFisico = tipoEjercFisicos[0];
            document.getElementById("TipoEjercFisicoID").value = TipoEjercFisicoID;
            $("#ModalTitulo").text("Editar Tipo de Ejercicio");
            document.getElementById("nombre").value = tipoEjercFisico.nombre;
            $("#ModalTipoEjercFisico").modal("show");
        },
        error: function (xhr, status) {
            console.log('Disculpe, existió un problema al consultar el registro para ser modificado.');
        }
    });
}

function GuardarRegistro(){
    let tipoEjercFisicoID = document.getElementById("TipoEjercFisicoID").value;
    let nombre = document.getElementById("nombre").value;
    console.log(nombre);
    $.ajax({
        url: '../../TipoEjercFisicos/AgregarEjercFisico',
        data: { TipoEjercFisicoID: tipoEjercFisicoID, nombre: nombre},
        type: 'POST',
        dataType: 'json',
        success: function (resultado) {
            if(resultado != ""){
                alert(resultado);
            }
            ListadoTipoEjercicios();
        },
        error: function (xhr, status) {
            console.log('Disculpe, existió un problema al guardar el registro');
        }
    });    
}


function EliminarRegistro(tipoEjercFisicoID) {
    Swal.fire({
        title: "¿Seguro de eliminar?",
        icon: "question",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '../../TipoEjercFisicos/EliminarTipoEjercicio',
                data: { tipoEjercFisicoID: tipoEjercFisicoID },
                type: 'POST',
                dataType: 'json',
                success: function (resultado) {
                    Swal.fire({
                        title: "¡Eliminado!",
                        icon: "success"
                    });
                    ListadoTipoEjercicios();
                },
                error: function (xhr, status) {
                    console.log('Disculpe, existió un problema al eliminar el registro.');
                    Swal.fire({
                        title: "Error",
                        text: "Hubo un problema al eliminar el registro.",
                        icon: "error"
                    });
                }
            });
        }
    });
}

