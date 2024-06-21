//Interfaces para serializar las objetos JSON de la API
export interface NotificationAPI {
    id: number | null;
    notifyTitle: string;
    notifyMessage: string;    
    notifyCreationDate: string;
  }
