using Microsoft.AspNetCore.Mvc;
using Npgsql;
using RestApiCarRental.Models;

namespace RestApiCarRental.Controllers
{
    public partial class CarRentalController 
    {
        /*************************************************
         * Inventory
         */

        [HttpPost]
        [Route("AddInventory")]
        public Response AddInventory(Inventory inv)
        {
            Response response = new Response();
            NpgsqlConnection con = new NpgsqlConnection(_configuration.GetConnectionString("CustomerAuthorityConnection"));
            DbAppCarRental dba = new DbAppCarRental();
            response = dba.AddInventory(con, inv);

            return response;
        }

        [HttpDelete]
        [Route("DeleteInventorybyId/{id}")]
        public Response DeleteInventorybyId(int id)
        {
            Response response = new Response();
            NpgsqlConnection con = new NpgsqlConnection(_configuration.GetConnectionString("CustomerAuthorityConnection"));
            DbAppCarRental dba = new DbAppCarRental();
            response = dba.DeleteInventorybyId(con, id);
            return response;
        }

        [HttpPut]
        [Route("UpdateInventory")]
        public Response UpdateInventory(Inventory inv)
        {
            Response response = new Response();
            NpgsqlConnection con = new NpgsqlConnection(_configuration.GetConnectionString("CustomerAuthorityConnection"));
            DbAppCarRental dba = new DbAppCarRental();
            response = dba.UpdateInventory(con, inv);

            return response;
        }

        [HttpPost]
        [Route("GetInventorybyCarId")]
        public Response GetInventorybyCarId(int id)
        {
            Response response = new Response();
            NpgsqlConnection con = new NpgsqlConnection(_configuration.GetConnectionString("CustomerAuthorityConnection"));
            DbAppCarRental dba = new DbAppCarRental();
            response = dba.GetInventorybyCarId(con, id);

            return response;
        }

    }
}
