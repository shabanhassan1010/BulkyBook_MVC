using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Model.ViewModel
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShopingCart>  ShoppingCartList { get; set; }
        public double OrderTotal { get; set; }
    }
}
