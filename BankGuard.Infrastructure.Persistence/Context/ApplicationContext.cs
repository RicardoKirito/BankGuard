using BankGuard.Core.Application.Enums;
using BankGuard.Core.Domain.Entities;
using BankGuard.Infrastructure.Identity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankGuard.Infrastructure.Persistence.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }


        //public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        //{
        //    foreach (var entity in ChangeTracker.Entries())
        //    {
        //        Log logs = new Log();
        //        switch (entity.State)
        //        {
        //            case EntityState.Added:
        //                logs.Action = LogOperations.Create.ToString();                      
        //                break;
        //            case EntityState.Modified:
        //                logs.Action = LogOperations.Update.ToString();
        //                break;
        //            case EntityState.Deleted:
        //                logs.Action = LogOperations.Delete.ToString();
        //                break;
        //        }
        //        logs.By = "DefaultUser";
        //        logs.Date = DateTime.Now.ToString();
        //        base.AddAsync(logs);
        //    }
        //    return base.SaveChangesAsync(cancellationToken);
        //}

        public DbSet<Transactions> Transactions { get; set; }
        public DbSet<Log> Logs { get; set; }  
        public DbSet<Beneficiaries> Beneficiaries { get; set; }
        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder model)
        {
            model.Entity<Transactions>().ToTable("Transactions");
            model.Entity<Log>().ToTable("Logs");
            model.Entity<Product>().ToTable("Products");

            #region Keys
            model.Entity<Transactions>()
                .HasKey(t => t.Id);
            model.Entity<Log>()
                .HasKey(l => l.Id);
            model.Entity<Beneficiaries>()
                .HasKey(b => b.Id);
            model.Entity<Product>()
                .HasKey(a=> a.accountnumber);
            #endregion

            #region Relations
            model.Entity<Beneficiaries>()
                .HasOne<Product>(b => b.Product)
                .WithOne()
                .HasForeignKey<Beneficiaries>(b => b.Accountnumber)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region Products
            
             model.Entity<Product>()
                .Property(a => a.accountnumber)
                .HasMaxLength(9)
                .IsFixedLength(true)
                .IsRequired();

            #endregion
        }

    }
}
