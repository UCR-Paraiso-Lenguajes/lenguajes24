using storeApi;
using storeApi.Models;

public class KeywordSearchTree
{
    private Dictionary<string, List<Product>> index;

    public KeywordSearchTree()
    {
        index = new Dictionary<string, List<Product>>();
    }

    public void IndexProduct(Product product)
    {
        string[] keywords = GetKeywords(product.Name + " " + product.Description);

        foreach (string keyword in keywords)
        {
            if (!index.ContainsKey(keyword))
            {
                index[keyword] = new List<Product>();
            }
            index[keyword].Add(product);
        }
    }

    public List<Product> Search(string keyword)
    {
        if (index.ContainsKey(keyword))
        {
            return index[keyword];
        }
        else
        {
            return new List<Product>();
        }
    }

    private string[] GetKeywords(string text)
    {
        // Aquí puedes implementar la lógica para dividir el texto en palabras clave.
        // Por ejemplo, puedes utilizar expresiones regulares o métodos de procesamiento de texto.
        return text.Split(new char[] { ' ', ',', '.', ';', ':', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
    }
}
