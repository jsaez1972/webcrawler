using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Js.Business
{
    public class Context : DbContext
    {
        public Context() : base("ProductsDB") { }
        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<Price > Prices { get; set; }
    }
}
