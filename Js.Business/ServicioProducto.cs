using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Js.Business
{
    public class ServicioProducto
    {

        public void Ingresa(string name, decimal precionormal, decimal preciointernet)
        {
            var product = new Product { Name = name, PrecioInternet = preciointernet, PrecioNormal = precionormal };

            using (var ctx = new Context())
            {
                ctx.Products.Add(product);
                ctx.SaveChanges();
            }

        }

    }
}
