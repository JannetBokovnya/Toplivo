using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Toplivo.Web.Models.Toplivo;

namespace Toplivo.Web.Common
{
    public class ToplivoContext : DbContext
    {

        public ToplivoContext() : base("name=ToplivoContext")
        {
        }
        public virtual DbSet<Fuel> Fuels { get; set; }
        public virtual DbSet<Operation> Operations { get; set; }
        public virtual DbSet<Tank> Tanks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Configurations.Add();
        }
    }
}