window.onload = ListadoEjercicios();

function ListadoEjercicios(){
    $.ajax({
        url: '../../EjercFisicos/ListadoEjercicios',
        data: {},
        type: 'POST',
        dataType: 'json',
        success: function (ejercFisicos) {
            $("#ModalEjercFisico").modal("hide");
            LimpiarModal();
            let contenidoTabla = ``;
            $.each(ejercFisicos, function (index, ejercFisico) {  
                contenidoTabla += `
                    <tr>  
                        <td>${ejercFisico.tipoEjercFisicoID}</td>
                        <td>${ejercFisico.inicio}</td>
                        <td>${ejercFisico.fin}</td>
                        <td>${ejercFisico.estadoEmocionalInicio}</td>
                        <td>${ejercFisico.estadoEmocionalFin}</td>
                        <td>${ejercFisico.observaciones}</td>
                        <td class="text-center">
                            <button type="button" class="btn btn-success" onclick="AbrirModalEditar(${ejercFisico.ejercicioFisicoID})">
                                Editar
                            </button>
                        </td>
                        <td class="text-center">
                            <button type="button" class="btn btn-danger" onclick="EliminarRegistro(${ejercFisico.ejercicioFisicoID})">
                                Eliminar
                            </button>
                        </td>
                    </tr>
                `;
            });
            document.getElementById("tbody-ejercicio").innerHTML = contenidoTabla;
        },
        error: function (xhr, status) {
            console.log('Disculpe, existió un problema al cargar el listado');
        }
    });
}

function LimpiarModal(){
    document.getElementById("EjercicioFisicoID").value = 0;
    document.getElementById("TipoEjercFisicoID").selectedIndex = 0; // Limpiar selección
    document.getElementById("FechaInicio").value = "";
    document.getElementById("FechaFin").value = "";
    document.getElementById("EstadoEmocionalInicio").selectedIndex = 0; // Limpiar selección
    document.getElementById("EstadoEmocionalFin").selectedIndex = 0; // Limpiar selección
    document.getElementById("observaciones").value = "";
}

function NuevoRegistro(){
    $("#ModalTitulo").text("Nuevo Ejercicio");
}

function AbrirModalEditar(EjercicioFisicoID){
    $.ajax({
        url: '../../EjercFisicos/ListadoEjercicios',
        data: { id: EjercicioFisicoID},
        type: 'POST',
        dataType: 'json',
        success: function (ejercFisicos) {
            let ejercFisico = ejercFisicos[0];
            document.getElementById("EjercicioFisicoID").value = EjercicioFisicoID;
            $("#ModalTitulo").text("Editar Ejercicio");
            document.getElementById("TipoEjercFisicoID").value = ejercFisico.tipoEjercFisicoID;
            document.getElementById("FechaInicio").value = ejercFisico.inicio;
            document.getElementById("FechaFin").value = ejercFisico.fin;
            document.getElementById("EstadoEmocionalInicio").value = ejercFisico.estadoEmocionalInicio;
            document.getElementById("EstadoEmocionalFin").value = ejercFisico.estadoEmocionalFin;
            document.getElementById("observaciones").value = ejercFisico.observaciones;
            $("#ModalEjercFisico").modal("show");
        },
        error: function (xhr, status) {
            console.log('Disculpe, existió un problema al consultar el registro para ser modificado.');
        }
    });
}

function GuardarRegistro(){
    let ejercicioFisicoID = document.getElementById("EjercicioFisicoID").value;
    let tipoEjercFisicoID = document.getElementById("TipoEjercFisicoID").value;
    let inicio = document.getElementById("FechaInicio").value;
    let fin = document.getElementById("FechaFin").value;
    let estadoEmocionalInicio = document.getElementById("EstadoEmocionalInicio").value;
    let estadoEmocionalFin = document.getElementById("EstadoEmocionalFin").value;
    let observaciones = document.getElementById("observaciones").value;
    console.log(observaciones);
    $.ajax({
        url: '../../EjercFisicos/AgregarUnEjercFisico',
        data: { EjercicioFisicoID: ejercicioFisicoID, TipoEjercFisicoID: tipoEjercFisicoID, Inicio: inicio, 
            Fin: fin, EstadoEmocionalInicio: estadoEmocionalInicio, EstadoEmocionalFin: estadoEmocionalFin, observaciones: observaciones},
        type: 'POST',
        dataType: 'json',
        success: function (resultado) {
            if(resultado != ""){
                alert(resultado);
            }
            ListadoEjercicios();
        },
        error: function (xhr, status) {
            console.log('Disculpe, existió un problema al guardar el registro');
        }
    });    
}

function EliminarRegistro(EjercicioFisicoID){
    $.ajax({
        url: '../../EjercFisicos/EliminarEjercicio',
        data: {ejercicioFisicoID : EjercicioFisicoID},
        type: 'POST',
        dataType: 'json',
        success: function (resultado) {           
            ListadoEjercicios();
        },
        error: function (xhr, status) {
            console.log('Disculpe, existió un problema al eliminar el registro.');
        }
    });    
}
