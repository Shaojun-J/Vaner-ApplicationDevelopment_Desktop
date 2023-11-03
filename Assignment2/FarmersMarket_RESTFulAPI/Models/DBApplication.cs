using Npgsql;
using System.Data;

namespace FarmersMarket_RESTFulAPI.Models
{
    public class DBApplication
    {
        public Response GetAllProducts(NpgsqlConnection con)
        {
            string Query = "Select * from products";

            NpgsqlDataAdapter da = new NpgsqlDataAdapter(Query, con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            Response response = new Response();
            List<Product> products = new List<Product>();

            if(dt.Rows.Count > 0 )
            {
                for( int i = 0; i < dt.Rows.Count; i++ ) { 
                    Product p = new Product();
                    p.pd_name = (string)dt.Rows[i]["pd_name"];
                    p.pd_id = (int)dt.Rows[i]["pd_id"];
                    p.pd_amount = (int)dt.Rows[i]["pd_amount"];
                    p.pd_price = (decimal)dt.Rows[i]["pd_price"];

                    products.Add(p);
                }
            }

            
            if(products.Count > 0)
            {
                response.statusCode = 200;
                response.messageCode = "Data Retrived Successfully";
                response.product = null;
                response.products = products;

            }
            else
            {
                response.statusCode = 100;
                response.messageCode = "Data failed to retrive or maybe table is empty";
                response.product = null;
                response.products = null;
            }

        

            return response;
        }


        public Response GetFruitbyId(NpgsqlConnection con, int id)
        {
            Response response = new Response();

            string Query = "Select * from products where pd_id=' "+id+" '";

            NpgsqlDataAdapter da = new NpgsqlDataAdapter(Query, con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if(dt.Rows.Count > 0)
            {
                Product p = new Product();
                p.pd_name = (string)dt.Rows[0]["pd_name"];
                p.pd_id = (int)dt.Rows[0]["pd_id"];
                p.pd_amount = (int)dt.Rows[0]["pd_amount"];
                p.pd_price = (decimal)dt.Rows[0]["pd_price"];

                response.statusCode=200;
                response.messageCode = "Data Successfully Retrieved";
                response.product = p;
                response.products = null;
            }
            else
            {
                response.statusCode = 100;
                response.messageCode = "Data not found. Please check the ID";
                response.products = null;
                response.product = null;
            }

            return response;

        }

        public Response GetFruitbyName(NpgsqlConnection con, string name)
        {
            Response response = new Response();

            string Query = "Select * from products where pd_name='" +name+ "'";

            NpgsqlDataAdapter da = new NpgsqlDataAdapter(Query, con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                Product p = new Product();
                p.pd_name = (string)dt.Rows[0]["pd_name"];
                p.pd_id = (int)dt.Rows[0]["pd_id"];
                p.pd_amount = (int)dt.Rows[0]["pd_amount"];
                p.pd_price = (decimal)dt.Rows[0]["pd_price"];

                response.statusCode = 200;
                response.messageCode = "Data Successfully Retrieved";
                response.product = p;
                response.products = null;
            }
            else
            {
                response.statusCode = 100;
                response.messageCode = "Data not found. Please check the name";
                response.products = null;
                response.product = null;
            }

            return response;

        }

        public Response AddFruit(NpgsqlConnection con, Product product)
        {
            con.Open();
            Response response = new Response();
            string Query = "insert into products values(@pd_name, @pd_id, @pd_amount, @pd_price)";
            NpgsqlCommand cmd = new NpgsqlCommand(Query,con);
            cmd.Parameters.AddWithValue("@pd_name", product.pd_name);
            cmd.Parameters.AddWithValue("pd_id",product.pd_id);
            cmd.Parameters.AddWithValue("pd_amount", product.pd_amount);
            cmd.Parameters.AddWithValue("pd_price", product.pd_price);

            int i = cmd.ExecuteNonQuery();
            if(i > 0)
            {
                response.statusCode=200;
                response.messageCode = "Successfully add data";
                response.product = product;
                response.products = null;

            }else
            {
                response.statusCode = 100;
                response.messageCode = "Insertion is not successful";
                response.product = null;
                response.products = null;
            }

            con.Close();
            return response;


        }

        public Response UpdateFruit (NpgsqlConnection con, Product product)
        {
            con.Open();
            Response response = new Response();
            string Query = "Update products set pd_name=@name, pd_id=@ID, pd_amount=@amount, pd_price=@price where pd_id=@ID";
            NpgsqlCommand cmd = new NpgsqlCommand( Query,con);

            cmd.Parameters.AddWithValue("@name", product.pd_name);
            cmd.Parameters.AddWithValue("@ID", product.pd_id);
            cmd.Parameters.AddWithValue("amount", product.pd_amount);
            cmd.Parameters.AddWithValue("price", product.pd_price);

            int i = cmd.ExecuteNonQuery();

            if( i > 0)
            {
                response.statusCode = 200;
                response.messageCode = "Data updated successfully";
                response.product = product;
            }
            else
            {
                response.statusCode = 100;
                response.messageCode = "Updata failed or ID is not in correct form";
            }
            con.Close();
            return response;
        }

        public Response DeleteFruitById(NpgsqlConnection con, int id)
        {
            con.Open();
            Response response = new Response();
            string Query = "Delete from products where pd_id ='" + id + "'";
            NpgsqlCommand cmd = new NpgsqlCommand(Query,con);

            int i = cmd.ExecuteNonQuery();
            if( i > 0)
            {
                response.statusCode = 200;
                response.messageCode = "Data deleted successfully";

            }
            else
            {
                response.statusCode = 100;
                response.messageCode = "Data not found !";
            }
            con.Close();
            return response;    
        }
    }

}
