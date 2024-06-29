//Interfaces para serializar las objetos JSON de la API
export interface NotificationAPI {
    notifyId: number;
    notifyTitle: string;
    notifyMessage: string;    
    notifyCreationDate: string;
    notifyStatus: number;
  }
