using System.ComponentModel.DataAnnotations;

namespace StoreApi.Models
{
    public sealed class Category
    {
        [Key]
        public Guid Uuid { get; set; }
        public string Name { get; set; }
    }
}