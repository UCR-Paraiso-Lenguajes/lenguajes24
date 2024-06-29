using System.Text.Json.Serialization;
using MyStoreAPI.Models;
namespace MyStoreAPI.Models
{

    public class Notification{

        public int notifyId { get; private set; }
        public string notifyTitle { get; private  set; }
        public string notifyMessage { get; private  set; }
        public DateTime notifyCreationDate { get; set; }
        public int notifyStatus { get; set; }


        public Notification(int notifyId, string notifyTitle, string notifyMessage,DateTime notifyCreationDate, int notifyStatus){
            if (notifyId == null)
                throw new ArgumentException($"{nameof(notifyId)} no puede ser negativo ni igual a cero.");
                        
            if (string.IsNullOrWhiteSpace(notifyTitle))
                throw new ArgumentException($"{nameof(notifyTitle)} no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(notifyMessage))
                throw new ArgumentException($"{nameof(notifyMessage)} no puede estar vacío.");
            
            if (notifyMessage.Length > 5000)
                throw new ArgumentException($"{nameof(notifyMessage)} no puede tener más de 5000 caracteres.");

            if (notifyStatus != 0 && notifyStatus != 1)
                throw new ArgumentException($"{nameof(notifyStatus)} debe ser 0 o 1.");

            this.notifyId = notifyId;
            this.notifyTitle = notifyTitle;
            this.notifyMessage = notifyMessage;
            this.notifyCreationDate = notifyCreationDate;
            this.notifyStatus= notifyStatus;
        }
    }
    
}