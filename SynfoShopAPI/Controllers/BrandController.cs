
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using SynfoShopAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace SynfoShopAPI.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IConfiguration _config;

        public BrandController(IConfiguration config)
        {
            _config = config;
        }
        [Route("api/commonbrand")]
        [HttpPost]
        public IActionResult CommonBrand(Brand brand)
        {
            try
            {
                int result;
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult;

                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_commonbrand", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_Id", brand.Id);
                    command.Parameters.AddWithValue("_BrandName", brand.BrandName);
                    command.Parameters.AddWithValue("_LogoUrl", brand.LogoUrl);
                    command.Parameters.AddWithValue("_DisplayOrder", brand.DisplayOrder);
                    command.Parameters.AddWithValue("_IsActive", brand.IsActive);
                    command.Parameters.AddWithValue("_SpType", brand.SpType);

                    MySqlParameter resultParam = new MySqlParameter("_Result", MySqlDbType.Int32);
                    resultParam.Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add(resultParam);

                    if (brand.SpType == "C" || brand.SpType == "U" || brand.SpType == "D")
                    {
                        command.ExecuteNonQuery();
                        result = Convert.ToInt32(command.Parameters["_Result"].Value);

                        if (result != 0)
                        {
                            apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                            apiResult.data = new { Id = result };
                            return Ok(apiResult);
                        }
                        else
                        {
                            apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.InsertUpdateFailed, ServiceRequestProcessor.StatusCode.InsertUpdateFailed.ToString());
                            apiResult.data = new { Id = result };
                            return Ok(apiResult);
                        }
                    }
                    else if (brand.SpType == "E")
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                List<Brand> brandList = new List<Brand>();

                                while (reader.Read())
                                {
                                    var brandModel = new Brand
                                    {
                                        Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32("Id"),
                                        BrandName = reader.IsDBNull(reader.GetOrdinal("BrandName")) ? null : reader.GetString("BrandName"),
                                        LogoUrl = reader.IsDBNull(reader.GetOrdinal("LogoUrl")) ? null : reader.GetString("LogoUrl"),
                                        DisplayOrder = reader.IsDBNull(reader.GetOrdinal("DisplayOrder")) ? 0 : reader.GetInt32("DisplayOrder"),
                                        IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                        CreatedDate = reader.IsDBNull(reader.GetOrdinal("CreatedDate")) ? DateTime.MinValue : reader.GetDateTime("CreatedDate"),
                                        ModifiedDate = reader.IsDBNull(reader.GetOrdinal("ModifiedDate")) ? DateTime.MinValue : reader.GetDateTime("ModifiedDate")
                                    };

                                    brandList.Add(brandModel);
                                }
                                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                                apiResult.data = brandList;
                                return Ok(apiResult);

                                //return Ok(brandList);
                            }
                            else
                            {
                                //return NotFound();
                                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                                apiResult.data = new { };
                                return NotFound(apiResult);
                            }
                        }
                    }
                    else if (brand.SpType == "R")
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                List<Brand> brandList = new List<Brand>();

                                while (reader.Read())
                                {
                                    var brandModel = new Brand
                                    {
                                        Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32("Id"),
                                        BrandName = reader.IsDBNull(reader.GetOrdinal("BrandName")) ? null : reader.GetString("BrandName"),
                                        LogoUrl = reader.IsDBNull(reader.GetOrdinal("LogoUrl")) ? null : reader.GetString("LogoUrl"),
                                        DisplayOrder = reader.IsDBNull(reader.GetOrdinal("DisplayOrder")) ? 0 : reader.GetInt32("DisplayOrder"),
                                        IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                        CreatedDate = reader.IsDBNull(reader.GetOrdinal("CreatedDate")) ? DateTime.MinValue : reader.GetDateTime("CreatedDate"),
                                        ModifiedDate = reader.IsDBNull(reader.GetOrdinal("ModifiedDate")) ? DateTime.MinValue : reader.GetDateTime("ModifiedDate")
                                    };

                                    brandList.Add(brandModel);
                                }

                                //return Ok(brandList);
                                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                                apiResult.data = brandList;
                                return Ok(apiResult);
                            }
                            else
                            {
                                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                                apiResult.data = new { };
                                return NotFound(apiResult);
                            }
                        }
                    }
                    else
                    {
                        //return BadRequest("Invalid SpType");
                        apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.ServerError, "Invalid SpType");
                        apiResult.data = new { };
                        return BadRequest(apiResult);
                    }
                }
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }

            //  return Ok(result);
        }

        //[Route("api/Brand")]
        //[HttpPost]
        //public IActionResult GetActiveBrands()
        //{
        //    List<Brand> brandList = new List<Brand>();

        //    using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
        //    {
        //        connection.Open();
        //        MySqlCommand command = new MySqlCommand("usp_getbrand", connection);
        //        command.CommandType = CommandType.StoredProcedure;

        //        using (var reader = command.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                var brand = new Brand
        //                {
        //                    Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32("Id"),
        //                    BrandName = reader.IsDBNull(reader.GetOrdinal("BrandName")) ? null : reader.GetString("BrandName"),
        //                    LogoUrl = reader.IsDBNull(reader.GetOrdinal("LogoUrl")) ? null : reader.GetString("LogoUrl"),
        //                    DisplayOrder = reader.IsDBNull(reader.GetOrdinal("DisplayOrder")) ? 0 : reader.GetInt32("DisplayOrder"),
        //                    IsActive = reader.IsDBNull(reader.GetOrdinal("IsActive")) ? false : reader.GetBoolean("IsActive"),
        //                    CreatedDate = reader.IsDBNull(reader.GetOrdinal("CreatedDate")) ? DateTime.MinValue : reader.GetDateTime("CreatedDate"),
        //                    ModifiedDate = reader.IsDBNull(reader.GetOrdinal("ModifiedDate")) ? DateTime.MinValue : reader.GetDateTime("ModifiedDate")
        //                };

        //                brandList.Add(brand);
        //            }
        //        }
        //    }

        //    return Ok(brandList);
        //}


        [Route("api/Brand")]
        [HttpPost]
        public IActionResult GetActiveBrands()
        {
            try
            {
                List<Brand> brandList = new List<Brand>();

                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_getbrand", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var brand = new Brand
                            {
                                Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32("Id"),
                                BrandName = reader.IsDBNull(reader.GetOrdinal("BrandName")) ? null : reader.GetString("BrandName"),
                                LogoUrl = reader.IsDBNull(reader.GetOrdinal("LogoUrl")) ? null : reader.GetString("LogoUrl"),
                                DisplayOrder = reader.IsDBNull(reader.GetOrdinal("DisplayOrder")) ? 0 : reader.GetInt32("DisplayOrder"),
                                IsActive = reader.IsDBNull(reader.GetOrdinal("IsActive")) ? false : reader.GetBoolean("IsActive"),
                                CreatedDate = reader.IsDBNull(reader.GetOrdinal("CreatedDate")) ? DateTime.MinValue : reader.GetDateTime("CreatedDate"),
                                ModifiedDate = reader.IsDBNull(reader.GetOrdinal("ModifiedDate")) ? DateTime.MinValue : reader.GetDateTime("ModifiedDate")
                            };

                            brandList.Add(brand);
                        }
                    }
                }

                if (brandList.Count > 0)
                {
                    ServiceRequestProcessor processor = new ServiceRequestProcessor();
                    APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                    apiResult.data = brandList;
                    return Ok(apiResult);
                }
                else
                {
                    ServiceRequestProcessor processor = new ServiceRequestProcessor();
                    APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                    apiResult.data = new List<Brand>();
                    return NotFound(apiResult);
                }
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }

        [Route("api/Commonbanner")]
        [HttpPost]
        public IActionResult CommonBanner(AddupdateBanner AddUpdate)
        {
            try
            {
                int result;
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult;

                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_commonbanner", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_Id", AddUpdate.Id);
                    command.Parameters.AddWithValue("_CategoryId", AddUpdate.CategoryId);
                    command.Parameters.AddWithValue("_HeadingText", AddUpdate.HeadingText);
                    command.Parameters.AddWithValue("_BannerUrl", AddUpdate.BannerUrl);
                    command.Parameters.AddWithValue("_DisplayOrder", AddUpdate.DisplayOrder);
                    command.Parameters.AddWithValue("_IsCategory", AddUpdate.IsCategory);
                    command.Parameters.AddWithValue("_SpType", AddUpdate.SpType);
                    command.Parameters.AddWithValue("_IsActive", AddUpdate.IsActive);

                    MySqlParameter resultParam = new MySqlParameter("_Result", MySqlDbType.Int32);
                    resultParam.Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add(resultParam);

                    if (AddUpdate.SpType == "C" || AddUpdate.SpType == "U")
                    {
                        command.ExecuteNonQuery();
                        result = Convert.ToInt32(command.Parameters["_Result"].Value);


                        if (result != 0)
                        {
                            apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                            apiResult.data = new { Id = result };
                            return Ok(apiResult);
                        }
                        else
                        {
                            apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.InsertUpdateFailed, ServiceRequestProcessor.StatusCode.InsertUpdateFailed.ToString());
                            apiResult.data = new { Id = result };
                            return Ok(apiResult);
                        }


                    }
                    else if (AddUpdate.SpType == "R")
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                List<BannerView> _bannerview = new List<BannerView>();

                                while (reader.Read())
                                {
                                    var _banner = new BannerView
                                    {
                                        Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32("Id"),
                                        CategoryId = reader.IsDBNull(reader.GetOrdinal("CategoryId")) ? 0 : reader.GetInt32("CategoryId"),
                                        HeadingText = reader.IsDBNull(reader.GetOrdinal("HeadingText")) ? null : reader.GetString("HeadingText"),
                                        BannerUrl = reader.IsDBNull(reader.GetOrdinal("BannerUrl")) ? null : reader.GetString("BannerUrl"),
                                        DisplayOrder = reader.IsDBNull(reader.GetOrdinal("DisplayOrder")) ? 0 : reader.GetInt32("DisplayOrder"),
                                        IsCategory = reader.IsDBNull(reader.GetOrdinal("IsCategory")) ? 0 : reader.GetInt32("IsCategory"),
                                        CreatedDate = reader.IsDBNull(reader.GetOrdinal("CreatedDate")) ? DateTime.MinValue : reader.GetDateTime("CreatedDate"),
                                        IsActive = reader.IsDBNull(reader.GetOrdinal("IsActive")) ? true : reader.GetBoolean("IsActive")
                                    };

                                    _bannerview.Add(_banner);
                                }


                                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                                apiResult.data = _bannerview;
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
                    else
                    {
                        apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.ServerError, "Invalid SpType");
                        apiResult.data = new { };
                        return BadRequest(apiResult);
                    }
                }
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }


        [Route("api/GetBanner")]
        [HttpGet]
        public IActionResult GetBanner()
        {
            try
            {
                int result;
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult;

                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    string SpType = "R";
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_commonbanner", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_Id", 0);
                    command.Parameters.AddWithValue("_CategoryId", 0);
                    command.Parameters.AddWithValue("_HeadingText", "");
                    command.Parameters.AddWithValue("_HeadingText2", "");
                    command.Parameters.AddWithValue("_BannerUrl", "");
                    command.Parameters.AddWithValue("_DisplayOrder", 0);
                    command.Parameters.AddWithValue("_IsCategory", 0);
                    command.Parameters.AddWithValue("_SpType", "R");
                    command.Parameters.AddWithValue("_IsActive", false);

                    MySqlParameter resultParam = new MySqlParameter("_Result", MySqlDbType.Int32);
                    resultParam.Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add(resultParam);

                    if (SpType == "R")
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                List<BannerView> _bannerview = new List<BannerView>();

                                while (reader.Read())
                                {
                                    var _banner = new BannerView
                                    {
                                        Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32("Id"),
                                        CategoryId = reader.IsDBNull(reader.GetOrdinal("CategoryId")) ? 0 : reader.GetInt32("CategoryId"),
                                        HeadingText = reader.IsDBNull(reader.GetOrdinal("HeadingText")) ? null : reader.GetString("HeadingText"),
                                        BannerUrl = reader.IsDBNull(reader.GetOrdinal("BannerUrl")) ? null : reader.GetString("BannerUrl"),
                                        DisplayOrder = reader.IsDBNull(reader.GetOrdinal("DisplayOrder")) ? 0 : reader.GetInt32("DisplayOrder"),
                                        IsCategory = reader.IsDBNull(reader.GetOrdinal("IsCategory")) ? 0 : reader.GetInt32("IsCategory"),
                                        CreatedDate = reader.IsDBNull(reader.GetOrdinal("CreatedDate")) ? DateTime.MinValue : reader.GetDateTime("CreatedDate"),
                                        IsActive = reader.IsDBNull(reader.GetOrdinal("IsActive")) ? true : reader.GetBoolean("IsActive")
                                    };

                                    _bannerview.Add(_banner);
                                }

                                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                                apiResult.data = _bannerview;
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
                    else
                    {
                        apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.ServerError, "Invalid SpType");
                        apiResult.data = new { };
                        return BadRequest(apiResult);
                    }
                }
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }
        [Route("api/CommonStory")]
        [HttpPost]
        public IActionResult CommonStory(AddupdateStory AddUpdate)
        {
            try
            {
                int result;
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult;

                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_commonstory", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_Id", AddUpdate.Id);
                    command.Parameters.AddWithValue("_ThumbnailUrl", AddUpdate.ThumbnailUrl);
                    command.Parameters.AddWithValue("_FileUrl", AddUpdate.FileUrl);
                    command.Parameters.AddWithValue("_FileType", AddUpdate.FileType);
                    command.Parameters.AddWithValue("_DisplayOrder", AddUpdate.DisplayOrder);
                    command.Parameters.AddWithValue("_SpType", AddUpdate.SpType);
                    command.Parameters.AddWithValue("_IsActive", AddUpdate.IsActive);

                    MySqlParameter resultParam = new MySqlParameter("_Result", MySqlDbType.Int32);
                    resultParam.Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add(resultParam);

                    if (AddUpdate.SpType == "C" || AddUpdate.SpType == "U")
                    {
                        command.ExecuteNonQuery();
                        result = Convert.ToInt32(command.Parameters["_Result"].Value);


                        if (result != 0)
                        {
                            apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                            apiResult.data = new { Id = result };
                            return Ok(apiResult);
                        }
                        else
                        {
                            apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.InsertUpdateFailed, ServiceRequestProcessor.StatusCode.InsertUpdateFailed.ToString());
                            apiResult.data = new { Id = result };
                            return Ok(apiResult);
                        }


                    }
                    else if (AddUpdate.SpType == "R")
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                List<StoryView> _bannerview = new List<StoryView>();

                                while (reader.Read())
                                {
                                    var _banner = new StoryView
                                    {
                                        Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32("Id"),
                                        ThumbnailUrl = reader.IsDBNull(reader.GetOrdinal("ThumbnailUrl")) ? null : reader.GetString("ThumbnailUrl"),
                                        FileUrl = reader.IsDBNull(reader.GetOrdinal("FileUrl")) ? null : reader.GetString("FileUrl"),
                                        FileType = reader.IsDBNull(reader.GetOrdinal("FileType")) ? 0 : reader.GetInt32("FileType"),
                                        DisplayOrder = reader.IsDBNull(reader.GetOrdinal("DisplayOrder")) ? 0 : reader.GetInt32("DisplayOrder"),
                                        CreatedDate = reader.IsDBNull(reader.GetOrdinal("CreatedDate")) ? DateTime.MinValue : reader.GetDateTime("CreatedDate"),
                                        IsActive = reader.IsDBNull(reader.GetOrdinal("IsActive")) ? true : reader.GetBoolean("IsActive")
                                    };

                                    _bannerview.Add(_banner);
                                }
                                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                                apiResult.data = _bannerview;
                                return Ok(apiResult);

                            }
                            else
                            {
                                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                                apiResult.data = new { };
                                return NotFound(apiResult);
                            }
                        }
                    }
                    else
                    {
                        apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.ServerError, "Invalid SpType");
                        apiResult.data = new { };
                        return BadRequest(apiResult);
                    }
                }
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }

        [Route("api/GetStory")]
        [HttpGet]
        public IActionResult GetStory()
        {
            try
            {
                string SpType = "R";
                int result;
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult;

                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_commonstory", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_Id", 0);
                    command.Parameters.AddWithValue("_HeaderText", "");
                    command.Parameters.AddWithValue("_CategoryId", 0);
                    command.Parameters.AddWithValue("_IsCategory", 0);
                    command.Parameters.AddWithValue("_ThumbnailUrl", "");
                    command.Parameters.AddWithValue("_FileUrl", "");
                    command.Parameters.AddWithValue("_FileType", 0);
                    command.Parameters.AddWithValue("_DisplayOrder", 0);
                    command.Parameters.AddWithValue("_SpType", SpType);
                    command.Parameters.AddWithValue("_IsActive", true);

                    MySqlParameter resultParam = new MySqlParameter("_Result", MySqlDbType.Int32);
                    resultParam.Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add(resultParam);

                    if (SpType == "R")
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                List<StoryView> _bannerview = new List<StoryView>();

                                while (reader.Read())
                                {
                                    var _banner = new StoryView
                                    {
                                        Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32("Id"),
                                        HeaderText = reader.IsDBNull(reader.GetOrdinal("HeaderText")) ? null : reader.GetString("HeaderText"),
                                        CategoryId = reader.IsDBNull(reader.GetOrdinal("CategoryId")) ? 0 : reader.GetInt32("CategoryId"),
                                        CategoryName = reader.IsDBNull(reader.GetOrdinal("CategoryName")) ? null : reader.GetString("CategoryName"),
                                        IsCategory = reader.IsDBNull(reader.GetOrdinal("IsCategory")) ? 0 : reader.GetInt32("IsCategory"),
                                        ThumbnailUrl = reader.IsDBNull(reader.GetOrdinal("ThumbnailUrl")) ? null : reader.GetString("ThumbnailUrl"),
                                        FileUrl = reader.IsDBNull(reader.GetOrdinal("FileUrl")) ? null : reader.GetString("FileUrl"),
                                        FileType = reader.IsDBNull(reader.GetOrdinal("FileType")) ? 0 : reader.GetInt32("FileType"),
                                        DisplayOrder = reader.IsDBNull(reader.GetOrdinal("DisplayOrder")) ? 0 : reader.GetInt32("DisplayOrder"),
                                        CreatedDate = reader.IsDBNull(reader.GetOrdinal("CreatedDate")) ? DateTime.MinValue : reader.GetDateTime("CreatedDate"),
                                        IsActive = reader.IsDBNull(reader.GetOrdinal("IsActive")) ? true : reader.GetBoolean("IsActive")
                                    };

                                    _bannerview.Add(_banner);
                                }
                                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                                apiResult.data = _bannerview;
                                return Ok(apiResult);
                            }
                            else
                            {
                                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                                apiResult.data = new { };
                                return NotFound(apiResult);
                            }
                        }
                    }
                    else
                    {
                        apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.ServerError, "Invalid SpType");
                        apiResult.data = new { };
                        return BadRequest(apiResult);
                    }
                }
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }

        [Route("api/CommonColor")]
        [HttpPost]
        public IActionResult CommonColor(AddUpdateColor AddUpdate)
        {
            try
            {
                int result;
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult;

                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_commoncolor", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_Id", AddUpdate.Id);
                    command.Parameters.AddWithValue("_Color", AddUpdate.Color);
                    command.Parameters.AddWithValue("_Image", AddUpdate.Image);
                    command.Parameters.AddWithValue("_ColorCode", AddUpdate.ColorCode);
                    command.Parameters.AddWithValue("_SpType", AddUpdate.SpType);

                    MySqlParameter resultParam = new MySqlParameter("_Result", MySqlDbType.Int32);
                    resultParam.Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add(resultParam);

                    if (AddUpdate.SpType == "C" || AddUpdate.SpType == "U")
                    {
                        command.ExecuteNonQuery();
                        result = Convert.ToInt32(command.Parameters["_Result"].Value);


                        if (result != 0)
                        {
                            apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                            apiResult.data = new { Id = result };
                            return Ok(apiResult);
                        }
                        else
                        {
                            apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.InsertUpdateFailed, ServiceRequestProcessor.StatusCode.InsertUpdateFailed.ToString());
                            apiResult.data = new { Id = result };
                            return Ok(apiResult);
                        }


                    }
                    else if (AddUpdate.SpType == "R")
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                List<Colors> _bannerview = new List<Colors>();

                                while (reader.Read())
                                {
                                    var _banner = new Colors
                                    {
                                        Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32("Id"),
                                        Color = reader.IsDBNull(reader.GetOrdinal("Color")) ? null : reader.GetString("Color"),
                                        ColorCode= reader.IsDBNull(reader.GetOrdinal("ColorCode")) ? null : reader.GetString("ColorCode"),
                                        Image = reader.IsDBNull(reader.GetOrdinal("Image")) ? null : reader.GetString("Image"),
                                        };

                                    _bannerview.Add(_banner);
                                }


                                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                                apiResult.data = _bannerview;
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
                    else
                    {
                        apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.ServerError, "Invalid SpType");
                        apiResult.data = new { };
                        return BadRequest(apiResult);
                    }
                }
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }

        [Route("api/CommonSize")]
        [HttpPost]
        public IActionResult CommonSize(AddUpdateSize AddUpdate)
        {
            try
            {
                int result;
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult;

                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_commonsize", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_Id", AddUpdate.Id);
                    command.Parameters.AddWithValue("_Size", AddUpdate.Size);
                    command.Parameters.AddWithValue("_SpType", AddUpdate.SpType);

                    MySqlParameter resultParam = new MySqlParameter("_Result", MySqlDbType.Int32);
                    resultParam.Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add(resultParam);

                    if (AddUpdate.SpType == "C" || AddUpdate.SpType == "U")
                    {
                        command.ExecuteNonQuery();
                        result = Convert.ToInt32(command.Parameters["_Result"].Value);


                        if (result != 0)
                        {
                            apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                            apiResult.data = new { Id = result };
                            return Ok(apiResult);
                        }
                        else
                        {
                            apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.InsertUpdateFailed, ServiceRequestProcessor.StatusCode.InsertUpdateFailed.ToString());
                            apiResult.data = new { Id = result };
                            return Ok(apiResult);
                        }


                    }
                    else if (AddUpdate.SpType == "R")
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                List<Sizes> _bannerview = new List<Sizes>();

                                while (reader.Read())
                                {
                                    var _banner = new Sizes
                                    {
                                        Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32("Id"),
                                        Size = reader.IsDBNull(reader.GetOrdinal("Size")) ? null : reader.GetString("Size")
                                    };

                                    _bannerview.Add(_banner);
                                }


                                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                                apiResult.data = _bannerview;
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
                    else
                    {
                        apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.ServerError, "Invalid SpType");
                        apiResult.data = new { };
                        return BadRequest(apiResult);
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
