using Microsoft.AspNetCore.Mvc;
using Trabajos.Data;
using Trabajos.Models;
using Microsoft.AspNetCore.Authorization;

namespace Trabajos.Controllers;

[Authorize]

public class LugaresController : Controller
{

    private ApplicationDbContext _context;

    //Constructor
    public LugaresController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public JsonResult ListadoLugares(int? id)
    {
        //DEFINIMOS UNA VARIABLE EN DONDE GUARDAMOS EL LISTADO COMPLETO DE TIPOS DE EJERCICIOS
        var lugares = _context.Lugares.ToList();

        //LUEGO PREGUNTAMOS SI EL USUARIO INGRESO UN ID
        //QUIERE DECIR QUE QUIERE UN EJERCICIO EN PARTICULAR
        if (id != null)
        {
            //FILTRAMOS EL LISTADO COMPLETO DE EJERCICIOS POR EL EJERCICIO QUE COINCIDA CON ESE ID
            lugares = lugares.Where(l => l.LugarID == id).ToList();
        }

        return Json(lugares);
    }

    public JsonResult AgregarLugar(int lugarID, string nombre)
    {
       
        string resultado = "";

        if (!String.IsNullOrEmpty(nombre))
        {
            nombre = nombre.ToUpper();
            //INGRESA SI ESCRIBIO SI O SI 

            //2- VERIFICAR SI ESTA EDITANDO O CREANDO NUEVO REGISTRO
            if (lugarID == 0)
            {
                //3- VERIFICAMOS SI EXISTE EN BASE DE DATOS UN REGISTRO CON LA MISMA DESCRIPCION
                //PARA REALIZAR ESA VERIFICACION BUSCAMOS EN EL CONTEXTO, ES DECIR EN BASE DE DATOS 
                //SI EXISTE UN REGISTRO CON ESA DESCRIPCION  
                var existeLugar = _context.Lugares.Where(l => l.Nombre == nombre).Count();
                if (existeLugar == 0)
                {
                    //4- GUARDAR EL TIPO DE EJERCICIO
                    var lugar = new Lugar
                    {
                        Nombre = nombre
                    };
                    _context.Add(lugar);
                    _context.SaveChanges();
                }
                else
                {
                    resultado = "YA EXISTE UN REGISTRO CON LA MISMA DESCRIPCIÓN";
                }
            }
            else
            {
                //QUIERE DECIR QUE VAMOS A EDITAR EL REGISTRO
                var lugarEditar = _context.Lugares.Where(l => l.LugarID == lugarID).SingleOrDefault();
                if (lugarEditar != null)
                {
                    //BUSCAMOS EN LA TABLA SI EXISTE UN REGISTRO CON EL MISMO NOMBRE PERO QUE EL ID SEA DISTINTO AL QUE ESTAMOS EDITANDO
                    var existeLugar = _context.Lugares.Where(l => l.Nombre == nombre && l.LugarID != lugarID).Count();
                    if (existeLugar == 0)
                    {
                        //QUIERE DECIR QUE EL ELEMENTO EXISTE Y ES CORRECTO ENTONCES CONTINUAMOS CON EL EDITAR
                        lugarEditar.Nombre = nombre;
                        _context.SaveChanges();
                    }
                    else
                    {
                        resultado = "YA EXISTE UN REGISTRO CON LA MISMA DESCRIPCIÓN";
                    }
                }
            }
        }
        else
        {
            resultado = "DEBE INGRESAR UNA DESCRIPCIÓN.";
        }

        return Json(resultado);
    }

    public JsonResult EliminarLugar(int lugarID)
    {
    var tieneEjercFisicoAsociado = _context.EjercFisicos.Any(e => e.LugarID == lugarID);


        if (tieneEjercFisicoAsociado){
                return Json(new { resultado = "Error",});
        }


        var lugar = _context.Lugares.Find(lugarID);
        _context.Remove(lugar);
        _context.SaveChanges();


        return Json(true);
    }
}
   