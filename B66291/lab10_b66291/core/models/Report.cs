using MySqlConnector;

namespace core.Models
{

    public class Report 
    {

        public string purchaseNumber {get; set;}
        public DateTime purchase_date {get; set;}
        public decimal total {get; set;}
        public int pcantidad {get; set;}

        public Report(string purchaseNumber, DateTime purchase_date, decimal total, int cantidad){
           if (string.IsNullOrEmpty(purchaseNumber)){
            throw new ArgumentException("El número de compra no puede estar vacío o nulo.", nameof(purchaseNumber));
           }
           if (purchase_date == DateTime.MinValue || purchase_date == DateTime.MaxValue) { 
            throw new ArgumentException($"La variable {nameof(purchase_date)} no puede ser default.");
           }
           if (total < 0){
            throw new ArgumentException($"La variable {nameof(total)} no puede ser negativa.");
           }
           if (cantidad < 0){
            throw new ArgumentException($"La variable {nameof(cantidad)} no puede ser negativa.");
           }
           this.purchaseNumber = purchaseNumber;
           this.purchase_date = purchase_date;
           this.total = total;
           this.pcantidad = cantidad;
        } 
       
    }
  
}

