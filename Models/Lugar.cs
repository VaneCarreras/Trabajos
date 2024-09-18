using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace Trabajos.Models;

public class Lugar

{
    [Key]
    public int LugarID { get; set; }

    public string? Nombre { get; set; }

    public virtual ICollection<EjercFisico> EjerciciosFisicos { get; set; }
}

// public class VistaLugar
// {   
//      public int LugarID { get; set; }
//      public string? Nombre { get; set; }
//      public List<VistaLugar>? ListadoEjercicios { get; set; }
// }

public class VistaLugar
{   
     public int LugarID { get; set; }
     public int TipoEjercFisicoID { get; set; }
     public string? Nombre { get; set; }
     public string? TipoEjercFisicoNombre { get; set; }
     public List<VistaEjercicioFisico>? ListadoEjercicios { get; set; }
}