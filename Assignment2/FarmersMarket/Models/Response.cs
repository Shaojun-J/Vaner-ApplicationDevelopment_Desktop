using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1.Models
{
    internal class Response //this class will provide the strurcture that the remote server will maintain and send us 
    {
        public int statusCode {  get; set; }
        public string messageCode { get; set; }
        public Product product { get; set; }
        public List<Product> products { get; set; }
    }
}
