//Interfaces para serializar las objetos JSON de la API
export enum PaymentMethodNumber {
    CASH = 1,
    CREDIT_CARD = 2,
    DEBIT_CARD = 3,
    SINPE = 4  
  }
  
  export interface PaymentMethod {
    payment: PaymentMethodNumber; // Usamos el enum PaymentMethods para definir los tipos de pago
    verify: boolean;
  }
  
  export const PaymentMethods: PaymentMethod[] = [
    {
      payment: PaymentMethodNumber.CASH,
      verify: false
    },
    {
      payment: PaymentMethodNumber.CREDIT_CARD,
      verify: true
    },
    {
      payment: PaymentMethodNumber.DEBIT_CARD,
      verify: true
    },
    {
      payment: PaymentMethodNumber.SINPE,
      verify: true
    }
  ];