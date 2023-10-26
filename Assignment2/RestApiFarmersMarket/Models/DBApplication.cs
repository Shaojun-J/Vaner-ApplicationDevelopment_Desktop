
using Npgsql;
using System.Data;

namespace RestApiFarmersMarket.Models
{
    public class DBApplication
    {

        public Response GetAllProducts(NpgsqlConnection con)
        {
            Response response = new Response();
            string query = "select * from products";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<Product> products = new List<Product>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Product p = new Product();
                    p.pd_name = (string)dt.Rows[i]["pd_name"];
                    p.pd_id = (int)dt.Rows[i]["pd_id"];
                    p.pd_amount = (int)dt.Rows[i]["pd_amount"];
                    p.pd_price = (decimal)dt.Rows[i]["pd_price"];
                    products.Add(p);
                }
            }

            if (products.Count > 0)
            {
                response.statusCode = 200;
                response.messageCode = "Data retrived successfully";
                response.product = null;
                response.products = products;
            }
            else
            {
                response.statusCode = 100;
                response.messageCode = "Data retriving failed";
                response.product = null;
                response.products = null;
            }
            return response;
        }


        public Response GetProductbyId(NpgsqlConnection con, int id)
        {
            Response response = new Response();
            string query = "select * from products where pd_id='" + id + "'";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, con);
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
                response.messageCode = "Data retrived successfully";
                response.product = p;
                response.products = null;
            }
            else
            {
                response.statusCode = 100;
                response.messageCode = "Data retriving failed";
                response.product = null;
                response.products = null;
            }
            return response;
        }

        public Response GetProductbyName(NpgsqlConnection con, string name)
        {
            Response response = new Response();
            string query = "select * from products where pd_name='" + name + "'";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, con);
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
                response.messageCode = "Data retrived successfully";
                response.product = p;
                response.products = null;
            }
            else
            {
                response.statusCode = 100;
                response.messageCode = "Data retriving failed";
                response.product = null;
                response.products = null;
            }
            return response;
        }

        public Response AddProduct(NpgsqlConnection con, Product product)
        {
            con.Open();
            Response response = new Response();
            string query = "insert into products values(@pd_name, @pd_id, @pd_amount, @pd_price)";
            NpgsqlCommand cmd = new NpgsqlCommand(query, con);
            cmd.Parameters.AddWithValue("@pd_name", product.pd_name);
            cmd.Parameters.AddWithValue("@pd_id", product.pd_id);
            cmd.Parameters.AddWithValue("@pd_amount", product.pd_amount);
            cmd.Parameters.AddWithValue("@pd_price", product.pd_price);

            if(cmd.ExecuteNonQuery()>0)
            {
                response.statusCode = 200;
                response.messageCode = "Insertion successful";
                response.product = product;
                response.products = null;
            }
            else
            {
                response.statusCode = 100;
                response.messageCode = "Insertion failed ";
                response.product = null;
                response.products = null;
            }
            con.Close();
            return response;
        }

        public Response DeleteProductbyId(NpgsqlConnection con, int id)
        {
            con.Open();
            Response response = new Response();
            string query = "delete from products where pd_id='" + id +"'";
            NpgsqlCommand cmd = new NpgsqlCommand(query, con);
            if (cmd.ExecuteNonQuery() > 0)
            {
                response.statusCode = 200;
                response.messageCode = "successfully deleted";
            }
            else
            {
                response.statusCode = 100;
                response.messageCode = "Failed to detete";
            }
            con.Close();
            return response;
        }

        public Response UpdateProduct(NpgsqlConnection con, Product product)
        {
            Response response = new Response();
            con.Open();
            string query = "update products set pd_name=@name, pd_amount=@amount, pd_price=@price where pd_id=@id";
            NpgsqlCommand cmd = new NpgsqlCommand(query, con);
            cmd.Parameters.AddWithValue("@name", product.pd_name);
            cmd.Parameters.AddWithValue("@amount", product.pd_amount);
            cmd.Parameters.AddWithValue("@price", product.pd_price);
            cmd.Parameters.AddWithValue("@id", product.pd_id);
            if (cmd.ExecuteNonQuery() > 0)
            {
                response.statusCode= 200;
                response.messageCode= "Update successfully";
                response.product = product;
            }
            else
            {
                response.statusCode = 100;
                response.messageCode = "failed to update";
            }

            con.Close();
            return response;
        }


    }
}
