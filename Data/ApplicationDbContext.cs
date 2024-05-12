using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Trabajos.Models;

namespace Trabajos.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<TipoEjercFisico> TipoEjercFisicos { get; set; }

    public DbSet<EjercFisico> EjercFisicos { get; set; }
}
