using MediatR;
using StoreApi.Models;

namespace StoreApi.Commands
{
    public sealed class CreateProductCommand : IRequest<Product>
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public Guid Category { get; set; }
        public CreateProductCommand(string name, string imageUrl, decimal price, string description, Guid category)
        {
            Name = name;
            ImageUrl = imageUrl;
            Price = price;
            Description = description;
            Category = category;
        }
    }
}

