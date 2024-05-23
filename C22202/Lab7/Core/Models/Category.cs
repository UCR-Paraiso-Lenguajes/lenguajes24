namespace ShopApi.Models;
public struct Category{
    public int id {get;}
    public string name {get;}
    public Category(int id, string name)
    {
        if (id <= -1) throw new ArgumentException($"A Category {nameof(id)} is required");
        if (name == null) throw new ArgumentException($"The category {nameof(name)} is required");

        this.id = id;
        this.name = name;
    }
}