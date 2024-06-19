using System.Text.Json.Serialization;
using MyStoreAPI.Models;
namespace MyStoreAPI.Models
{

    public class Notification{

        public int notifyId { get; private set; }
        public string notifyTitle { get; private  set; }
        public string notifyMessage { get; private  set; }
        public DateTime notifyCreationDate { get; set; }


        public void Notification(){
            if (notifyId <= 0)
                throw new ArgumentException($"{nameof(notifyId)} no puede ser negativo ni igual a cero.");
                        
            if (string.IsNullOrWhiteSpace(notifyTitle))
                throw new ArgumentException($"{nameof(notifyTitle)} no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(notifyMessage))
                throw new ArgumentException($"{nameof(notifyMessage)} no puede estar vacío.");
            
            if (Message.Length > 5000)
                throw new ArgumentException($"{nameof(Message)} no puede tener más de 5000 caracteres.");
        }
    }
    
}