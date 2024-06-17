namespace Core;

public class Categorias
{
        private static Categorias instance;
        private static List<Categoria> ListaCategorias;

        public Categorias()
        {
            ListaCategorias = new List<Categoria>();
        }

        private static void categoriasBuild() 
        {
            agregaCategoria(1, "Electrónica");
            agregaCategoria(2, "Moda");
            agregaCategoria(3, "Hogar y Jardín");
            agregaCategoria(4, "Deportes");
            agregaCategoria(5, "Belleza");
            agregaCategoria(6, "Alimentación");
            agregaCategoria(7, "Entretenimiento");
            agregaCategoria(8, "Tecnología");

            ListaCategorias.Sort((categoria1, cateogoria2) => string.Compare(categoria1.Nombre, cateogoria2.Nombre));
        }

        private static void agregaCategoria(int id, string nombre)
        {
            Categoria newCategoria = new Categoria(id, nombre);
            ListaCategorias.Add(newCategoria);
        }

        public static Categorias Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Categorias();
                    categoriasBuild();
                }
                return instance;
            }
        }
        public IEnumerable<Categoria> GetCategorias(){return ListaCategorias;}
}
