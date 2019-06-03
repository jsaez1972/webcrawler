using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Js.Business
{
    public class Price
    {
        public int Id { get; set; }

        [Index]
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        public decimal PrecioInternet { get; set; }
        public decimal PrecioNormal { get; set; }

        public DateTime Fecha { get; set; }

    }
}
