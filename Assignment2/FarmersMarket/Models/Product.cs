using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarket.Models
{
    internal class Product
    {
        public string pd_name { get; set; }
        public int pd_id { get; set; }
        public int pd_amount { get; set; }
        public decimal pd_price { get; set; }
    }
}
