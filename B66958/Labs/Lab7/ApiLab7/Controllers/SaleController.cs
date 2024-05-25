using System;
using Microsoft.AspNetCore.Mvc;

namespace ApiLab7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        [HttpGet("sales")]
        public async Task<IActionResult> GetSalesAsync(DateTime dateToFind)
        {
            ValidateDate(dateToFind);
            SaleBusiness saleBusiness = new SaleBusiness();

            var taskSalesOfTheDay = saleBusiness.GetSalesAsync(dateToFind);
            var taskSalesOfTheWeek = saleBusiness.GetTotalSalesAsync(dateToFind);

            await Task.WhenAll(taskSalesOfTheDay, taskSalesOfTheWeek);

            var salesOfTheDay = await taskSalesOfTheDay;
            var salesOfTheWeek = await taskSalesOfTheWeek;

            return Ok(new { SalesOfTheDay = salesOfTheDay, SalesOfTheWeek = salesOfTheWeek });
        }

        private void ValidateDate(DateTime dateToValidate)
        {
            if (dateToValidate == DateTime.MinValue)
                throw new ArgumentException("A valid date is expected");
        }
    }
}
