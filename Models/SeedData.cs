using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tutor1.Models.Context;
using tutor1.Models.Entity;

namespace tutor1.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ClinicContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ClinicContext>>()))
            {
                // Look for any movies.
                if (context.ClinicOrders.Any())
                {
                    return;   // DB has been seeded
                }
                var products = new List<Product>()
                {
                    new Product() { Name = "Duplixent", Price = 7500,  ProductID=1 },
                    new Product() { Name = "Consult Fee", Price = 1000,  ProductID =2 }
                };
                products.ForEach(p => context.products.Add(p));
                context.SaveChanges();
                

                var order = new ClinicOrder[] { 
                   new ClinicOrder{ customer = "Nicole",
                    ClinicOrderId = 1,
                    DateOfClinicOrder= DateTime.Parse("2023-01-01"),
                    seeDoctor=false,                    
                    clinicOrder_seqid="20230101_0001",
                    LastUpdatedTime=DateTime.Parse("2023-01-01") },
                   new ClinicOrder{ customer = "Nicole",
                    ClinicOrderId = 2,
                    DateOfClinicOrder= DateTime.Parse("2023-01-02"),
                    seeDoctor=false,                    
                    clinicOrder_seqid="20230102_0001",
                    LastUpdatedTime=DateTime.Parse("2023-01-02") },
                   new ClinicOrder{ customer = "Nicole",
                    ClinicOrderId = 3,
                    DateOfClinicOrder= DateTime.Parse("2023-01-03"),
                    seeDoctor=false,                    
                    clinicOrder_seqid="20230103_0001",
                    LastUpdatedTime=DateTime.Parse("2023-01-03") },
                };
                
                foreach (ClinicOrder s in order)
                {
                    context.Entry(s).State = EntityState.Detached;
                    context.ClinicOrders.Add(s);
                }                
                context.SaveChanges();
                
                

                var od = new List<ClinicOrderDetail>()
                {
                    new ClinicOrderDetail() {Quantity = 2, ClinicOrderDetailID=1, ClinicOrderID=1, ProductID= products.Single( s => s.ProductID == 1).ProductID},
                    new ClinicOrderDetail() {Quantity = 1, ClinicOrderDetailID=2, ClinicOrderID=1, ProductID= products.Single( s => s.ProductID == 2).ProductID},
                    new ClinicOrderDetail() {Quantity = 2, ClinicOrderDetailID=1, ClinicOrderID=2, ProductID= products.Single( s => s.ProductID == 1).ProductID},
                    new ClinicOrderDetail() {Quantity = 1, ClinicOrderDetailID=2, ClinicOrderID=2, ProductID= products.Single( s => s.ProductID == 2).ProductID},
                    new ClinicOrderDetail() {Quantity = 1, ClinicOrderDetailID=1, ClinicOrderID=3, ProductID= products.Single( s => s.ProductID == 1).ProductID},
                    
                };
                foreach (ClinicOrderDetail s in od)
                {
                    context.Entry(s).State = EntityState.Detached;
                    context.ClinicOrderDetails.Add(s);
                }
                //od.ForEach(o => context.ClinicOrderDetails.Add(o));                
                context.SaveChanges();
                
            }
        }
    }
}
