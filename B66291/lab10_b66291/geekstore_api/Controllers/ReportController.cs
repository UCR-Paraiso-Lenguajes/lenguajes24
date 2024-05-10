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
        public async Task<ActionResult<Ienumerable<Report>>> GetSalesAsync(string date)
        {          
            if(date == null){
                return BadRequest("La fecha no puede ser nula");
            } 
            
            var dailySalesTask = ReportDb.ExtraerVentasDiariasAsync(selectedDate); 
            var weeklySalesTask = ReportDb.ExtraerVentasSemanalAsync(selectedDate); 

            await Task.WhenAll(dailySalesTask, weeklySalesTask);

            var dailySales = await dailySalesTask;
            var weeklySales = await weeklySalesTask;

            var dailySalesList = ReportLogic.TransformarDatos(dailySales);
            var weeklySalesList = ReportLogic.TransformarDatos(weeklySales);

            return Ok(new { dailySalesList, weeklySalesList });
        }
    }
}

 