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
    public class CategoryController : ControllerBase
    {
        private readonly IConfiguration _config;

        public CategoryController(IConfiguration config)
        {
            _config = config;
        }
        [Route("api/CommonCategory")]
        [HttpPost]
        public IActionResult CommonCategory(Category categoryData)
        {
            try
            {
                int result;
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult;
                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_commoncategory", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_Id", categoryData.Id);
                    command.Parameters.AddWithValue("_CategoryName", categoryData.CategoryName);
                    command.Parameters.AddWithValue("_MainImage", categoryData.MainImage);
                    command.Parameters.AddWithValue("_ParentId", categoryData.ParentId);
                    command.Parameters.AddWithValue("_CategoryLevel", categoryData.CategoryLevel);
                    command.Parameters.AddWithValue("_DisplayOrder", categoryData.DisplayOrder);
                    command.Parameters.AddWithValue("_CategoryHeading", categoryData.CategoryHeading);
                    command.Parameters.AddWithValue("_SpType", categoryData.SpType);
                    command.Parameters.AddWithValue("_IsActive", categoryData.IsActive);

                    MySqlParameter resultParam = new MySqlParameter("_Result", MySqlDbType.Int32);
                    resultParam.Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add(resultParam);

                    if (categoryData.SpType == "C" || categoryData.SpType == "U" || categoryData.SpType == "D")
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
                    else if (categoryData.SpType == "E" || categoryData.SpType == "R" || categoryData.SpType == "A" || categoryData.SpType == "ALL")
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                List<Category> categoryList = new List<Category>();

                                while (reader.Read())
                                {
                                    var categoryModel = new Category
                                    {
                                        Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32("Id"),
                                        CategoryName = reader.IsDBNull(reader.GetOrdinal("CategoryName")) ? null : reader.GetString("CategoryName"),
                                        MainImage = reader.IsDBNull(reader.GetOrdinal("MainImage")) ? null : reader.GetString("MainImage"),
                                        ParentId = reader.IsDBNull(reader.GetOrdinal("ParentId")) ? 0 : reader.GetInt32("ParentId"),
                                        CategoryLevel = reader.IsDBNull(reader.GetOrdinal("CategoryLevel")) ? 0 : reader.GetInt32("CategoryLevel"),
                                        DisplayOrder = reader.IsDBNull(reader.GetOrdinal("DisplayOrder")) ? 0 : reader.GetInt32("DisplayOrder"),
                                        CategoryHeading = reader.IsDBNull(reader.GetOrdinal("CategoryHeading")) ? null : reader.GetString("CategoryHeading"),
                                        IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                        CreatedDate = reader.IsDBNull(reader.GetOrdinal("CreatedDate")) ? DateTime.MinValue : reader.GetDateTime("CreatedDate")
                                    };

                                    categoryList.Add(categoryModel);
                                }
                                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                                apiResult.data = categoryList;
                                return Ok(apiResult);
                                //return Ok(categoryList);
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
                    else if (categoryData.SpType == "SC")
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                List<Category> subCategoryList = new List<Category>();

                                while (reader.Read())
                                {
                                    var subCategoryModel = new Category
                                    {
                                        Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32("Id"),
                                        CategoryName = reader.IsDBNull(reader.GetOrdinal("CategoryName")) ? null : reader.GetString("CategoryName"),
                                        MainImage = reader.IsDBNull(reader.GetOrdinal("MainImage")) ? null : reader.GetString("MainImage"),
                                        ParentId = reader.IsDBNull(reader.GetOrdinal("ParentId")) ? 0 : reader.GetInt32("ParentId"),
                                        CategoryLevel = reader.IsDBNull(reader.GetOrdinal("CategoryLevel")) ? 0 : reader.GetInt32("CategoryLevel"),
                                        DisplayOrder = reader.IsDBNull(reader.GetOrdinal("DisplayOrder")) ? 0 : reader.GetInt32("DisplayOrder"),
                                        CategoryHeading = reader.IsDBNull(reader.GetOrdinal("CategoryHeading")) ? null : reader.GetString("CategoryHeading"),
                                        IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                        CreatedDate = reader.IsDBNull(reader.GetOrdinal("CreatedDate")) ? DateTime.MinValue : reader.GetDateTime("CreatedDate")
                                    };

                                    subCategoryList.Add(subCategoryModel);
                                }

                                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                                apiResult.data = subCategoryList;
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

        }




        [Route("api/SubCategories")]
        [HttpPost]
        public IActionResult GetCategories(string parentIds)
        {
            try
            {
                List<Category> categoryList = new List<Category>();

                using (var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    using (var command = new MySqlCommand("usp_get_categories", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@_ParentId", parentIds);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var category = new Category
                                {
                                    Id = reader.GetInt32("Id"),
                                    CategoryName = reader.GetString("CategoryName"),
                                    MainImage = reader.GetString("MainImage"),
                                    ParentId = reader.GetInt32("ParentId"),
                                    CategoryLevel = reader.GetInt32("CategoryLevel"),
                                    DisplayOrder = reader.GetInt32("DisplayOrder"),
                                    CategoryHeading = reader.GetString("CategoryHeading"),
                                    IsActive = reader.GetBoolean("IsActive"),
                                    CreatedDate = reader.GetDateTime("CreatedDate")
                                };

                                categoryList.Add(category);
                            }
                        }
                    }
                }

                if (categoryList.Count > 0)
                {
                    ServiceRequestProcessor processor = new ServiceRequestProcessor();
                    APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                    apiResult.data = categoryList;
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
                return BadRequest(new { Status = "Error", Message = ex.Message });
            }
        }


        [Route("api/Categories")]
        [HttpPost]
        public IActionResult GetCategories()
        {
            try
            {
                List<Category> categoryList = new List<Category>();

                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_getcategory", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var category = new Category
                            {
                                Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32("Id"),
                                CategoryName = reader.IsDBNull(reader.GetOrdinal("CategoryName")) ? string.Empty : reader.GetString("CategoryName"),
                                MainImage = reader.IsDBNull(reader.GetOrdinal("MainImage")) ? string.Empty : reader.GetString("MainImage"),
                                ParentId = reader.IsDBNull(reader.GetOrdinal("ParentId")) ? 0 : reader.GetInt32("ParentId"),
                                CategoryLevel = reader.IsDBNull(reader.GetOrdinal("CategoryLevel")) ? 0 : reader.GetInt32("CategoryLevel"),
                                DisplayOrder = reader.IsDBNull(reader.GetOrdinal("DisplayOrder")) ? 0 : reader.GetInt32("DisplayOrder"),
                                CategoryHeading = reader.IsDBNull(reader.GetOrdinal("CategoryHeading")) ? string.Empty : reader.GetString("CategoryHeading"),
                                IsActive = reader.IsDBNull(reader.GetOrdinal("IsActive")) ? false : reader.GetBoolean("IsActive"),
                                CreatedDate = reader.IsDBNull(reader.GetOrdinal("CreatedDate")) ? DateTime.MinValue : reader.GetDateTime("CreatedDate")
                            };

                            categoryList.Add(category);
                        }
                    }
                }

                if (categoryList.Count > 0)
                {
                    ServiceRequestProcessor processor = new ServiceRequestProcessor();
                    APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                    apiResult.data = categoryList;
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

        [Route("api/GetCategoryView")]
        [HttpPost]
        public IActionResult GetCategoryView()
        {
            try
            {
                List<CategoryView> categoryList;
                List<CategoryView> liMainCategory;
                List<SubCategoryView> liSubCategory;
                List<childCategoryView> liChildCategory;

                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_cat_subcat", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    DataTable dt = new DataTable();
                    MySqlDataAdapter da = new MySqlDataAdapter(command);
                    da.Fill(dt);
                    DataTable dt_Menu;
                    DataTable dt_Menu_Category;
                    DataTable dt_Menu_SubCategory;
                    dt_Menu = dt.Clone();
                    dt_Menu_Category = dt.Clone();
                    dt_Menu_SubCategory = dt.Clone();

                    foreach (DataRow row in dt.Rows)
                    {
                        DataRow row1 = row;
                        //if (Convert.ToInt32(row["CategoryLevel"].ToString()) == 0)
                        //{
                        //    dt_Menu_.Rows.Add(row1.ItemArray);
                        //}
                        if (Convert.ToInt32(row["CategoryLevel"].ToString()) == 1)
                        {
                            dt_Menu.Rows.Add(row1.ItemArray);
                        }
                        else if (Convert.ToInt32(row["CategoryLevel"].ToString()) == 2)
                        {
                            dt_Menu_Category.Rows.Add(row1.ItemArray);
                        }
                        else if (Convert.ToInt32(row["CategoryLevel"].ToString()) == 3)
                        {
                            dt_Menu_SubCategory.Rows.Add(row1.ItemArray);
                        }
                    }
                    liMainCategory = new List<CategoryView>();

                    foreach (DataRow row in dt_Menu.Rows)
                    {
                        CategoryView mainCategory = new CategoryView();
                        mainCategory.main_category_id = Convert.ToInt32(row["main_category_id"]);
                        mainCategory.main_category_name = row["main_category_name"].ToString();
                        mainCategory.main_category_image_url = row["main_category_image_url"].ToString();

                        liSubCategory = new List<SubCategoryView>();
                        foreach (DataRow row1 in dt_Menu_Category.Rows)
                        {
                            if (Convert.ToInt32(row1["CategoryParentId"]) == Convert.ToInt32(row["main_category_id"].ToString()))
                            {
                                SubCategoryView SubCategory = new SubCategoryView();
                                SubCategory.sub_category_id = Convert.ToInt32(row1["main_category_id"]);
                                SubCategory.sub_category_name = row1["main_category_name"].ToString();
                                SubCategory.sub_category_image_url = row1["main_category_image_url"].ToString();
                                liSubCategory.Add(SubCategory);

                                liChildCategory = new List<childCategoryView>();
                                foreach (DataRow row2 in dt_Menu_SubCategory.Rows)
                                {
                                    if (Convert.ToInt32(row2["CategoryParentId"]) == Convert.ToInt32(row1["main_category_id"].ToString()))
                                    {
                                        childCategoryView ChildCategory = new childCategoryView();
                                        ChildCategory.child_category_id = Convert.ToInt32(row2["main_category_id"]);
                                        ChildCategory.child_category_name = row2["main_category_name"].ToString();
                                        ChildCategory.child_category_image_url = row2["main_category_image_url"].ToString();
                                        liChildCategory.Add(ChildCategory);
                                    }
                                }
                                SubCategory.child_category = liChildCategory;
                            }
                        }
                        mainCategory.sub_categories = liSubCategory;

                        liMainCategory.Add(mainCategory);
                    }
                    //using (var reader = command.ExecuteReader())
                    //{
                    //    while (reader.Read())
                    //    {
                    //        //List<Category> liSubCat = new List<Category>();
                    //        var category = new CategoryView
                    //        {
                    //            main_category_id = reader.IsDBNull(reader.GetOrdinal("main_category_id")) ? 0 : reader.GetInt32("main_category_id"),
                    //            main_category_name = reader.IsDBNull(reader.GetOrdinal("main_category_name")) ? string.Empty : reader.GetString("main_category_name"),
                    //            main_category_image_url = reader.IsDBNull(reader.GetOrdinal("main_category_image_url")) ? "" : reader.GetString("main_category_image_url"),

                    //            sub_categories = new List<Category>
                    //                {
                    //                    new Category
                    //                    {
                    //                        Id = reader.IsDBNull(reader.GetOrdinal("sub_category_id_1")). ? 0 : reader.GetInt32("sub_category_id_1"),
                    //                        CategoryName = reader.IsDBNull(reader.GetOrdinal("sub_category_name_1")) ? string.Empty : reader.GetString("sub_category_name_1"),
                    //                        MainImage = reader.IsDBNull(reader.GetOrdinal("sub_category_image_url_1")) ? "" : reader.GetString("sub_category_image_url_1"),
                    //                    },

                    //                }
                    //        };
                    //        categoryList.Add(category);
                    //    }
                    //}
                }

                if (liMainCategory.Count > 0)
                {
                    ServiceRequestProcessor processor = new ServiceRequestProcessor();
                    APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                    apiResult.data = liMainCategory;
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

    }
}
