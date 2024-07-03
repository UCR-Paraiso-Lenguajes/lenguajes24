using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreApi.Models
{
    public sealed class Sinpe
    {
        [Key]
        public Guid Uuid { get; set; }
        public Guid SalesUuid { get; set; }
        public string ConfirmationNumber { get; set; }
    }
}