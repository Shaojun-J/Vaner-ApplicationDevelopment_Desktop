namespace FarmersMarket_RESTFulAPI.Models
{
    public class Product
    {
        //this class is made to get all info from database,same num of variables, better maintain the sequence
        public string pd_name { get; set; }
        public int pd_id { get; set; }
        public int pd_amount { get; set;}

        public decimal pd_price { get; set; }

    }
}
