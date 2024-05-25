'use client';
import { useEffect, useState } from 'react';

//Componentes
import Dropdown from 'react-bootstrap/Dropdown';
import { Chart } from "react-google-charts";//no se le pueden pasar useStates, solo listas de listas

//Funciones
import { getRegisteredSalesFromAPI} from '@/app/src/api/get-post-api';

//Recursos
import 'bootstrap/dist/css/bootstrap.min.css';
//import "./../../../src/css/sales_report.css"

import { RegisteredSaleAPI } from '@/app/src/models-data/RegisteredSale'
import { RegisteredSaleReport } from '@/app/src/models-data/RegisteredSaleReport';
import { AlertShop } from '@/app/global-components/generic_overlay';

export default function SalesReport(){
    
    const [eventDate, setEventDate] = useState<string | null>(null);
    const [dataForTable, setDataForTable] = useState<any[]>([]);
    const [dataForPie, setDataForPie] = useState<any[]>([]);
    const [validateData, setValidateData] = useState(false);
    
    //Estados  para los alert de Boostrap
    const [showAlert, setShowAlert] = useState(false);
    const [alertInfo,setAlertInfo] = useState("");
    const [alertTitle,setAlertTitle] = useState("");
    const [alertType,setAlertType] = useState("");

    //funciones para gestionar los alert
    function closeAlertShop(): void {
        setShowAlert(false);     
    }
    function callAlertShop (alertType:string,alertTitle:string,alertInfo:string): void {
        setAlertTitle(alertTitle);
        setAlertInfo(alertInfo);
        setAlertType(alertType)
        setShowAlert(true);
    }

    //Colocamos los tipos de datos
    //Donde la primera lista anidada son los nombres de columnas y sus tipos
    const defaultTableColName =
        ["Id Venta","Codigo de Compra", "Subtotal","Total","Direccion","Tax","Metodo de Pago","Fecha de compra"];
    const defaultPieTagName =
        ["Dia","Total de Ventas"];
    
    //Datos adicionales de Chart
    const tableOptions = {
        showRowNumber: false,
        page: 'enable',
        pageSize: 5
    };

    const pieOptions = {
        title: "Total de ventas de la ultima semana: ",
    };

    //Validar que los datos de la API esten seguros
    function validateRegisteredSaleReport(reportSale: RegisteredSaleReport | null) : void{
        const isSalesByDayValid = reportSale !== null && reportSale.salesByDay !== null;    
        const isSalesByWeekValid = reportSale !== null && reportSale.salesByWeek !== null;    
        if (isSalesByDayValid && isSalesByWeekValid) {            
            setValidateData(true);

            const tableData = [
                        defaultTableColName, 
                        ...reportSale.salesByDay.map((sale: any) => [
                            sale.idSale,
                            sale.purchaseNum,
                            sale.subTotal,
                            sale.total,                            
                            sale.direction,
                            sale.tax,
                            sale.paymentMethod.payment,
                            sale.dateTimeSale
                        ])
                    ];
                    setDataForTable(tableData);

                    const pieData = [
                        defaultPieTagName, 
                        ...reportSale.salesByWeek.map((sale: any) => [
                            sale.dayOfWeek,
                            sale.total,                            
                        ])
                    ];
                    setDataForPie(pieData);
        } else {
            setValidateData(false);
        }
    }    

    //Funcion base de todo
    const selectDateResetTable = (e: { target: { value: any; }; }) =>{
        var selectedDate = e.target.value;
        const dateParts = selectedDate.split('-');
        const year = parseInt(dateParts[0], 10);
        const month = parseInt(dateParts[1], 10) - 1; // Los meses en JS son de 0 a 11
        const day = parseInt(dateParts[2], 10);
        var validDate = new Date(year,month,day);
        //Usamos formato estandar ISO                  
        setEventDate(validDate.toISOString());
    }

    //Recibimos los datos cada que se detecte que cambio el valor seleccionado del input date
    useEffect(() => {
        const fetchData = async () => {
            try {
                const registeredSalesReport = await getRegisteredSalesFromAPI(eventDate);

                //Validar el tipo de informacion recibida (string = error 504/501...etc)
                if (typeof registeredSalesReport === "string") {                    
                    setValidateData(false);
                    //Si es un objeto usamos los useState para cada Chart
                }else if (typeof registeredSalesReport  === "object") {                        
                    validateRegisteredSaleReport(registeredSalesReport);                                                       
                } else{                    
                    setValidateData(false);
                }
            } catch (error) {
                callAlertShop("Error","Error al obtener datos","Al parecer los datos no pueden ser mostrados. Por favor intentalo de nuevo");
            }            
        };    
        if (eventDate) fetchData();
    }, [eventDate]);

    return(                
        
        <section className="row">

            <div className="col-sm-8">
                <h4>Reporte de Ventas:</h4>            
                <div className="row">
                    <label style={{ fontWeight: 'bolder',padding: '1rem' }}>Seleccione una fecha en específico:</label>
                    <br />
                    <input type="date" onChange={selectDateResetTable} style={{ border: '1px solid black',padding: '1rem', width: '50%' }} />
                </div>
                                                            
                <div>                
                    {
                        validateData ? (
                                <Chart
                                chartType="Table"
                                width="100%"
                                height="400px"
                                data={dataForTable}
                                options={tableOptions}                        
                                />
                        ):
                        null
                    }
                    
                </div>
            </div>            

            <div className="col-sm-4">
                {
                    validateData ? (
                        <Chart
                            chartType="PieChart"
                            data={dataForPie}
                            options={pieOptions}
                            width={"100%"}
                            height={"400px"}
                        />
                    ): null                
                }
            </div>
            <AlertShop alertTitle={alertTitle} alertInfo={alertInfo} alertType={alertType} showAlert={showAlert} onClose={closeAlertShop}/>                    
        </section>
    );
}