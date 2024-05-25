using System.Collections;
using System.Diagnostics.SymbolStore;

namespace storeApi.Models.Data;

public class Categories
{
    private Category categoryStruct;
    private List<Category> listCategories;

    public Categories()
    {
        categoryStruct = new Category();
        listCategories = new List<Category>();
        makeCategories();
    }
    private void makeCategories()
    {
        listCategories.Add(categoryStruct.crearCategoria(1, "Electrónica"));
        listCategories.Add(categoryStruct.crearCategoria(2, "Moda"));
        listCategories.Add(categoryStruct.crearCategoria(3, "Hogar y jardín"));
        listCategories.Add(categoryStruct.crearCategoria(4, "Deportes"));
        listCategories.Add(categoryStruct.crearCategoria(5, "Belleza"));
        listCategories.Add(categoryStruct.crearCategoria(6, "Alimentación"));
        listCategories.Add(categoryStruct.crearCategoria(7, "Tecnología"));
        listCategories.Add(categoryStruct.crearCategoria(8, "Actividades al aire libre"));
        listCategories.Add(categoryStruct.crearCategoria(9, "Entretenimiento"));
    }
    public Category GetCategoryById(int categoryId)
    {
        if (categoryId<1)throw new ArgumentException($"No se permiten categorias con ID negativo o cero, ID:{categoryId}.");
        var category = listCategories.FirstOrDefault(cat => cat.CategoryID == categoryId);
        if (category.CategoryID == 0)throw new InvalidOperationException($"No se encontró ninguna categoría con el ID {categoryId}.");
        return category;
    }
    public IEnumerable<Category> GetCategories()
    {
        return listCategories.OrderBy(category => category.NameCategory);
    }
}

public struct Category
{
    public string NameCategory { private set; get; }
    public int CategoryID {private set; get; }
    public Category crearCategoria(int id, string name)
    {
        if (id < 1) throw new ArgumentException("Se necesita un id valido, para crear la categoria");
        if (string.IsNullOrWhiteSpace(name) || name.Trim() == "") throw new ArgumentException("Se necesita un nombre para la categoria");
        return new Category { CategoryID = id, NameCategory = name };
    }
}
