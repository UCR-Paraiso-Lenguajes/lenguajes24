using Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UT
{
    public class CategoriasTest
    {
        private Categorias categorias;

        [SetUp]
        public void SetUp()
        {
            categorias = Categorias.Instance;
        }

        [Test]
        public void Categorias_Build_CreatesCategories()
        {
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
        public void GetCategorias_Ordenadas()
        {
            var categoriasEsperadas = new List<string>
            {
                "Accesorios de Tecnología", "Computadoras", "Electrónica de Consumo", "Gaming", "Hardware",
                "Robótica", "Smartphones", "Software"
            };
            var nombresCategorias = categorias.GetCategorias().Select(categoria => categoria.Nombre).ToList();
            CollectionAssert.AreEqual(categoriasEsperadas, nombresCategorias);
        }

        [Test]
        public void CantidadCategorias()
        {
            int cantidadEsperada = 8;
            var categorias = this.categorias.GetCategorias();
            Assert.AreEqual(cantidadEsperada, categorias.Count());
        }
    }
}