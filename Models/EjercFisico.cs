using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Trabajos.Models;

namespace Trabajos.Models
{
    public class EjercFisico{

        [Key]
        public int EjercicioFisicoID { get; set; }

        public int TipoEjercFisicoID { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
        public EstadoEmocional EstadoEmocionalInicio {get; set; } 
        public EstadoEmocional EstadoEmocionalFin {get; set; } 
        public string? Observaciones {get; set; }

        public virtual TipoEjercFisico TipoEjercFisico { get; set; }
    }

    public enum EstadoEmocional{
        Feliz = 1,
        Triste,
        Enojado,
        Ansioso,
        Estresado,
        Relajado,
        Aburrido,
        Emocionado,
        Agobiado,
        Confundido,
        Optimista,
        Pesimista,
        Motivado,
        Cansado,
        Eufórico,
        Agitado,
        Satisfecho,
        Desanimado
    }  
        public class VistaSumaEjercicioFisico
    {
        public string? TipoEjercicioNombre {get; set;}
        public int TotalidadMinutos {get; set; }
        public int TotalidadDiasConEjercicio {get;set;}
        public int TotalidadDiasSinEjercicio {get;set;}

        public List<VistaEjercicioFisico>? DiasEjercicios {get;set;}
    }

    public class VistaEjercicioFisico
    {   
        public int EjercicioFisicoID { get; set; }
        public int TipoEjercFisicoID { get; set; }
        public string? TipoEjercFisicoNombre {get; set; }
        public DateTime Inicio { get; set; }
        public string? InicioNombre { get; set; }
        public DateTime Fin { get; set; }
        public string? FinNombre { get; set; }
        public EstadoEmocional EstadoEmocionalInicio {get; set; } 
        public string? EstadoEmocionalInicioNombre {get; set; }
        public EstadoEmocional EstadoEmocionalFin {get; set; } 
        public string? EstadoEmocionalFinNombre {get; set; }
        public string? Observaciones {get; set; }
    }

    public class VistaPorDiaEjercicioFisico
    {   
        public int Anio {get; set; }  
        public string? Mes { get; set; }
        public int? Dia { get; set; }
        public int CantidadMinutos { get; set; }
    }

    }
