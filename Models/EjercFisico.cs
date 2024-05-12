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
    }
