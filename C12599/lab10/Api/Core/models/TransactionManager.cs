using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using storeapi.Database;

namespace storeapi.Models
{
    public class TransactionManager
    {
        public class TransactionRecord
        {
            public decimal TotalAmount { get; set; }
            public DateTime TransactionDate { get; set; }
            public string PurchaseNumber { get; set; }
            public PaymentMethods PaymentMethod { get; set; }
        }

        public static async Task<IEnumerable<TransactionRecord>> LoadTransactionsFromDatabaseAsync(IEnumerable<string[]> transactionData)
        {
            List<TransactionRecord> transactions = new List<TransactionRecord>();

            foreach (string[] row in transactionData)
            {
                if (row.Length < 4)
                {
                    continue;
                }

                if (decimal.TryParse(row[0], out decimal totalAmount) &&
                    DateTime.TryParse(row[1], out DateTime transactionDate) &&
                    int.TryParse(row[2], out int purchaseNumber) &&
                    Enum.TryParse(row[3], out PaymentMethods.Type paymentMethodType))
                {
                    // Simula la búsqueda del método de pago de forma asincrónica
                    PaymentMethods paymentMethod = await Task.FromResult(PaymentMethods.Find(paymentMethodType));

                    if (paymentMethod != null)
                    {
                        if (totalAmount <= 0)
                        {
                            throw new ArgumentException("El monto total debe ser mayor que cero.");
                        }

                        if (string.IsNullOrWhiteSpace(row[2]))
                        {
                            throw new ArgumentException("El número de compra no puede estar vacío.");
                        }

                        TransactionRecord transaction = new TransactionRecord
                        {
                            TotalAmount = totalAmount,
                            TransactionDate = transactionDate,
                            PurchaseNumber = purchaseNumber.ToString(),
                            PaymentMethod = paymentMethod
                        };

                        transactions.Add(transaction);
                    }
                    else
                    {
                        throw new InvalidOperationException($"No se encontró el método de pago correspondiente: {paymentMethodType}");
                    }
                }
            }

            return transactions;
        }
    }
}
