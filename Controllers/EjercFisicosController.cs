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
        var lugares = _context.Lugares.ToList();
        var tiposEjercFisicosBuscar = tipoEjercicios.ToList();

        tipoEjercicios.Add(new TipoEjercFisico{TipoEjercFisicoID = 0, Nombre = "[SELECCIONE...]"});
        ViewBag.TipoEjercFisicoID = new SelectList(tipoEjercicios.OrderBy(c => c.Nombre), "TipoEjercFisicoID", "Nombre");

        lugares.Add(new Lugar{LugarID = 0, Nombre = "[SELECCIONE...]"});
        ViewBag.LugarID = new SelectList(lugares.OrderBy(l => l.Nombre), "LugarID", "Nombre");

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
        var lugares = _context.EjercFisicos.Include(l => l.Lugar).ToList();


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
            IntervaloEjercicio = e.IntervaloEjercicio,
            EstadoEmocionalFin = e.EstadoEmocionalFin,
            EstadoEmocionalFinNombre = e.EstadoEmocionalFin.ToString(),
            EstadoEmocionalInicio = e.EstadoEmocionalInicio,
            EstadoEmocionalInicioNombre = e.EstadoEmocionalInicio.ToString(),
            Observaciones = e.Observaciones,
            LugarID = e.LugarID,
            LugarNombre = e.Lugar.Nombre,
        })
        .ToList();

        return Json(vistaEjercicioFisico);
    }


    public IActionResult EjerciciosPorLugar()
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

        var lugares = _context.Lugares.ToList();
        var lugaresBuscar = lugares.ToList();
        var tipos = _context.TipoEjercFisicos.ToList();

        lugares.Add(new Lugar { LugarID = 0, Nombre = "[SELECCIONE...]" });
        ViewBag.LugarID = new SelectList(lugares.OrderBy(c => c.Nombre), "LugarID", "Nombre");

        lugaresBuscar.Add(new Lugar { LugarID = 0, Nombre = "[TODOS]" });
        ViewBag.LugarBuscarID = new SelectList(lugaresBuscar.OrderBy(c => c.Nombre), "LugarID", "Nombre");

        return View();
    }


      public JsonResult MostrarEjerciciosPorLugar(DateTime? FechaDesdeBuscar, DateTime? FechaHastaBuscar)
    {
        //INICIALIZAMOS EL LISTADO DE ELEMENTOS VACIOS
        List<VistaLugar> lugaresMostrar = new List<VistaLugar>();


        //BUSCAMOS EL LISTADO COMPLETO DE EJERCICIOS FISICOS
        var ejerciciosFisicos = _context.EjercFisicos.Include(t => t.Lugar).Include(te => te.TipoEjercFisico).ToList();


        //FILTRAMOS POR FECHA EN EL CASO DE QUE SEAN DISTINTOS DE NULO
        if(FechaDesdeBuscar != null && FechaHastaBuscar != null)
        {
             ejerciciosFisicos = ejerciciosFisicos.Where(t => t.Inicio >= FechaDesdeBuscar && t.Inicio <= FechaHastaBuscar).ToList();
        }

        //RECORREMOS LOS EJERCICIOS ORDENADOS POR DESCRIPCION DE TIPO DE EJERCICIO Y LUEGO POR FECHA DE INICIO
        foreach (var e in ejerciciosFisicos.OrderBy(t => t.Inicio).OrderBy(t => t.Lugar.Nombre))
        {
           
            //POR CADA EJERCICIO BUSCAR SI EXISTE EN EL LISTADO EL TIPO DE EJERCICIO 
            var lugarMostrar = lugaresMostrar.Where(t => t.LugarID == e.LugarID).SingleOrDefault();
            if(lugarMostrar == null){
                //SI NO EXISTE, AGREGARLO AL LISTADO 
                lugarMostrar = new VistaLugar
                {
                    LugarID = e.LugarID,
                    Nombre = e.Lugar.Nombre,
                    TipoEjercFisicoID = e.TipoEjercFisico.TipoEjercFisicoID,
                    TipoEjercFisicoNombre = e.TipoEjercFisico.Nombre,
                    ListadoEjercicios = new List<VistaEjercicioFisico>()
                };
                lugaresMostrar.Add(lugarMostrar);
            }

            
            //LUEGO ARMAMOS EL OBJETO DE SEGUNDO NIVEL CON LOS DATOS DEL EJERCICIO
            var vistaEjercicio = new VistaEjercicioFisico
            {
            EjercicioFisicoID = e.EjercicioFisicoID,
            // TipoEjercFisicoNombre = e.TipoEjercFisico.Nombre,
            LugarID = e.LugarID,
            TipoEjercFisicoID = e.TipoEjercFisicoID,
            LugarNombre = e.Lugar.Nombre,
            TipoEjercFisicoNombre = e.TipoEjercFisico.Nombre,
            Inicio = e.Inicio,
            InicioNombre = e.Inicio.ToString("dd/MM/yyyy HH:mm"),
            Fin = e.Fin,
            FinNombre = e.Fin.ToString("dd/MM/yyyy HH:mm"),
            IntervaloEjercicio = e.IntervaloEjercicio,
            EstadoEmocionalFin = e.EstadoEmocionalFin,
            EstadoEmocionalFinNombre = e.EstadoEmocionalFin.ToString().ToUpper(),
            EstadoEmocionalInicio = e.EstadoEmocionalInicio,
            EstadoEmocionalInicioNombre = e.EstadoEmocionalInicio.ToString().ToUpper(),

            //ATENCIÓN A LA CONDICION PARA MOSTRAR O NO LA OBSERVACIÓN
            Observaciones = String.IsNullOrEmpty(e.Observaciones) ? "" : e.Observaciones
            };

            //LUEGO AGREGAMOS ESE OBJETO DE EJERCICIO AL LISTADO DE EJERCICIOS DE ESE TIPO DE EJERCICIO CORRESPONDIENTE
            lugarMostrar.ListadoEjercicios.Add(vistaEjercicio);          
        }

       return Json(lugaresMostrar);
    }

        public JsonResult AgregarUnEjercFisico(int ejercicioFisicoID, int tipoEjercFisicoID, DateTime inicio, DateTime fin, 
        EstadoEmocional estadoEmocionalInicio, EstadoEmocional estadoEmocionalFin, string observaciones, int lugarID)
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
                        LugarID = lugarID,
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
                        ejercicioEditar.LugarID = lugarID;  
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

        


    public IActionResult EjerciciosPorTipo()
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
        var tiposEjerciciosBuscar = tipoEjercicios.ToList();

        tipoEjercicios.Add(new TipoEjercFisico { TipoEjercFisicoID = 0, Nombre = "[SELECCIONE...]" });
        ViewBag.TipoEjercFisicoID = new SelectList(tipoEjercicios.OrderBy(c => c.Nombre), "TipoEjercicioID", "Nombre");

        tiposEjerciciosBuscar.Add(new TipoEjercFisico { TipoEjercFisicoID = 0, Nombre = "[TODOS]" });
        ViewBag.TipoEjercicioBuscarID = new SelectList(tiposEjerciciosBuscar.OrderBy(c => c.Nombre), "TipoEjercicioID", "Nombre");

        return View();
    }


     
      public JsonResult MostrarEjerciciosPorTipo(DateTime? FechaDesdeBuscar, DateTime? FechaHastaBuscar)
    {
        //INICIALIZAMOS EL LISTADO DE ELEMENTOS VACIOS
        List<VistaTipoEjercicio> tiposEjerciciosMostrar = new List<VistaTipoEjercicio>();


        //BUSCAMOS EL LISTADO COMPLETO DE EJERCICIOS FISICOS
        // var ejerciciosFisicos = _context.EjercFisicos.Include(t => t.TipoEjercFisico).ToList();
        var ejerciciosFisicos = _context.EjercFisicos.Include(t => t.Lugar).Include(te => te.TipoEjercFisico).ToList();



        //FILTRAMOS POR FECHA EN EL CASO DE QUE SEAN DISTINTOS DE NULO
        if(FechaDesdeBuscar != null && FechaHastaBuscar != null)
        {
             ejerciciosFisicos = ejerciciosFisicos.Where(t => t.Inicio >= FechaDesdeBuscar && t.Inicio <= FechaHastaBuscar).ToList();
        }

        //RECORREMOS LOS EJERCICIOS ORDENADOS POR DESCRIPCION DE TIPO DE EJERCICIO Y LUEGO POR FECHA DE INICIO
        foreach (var e in ejerciciosFisicos.OrderBy(t => t.Inicio).OrderBy(t => t.TipoEjercFisico.Nombre))
        {
           
            //POR CADA EJERCICIO BUSCAR SI EXISTE EN EL LISTADO EL TIPO DE EJERCICIO 
            var tipoEjercicioMostrar = tiposEjerciciosMostrar.Where(t => t.TipoEjercFisicoID == e.TipoEjercFisicoID).SingleOrDefault();
            if(tipoEjercicioMostrar == null){
                //SI NO EXISTE, AGREGARLO AL LISTADO 
                tipoEjercicioMostrar = new VistaTipoEjercicio
                {

                    ////////////acacacacacacacacacacacacaccaca
                    
                    TipoEjercFisicoID = e.TipoEjercFisicoID,
                    Nombre = e.TipoEjercFisico.Nombre,
                    ListadoEjercicios = new List<VistaEjercicioFisico>()
                };
                tiposEjerciciosMostrar.Add(tipoEjercicioMostrar);
            }

            
            //LUEGO ARMAMOS EL OBJETO DE SEGUNDO NIVEL CON LOS DATOS DEL EJERCICIO
            var vistaEjercicio = new VistaEjercicioFisico
            {
            EjercicioFisicoID = e.EjercicioFisicoID,
            // TipoEjercFisicoNombre = e.TipoEjercFisico.Nombre,

            LugarID = e.LugarID,
            TipoEjercFisicoID = e.TipoEjercFisicoID,
            LugarNombre = e.Lugar.Nombre,
            TipoEjercFisicoNombre = e.TipoEjercFisico.Nombre,
            Inicio = e.Inicio,
            InicioNombre = e.Inicio.ToString("dd/MM/yyyy HH:mm"),
            Fin = e.Fin,
            FinNombre = e.Fin.ToString("dd/MM/yyyy HH:mm"),
            IntervaloEjercicio = e.IntervaloEjercicio,
            EstadoEmocionalFin = e.EstadoEmocionalFin,
            EstadoEmocionalFinNombre = e.EstadoEmocionalFin.ToString().ToUpper(),
            EstadoEmocionalInicio = e.EstadoEmocionalInicio,
            EstadoEmocionalInicioNombre = e.EstadoEmocionalInicio.ToString().ToUpper(),

            //ATENCIÓN A LA CONDICION PARA MOSTRAR O NO LA OBSERVACIÓN
            Observaciones = String.IsNullOrEmpty(e.Observaciones) ? "" : e.Observaciones
            };

            //LUEGO AGREGAMOS ESE OBJETO DE EJERCICIO AL LISTADO DE EJERCICIOS DE ESE TIPO DE EJERCICIO CORRESPONDIENTE
            tipoEjercicioMostrar.ListadoEjercicios.Add(vistaEjercicio);          
        }

       return Json(tiposEjerciciosMostrar);
    } 

}       