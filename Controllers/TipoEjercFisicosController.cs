using Microsoft.AspNetCore.Mvc;
using Trabajos.Data;
using Trabajos.Models;
using Microsoft.AspNetCore.Authorization;

namespace Trabajos.Controllers;

[Authorize]

public class TipoEjercFisicosController : Controller
{

    private ApplicationDbContext _context;

    //Constructor
    public TipoEjercFisicosController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public JsonResult ListadoTipoEjercicios(int? id)
    {
        //DEFINIMOS UNA VARIABLE EN DONDE GUARDAMOS EL LISTADO COMPLETO DE TIPOS DE EJERCICIOS
        var tipoDeEjercicios = _context.TipoEjercFisicos.ToList();

        //LUEGO PREGUNTAMOS SI EL USUARIO INGRESO UN ID
        //QUIERE DECIR QUE QUIERE UN EJERCICIO EN PARTICULAR
        if (id != null)
        {
            //FILTRAMOS EL LISTADO COMPLETO DE EJERCICIOS POR EL EJERCICIO QUE COINCIDA CON ESE ID
            tipoDeEjercicios = tipoDeEjercicios.Where(t => t.TipoEjercFisicoID == id).ToList();
        }

        return Json(tipoDeEjercicios);
    }

    public JsonResult AgregarEjercFisico(int tipoEjercFisicoID, string nombre)
    {
       
        string resultado = "";

        if (!String.IsNullOrEmpty(nombre))
        {
            nombre = nombre.ToUpper();
            //INGRESA SI ESCRIBIO SI O SI 

            //2- VERIFICAR SI ESTA EDITANDO O CREANDO NUEVO REGISTRO
            if (tipoEjercFisicoID == 0)
            {
                //3- VERIFICAMOS SI EXISTE EN BASE DE DATOS UN REGISTRO CON LA MISMA DESCRIPCION
                //PARA REALIZAR ESA VERIFICACION BUSCAMOS EN EL CONTEXTO, ES DECIR EN BASE DE DATOS 
                //SI EXISTE UN REGISTRO CON ESA DESCRIPCION  
                var existeTipoEjercicio = _context.TipoEjercFisicos.Where(t => t.Nombre == nombre).Count();
                if (existeTipoEjercicio == 0)
                {
                    //4- GUARDAR EL TIPO DE EJERCICIO
                    var tipoEjercicio = new TipoEjercFisico
                    {
                        Nombre = nombre
                    };
                    _context.Add(tipoEjercicio);
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
                var tipoEjercicioEditar = _context.TipoEjercFisicos.Where(t => t.TipoEjercFisicoID == tipoEjercFisicoID).SingleOrDefault();
                if (tipoEjercicioEditar != null)
                {
                    //BUSCAMOS EN LA TABLA SI EXISTE UN REGISTRO CON EL MISMO NOMBRE PERO QUE EL ID SEA DISTINTO AL QUE ESTAMOS EDITANDO
                    var existeTipoEjercicio = _context.TipoEjercFisicos.Where(t => t.Nombre == nombre && t.TipoEjercFisicoID != tipoEjercFisicoID).Count();
                    if (existeTipoEjercicio == 0)
                    {
                        //QUIERE DECIR QUE EL ELEMENTO EXISTE Y ES CORRECTO ENTONCES CONTINUAMOS CON EL EDITAR
                        tipoEjercicioEditar.Nombre = nombre;
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

    public JsonResult EliminarTipoEjercicio(int tipoEjercFisicoID)
    {
        var tipoEjercicio = _context.TipoEjercFisicos.Find(tipoEjercFisicoID);
        _context.Remove(tipoEjercicio);
        _context.SaveChanges();


        return Json(true);
    }
}
   