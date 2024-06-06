using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Trabajos.Models;


public class PanelEjercicios 
{
    public List<EjerciciosPorDia>? EjerciciosPorDias { get; set; }
    public List<VistaTipoEjercicioFisico>? VistaTipoEjercicioFisico { get; set; }
}

public class EjerciciosPorDia
{   
    public int Dia { get; set; }
    public string? Mes { get; set; }
    public int CantidadMinutos { get; set; }    
}

public class VistaTipoEjercicioFisico
{
     public int TipoEjercFisicoID { get; set; }
     public string? Nombre { get; set; } 

     public decimal CantidadMinutos { get; set; }

}

