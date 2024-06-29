using MediatR;

namespace StoreApi.Commands
{
    public sealed class UpdateProductCommand : IRequest<int>
    {
        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public Guid Category { get; set; }

        public UpdateProductCommand(Guid uuid, string name, string imageUrl, decimal price, string description, Guid category)
        {
            Uuid = uuid;
            Name = name;
            ImageUrl = imageUrl;
            Price = price;
            Description = description;
            Category = category;
        }
    }
}