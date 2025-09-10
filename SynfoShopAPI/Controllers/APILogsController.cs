using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using DAE.DAL.SQL;
using DaeConfiguration;
using System;
using SynfoShopAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace SynfoShopAPI.Controllers
{
    [Authorize]
    [ApiController]
    public class APILogsController : ControllerBase
    {
        private readonly IDaeConfigManager _configurationIG;
        private ServiceRequestProcessor oServiceRequestProcessor;

        public APILogsController(IDaeConfigManager configuration)
        {
            _configurationIG = configuration;
        }

        [Route("APILogs/get")]
        [HttpPost]
        public IActionResult Get(SynfoShopAPI.Models.APILogs aPILogs)
        {
            try
            {
                DBUtility oDBUtility = new DBUtility(_configurationIG);

             
                DataSet ds = oDBUtility.Execute_StoreProc_DataSet("USP_GET_City");
                oServiceRequestProcessor = new ServiceRequestProcessor();
                return Ok(oServiceRequestProcessor.ProcessRequest(ds));
            }
            catch (Exception ex)
            {
                oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }
    }
}
