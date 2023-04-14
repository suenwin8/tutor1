using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tutor1.Models.Entity;

namespace tutor1.Models.Context
{
    public class ClinicContext : DbContext
    {
        public ClinicContext(DbContextOptions<ClinicContext> options)
        : base(options)
        {
        }

        //nicole 20230314: set the PK of clinicOrderDetail could help to solve the circular object issue
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClinicOrderDetail>()
                .HasKey(c => new { c.ClinicOrderID, c.ClinicOrderDetailID });

            
        }
        //20230413 variable name should same with database table name
        public DbSet<ClinicOrder> ClinicOrders { get; set; } = null!;

        public DbSet<ClinicOrderDetail> ClinicOrderDetails { get; set; } = null!;
        public DbSet<Product> products { get; set; } = null!;
        public DbSet<AppSetting> appSettings { get; set; } = null!;
    }
}
