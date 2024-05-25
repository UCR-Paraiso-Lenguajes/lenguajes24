namespace MyStoreAPI.Models
{

    public struct Category{

        public int id {get; private set;}
        public string name {get; private set;}

        public Category(int id, string name){            
            if(id < 0 ) throw new ArgumentException($"{nameof(id)} no puede ser igual ni menor a cero" );
            if(string.IsNullOrEmpty(name)) throw new ArgumentException($"{nameof(name)} no puede ser nulo ni vacío" );
            this.id = id;
            this.name = name;
        }
    }
}