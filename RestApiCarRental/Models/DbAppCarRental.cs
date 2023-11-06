﻿using Npgsql;
using System.Data;

namespace RestApiCarRental.Models
{


    public partial  class DbAppCarRental
    {
        public CustomerAuthorityResponse GetAllCustomer(NpgsqlConnection con)
        {
            CustomerAuthorityResponse response = new CustomerAuthorityResponse();
            string query = "select * from car_rental.customer_authority";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<CustomerAuthority> customers = new List<CustomerAuthority>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CustomerAuthority c = new CustomerAuthority();
                    c.id = (int)dt.Rows[i]["id"];
                    c.user_name = (string)dt.Rows[i]["user_name"];
                    c.password = (string)dt.Rows[i]["password"];
                    c.salt = (string)dt.Rows[i]["salt"];
                    c.phone = (string)dt.Rows[i]["phone"];
                    c.email = (string)dt.Rows[i]["email"];
                    customers.Add(c);
                }
            }

            if (customers.Count > 0)
            {
                response.statusCode = 200;
                response.message = "Data retrived successfully";
                response.customer = null;
                response.customers = customers;
            }
            else
            {
                response.statusCode = 100;
                response.message = "Data retriving failed";
                response.customer = null;
                response.customers = null;
            }
            return response;

        }

        public CustomerAuthorityResponse GetCustomerbyUsername(NpgsqlConnection con, string username)
        {
            CustomerAuthorityResponse response = new CustomerAuthorityResponse();
            string query = "select * from car_rental.customer_authority where user_name='" + username + "'";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                CustomerAuthority c = new CustomerAuthority();
                c.id = (int)dt.Rows[0]["id"];
                c.user_name = (string)dt.Rows[0]["user_name"];
                c.password = (string)dt.Rows[0]["password"];
                c.salt = (string)dt.Rows[0]["salt"];
                c.phone = (string)dt.Rows[0]["phone"];
                c.email = (string)dt.Rows[0]["email"];
            
                response.statusCode = 200;
                response.message = "Data retrived successfully";
                response.customer = c;
                response.customers = null;
            }
            else
            {
                response.statusCode = 100;
                response.message = "Data retriving failed";
                response.customer = null;
                response.customers = null;
            }
            return response;

        }

        public CustomerAuthorityResponse AddCustomer(NpgsqlConnection con, CustomerAuthority customer)
        {
            CustomerAuthorityResponse response = new CustomerAuthorityResponse();
            con.Open();
            string query = "insert into car_rental.customer_authority values(default, @user_name, @password, @salt, @phone, @email)";
            NpgsqlCommand cmd = new NpgsqlCommand(query, con);
            cmd.Parameters.AddWithValue("user_name", customer.user_name);
            cmd.Parameters.AddWithValue("password", customer.password);
            cmd.Parameters.AddWithValue("salt", MyUtility.getMD5(DateTime.Now.ToString()));
            cmd.Parameters.AddWithValue("phone", customer.phone);
            cmd.Parameters.AddWithValue("email", customer.email);
            if (cmd.ExecuteNonQuery() > 0)
            {
                response.statusCode = 200;
                response.message = "Insertion Successful";
                response.customer = customer;
                response.customers = null;

            }
            else
            {
                response.statusCode = 100;
                response.message = "Insertion failed";
                response.customer = null;
                response.customers = null;
            }
            con.Close();
            return response;
        }


        public Response AddStaff(NpgsqlConnection con, Staff staff)
        {
            Response response = new Response();
            con.Open();
            string query = "insert into car_rental.staff_authority values(default, @user_name, @password, @salt, @phone, @email, @authority)";
            NpgsqlCommand cmd = new NpgsqlCommand(query, con);
            cmd.Parameters.AddWithValue("user_name", staff.user_name);
            cmd.Parameters.AddWithValue("password", staff.password);
            cmd.Parameters.AddWithValue("salt", MyUtility.getMD5(DateTime.Now.ToString()));
            cmd.Parameters.AddWithValue("phone", staff.phone);
            cmd.Parameters.AddWithValue("email", staff.email);
            cmd.Parameters.AddWithValue("authority", staff.authority);

            if (cmd.ExecuteNonQuery() > 0)
            {
                response.statusCode = 200;
                response.message = "Insertion Successful";
                response.obj = staff;
                response.objs = null;

            }
            else
            {
                response.statusCode = 100;
                response.message = "Insertion failed";
                response.obj = null;
                response.objs = null;
            }
            con.Close();
            return response;
        }

        private bool DeleteCustomerbyId(NpgsqlConnection con,int id)
        { 
            con.Open();
            string query = "delete from car_rental.customer_authority where id='" + id + "'";
            NpgsqlCommand cmd = new NpgsqlCommand( query, con);
            con.Close();

            if(cmd.ExecuteNonQuery() > 0)
            {
                return true;
            }
            return false;
        }

        public CustomerAuthorityResponse DeleteCustomer(NpgsqlConnection con, CustomerAuthority customer)
        {
            CustomerAuthorityResponse response = new CustomerAuthorityResponse();
            string query = "select * from car_rental.customer_authority where user_name='" + customer.user_name + "'";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<CustomerAuthority> products = new List<CustomerAuthority>();
            if (dt.Rows.Count > 0)
            {

                CustomerAuthority c = new CustomerAuthority();
                c.id = (int)dt.Rows[0]["id"];
                c.user_name = (string)dt.Rows[0]["user_name"];
                c.password = (string)dt.Rows[0]["password"];
                c.salt = (string)dt.Rows[0]["salt"];
                c.phone = (string)dt.Rows[0]["phone"];
                c.email = (string)dt.Rows[0]["email"];

                if (c.password == customer.password)
                {
                    if(DeleteCustomerbyId(con, c.id))
                    {
                        response.statusCode = 200;
                        response.message = "Delete successfully";
                        response.customer = null;
                        response.customers = null;
                    }
                    else
                    {
                        response.statusCode = 100;
                        response.message = "Failed to delete";
                        response.customer = null;
                        response.customers = null;
                    }
                }
                else
                {
                    response.statusCode = 100;
                    response.message = "Failed to delete";
                    response.customer = null;
                    response.customers = null;
                }

            }
            return response;
        }



        public CustomerAuthorityResponse CheckAuthority(NpgsqlConnection con, CustomerAuthority customer)
        {
            CustomerAuthorityResponse response = new CustomerAuthorityResponse();
            string query = "select * from car_rental.customer_authority where user_name='" + customer.user_name + "'";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            
            if (dt.Rows.Count > 0)
            {
                CustomerAuthority c = new CustomerAuthority();
                c.id = (int)dt.Rows[0]["id"];
                c.user_name = (string)dt.Rows[0]["user_name"];
                c.password = (string)dt.Rows[0]["password"];
                c.salt = (string)dt.Rows[0]["salt"];
                c.phone = (string)dt.Rows[0]["phone"];
                c.email = (string)dt.Rows[0]["email"];

                if (c.password.Equals(customer.password))
                {
                    response.statusCode = 200;
                    response.message = "Authority successful ";
                    response.customer = c;
                    response.customers = null;
                }
                else
                {
                    response.statusCode = 100;
                    response.message = "Authority failed";
                    response.customer = null;
                    response.customers = null;
                }

            }
            else
            {
                response.statusCode = 100;
                response.message = "Authority failed";
                response.customer = null;
                response.customers = null;
            }
            return response;
        }

        public Response StaffLogin(NpgsqlConnection con, Staff staff)
        {
            Response response = new Response();
            string query = "select * from car_rental.staff_authority where user_name='" + staff.user_name + "'";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                if (((string)dt.Rows[0]["password"]).Equals(staff.password))
                {
                    Staff c = new Staff();
                    c.id = (int)dt.Rows[0]["id"];
                    c.user_name = (string)dt.Rows[0]["user_name"];
                    c.password = (string)dt.Rows[0]["password"];
                    c.salt = (string)dt.Rows[0]["salt"];
                    c.phone = (string)dt.Rows[0]["phone"];
                    c.email = (string)dt.Rows[0]["email"];
                    c.authority = (int)dt.Rows[0]["authority"];

                    response.statusCode = 200;
                    response.message = "Authority successful ";
                    response.obj = c;
                    response.objs = null;
                }
                else
                {
                    response.statusCode = 100;
                    response.message = "Authority failed";
                    response.obj = null;
                    response.objs = null;
                }

            }
            return response;
        }


        /**************************************************************************
         * Car
         */
        public Response AddCar(NpgsqlConnection con, Car car)
        {
            Response response = new Response();
            con.Open();
            string query = $"insert into car_rental.car values(default,'{car.brand}',  '{car.model}', '{car.trim}', {car.year},   '{car.transmission}', '{car.fuel_type}', '{car.body_type}', {car.seats}, {car.doors} )";
            //string query = "insert into car_rental.car values(default, @user_name, @password, @salt, @phone, @email)";
            NpgsqlCommand cmd = new NpgsqlCommand(query, con);
            Console.WriteLine(query);
            if (cmd.ExecuteNonQuery() > 0)
            {
                response.statusCode = 200;
                response.message = "Insertion Successful";
                response.obj = car;
                response.objs = null;

            }
            else
            {
                response.statusCode = 100;
                response.message = "Insertion failed";
                response.obj = null;
                response.objs = null;
            }
            con.Close();
            return response;
        }

        public Response DeleteCarbyId(NpgsqlConnection con, int id)
        {
            con.Open();
            Response response = new Response();
            string query = "delete from car_rental.car where car_id='" + id + "'";
            NpgsqlCommand cmd = new NpgsqlCommand(query, con);
            

            if (cmd.ExecuteNonQuery() > 0)
            {
                response.statusCode = 200;
                response.message = "Delete successfully";
                response.obj = null;
                response.objs = null;
            }
            else
            {
                response.statusCode = 100;
                response.message = "Delete failed";
                response.obj = null;
                response.objs = null;
            }
            con.Close();
            return response;
        }

        public Response UpdateCar(NpgsqlConnection con, Car car)
        {
            Response response = new Response();
            con.Open();
            string query = $"update car_rental.car set brand='{car.brand}', model='{car.model}', trim='{car.trim}'," +
                $"year={car.year}, transmission='{car.transmission.ToLower()}', fuel_type='{car.fuel_type.ToLower()}', body_type='{car.body_type.ToLower()}'," +
                $"seats={car.seats}, doors={car.doors} where car_id={car.car_id}";
            NpgsqlCommand cmd = new NpgsqlCommand(query, con);

            if (cmd.ExecuteNonQuery() > 0)
            {
                response.statusCode = 200;
                response.message = "Update successfully";
                response.obj = car;
            }
            else
            {
                response.statusCode = 100;
                response.message = "failed to update";
            }

            con.Close();
            return response;
        }

        public Response GetCarbyFilter(NpgsqlConnection con, Car car)
        {
            Response response = new Response();
            string transmission = $"('{car.transmission}')";
            if ("any".Equals(car.transmission?.ToLower()))
            {
                transmission = "('manual','auto')";
            }

            string fuel_type = $"('{car.fuel_type}')";
            if ("any".Equals(car.fuel_type?.ToLower()))
            {
                fuel_type = "('gasoline','electric', 'hybird')";
            }
            
            string body_type = $"('{car.body_type}')";
            if ("any".Equals(car.body_type?.ToLower()))
            {
                body_type = "('suv','sedan')";
            }

            string query = $"select * from car_rental.car where transmission IN {transmission} and fuel_type IN {fuel_type} and body_type IN {body_type} ";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                object obj = new Car();
                List<object> list = new List<object>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Car c = new Car();
                    c.car_id = (int)dt.Rows[i]["car_id"];
                    c.brand = (string)dt.Rows[i]["brand"];
                    c.model = (string)dt.Rows[i]["model"];
                    c.trim = (string)dt.Rows[i]["trim"];
                    c.year = (int)dt.Rows[i]["year"];
                    c.transmission = (string)dt.Rows[i]["transmission"];
                    c.fuel_type = (string)dt.Rows[i]["fuel_type"];
                    c.body_type = (string)dt.Rows[i]["body_type"];
                    c.seats = (int)dt.Rows[i]["seats"];
                    c.doors = (int)dt.Rows[i]["doors"];
                    //c.category_id = (int)dt.Rows[i]["category_id"];
                    list.Add(c);
                }

                response.statusCode = 200;
                response.message = "Data retriving successful";
                response.obj = null;
                response.objs = list;
            }
            else
            {
                response.statusCode = 100;
                response.message = "Data retriving failed";
                response.obj = null;
                response.objs = null;
            }
            return response;

        }


        
    }


}
