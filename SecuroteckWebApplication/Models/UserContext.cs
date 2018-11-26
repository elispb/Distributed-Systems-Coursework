using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace SecuroteckWebApplication.Models
{
    public class UserContext : DbContext
    {
        public UserContext() : base()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<UserContext, Migrations.Configuration>());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Log> Logs { get; set; }
        //public DbSet<ArchivedLog> ArchivedLogs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }
}