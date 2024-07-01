﻿namespace Core;

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
        agregaCategoria(1, "Computadoras");
        agregaCategoria(2, "Smartphones");
        agregaCategoria(3, "Accesorios de Tecnología");
        agregaCategoria(4, "Software");
        agregaCategoria(5, "Gaming");
        agregaCategoria(6, "Electrónica de Consumo");
        agregaCategoria(7, "Hardware");
        agregaCategoria(8, "Robótica");

        ListaCategorias.Sort((categoria1, categoria2) => string.Compare(categoria1.Nombre, categoria2.Nombre));
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
    public IEnumerable<Categoria> GetCategorias() { return ListaCategorias; }
}