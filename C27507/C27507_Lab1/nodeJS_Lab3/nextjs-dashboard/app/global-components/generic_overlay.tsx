import { useState } from 'react';
import Alert from 'react-bootstrap/Alert';
import Button from 'react-bootstrap/Button';

interface AlertShopProps {  
  alertType: string;     
  alertTitle: string;   
  alertInfo: string;  
  showAlert: boolean;
  onClose: () => void;
}

export const AlertShop: React.FC<AlertShopProps> = ({ alertTitle,alertInfo,alertType,showAlert,onClose}) => {
  if (showAlert) {
    return (
      <Alert variant={alertType}  onClose={onClose} dismissible style={{ zIndex: 9999 }}>
        <Alert.Heading>{alertTitle}</Alert.Heading>
        <p>
          {alertInfo}
        </p>
      </Alert>
    );
  }
}