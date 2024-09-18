window.onload = ListadoEjercicios();

function ListadoEjercicios(){
    let fechaDesdeBuscar = $("#FechaDesdeBuscar").val();
    let fechaHastaBuscar = $("#FechaHastaBuscar").val();

    $.ajax({
        // la URL para la petición
        url: '../../EjercFisicos/MostrarEjerciciosPorLugar',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: { FechaDesdeBuscar: fechaDesdeBuscar,
            FechaHastaBuscar:fechaHastaBuscar
         },
        // especifica si será una petición POST o GET
        type: 'POST',
        // el tipo de información que se espera de respuesta
        dataType: 'json',
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success: function (lugaresMostrar) {


            let contenidoTabla = ``;

            $.each(lugaresMostrar, function (index, lugar) {  
                

                contenidoTabla += `
                <tr class="table-success">
                    <td class="anchoCelda">${lugar.nombre}</td>
                    <td class="text-center anchoCelda"></td>
                    <td class="anchoCelda"></td>
                    <td class="text-center anchoCelda"></td>
                    <td class="anchoCelda"></td>
                    <td class="text-center anchoCelda anchoBotones"></td>
                </tr>
             `;

            

                $.each(lugar.listadoEjercicios, function (index, ejercFisico) {  
                    contenidoTabla += `
                    <tr>
                        <td class="anchoCelda"></td>
                        <td class="text-center anchoCelda">${ejercFisico.tipoEjercFisicoNombre}</td>
                        <td class="text-center anchoCelda">${ejercFisico.inicioNombre}</td>
                        <td class="text-center anchoCelda">${ejercFisico.finNombre}</td>
                        <td class="text-center anchoCelda">${ejercFisico.intervaloEjercicio}</td>
                        <td class="text-center anchoCelda">${ejercFisico.observaciones}</td>
                    </tr>
                 `;
                });


            });

            document.getElementById("tbody-ejerciciosfisicos").innerHTML = contenidoTabla;

        },

        // código a ejecutar si la petición falla;
        // son pasados como argumentos a la función
        // el objeto de la petición en crudo y código de estatus de la petición
        error: function (xhr, status) {
            console.log('Disculpe, existió un problema al cargar el listado');
        }
    });
}
