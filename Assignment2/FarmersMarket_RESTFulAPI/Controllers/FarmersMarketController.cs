using FarmersMarket_RESTFulAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace FarmersMarket_RESTFulAPI.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class FarmersMarketController : ControllerBase
    {
       private readonly IConfiguration _configuration; //create a state receiver in the programme, hold the connection info from the remote database to the local one

        public FarmersMarketController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetAllFruits")]

        //Create API Methods

        public Response GetAllFruits()
        {
            Response response = new Response();

            NpgsqlConnection con = new NpgsqlConnection(_configuration.GetConnectionString("farmersMarketConnection"));

            DBApplication dBA = new DBApplication();
           response = dBA.GetAllProducts(con);

            return response;
        }

        [HttpGet]
        [Route("GetFruitbyId/{id}")]
        public Response GetFruitbyId(int id) {
        
            Response response = new Response();
            NpgsqlConnection con = new NpgsqlConnection(_configuration.GetConnectionString("farmersMarketConnection"));
            DBApplication dBA = new DBApplication();

            
           response =  dBA.GetFruitbyId(con, id);

            return response;

        }

        [HttpGet]
        [Route("GetFruitbyName/{name}")]
        public Response GetFruitbyName(string name)
        {

            Response response = new Response();
            NpgsqlConnection con = new NpgsqlConnection(_configuration.GetConnectionString("farmersMarketConnection"));
            DBApplication dBA = new DBApplication();


            response = dBA.GetFruitbyName(con, name);

            return response;

        }

        [HttpPost]
        [Route("AddFruit")]

        public Response AddFruit(Product product)
        {
            Response response = new Response();
            NpgsqlConnection con = new NpgsqlConnection(_configuration.GetConnectionString("farmersMarketConnection"));
            DBApplication dBA = new DBApplication();

            response = dBA.AddFruit(con, product);
            return response;
        }

        [HttpPut]
        [Route("UpdateFruit")]

        public Response UpdateFruit(Product product)
        {
            Response response = new Response();
            NpgsqlConnection con = new NpgsqlConnection(_configuration.GetConnectionString("farmersMarketConnection"));
            DBApplication dBA = new DBApplication();

            response = dBA.UpdateFruit(con, product);
            return response;
        }

        [HttpDelete]
        [Route("DeleteFruitById/{id}")]

        public Response DeleteFruitById(int id)
        {
            Response response = new Response();
            NpgsqlConnection con = new NpgsqlConnection(_configuration.GetConnectionString("farmersMarketConnection"));
            DBApplication dBA = new DBApplication();

            response = dBA.DeleteFruitById(con,  id);
            return response;
        }
    }
}
