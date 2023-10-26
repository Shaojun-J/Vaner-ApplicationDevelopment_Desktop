using Microsoft.AspNetCore.Mvc;
using Npgsql;
using RestApiFarmersMarket.Models;

namespace RestApiFarmersMarket.Controllers
{
    [Route("controller")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ProductsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetAllProducts")]
        public Response GetAllProducts()
        {
            Response response = new Response();
            NpgsqlConnection con = new NpgsqlConnection(_configuration.GetConnectionString("productConnection"));
            DBApplication dba = new DBApplication();
            response = dba.GetAllProducts(con);

            return response;
        }

        [HttpGet]
        [Route("GetProductbyId/{id}")]
        public Response GetProductbyId(int id)
        {
            Response response = new Response();
            NpgsqlConnection con = new NpgsqlConnection(_configuration.GetConnectionString("productConnection"));
            DBApplication dba = new DBApplication();
            response = dba.GetProductbyId(con, id);

            return response;
        }

        [HttpGet]
        [Route("GetProductbyName/{name}")]
        public Response GetProductbyName(string name)
        {
            Response response = new Response();
            NpgsqlConnection con = new NpgsqlConnection(_configuration.GetConnectionString("productConnection"));
            DBApplication dba = new DBApplication();
            response = dba.GetProductbyName(con, name);

            return response;
        }


        [HttpPost]
        [Route("AddProduct")]
        public Response AddProduct(Product product)
        {
            Response response = new Response();
            NpgsqlConnection con = new NpgsqlConnection(_configuration.GetConnectionString("productConnection"));
            DBApplication dba = new DBApplication();
            response = dba.AddProduct(con, product);

            return response;
        }

        


        [HttpPut]
        [Route("UpdateProduct")]
        public Response UpdateProduct(Product product)
        {
            Response response = new Response();
            NpgsqlConnection con = new NpgsqlConnection(_configuration.GetConnectionString("productConnection"));
            DBApplication dba = new DBApplication();
            response = dba.UpdateProduct(con, product);

            return response;
        }


        [HttpDelete]
        [Route("DeleteProductbyId/{id}")]
        public Response DeleteProductbyId(int id)
        {
            Response response = new Response();
            NpgsqlConnection con = new NpgsqlConnection(_configuration.GetConnectionString("productConnection"));
            DBApplication dba = new DBApplication();
            response = dba.DeleteProductbyId(con, id);
            return response;
        }

    }
}
