using Core;

namespace UT;

public class CategoriasTest
{


    [SetUp]

    public void SetUp()
    {
        Categorias categorias;

        categorias = Categorias.Instance;
    }


    [Test]
    public void Categorias_Build_CreatesCategories()
    {

        var categorias = Categorias.Instance;

        var listaCategorias = categorias.GetCategorias();

        Assert.IsNotNull(listaCategorias);
        Assert.IsNotEmpty(listaCategorias);
    }

    [Test]

    public void Categoria_Creation_WithInvalidId_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Categoria(0, "Alimento"));
        Assert.Throws<ArgumentException>(() => new Categoria(-1, "Belleza"));
    }

    [Test]
    public void Categoria_Creation_WithInvalidNombre_ThrowsArgumentException()
    {

        Assert.Throws<ArgumentException>(() => new Categoria(1, null));
        Assert.Throws<ArgumentException>(() => new Categoria(1, ""));
        Assert.Throws<ArgumentException>(() => new Categoria(1, "    "));
    }

    [Test]
    public void getCategorias_Ordenadas()
    {
        var categorias = Categorias.Instance.GetCategorias();
        var categoriasEsperadas = new List<string> { "Alimentación", "Belleza", "Deportes", "Electrónica", "Entretenimiento", "Hogar y Jardín", "Moda", "Tecnología" };
        var nombresCategorias = categorias.Select(category => category.Nombre).ToList();
        CollectionAssert.AreEqual(categoriasEsperadas, nombresCategorias);
    }

    [Test]
    public void cantidadCategorias()
    {
        int cantidadEsperada = 8;
        var categorias = Categorias.Instance.GetCategorias();
        Assert.AreEqual(cantidadEsperada, categorias.Count());
    }

}
