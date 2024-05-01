using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using storeapi.Database;
using storeapi.Models;

namespace storeapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        [HttpGet("transactions")]
        public async Task<IActionResult> GetTransactions([FromQuery] string date)
        {
            if (string.IsNullOrWhiteSpace(date))
            {
                return BadRequest("La fecha no puede estar vacía.");
            }

          
            Task<IEnumerable<string[]>> taskTransactionDay =  SalesDB.GetForDayAsync(date);
            Task<IEnumerable<string[]>> taskTransactionWeek = SalesDB.GetForWeekAsync(date);

        
            await Task.WhenAll(taskTransactionDay, taskTransactionWeek);

        
            IEnumerable<TransactionManager.TransactionRecord> transactionsDays =
                await TransactionManager.LoadTransactionsFromDatabaseAsync(taskTransactionDay.Result);
            IEnumerable<TransactionManager.TransactionRecord> transactionsWeeks =
                await TransactionManager.LoadTransactionsFromDatabaseAsync(taskTransactionWeek.Result);

        
            var response = new Dictionary<string, IEnumerable<TransactionManager.TransactionRecord>>
            {
                ["transactionsDays"] = transactionsDays,
                ["transactionsWeeks"] = transactionsWeeks
            };

            return Ok(response);
        }
    }
}

