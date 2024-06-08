using Cloth.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloth.Domain.Cart
{
    public class CartItem
    {
        public Kosuhi Item { get; set; }
        public int Qty { get; set; }

    }
}
