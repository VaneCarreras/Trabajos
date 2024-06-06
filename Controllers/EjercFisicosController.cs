using Microsoft.AspNetCore.Mvc;
using Trabajos.Data;
using Trabajos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Diagnostics.Tracing;
using Microsoft.EntityFrameworkCore;

namespace Trabajos.Controllers;

    [Authorize]
    public class EjercFisicosController : Controller
    {
        private ApplicationDbContext _context;

//CONSTRUCTOR

    public EjercFisicosController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        // Crear una lista de SelectListItem que incluya el elemento adicional
        var selectListItems = new List<SelectListItem>
        {
            new SelectListItem { Value = "0", Text = "[SELECCIONE...]" }
        };

        // Obtener todas las opciones del enum
        var enumValues = Enum.GetValues(typeof(EstadoEmocional)).Cast<EstadoEmocional>();

        // Convertir las opciones del enum en SelectListItem
        selectListItems.AddRange(enumValues.Select(e => new SelectListItem
        {
            Value = e.GetHashCode().ToString(),
            Text = e.ToString().ToUpper()
        }));

        // Pasar la lista de opciones al modelo de la vista
        ViewBag.EstadoEmocionalInicio = selectListItems.OrderBy(t => t.Text).ToList();
        ViewBag.EstadoEmocionalFin = selectListItems.OrderBy(t => t.Text).ToList();

        var tipoEjercicios = _context.TipoEjercFisicos.ToList();
        var tiposEjercFisicosBuscar = tipoEjercicios.ToList();

        tipoEjercicios.Add(new TipoEjercFisico{TipoEjercFisicoID = 0, Nombre = "[SELECCIONE...]"});
        ViewBag.TipoEjercFisicoID = new SelectList(tipoEjercicios.OrderBy(c => c.Nombre), "TipoEjercFisicoID", "Nombre");

        // ViewBag.TipoEjercFisicoBuscarID = new SelectList(tipoEjercicios.OrderBy(c => c.Nombre), "TipoEjercFisicoID", "Nombre");
        tiposEjercFisicosBuscar.Add(new TipoEjercFisico { TipoEjercFisicoID = 0, Nombre = "[TODOS LOS TIPOS DE EJERCICIOS]" });
        ViewBag.TipoEjercFisicoBuscarID = new SelectList(tiposEjercFisicosBuscar.OrderBy(c => c.Nombre), "TipoEjercFisicoID", "Nombre");

        return View();
    }

    public JsonResult ListadoEjercicios(int? id, DateTime? FechaInicioBuscar, DateTime? FechaFinBuscar, int? TipoEjercFisicoBuscarID)
    {
        var fechaInicioBuscar = FechaInicioBuscar;
        var fechaFinBuscar = FechaFinBuscar;
        var tipoEjercFisicoBuscarID = TipoEjercFisicoBuscarID;

        
        //DEFINIMOS UNA VARIABLE EN DONDE GUARDAMOS EL LISTADO COMPLETO DE EJERCICIOS
        var ejercicios = _context.EjercFisicos.Include(t => t.TipoEjercFisico).ToList();

        if (fechaInicioBuscar != null && fechaFinBuscar != null && tipoEjercFisicoBuscarID != null &&  tipoEjercFisicoBuscarID !=0)
        {
            ejercicios = ejercicios.Where(e => e.Inicio >= fechaInicioBuscar && e.Inicio <= fechaFinBuscar && e.TipoEjercFisicoID == TipoEjercFisicoBuscarID).ToList();

        }


        //LUEGO PREGUNTAMOS SI EL USUARIO INGRESO UN ID
        //QUIERE DECIR QUE QUIERE UN EJERCICIO EN PARTICULAR
        if (id != null)
        {
            //FILTRAMOS EL LISTADO COMPLETO DE EJERCICIOS POR EL EJERCICIO QUE COINCIDA CON ESE ID
            ejercicios = ejercicios.Where(e => e.EjercicioFisicoID == id).ToList();
        }
        
        var vistaEjercicioFisico = ejercicios
        .Select(e => new VistaEjercicioFisico
        {
            EjercicioFisicoID = e.EjercicioFisicoID,
            TipoEjercFisicoID = e.TipoEjercFisicoID,
            TipoEjercFisicoNombre = e.TipoEjercFisico.Nombre,
            Inicio = e.Inicio,
            InicioNombre = e.Inicio.ToString("dd/MM/yyyy HH:mm"),
            Fin = e.Fin,
            FinNombre = e.Fin.ToString("dd/MM/yyyy HH:mm"),
            EstadoEmocionalFin = e.EstadoEmocionalFin,
            EstadoEmocionalFinNombre = e.EstadoEmocionalFin.ToString(),
            EstadoEmocionalInicio = e.EstadoEmocionalInicio,
            EstadoEmocionalInicioNombre = e.EstadoEmocionalInicio.ToString(),
            Observaciones = e.Observaciones
        })
        .ToList();

        return Json(vistaEjercicioFisico);
    }

        public JsonResult AgregarUnEjercFisico(int ejercicioFisicoID, int tipoEjercFisicoID, DateTime inicio, DateTime fin, 
        EstadoEmocional estadoEmocionalInicio, EstadoEmocional estadoEmocionalFin, string observaciones)
    {
       
        string resultado = "";

        if (tipoEjercFisicoID != 0 && observaciones != null)
        {
            observaciones = observaciones.ToUpper();

            //1- VERIFICAR SI ESTA EDITANDO O CREANDO NUEVO REGISTRO
            if (ejercicioFisicoID == 0)
            {
                
                    //2- GUARDAR EL EJERCICIO
                    var ejercFisico = new EjercFisico
                    {
                        TipoEjercFisicoID = tipoEjercFisicoID,
                        Inicio = inicio,
                        Fin = fin,
                        EstadoEmocionalInicio = estadoEmocionalInicio,
                        EstadoEmocionalFin = estadoEmocionalFin,
                        Observaciones = observaciones,
                    };
                    _context.Add(ejercFisico);
                    _context.SaveChanges();
                
                
            }
            else
            {   
                //QUIERE DECIR QUE VAMOS A EDITAR EL REGISTRO
                var ejercicioEditar = _context.EjercFisicos.Where(e => e.EjercicioFisicoID == ejercicioFisicoID).SingleOrDefault();
                if (ejercicioEditar != null)
                
                    
                
                    {
                        //CONTINUAMOS CON EL EDITAR

                        ejercicioEditar.TipoEjercFisicoID = tipoEjercFisicoID;
                        ejercicioEditar.Inicio = inicio;
                        ejercicioEditar.EstadoEmocionalInicio = estadoEmocionalInicio;
                        ejercicioEditar.Fin = fin;
                        ejercicioEditar.EstadoEmocionalFin = estadoEmocionalFin;
                        ejercicioEditar.Observaciones = observaciones;
                        _context.SaveChanges();
                    }
                    
                
            }
        }
        else
        {
            resultado = "DEBE INGRESAR UN TIPO Y UNA OBSERVACIÓN.";
        }

        return Json(resultado);
    }

    
        public JsonResult EliminarEjercicio(int EjercicioFisicoID)
    {
        var ejercicio = _context.EjercFisicos.Find(EjercicioFisicoID);
        _context.Remove(ejercicio);
        _context.SaveChanges();


        return Json(true);
    }
}       