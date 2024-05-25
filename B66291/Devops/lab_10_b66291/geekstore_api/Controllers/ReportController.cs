using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Threading.Tasks;
using core;
using core.Business;
using core.DataBase;
using core.Models;


namespace geekstore_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<Report>>> GetSales(string date)
        {
            try {
            DateTime selectedDate= DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var dailySalesTask = ReportDb.ExtraerVentasDiariasAsync(selectedDate); 
            var weeklySalesTask = ReportDb.ExtraerVentasSemanalAsync(selectedDate); 

            await Task.WhenAll(dailySalesTask, weeklySalesTask);

            var dailySales = await dailySalesTask;
            var dailySalesList = ReportLogic.TransformarDatos(dailySales);

            var weeklySales = await weeklySalesTask;
            var weeklySalesList = ReportLogic.TransformarDatos(weeklySales);

            IEnumerable<object>[] reportList = new List<object>[2];
            reportList[0] = dailySalesList;
            reportList[1] = weeklySalesList;

            return Ok(reportList); 
            
            } catch(ArgumentException ex){
                return BadRequest(ex.Message);
            }
        }
    }
}

 