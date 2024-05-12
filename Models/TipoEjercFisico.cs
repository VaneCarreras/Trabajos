using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace Trabajos.Models;

public class TipoEjercFisico

{
    [Key]
    public int TipoEjercFisicoID { get; set; }

    public string? Nombre { get; set; }

    public bool Eliminado { get; set; }

    public virtual ICollection<EjercFisico> EjerciciosFisicos { get; set; }
}