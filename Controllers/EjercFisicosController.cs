using Microsoft.AspNetCore.Mvc;
using Trabajos.Data;
using Trabajos.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;

namespace Trabajos.Controllers
{
    [Authorize]
    public class EjercFisicosController : Controller
    {
        private ApplicationDbContext _context;

        //Constructor
        public EjercFisicosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Obtener lista de ejercicios físicos
        public JsonResult ListadoEjercicios(int? id)
        {
            var ejercicios = _context.EjercFisicos.ToList();

            if (id != null)
            {
                ejercicios = ejercicios.Where(e => e.EjercicioFisicoID == id).ToList();
            }

            return Json(ejercicios);
        }

        // Agregar nuevo ejercicio físico
        public JsonResult AgregarEjercicio(int EjercicioFisicoID, int Id, DateTime inicio, DateTime fin, EstadoEmocional estadoInicio, EstadoEmocional estadoFin, string observaciones)
        {
            string resultado = "";

            if (inicio != null && fin != null)
            {
                var ejercicio = new EjercFisico
                {
                    EjercicioFisicoID = EjercicioFisicoID,
                    TipoEjercFisicoID = Id,
                    Inicio = inicio,
                    Fin = fin,
                    EstadoEmocionalInicio = estadoInicio,
                    EstadoEmocionalFin = estadoFin,
                    Observaciones = observaciones
                };

                _context.EjercFisicos.Add(ejercicio);
                _context.SaveChanges();
            }
            else
            {
                resultado = "Los campos de inicio y fin son obligatorios.";
            }

            return Json(resultado);
        }

        // Editar ejercicio físico
        public JsonResult EditarEjercicio(int EjercicioFisicoID, int Id, DateTime inicio, DateTime fin, EstadoEmocional estadoInicio, EstadoEmocional estadoFin, string observaciones, TipoEjercFisico tipoEjercFisico)
        {
            string resultado = "";

            var ejercicio = _context.EjercFisicos.Find(EjercicioFisicoID);
            if (ejercicio != null)
            {
                ejercicio.TipoEjercFisicoID = Id;
                ejercicio.Inicio = inicio;
                ejercicio.Fin = fin;
                ejercicio.EstadoEmocionalInicio = estadoInicio;
                ejercicio.EstadoEmocionalFin = estadoFin;
                ejercicio.Observaciones = observaciones;
                ejercicio.TipoEjercFisico = tipoEjercFisico;

                _context.SaveChanges();
            }
            else
            {
                resultado = "No se encontró el ejercicio físico especificado.";
            }

            return Json(resultado);
        }

        // Eliminar ejercicio físico
        public JsonResult EliminarEjercicio(int EjercicioFisicoID)
        {
            var ejercicio = _context.EjercFisicos.Find(EjercicioFisicoID);
            if (ejercicio != null)
            {
                _context.EjercFisicos.Remove(ejercicio);
                _context.SaveChanges();
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }
    }
}
