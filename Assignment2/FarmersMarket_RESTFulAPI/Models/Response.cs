namespace FarmersMarket_RESTFulAPI.Models
{
    public class Response //this is the response message we need database to provide us
    {
        public int statusCode {  get; set; }
        public string messageCode { get; set; }

        public Product product { get; set; }

        public List<Product> products { get; set; }


    }
}
