using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using SynfoShopAPI.Models;
using System.Collections.Generic;
using System.Data;
using System;

namespace SynfoShopAPI.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class SalesUserController : ControllerBase
    {
        private readonly IConfiguration _config;
        public SalesUserController(IConfiguration config)
        {
            _config = config;
        }

        [Route("api/GetSalesUser")]
        [HttpGet]
        public IActionResult GetSalesUsers()
        {
            List<SalesUser> salesusers = new List<SalesUser>();

            using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                using (MySqlCommand command = new MySqlCommand("usp_getsalesusers", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SalesUser salesuser = new SalesUser
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                FullName = reader["FullName"].ToString(),
                                EmailId = reader["EmailId"].ToString(),
                                MobileNo = reader["MobileNo"].ToString(),
                                BrachName = reader["BrachName"].ToString(),
                                ReferCode = reader["ReferCode"].ToString()
                            };

                            salesusers.Add(salesuser);
                        }
                    }
                }
            }

            return Ok(salesusers);
        }
    }
}
