
window.onload = ListadoLugares();

function ListadoLugares(){
    $.ajax({
        url: '../../Lugares/ListadoLugares',
        data: {},
        type: 'POST',
        dataType: 'json',
        success: function (lugares) {
            $("#ModalLugar").modal("hide");
            LimpiarModal();
            let contenidoTabla = ``;
            $.each(lugares, function (index, lugar) {  
                contenidoTabla += `
                    <tr>
                        <td>${lugar.nombre}</td>
                        <td class="text-center">
                            <button type="button" onclick="AbrirModalEditar(${lugar.lugarID})">
                            <i class="fa-solid fa-pen-nib" style="color: #B300FC;"></i></button>
                        </td>
                        <td class="text-center">
                            <button type="button" onclick="EliminarRegistro(${lugar.lugarID})">
                            <i class="fa-solid fa-poo" style="color: #820d19;"></i>
                            </button>
                        </td>
                    </tr>
                `;
            });
            document.getElementById("tbody-lugares").innerHTML = contenidoTabla;
        },
        error: function (xhr, status) {
            console.log('Disculpe, existió un problema al cargar el listado');
        }
    });
}

function LimpiarModal(){
    document.getElementById("LugarID").value = 0;
    document.getElementById("nombre").value = "";
}

function NuevoRegistro(){
    $("#ModalTitulo").text("Nuevo Lugar");
}

function AbrirModalEditar(LugarID){
    $.ajax({
        url: '../../Lugares/ListadoLugares',
        data: { id: LugarID},
        type: 'POST',
        dataType: 'json',
        success: function (lugares) {
            let lugar = lugares[0];
            document.getElementById("LugarID").value = LugarID;
            $("#ModalTitulo").text("Editar Lugar");
            document.getElementById("nombre").value = lugar.nombre;
            $("#ModalLugar").modal("show");
        },
        error: function (xhr, status) {
            console.log('Disculpe, existió un problema al consultar el registro para ser modificado.');
        }
    });
}

function GuardarRegistro(){
    let lugarID = document.getElementById("LugarID").value;
    let nombre = document.getElementById("nombre").value;
    console.log(nombre);
    $.ajax({
        url: '../../Lugares/AgregarLugar',
        data: { LugarID: lugarID, nombre: nombre},
        type: 'POST',
        dataType: 'json',
        success: function (resultado) {
            if(resultado != ""){
                alert(resultado);
            }
            ListadoLugares();
        },
        error: function (xhr, status) {
            console.log('Disculpe, existió un problema al guardar el registro');
        }
    });    
}


function EliminarRegistro(lugarID) {
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
                url: '../../Lugares/EliminarLugar',
                data: { lugarID: lugarID },
                type: 'POST',
                dataType: 'json',
                success: function (resultado) 
                {

                    Swal.fire({
                        title: "¡Eliminado!",
                        icon: "success"
                    });
                    ListadoLugares();
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

