using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Js.Business
{
    public class ServicioProducto
    {

        public void Ingresa(string name, decimal precionormal, decimal preciointernet, string categoria,
            string tienda, int idSku)
        {

            name= name.Replace ("'","");

            var product = new Product
            {
                Name = name,               
                Categoria = categoria,
                 SKUId =idSku,
                 Tienda=tienda
               
            };

            var price = new Price
            {
                Fecha = DateTime.Now,
                PrecioInternet = preciointernet,
                PrecioNormal = precionormal
            };



            using (var ctx = new Context())
            {
                var productdb = ctx.Products.Where(m => m.SKUId   == idSku ).FirstOrDefault();

                if (productdb == null)
                {
                    productdb = ctx.Products.Add(product);
                    ctx.SaveChanges();
                }


                var pricedb = ctx.Prices.Where(m => m.ProductId == productdb.Id)
                    .OrderByDescending(m => m.Id)
                    .FirstOrDefault();

                if (pricedb ==null || pricedb.PrecioInternet !=price .PrecioInternet || pricedb.PrecioNormal != price.PrecioNormal)
                {
                    price.ProductId = productdb.Id;
                    ctx.Prices.Add(price);
                    ctx.SaveChanges();
                }
            }

        }

    }
}
