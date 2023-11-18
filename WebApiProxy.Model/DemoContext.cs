
using Microsoft.EntityFrameworkCore;
using Proyecto.Model.Entities;

namespace Proyecto.Model
{
    public class DemoContext : DbContext
    {
        public DemoContext(DbContextOptions<DemoContext> options) : base(options) { }

        public DbSet<WeatherForecast> WeatherForecast { get; set; }
        //Area DbSet tablas y entidades

    }
}
