using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using SynfoShopAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace SynfoShopAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _config;

        public UserController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            List<User> users = new List<User>();

            using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                using (MySqlCommand command = new MySqlCommand("GetAllUsers", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            User user = new User
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Username = reader["Username"].ToString(),
                                Password = reader["Password"].ToString(),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                                IsActive = Convert.ToInt32(reader["IsActive"])
                            };

                            users.Add(user);
                        }
                    }
                }
            }

            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                using (MySqlCommand command = new MySqlCommand("GetUserById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@userId", id);

                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            User user = new User
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Username = reader["Username"].ToString(),
                                Password = reader["Password"].ToString(),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                                IsActive = Convert.ToInt32(reader["IsActive"])
                            };

                            return Ok(user);
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                }
            }
        }

        [HttpPost]
        public IActionResult CreateUser(User user)
        {
            using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                using (MySqlCommand command = new MySqlCommand("usp_CreateUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@username", user.Username);
                    command.Parameters.AddWithValue("@password", user.Password);
                    command.Parameters.AddWithValue("@firstName", user.FirstName);
                    command.Parameters.AddWithValue("@lastName", user.LastName);
                    command.Parameters.AddWithValue("@isActive", user.IsActive);

                    connection.Open();
                    command.ExecuteNonQuery();

                    return Ok();
                }
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, User user)
        {
            using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                using (MySqlCommand command = new MySqlCommand("UpdateUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@userId", id);
                    command.Parameters.AddWithValue("@username", user.Username);
                    command.Parameters.AddWithValue("@password", user.Password);
                    command.Parameters.AddWithValue("@firstName", user.FirstName);
                    command.Parameters.AddWithValue("@lastName", user.LastName);
                    command.Parameters.AddWithValue("@createdDate", user.CreatedDate);
                    command.Parameters.AddWithValue("@isActive", user.IsActive);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return Ok();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                using (MySqlCommand command = new MySqlCommand("DeleteUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@userId", id);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return Ok();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
        }


        [HttpPost("login")]
        public IActionResult Login(User model)
        {
            int result = 0;
            using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                using (MySqlCommand command = new MySqlCommand("usp_Login", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@_Type", model.Type);
                    command.Parameters.AddWithValue("@_MobileNo", model.MobileNo);
                    command.Parameters.Add("@_Result", MySqlDbType.Int32).Direction = ParameterDirection.Output;

                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                // Handle the data from the tbluser table here
                                // Example: var id = reader.GetInt32("id");
                            }
                        }
                    }

                    result = (int)command.Parameters["@_Result"].Value;
                }
            }

            switch (result)
            {
                case 1:
                    // Valid admin credential
                    return Ok(new { Result = "Valid admin credential" });
                case 2:
                    // Invalid user credential
                    return Unauthorized(new { Result = "Invalid user credential" });
                case 3:
                    // Valid user credential
                    return Ok(new { Result = "Valid user credential" });
                default:
                    // Invalid admin credential
                    return Unauthorized(new { Result = "Invalid admin credential" });
            }
        }



    }
}
