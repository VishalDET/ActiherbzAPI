using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using SynfoShopAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection.PortableExecutable;

namespace SynfoShopAPI.Controllers
{

    [ApiController]
    public class FranchiseController : ControllerBase
    {
        private readonly IConfiguration _config;

        public FranchiseController(IConfiguration config)
        {
            _config = config;
        }
        [Route("api/CommonFranchise")]
        [HttpPost]


        public IActionResult CommonFranchise(Franchise franchiseData)
        {
            try
            {
                int result;
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult;
                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_commonfranchise", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_Id", franchiseData.Id);
                    command.Parameters.AddWithValue("_FranchiseName", franchiseData.FranchiseName);
                    command.Parameters.AddWithValue("_MobileNo", franchiseData.MobileNo);
                    command.Parameters.AddWithValue("_EmailId", franchiseData.EmailId);
                    command.Parameters.AddWithValue("_CompanyName", franchiseData.CompanyName);
                    command.Parameters.AddWithValue("_CompanyAddress", franchiseData.CompanyAddress);
                    command.Parameters.AddWithValue("_IsGSTIN", franchiseData.IsGSTIN);
                    command.Parameters.AddWithValue("_GSTNumber", franchiseData.GSTNumber);
                    command.Parameters.AddWithValue("_CityId", franchiseData.CityId);
                    command.Parameters.AddWithValue("_StateId", franchiseData.StateId);
                    command.Parameters.AddWithValue("_CountryId", franchiseData.CountryId);
                    command.Parameters.AddWithValue("_Taluka", franchiseData.Taluka);
                    command.Parameters.AddWithValue("_IsActive", franchiseData.IsActive);
                    command.Parameters.AddWithValue("_SpType", franchiseData.SpType);
                    MySqlParameter resultParam = new MySqlParameter("_Result", MySqlDbType.Int32);
                    resultParam.Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add(resultParam);

                    if (franchiseData.SpType == "C" || franchiseData.SpType == "U" || franchiseData.SpType == "D")
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
                    else if (franchiseData.SpType == "E" || franchiseData.SpType == "R")
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                List<FranchiseView> FranchiseList = new List<FranchiseView>();

                                while (reader.Read())
                                {
                                    var franchiseModel = new FranchiseView
                                    {
                                        Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32("Id"),
                                        FranchiseName = reader.IsDBNull(reader.GetOrdinal("FranchiseName")) ? null : reader.GetString("FranchiseName"),
                                        MobileNo = reader.IsDBNull(reader.GetOrdinal("MobileNo")) ? null : reader.GetString("MobileNo"),
                                        EmailId = reader.IsDBNull(reader.GetOrdinal("EmailId")) ? null : reader.GetString("EmailId"),
                                        CompanyName = reader.IsDBNull(reader.GetOrdinal("CompanyName")) ? null : reader.GetString("CompanyName"),
                                        CompanyAddress = reader.IsDBNull(reader.GetOrdinal("CompanyAddress")) ? null : reader.GetString("CompanyAddress"),
                                        IsGSTIN = reader.GetBoolean(reader.GetOrdinal("IsGSTIN")),
                                        GSTNumber = reader.IsDBNull(reader.GetOrdinal("GSTNumber")) ? null : reader.GetString("GSTNumber"),
                                        CityId = reader.IsDBNull(reader.GetOrdinal("CityId")) ? 0 : reader.GetInt32("CityId"),
                                        City = reader.IsDBNull(reader.GetOrdinal("CityName")) ? null : reader.GetString("CityName"),
                                        StateId = reader.IsDBNull(reader.GetOrdinal("StateId")) ? 0 : reader.GetInt32("StateId"),
                                        State = reader.IsDBNull(reader.GetOrdinal("StateName")) ? null : reader.GetString("StateName"),
                                        CountryId = reader.IsDBNull(reader.GetOrdinal("CountryId")) ? 0 : reader.GetInt32("CountryId"),
                                        Country = reader.IsDBNull(reader.GetOrdinal("CountryName")) ? null : reader.GetString("CountryName"),
                                        Taluka = reader.IsDBNull(reader.GetOrdinal("Taluka")) ? null : reader.GetString("Taluka"),
                                        IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                        CreatedDate = reader.IsDBNull(reader.GetOrdinal("CreatedDate")) ? DateTime.MinValue : reader.GetDateTime("CreatedDate"),
                                        ModifiedDate = reader.IsDBNull(reader.GetOrdinal("ModifiedDate")) ? DateTime.MinValue : reader.GetDateTime("ModifiedDate")
                                    };
                                    FranchiseList.Add(franchiseModel);
                                }

                                //return Ok(FranchiseList);
                                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                                apiResult.data = FranchiseList;
                                return Ok(apiResult);
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
                    else if (franchiseData.SpType == "LS")
                    {
                        List<FranchiseView> franchiseList = new List<FranchiseView>();

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var franchiseModel = new FranchiseView
                                {
                                    Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32("Id"),
                                    FranchiseName = reader.IsDBNull(reader.GetOrdinal("FranchiseName")) ? null : reader.GetString("FranchiseName"),
                                    MobileNo = reader.IsDBNull(reader.GetOrdinal("MobileNo")) ? null : reader.GetString("MobileNo"),
                                    EmailId = reader.IsDBNull(reader.GetOrdinal("EmailId")) ? null : reader.GetString("EmailId"),
                                    CompanyName = reader.IsDBNull(reader.GetOrdinal("CompanyName")) ? null : reader.GetString("CompanyName"),
                                    CompanyAddress = reader.IsDBNull(reader.GetOrdinal("CompanyAddress")) ? null : reader.GetString("CompanyAddress"),
                                    IsGSTIN = reader.GetBoolean(reader.GetOrdinal("IsGSTIN")),
                                    GSTNumber = reader.IsDBNull(reader.GetOrdinal("GSTNumber")) ? null : reader.GetString("GSTNumber"),
                                    CityId = reader.IsDBNull(reader.GetOrdinal("CityId")) ? 0 : reader.GetInt32("CityId"),
                                    City = reader.IsDBNull(reader.GetOrdinal("CityName")) ? null : reader.GetString("CityName"),
                                    StateId = reader.IsDBNull(reader.GetOrdinal("StateId")) ? 0 : reader.GetInt32("StateId"),
                                    State = reader.IsDBNull(reader.GetOrdinal("StateName")) ? null : reader.GetString("StateName"),
                                    CountryId = reader.IsDBNull(reader.GetOrdinal("CountryId")) ? 0 : reader.GetInt32("CountryId"),
                                    Country = reader.IsDBNull(reader.GetOrdinal("CountryName")) ? null : reader.GetString("CountryName"),
                                    Taluka = reader.IsDBNull(reader.GetOrdinal("Taluka")) ? null : reader.GetString("Taluka"),
                                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                    CreatedDate = reader.IsDBNull(reader.GetOrdinal("CreatedDate")) ? DateTime.MinValue : reader.GetDateTime("CreatedDate"),
                                    ModifiedDate = reader.IsDBNull(reader.GetOrdinal("ModifiedDate")) ? DateTime.MinValue : reader.GetDateTime("ModifiedDate")
                                };

                                franchiseList.Add(franchiseModel);
                            }
                        }


                        apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                        apiResult.data = franchiseList;
                        return Ok(apiResult);
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



        [Route("api/FranchiseProducts")]
        [HttpPost]
        public IActionResult GetFranchiseProducts(int franchiseId)
        {
            try
            {
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult;
                List<FranchiseProduct> franchiseProducts = new List<FranchiseProduct>();

                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_getfranchiseproducts", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_FranchiseId", franchiseId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var franchiseProduct = new FranchiseProduct
                            {
                                Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32("Id"),
                                ProductId = reader.IsDBNull(reader.GetOrdinal("ProductId")) ? 0 : reader.GetInt32("ProductId"),
                                ProductTitle = reader.IsDBNull(reader.GetOrdinal("ProductTitle")) ? null : reader.GetString("ProductTitle"),
                                MainImage = reader.IsDBNull(reader.GetOrdinal("MainImage")) ? null : reader.GetString("MainImage"),
                                BrandId = reader.IsDBNull(reader.GetOrdinal("BrandId")) ? 0 : reader.GetInt32("BrandId"),
                                BrandName = reader.IsDBNull(reader.GetOrdinal("BrandName")) ? null : reader.GetString("BrandName"),
                                CategoryId = reader.IsDBNull(reader.GetOrdinal("CategoryId")) ? 0 : reader.GetInt32("CategoryId"),
                                Category = reader.IsDBNull(reader.GetOrdinal("Category")) ? null : reader.GetString("Category"),
                                SubCategory = reader.IsDBNull(reader.GetOrdinal("SubCategory")) ? null : reader.GetString("SubCategory"),
                                FranchiseId = reader.IsDBNull(reader.GetOrdinal("FranchiseId")) ? 0 : reader.GetInt32("FranchiseId"),
                                FranchiseName = reader.IsDBNull(reader.GetOrdinal("FranchiseName")) ? null : reader.GetString("FranchiseName"),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                CreatedDate = reader.IsDBNull(reader.GetOrdinal("CreatedDate")) ? DateTime.MinValue : reader.GetDateTime("CreatedDate"),
                                ModifiedDate = reader.IsDBNull(reader.GetOrdinal("ModifiedDate")) ? DateTime.MinValue : reader.GetDateTime("ModifiedDate")
                            };
                            franchiseProducts.Add(franchiseProduct);
                        }
                    }
                    apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                    apiResult.data = franchiseProducts;
                    return Ok(apiResult);
                }
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }

        [Route("api/FranchiseProductsForPricing")]
        [HttpGet]
        public IActionResult GetFranchiseProductsWith(int franchiseId, int ProductId)
        {
            try
            {
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult;
                List<ProductMappingView> productMapping = new List<ProductMappingView>();

                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_getpackegesforpricing", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_FranchiseId", franchiseId);
                    command.Parameters.AddWithValue("_ProductId", ProductId);

                    DataSet ds = new DataSet();
                    MySqlDataAdapter da = new MySqlDataAdapter(command);
                    da.Fill(ds);

                    DataTable dtfranchise = new DataTable();
                    dtfranchise = ds.Tables[0];

                    DataTable dtPackages = new DataTable();
                    dtPackages = ds.Tables[1];
                    foreach (DataRow dr in dtfranchise.Rows)
                    {
                        List<CommonPackage> _CommonPackageList = new List<CommonPackage>();
                        if (dtPackages.AsEnumerable().Where(p => p["ProductId"].ToString() == dr["ProductId"].ToString() && p["FranchiseId"].ToString() == dr["FranchiseId"].ToString()).AsDataView().Count > 0)
                        {
                            DataTable _dtNew = new DataTable();
                            _dtNew = dtPackages.AsEnumerable().Where(p => p["ProductId"].ToString() == dr["ProductId"].ToString() && p["FranchiseId"].ToString() == dr["FranchiseId"].ToString()).CopyToDataTable();
                            foreach (DataRow dr1 in _dtNew.Rows)
                            {
                                var packageModel = new CommonPackage
                                {
                                    Id = dr1.IsNull("Id") ? 0 : Convert.ToInt32(dr1["Id"]),
                                    ProductId = dr1.IsNull("ProductId") ? 0 : Convert.ToInt32(dr1["ProductId"]),
                                    FranchiseId = dr1.IsNull("FranchiseId") ? 0 : Convert.ToInt32(dr1["FranchiseId"]),
                                    PackageName = dr1.IsNull("PackageName") ? null : (dr1["PackageName"].ToString()),
                                    Price = dr1.IsNull("Price") ? 0.0 : Convert.ToDouble(dr1["Price"]),
                                    DiscountPercent = dr1.IsNull("DiscountPercent") ? 0.0 : Convert.ToDouble(dr1["DiscountPercent"]),
                                    MinQuantity = dr1.IsNull("MinQuantity") ? 0.0 : Convert.ToInt32(dr1["MinQuantity"]),
                                    SalePrice = dr1.IsNull("SalePrice") ? 0.0 : Convert.ToDouble(dr1["SalePrice"]),
                                    DisplayOrder = dr1.IsNull("DisplayOrder") ? 0 : Convert.ToInt32(dr1["DisplayOrder"]),
                                    ModifiedDate = dr1["ModifiedDate"] == null ? DateTime.MinValue : Convert.ToDateTime(dr1["ModifiedDate"].ToString()),
                                    BrandName = dr1.IsNull("BrandName") ? null : (dr1["BrandName"].ToString()),
                                   // CategoryName = dr1.IsNull("CategoryName") ? null : (dr1["CategoryName"].ToString()),
                                    //SubCategoryName = dr1.IsNull("SubCategoryName") ? null : (dr1["SubCategoryName"].ToString())
                                };
                                _CommonPackageList.Add(packageModel);
                            }
                        }
                        var _productMapping = new ProductMappingView
                        {
                            Id = dr.IsNull("Id") ? 0 : Convert.ToInt32(dr["Id"]),
                            FranchiseName = dr["FranchiseName"].ToString(),
                            RelationashipManager = dr["FranchiseName"].ToString(),
                            Zone = dr["Zone"].ToString(),
                            State = dr["State"].ToString(),
                            City = dr["City"].ToString(),
                            ProductId = dr.IsNull("ProductId") ? 0 : Convert.ToInt32(dr["ProductId"]),
                            ProductName = dr["ProductName"].ToString(),
                            MainImage = dr["MainImage"].ToString(),
                            StockQuantity = dr.IsNull("StockQuantity") ? 0 : Convert.ToInt32(dr["StockQuantity"]),
                            FranchiseId = dr.IsNull("FranchiseId") ? 0 : Convert.ToInt32(dr["FranchiseId"]),
                            MRP = dr.IsNull("MRP") ? 0 : Convert.ToDouble(dr["MRP"]),
                            IsActive = Convert.ToBoolean(dr["IsActive"]),
                            BrandName = dr["BrandName"].ToString(),
                           // CategoryName = dr["CategoryName"].ToString(),
                            //SubCategoryName = dr.IsNull("SubCategoryName") ? null : (dr["SubCategoryName"].ToString()),
                            packages = _CommonPackageList
                        };
                        productMapping.Add(_productMapping);
                    }
                    apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                    apiResult.data = productMapping;
                    return Ok(apiResult);
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
