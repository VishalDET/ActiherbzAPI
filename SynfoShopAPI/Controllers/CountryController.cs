using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using SynfoShopAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace SynfoShopAPI.Controllers
{

    [ApiController]

    public class CountryController : ControllerBase
    {

        private readonly IConfiguration _config;
        public CountryController(IConfiguration config)
        {
            _config = config;
        }

        [Route("api/getCountry")]
        [HttpPost]
        public IActionResult GetCountry()
        {
            try
            {
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult;

                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_getcountry", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    //command.Parameters.AddWithValue("_Id", registration.Id);
                    //command.Parameters.AddWithValue("_Fullname", registration.Fullname);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            List<Country> countryList = new List<Country>();

                            while (reader.Read())
                            {
                                var CountryModel = new Country
                                {
                                    Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32("Id"),
                                    CountryName = reader.IsDBNull(reader.GetOrdinal("Country")) ? null : reader.GetString("Country")
                                };

                                countryList.Add(CountryModel);
                            }


                            apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                            apiResult.data = countryList;
                            return Ok(apiResult);

                            //return Ok(registrationList);
                        }
                        else
                        {
                            apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                            apiResult.data = new { };
                            return NotFound(apiResult);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }

        [Route("api/getState")]
        [HttpPost]
        public IActionResult GetState(int CountryId = 0)
        {
            try
            {
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult;

                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_getstate", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_CountryId", CountryId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            List<State> stateList = new List<State>();

                            while (reader.Read())
                            {
                                var StateModel = new State
                                {
                                    Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32("Id"),
                                    StateName = reader.IsDBNull(reader.GetOrdinal("State")) ? null : reader.GetString("State"),
                                    StateCode = reader.IsDBNull(reader.GetOrdinal("StateCode")) ? 0 : reader.GetInt32("StateCode"),
                                    CountryId = reader.IsDBNull(reader.GetOrdinal("CountryId")) ? 0 : reader.GetInt32("CountryId"),
                                };

                                stateList.Add(StateModel);
                            }


                            apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                            apiResult.data = stateList;
                            return Ok(apiResult);

                            //return Ok(registrationList);
                        }
                        else
                        {
                            apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                            apiResult.data = new { };
                            return NotFound(apiResult);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }

        [Route("api/getCity")]
        [HttpPost]
        public IActionResult GetCity(int StateId = 0)
        {
            try
            {
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult;

                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_getcity", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_StateId", StateId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            List<City> cityList = new List<City>();

                            while (reader.Read())
                            {
                                var CityModel = new City
                                {
                                    Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32("Id"),
                                    CityName = reader.IsDBNull(reader.GetOrdinal("City")) ? null : reader.GetString("City"),
                                    StateId = reader.IsDBNull(reader.GetOrdinal("StateId")) ? 0 : reader.GetInt32("StateId"),
                                };

                                cityList.Add(CityModel);
                            }


                            apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                            apiResult.data = cityList;
                            return Ok(apiResult);

                            //return Ok(registrationList);
                        }
                        else
                        {
                            apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                            apiResult.data = new { };
                            return NotFound(apiResult);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }

        [Route("api/getstatecity")]
        [HttpPost]
        public IActionResult GetStateCity(int Pincode)
        {
            try
            {
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult;

                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_getstatecity", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_Pincode", Pincode);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            StateCity CityModel = null;
                            while (reader.Read())
                            {
                                CityModel = new StateCity
                                {
                                    CityId = reader.IsDBNull(reader.GetOrdinal("CityId")) ? 0 : reader.GetInt32("CityId"),
                                    CityName = reader.IsDBNull(reader.GetOrdinal("CityName")) ? null : reader.GetString("CityName"),
                                    StateId = reader.IsDBNull(reader.GetOrdinal("StateId")) ? 0 : reader.GetInt32("StateId"),
                                    StateName = reader.IsDBNull(reader.GetOrdinal("StateName")) ? null : reader.GetString("StateName"),
                                    CountryId = reader.IsDBNull(reader.GetOrdinal("CountryId")) ? 0 : reader.GetInt32("CountryId"),
                                    CountryName = reader.IsDBNull(reader.GetOrdinal("CountryName")) ? null : reader.GetString("CountryName")
                                };

                            }


                            apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                            apiResult.data = CityModel;
                            return Ok(apiResult);

                            //return Ok(registrationList);
                        }
                        else
                        {
                            apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                            apiResult.data = new { };
                            return NotFound(apiResult);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }
    }
}
