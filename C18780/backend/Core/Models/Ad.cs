using System.ComponentModel.DataAnnotations;
namespace StoreApi.Models
{
    public sealed class Ad
    {
        [Key]
        public Guid Uuid { get; set; }
        public DateTime Date { get; set; }
        public required string Message {get; set;}
    }
}