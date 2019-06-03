using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Js.Business
{
    public class Product
    {
        public int Id { get; set; }
        public string Tienda { get; set; }

        public string Name { get; set; }


        public string Categoria { get; set; }

        public int SKUId { get; set; }

    }
}
