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
                            <button type="button" class="btn btn-success" onclick="AbrirModalEditar(${tipoEjercFisico.tipoEjercFisicoID})">
                                Editar
                            </button>
                        </td>
                        <td class="text-center">
                            <button type="button" class="btn btn-danger" onclick="EliminarRegistro(${tipoEjercFisico.tipoEjercFisicoID})">
                                Eliminar
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

function EliminarRegistro(tipoEjercFisicoID){
    $.ajax({
        url: '../../TipoEjercFisicos/EliminarTipoEjercicio',
        data: {tipoEjercFisicoID : tipoEjercFisicoID},
        type: 'POST',
        dataType: 'json',
        success: function (resultado) {           
            ListadoTipoEjercicios();
        },
        error: function (xhr, status) {
            console.log('Disculpe, existió un problema al eliminar el registro.');
        }
    });    
}
