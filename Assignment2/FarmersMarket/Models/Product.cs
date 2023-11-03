using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1.Models
{
    internal class Product //this class will help to get the structure from database, will be encrypted inside the Response the server is sending
    {
        public  string pd_name {  get; set; }
        public int pd_id { get; set; }
        public int pd_amount { get; set; }
        public decimal pd_price { get; set;}

    }
}
